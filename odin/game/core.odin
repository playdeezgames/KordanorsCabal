package game

// The portable game core: the screen state machine of the original (MainProcessor and the *Processor classes) plus the
// world it drives. Platforms call core_init once and core_step once per presented frame (see api.odin).
//
// Structure: every screen is a UI_State. A screen is either a menu (a column of centred labels with a cursor), a page
// (text that any confirm or cancel dismisses) or a special screen (seed entry). Drawing is a pure function of the core's
// state; stepping applies input events. Screens live in ui_screens.odin.

import "core:mem"

UI_State :: enum u8 {
	Title,
	Seed_Entry,
	Instructions,
	About,
	Credits,
	Options,
	Sfx_Volume,
	Mux_Volume,
	Confirm_Quit,
	Load_Game,
	Save_Game,
	Export_Slot,
	Import_Wait,
	Import_Slot,
	Game_Menu,
	Confirm_Abandon,
	Finalize_Character,
	Level_Up,
	Prolog,
	In_Play,
	Inventory,
	Interact_Item,
	Ground_Inventory,
	Equipment,
	Equipment_Detail,
	Enemies,
	Message,
	Map,
	Status,
	Dead,
	Notice,
}

SLOT_COUNT :: 5
NOTICE_CAPACITY :: 192
IMPORT_LIMIT :: 2 * 1024 * 1024 // bytes; a bigger file is not a save of ours
SEED_DIGITS :: 9
CONFIG_KEY :: "kc:config"

Slot_Info :: struct { used: bool, summary: Save_Summary }

Core :: struct {
	services:     Services,
	screen:       Screen,
	frame:        [FRAME_WIDTH * FRAME_HEIGHT]u32,
	state:        UI_State,
	cursors:      [UI_State]int, // a menu's cursor survives leaving and coming back, as in the original
	sfx_volume:   f32,
	music_volume: f32,
	slots:        [SLOT_COUNT]Slot_Info,
	seed_digits:  [SEED_DIGITS]u8,
	seed_pos:     int,
	credits_top:  int,
	world:        World,
	has_world:    bool,
	notice:       [NOTICE_CAPACITY]u8,
	notice_len:   int,
	notice_back:  UI_State,
	import_text:  string, // a picked file that passed the check, waiting for a slot (owned)
	sfx:          [MAX_SFX_PER_STEP]Sfx,
	sfx_count:    int,
	quit:         bool,
	stack:        [UI_STACK_DEPTH]UI_State, // where to return after the messages (task 12: it is only ever used for that)
	stack_len:    int,
	button:       int,                       // the selected in-play button
	button_stack: [BUTTON_STACK_DEPTH]int,
	button_depth: int,
	list_cursor:  int,                       // the selected row of the list screens (inventory, ground, equipment)
	interact_item: Item_ID,                  // the item the Interact_Item screen is about
	equip_slot:   Equip_Slot,                // the slot the Equipment_Detail screen is about
	ticks:        u32,                       // counts steps; drives purely decorative effects (the orb's colour)
}

UI_STACK_DEPTH :: 4
BUTTON_STACK_DEPTH :: 8

core_init :: proc(core: ^Core, services: Services) {
	core^ = {}
	core.services = services
	config_load(core)
	enter_state(core, .Title)
}

// Frees what the core owns (the world and any pending import). The core cannot be stepped afterwards.
core_destroy :: proc(core: ^Core) {
	if core.has_world { world_destroy(&core.world) }
	delete(core.import_text)
	core^ = {}
}

play_sfx :: proc(core: ^Core, s: Sfx) {
	if core.sfx_count < MAX_SFX_PER_STEP {
		core.sfx[core.sfx_count] = s
		core.sfx_count += 1
	}
}

core_step :: proc(core: ^Core, input: Step_Input, out: ^Step_Output) {
	core.sfx_count = 0
	core.ticks += 1
	for e in input.events {
		switch e.kind {
		case .None:
		case .Command: handle_command(core, e.command)
		case .Tap: handle_tap(core, int(e.col), int(e.row), e.precise)
		case .File_Text: handle_file_text(core, e.text)
		case .File_Cancelled: handle_file_cancelled(core)
		}
	}
	if core.has_world { // sounds the rules raised during the events
		for i in 0 ..< core.world.sfx_count { play_sfx(core, core.world.sfx_queue[i]) }
		core.world.sfx_count = 0
	}
	draw_screen(core)
	rasterize(&core.screen, &core.frame)
	out.frame = &core.frame
	out.frame_changed = true
	out.sfx = core.sfx
	out.sfx_count = core.sfx_count
	out.sfx_volume = core.sfx_volume
	out.music_volume = core.music_volume
	out.quit_requested = core.quit
	free_all(context.temp_allocator)
}

// ---- config (volumes), stored under one key and written whenever it changes -------------------------------------

Config :: struct { sfx_volume, music_volume: f32 }

config_load :: proc(core: ^Core) {
	core.sfx_volume, core.music_volume = 0.5, 0.5
	if core.services.storage_get == nil { return }
	text, ok := core.services.storage_get(CONFIG_KEY, context.temp_allocator)
	if !ok { return }
	c: Config
	if config_parse(text, &c) { core.sfx_volume, core.music_volume = c.sfx_volume, c.music_volume }
}

config_save :: proc(core: ^Core) {
	if core.services.storage_set == nil { return }
	core.services.storage_set(CONFIG_KEY, config_text(Config{core.sfx_volume, core.music_volume}))
}

// ---- reading a world from text, shared by Continue and Import ------------------------------------------------------

// Parses into a scratch arena that is thrown away afterwards (decision D19), so loading leaves no garbage in the heap.
load_world_text :: proc(w: ^World, text: string) -> Load_Error {
	arena_size := max(1 << 20, 4 * len(text))
	buffer, err := mem.make_aligned([]byte, arena_size, 16)
	if err != nil { return .Shape }
	defer delete(buffer)
	arena: mem.Arena
	mem.arena_init(&arena, buffer)
	e, _ := world_load(w, transmute([]byte)text, mem.arena_allocator(&arena))
	return e
}
