package game

// The boilerplate screens of the original (title, instructions, about, options, quit, load and save, game menu, abandon,
// finalize character, prolog) plus the ones this port adds (seed entry, credits, export and import, notices).
// Texts and positions are copied from the VB *Processor classes; the reference frames in docs/reference/vb are the proof.

import "core:fmt"
import "core:strings"

MENU_MAX :: 12

Menu :: struct {
	labels: [MENU_MAX]string,
	count:  int,
	row:    int, // first row of the menu
}

menu_add :: proc(m: ^Menu, label: string) { m.labels[m.count] = label; m.count += 1 }

slot_label :: proc(core: ^Core, i: int) -> string {
	return core.slots[i].used ? fmt.tprintf("Slot %d", i + 1) : "(empty)"
}

// The menu a screen shows, or is_menu = false for pages and the seed screen. Labels are in the temporary allocator.
menu_of :: proc(core: ^Core, s: UI_State) -> (m: Menu, is_menu: bool) {
	switch s {
	case .Title:
		m.row = 14
		for l in ([]string{"Start", "Start with seed...", "Continue", "Instructions", "Options", "About", "Quit"}) { menu_add(&m, l) }
	case .Options:
		m.row = 5
		for l in ([]string{"Go Back", "SFX Volume...", "MUX Volume..."}) { menu_add(&m, l) }
	case .Sfx_Volume, .Mux_Volume:
		m.row = 6
		for p in 0 ..= 10 { menu_add(&m, fmt.tprintf("%d%%", p * 10)) }
	case .Confirm_Quit, .Confirm_Abandon:
		m.row = 14
		menu_add(&m, "No"); menu_add(&m, "Yes")
	case .Load_Game, .Save_Game, .Export_Slot, .Import_Slot:
		m.row = 5
		menu_add(&m, "Go Back")
		for i in 0 ..< SLOT_COUNT { menu_add(&m, slot_label(core, i)) }
		if s == .Load_Game { menu_add(&m, "Import a file...") }
		if s == .Save_Game { menu_add(&m, "Export a slot...") }
	case .Game_Menu:
		m.row = 5
		for l in ([]string{"Go Back", "Save Game", "Abandon Game"}) { menu_add(&m, l) }
	case .Finalize_Character, .Level_Up:
		m.row = 7
		menu_add(&m, "Cancel")
		for stat in Stat.Strength ..= Stat.Mana { menu_add(&m, fmt.tprintf("%s: %d", STATS[stat].name, player_character(&core.world).stats[stat])) }
	case .Interact_Item:
		m.row = 5
		for l in ([]string{"Cancel", "Drop", "Use", "Equip"}) { menu_add(&m, l) }
	case .Equipment_Detail:
		m.row = 14
		menu_add(&m, "Go Back"); menu_add(&m, "Unequip")
	case .Seed_Entry, .Instructions, .About, .Credits, .Import_Wait, .Prolog, .In_Play, .Enemies, .Inventory, .Ground_Inventory, .Equipment, .Message, .Map, .Status, .Dead, .Notice:
		return {}, false
	}
	return m, true
}

enter_state :: proc(core: ^Core, wanted: UI_State) {
	s := wanted
	// A list screen with nothing to list sends the player back to the in-play screen (the original divided by zero).
	if core.has_world {
		w := &core.world
		player := w.player.character
		#partial switch s {
		case .Inventory: if len(items_in_pack(w, player)) == 0 { s = .In_Play }
		case .Ground_Inventory: if len(items_on_ground(w, character_get(w, player).location)) == 0 { s = .In_Play }
		case .Equipment: if len(items_worn(w, player)) == 0 { s = .In_Play }
		case .Interact_Item: if item_get(w, core.interact_item) == nil { s = .Inventory; if len(items_in_pack(w, player)) == 0 { s = .In_Play } }
		}
	}
	core.state = s
	#partial switch s {
	case .Inventory, .Ground_Inventory, .Equipment: core.list_cursor = 0
	case .Interact_Item, .Equipment_Detail: core.cursors[s] = 0
	case .Load_Game, .Save_Game, .Export_Slot, .Import_Slot: refresh_slots(core)
	case .Sfx_Volume: core.cursors[s] = int(core.sfx_volume * 10 + 0.5)
	case .Mux_Volume: core.cursors[s] = int(core.music_volume * 10 + 0.5)
	case .Game_Menu, .Confirm_Abandon, .Confirm_Quit: core.cursors[s] = 0
	case .Credits: core.credits_top = 0
	case .Message: if m := message_head(&core.world); m != nil && m.sfx != .None { play_sfx(core, m.sfx); m.sfx = .None }
	}
}

show_notice :: proc(core: ^Core, back: UI_State, text: string) {
	core.notice_len = min(len(text), NOTICE_CAPACITY)
	copy(core.notice[:], text[:core.notice_len])
	core.notice_back = back
	enter_state(core, .Notice)
}

// ---- slots --------------------------------------------------------------------------------------------------------

slot_key :: proc(i: int) -> string { return fmt.tprintf("kc:slot%d", i + 1) }

refresh_slots :: proc(core: ^Core) {
	for i in 0 ..< SLOT_COUNT {
		core.slots[i] = {}
		if core.services.storage_get == nil { continue }
		text, ok := core.services.storage_get(slot_key(i), context.temp_allocator)
		if !ok { continue }
		if summary, good := save_peek_summary(text, context.temp_allocator); good { core.slots[i] = {true, summary} }
	}
}

// ---- starting, saving and loading games ---------------------------------------------------------------------------

random_seed :: proc(core: ^Core) -> u64 {
	if core.services.entropy == nil { return 1 }
	return core.services.entropy() % 1_000_000_000
}

start_game :: proc(core: ^Core, seed: u64) {
	if core.has_world { world_destroy(&core.world); core.has_world = false }
	world_init(&core.world, seed)
	core.has_world = true
	if !world_generate(&core.world) {
		world_destroy(&core.world); core.has_world = false
		show_notice(core, .Title, "The world could not be made. That should not happen!")
		return
	}
	reset_buttons(core)
	play_sfx(core, .Character_Creation)
	if player_character(&core.world).stats[.Unassigned] > 0 { enter_state(core, .Finalize_Character) } else { enter_state(core, .Prolog) }
}

abandon_world :: proc(core: ^Core) {
	reset_buttons(core)
	if core.has_world { world_destroy(&core.world); core.has_world = false }
}

save_to_slot :: proc(core: ^Core, i: int) {
	data, err := world_save(&core.world, false, context.allocator)
	if err != nil { show_notice(core, .Save_Game, "The game could not be saved."); return }
	defer delete(data)
	if core.services.storage_set == nil || !core.services.storage_set(slot_key(i), string(data)) {
		show_notice(core, .Save_Game, "The game could not be saved. The browser may be blocking storage or it may be full.")
		return
	}
	enter_state(core, .In_Play)
}

load_from_slot :: proc(core: ^Core, i: int) {
	if !core.slots[i].used || core.services.storage_get == nil { show_notice(core, .Load_Game, "There is no saved game in that slot."); return }
	text, ok := core.services.storage_get(slot_key(i), context.allocator)
	if !ok { show_notice(core, .Load_Game, "There is no saved game in that slot."); return }
	defer delete(text)
	loaded: World
	if load_world_text(&loaded, text) != .None { show_notice(core, .Load_Game, "That saved game is damaged and cannot be loaded."); return }
	if core.has_world { world_destroy(&core.world) }
	core.world = loaded
	core.has_world = true
	reset_buttons(core)
	enter_state(core, .In_Play)
}

export_slot :: proc(core: ^Core, i: int) {
	if !core.slots[i].used || core.services.storage_get == nil { show_notice(core, .Export_Slot, "That slot is empty. There is nothing to export."); return }
	text, ok := core.services.storage_get(slot_key(i), context.temp_allocator)
	if !ok || core.services.download_text == nil { show_notice(core, .Export_Slot, "The slot could not be exported."); return }
	core.services.download_text(fmt.tprintf("kordanors-cabal-slot%d.json", i + 1), text)
}

import_into_slot :: proc(core: ^Core, i: int) {
	defer { delete(core.import_text); core.import_text = "" }
	if core.services.storage_set == nil || !core.services.storage_set(slot_key(i), core.import_text) {
		show_notice(core, .Load_Game, "The file could not be stored. The browser may be blocking storage or it may be full.")
		return
	}
	show_notice(core, .Load_Game, fmt.tprintf("Imported into slot %d. Choose it under Continue.", i + 1))
}

handle_file_text :: proc(core: ^Core, text: string) {
	if core.state != .Import_Wait { return }
	if len(text) > IMPORT_LIMIT { show_notice(core, .Load_Game, "That file is too big to be a saved game."); return }
	probe: World
	switch load_world_text(&probe, text) {
	case .None:
		world_destroy(&probe)
		delete(core.import_text)
		core.import_text = strings.clone(text)
		enter_state(core, .Import_Slot)
	case .Parse, .Format: show_notice(core, .Load_Game, "That file is not a Kordanor's Cabal saved game.")
	case .Version: show_notice(core, .Load_Game, "That saved game is from a newer version of the game.")
	case .Shape, .Invariant: show_notice(core, .Load_Game, "That saved game is damaged and cannot be used.")
	}
}

handle_file_cancelled :: proc(core: ^Core) {
	if core.state == .Import_Wait { enter_state(core, .Load_Game) }
}

// ---- activating menu items ----------------------------------------------------------------------------------------

activate :: proc(core: ^Core, s: UI_State, index: int) {
	switch s {
	case .Title:
		switch index {
		case 0: start_game(core, random_seed(core))
		case 1: enter_state(core, .Seed_Entry)
		case 2: enter_state(core, .Load_Game)
		case 3: enter_state(core, .Instructions)
		case 4: enter_state(core, .Options)
		case 5: enter_state(core, .About)
		case 6: enter_state(core, .Confirm_Quit)
		}
	case .Options:
		switch index {
		case 0: enter_state(core, .Title)
		case 1: enter_state(core, .Sfx_Volume)
		case 2: enter_state(core, .Mux_Volume)
		}
	case .Sfx_Volume:
		core.sfx_volume = f32(index) / 10
		config_save(core)
		enter_state(core, .Options)
	case .Mux_Volume:
		core.music_volume = f32(index) / 10
		config_save(core)
		enter_state(core, .Options)
	case .Confirm_Quit:
		if index == 0 { enter_state(core, .Title) } else { core.quit = true }
	case .Confirm_Abandon:
		if index == 0 { enter_state(core, .In_Play) } else { abandon_world(core); enter_state(core, .Title) }
	case .Load_Game:
		switch index {
		case 0: enter_state(core, .Title)
		case 1 ..= SLOT_COUNT: load_from_slot(core, index - 1)
		case: // Import a file...
			if core.services.request_file_pick == nil { show_notice(core, .Load_Game, "Importing is not available here."); return }
			core.services.request_file_pick()
			enter_state(core, .Import_Wait)
		}
	case .Save_Game:
		switch index {
		case 0: enter_state(core, .Game_Menu)
		case 1 ..= SLOT_COUNT: save_to_slot(core, index - 1)
		case: enter_state(core, .Export_Slot)
		}
	case .Export_Slot:
		if index == 0 { enter_state(core, .Save_Game) } else { export_slot(core, index - 1) }
	case .Import_Slot:
		if index == 0 { delete(core.import_text); core.import_text = ""; enter_state(core, .Load_Game) } else { import_into_slot(core, index - 1) }
	case .Game_Menu:
		switch index {
		case 0: enter_state(core, .In_Play)
		case 1: enter_state(core, .Save_Game)
		case 2: enter_state(core, .Confirm_Abandon)
		}
	case .Finalize_Character, .Level_Up:
		if index == 0 {
			if s == .Level_Up { enter_state(core, .In_Play) } else { abandon_world(core); enter_state(core, .Title) }
			return
		}
		p := player_character(&core.world)
		if p.stats[.Unassigned] > 0 {
			stat_add(p, Stat(index), 1)
			stat_add(p, .Unassigned, -1)
		}
		if p.stats[.Unassigned] == 0 { enter_state(core, s == .Level_Up ? .In_Play : .Prolog) }
	case .Interact_Item: interact_item_activate(core, index)
	case .Equipment_Detail:
		if index == 0 { enter_state(core, .Equipment); return }
		unequip(&core.world, core.world.player.character, core.equip_slot)
		enter_state(core, .Equipment) // goes back to the in-play screen if nothing is left on
	case .Seed_Entry, .Instructions, .About, .Credits, .Import_Wait, .Prolog, .In_Play, .Enemies, .Inventory, .Ground_Inventory, .Equipment, .Message, .Map, .Status, .Dead, .Notice:
	}
}

// What Cancel does on a menu screen (HandleRed in the original; the default stays where it is).
cancel_menu :: proc(core: ^Core, s: UI_State) {
	#partial switch s {
	case .Options, .Confirm_Quit, .Load_Game: enter_state(core, .Title)
	case .Sfx_Volume, .Mux_Volume: enter_state(core, .Options)
	case .Save_Game: enter_state(core, .Game_Menu)
	case .Game_Menu, .Confirm_Abandon: enter_state(core, .In_Play)
	case .Export_Slot: enter_state(core, .Save_Game)
	case .Level_Up: enter_state(core, .In_Play)
	case .Interact_Item: enter_state(core, .Inventory)
	case .Equipment_Detail: enter_state(core, .Equipment)
	case .Import_Slot: activate(core, .Import_Slot, 0)
	}
}

// ---- input ----------------------------------------------------------------------------------------------------------

handle_command :: proc(core: ^Core, c: Command) {
	s := core.state
	if m, ok := menu_of(core, s); ok {
		switch c {
		case .Up: core.cursors[s] = (core.cursors[s] + m.count - 1) % m.count
		case .Down: core.cursors[s] = (core.cursors[s] + 1) % m.count
		case .Confirm: activate(core, s, core.cursors[s])
		case .Cancel: cancel_menu(core, s)
		case .None, .Left, .Right:
		}
		return
	}
	#partial switch s {
	case .Seed_Entry:
		switch c {
		case .Left: core.seed_pos = (core.seed_pos + SEED_DIGITS - 1) % SEED_DIGITS
		case .Right: core.seed_pos = (core.seed_pos + 1) % SEED_DIGITS
		case .Up: core.seed_digits[core.seed_pos] = (core.seed_digits[core.seed_pos] + 1) % 10
		case .Down: core.seed_digits[core.seed_pos] = (core.seed_digits[core.seed_pos] + 9) % 10
		case .Confirm: start_game(core, seed_value(core))
		case .Cancel: enter_state(core, .Title)
		case .None:
		}
	case .Credits:
		switch c {
		case .Up: core.credits_top = max(core.credits_top - 1, 0)
		case .Down: core.credits_top += 1 // clamped when drawn
		case .Confirm, .Cancel: enter_state(core, .About)
		case .None, .Left, .Right:
		}
	case .About:
		if c == .Confirm { enter_state(core, .Credits) } else if c == .Cancel { enter_state(core, .Title) }
	case .Instructions:
		if c == .Confirm || c == .Cancel { enter_state(core, .Title) }
	case .Prolog:
		if c == .Confirm { enter_state(core, .In_Play) }
	case .Enemies: if c == .Confirm || c == .Cancel { enter_state(core, .In_Play) }
	case .In_Play: play_command(core, c)
	case .Message: message_command(core, c)
	case .Inventory, .Ground_Inventory, .Equipment: list_command(core, c)
	case .Map: if c == .Confirm || c == .Cancel { enter_state(core, .In_Play) }
	case .Status: if c == .Confirm || c == .Cancel { enter_state(core, .In_Play) }
	case .Dead: if c == .Confirm { abandon_world(core); enter_state(core, .Title) }
	case .Import_Wait:
		if c == .Cancel { enter_state(core, .Load_Game) }
	case .Notice:
		if c == .Confirm || c == .Cancel { enter_state(core, core.notice_back) }
	}
}

seed_value :: proc(core: ^Core) -> (v: u64) {
	for d in core.seed_digits { v = v * 10 + u64(d) }
	return
}

SEED_DIGITS_COL :: (CELL_COLUMNS - (2 * SEED_DIGITS - 1)) / 2
SEED_ROW :: 5
SEED_START_ROW :: 12
SEED_BACK_ROW :: 13

// A tap on a cell. Menus: a precise tap (mouse) selects and confirms; a finger tap selects, and confirms when the item was
// already selected (decision D17). Pages treat any tap as Confirm.
handle_tap :: proc(core: ^Core, col, row: int, precise: bool) {
	if col < 0 || col >= CELL_COLUMNS || row < 0 || row >= CELL_ROWS { return }
	s := core.state
	if m, ok := menu_of(core, s); ok {
		i := row - m.row
		if i < 0 || i >= m.count { return }
		was := core.cursors[s]
		core.cursors[s] = i
		if precise || was == i { activate(core, s, i) }
		return
	}
	if s == .In_Play { play_tap(core, col, row, precise); return }
	if s == .Inventory || s == .Ground_Inventory || s == .Equipment { list_tap(core, row, precise); return }
	if s == .Seed_Entry {
		switch {
		case row == SEED_ROW:
			if i := (col - SEED_DIGITS_COL) / 2; col >= SEED_DIGITS_COL && i < SEED_DIGITS { core.seed_pos = i }
		case row == SEED_START_ROW: start_game(core, seed_value(core))
		case row == SEED_BACK_ROW: enter_state(core, .Title)
		}
		return
	}
	handle_command(core, .Confirm)
}

// ---- drawing --------------------------------------------------------------------------------------------------------

draw_screen :: proc(core: ^Core) {
	s := &core.screen
	screen_fill(s, GLYPH_SPACE, false, .Blue)
	state := core.state
	draw_prompt(core, state)
	if m, ok := menu_of(core, state); ok {
		for i in 0 ..< m.count { write_text_centered(s, m.row + i, m.labels[i], i == core.cursors[state], .Orange) }
	}
}

centered_header :: proc(s: ^Screen, text: string) {
	fill_cells(s, 0, 0, CELL_COLUMNS, 1, GLYPH_SPACE, true, .Blue)
	write_text_centered(s, 0, text, true, .Blue)
}

draw_prompt :: proc(core: ^Core, state: UI_State) {
	s := &core.screen
	switch state {
	case .Title:
		fill_cells(s, 0, 0, CELL_COLUMNS, 5, glyph_of('*'), true, .Blue)
		fill_cells(s, 1, 1, CELL_COLUMNS - 2, 3, GLYPH_SPACE, true, .Blue)
		write_text(s, 0, 2, "*  Kordanor's Cabal  *", true, .Blue)
		write_text(s, 0, 6, "A Game in VB.NET About", false, .Black)
		write_text(s, 0, 7, "Lookin' Like a Dungeon", false, .Black)
		write_text(s, 0, 8, " Crawler Written  for ", false, .Black)
		write_text(s, 0, 9, "      the VIC-20      ", false, .Black)
		write_text(s, 0, 11, "   A Production  of   ", false, .Black)
		write_text(s, 0, 12, "   TheGrumpyGameDev   ", false, .Black)
		write_text(s, 0, CELL_ROWS - 1, "Controls: Arrows/Space", false, .Blue)
	case .Seed_Entry:
		centered_header(s, "Start With Seed")
		write_text(s, 0, 2, "The same seed always  makes the same world.", false, .Black)
		for d, i in core.seed_digits {
			write_text(s, SEED_DIGITS_COL + 2 * i, SEED_ROW, string([]u8{'0' + d}), i == core.seed_pos, .Orange)
		}
		write_text_centered(s, 7, "Left/Right: digit", false, .Blue)
		write_text_centered(s, 8, "Up/Down: change it", false, .Blue)
		write_text_centered(s, SEED_START_ROW, "Start", false, .Orange)
		write_text_centered(s, SEED_BACK_ROW, "Go Back", false, .Orange)
	case .Instructions:
		write_text(s, 0, 0, "     Instructions     ", true, .Blue)
		write_text(s, 0, 2, "The game is pretty", false, .Black)
		write_text(s, 0, 3, "much menu driven. Use", false, .Black)
		write_text(s, 0, 4, "arrow keys, space, ", false, .Black)
		write_text(s, 0, 5, "enter, and rarely esc.", false, .Black)
	case .About:
		centered_header(s, "About")
		write_text(s, 0, 1, "A production of       TheGrumpyGameDev", false, .Black)
		write_text(s, 0, 3, "With \"help\" from his  malcontent and        neer-do-well twitch   channel viewers.", false, .Black)
		write_text(s, 0, 14, "Special thanks to:", false, .Black)
		write_text(s, 0, 15, "* Kordanor", false, .Black)
		write_text(s, 0, 16, "* Zooperdan", false, .Black)
		write_text(s, 0, 17, "* Vermux", false, .Black)
		write_text(s, 0, 18, "* Lorc", false, .Black)
		write_text(s, 0, 20, "Select to see credits and links to their  work!", false, .Purple)
	case .Credits: draw_credits(core)
	case .Options: write_text(s, 7, 0, "Options:", false, .Blue)
	case .Sfx_Volume: write_text_centered(s, 4, "SFX Volume", false, .Blue)
	case .Mux_Volume: write_text_centered(s, 4, "MUX Volume", false, .Blue)
	case .Confirm_Quit: write_text(s, 0, 10, "Are you sure you want to quit?", false, .Red)
	case .Confirm_Abandon: write_text(s, 0, 10, "Are you sure you want to abandon the game?", false, .Red)
	case .Load_Game: write_text_centered(s, 0, "Continue Game", false, .Blue)
	case .Save_Game: write_text_centered(s, 0, "Save Game", false, .Blue)
	case .Export_Slot: write_text_centered(s, 0, "Export Which Slot?", false, .Blue)
	case .Import_Slot: write_text_centered(s, 0, "Import Into Slot", false, .Blue)
	case .Import_Wait:
		centered_header(s, "Import")
		write_text(s, 0, 2, "Choose a saved game file in the box that opened. Cancel to go back.", false, .Black)
	case .Game_Menu: write_text_centered(s, 0, "Game Menu", false, .Blue)
	case .Finalize_Character, .Level_Up:
		centered_header(s, state == .Level_Up ? "Level Up Character" : "Finalize Character")
		write_text_centered(s, 2, fmt.tprintf("%s: %d", STATS[.Unassigned].name, player_character(&core.world).stats[.Unassigned]), false, .Purple)
		write_text(s, 0, 4, "Choose where to assignpoint(s):", false, .Black)
	case .Prolog:
		write_text_centered(s, 0, "        Prolog        ", true, .Blue)
		lines := []string{
			"The town of Zooperdan ", "has fallen upon hard  ", "times. A few years    ", "ago, the Cabal of     ", "Kordanor moved into a ",
			"nearby abandoned      ", "church. At first, no  ", "one thought much of   ", "the strange Cabal     ", "members, but soon the ",
			"villagers started     ", "disappearing one by   ", "one. So they pooled   ", "their money and set   ", "about hiring someone  ",
			"to go in there and put", "an end to the Cabal   ", "once and for all! They", "hired you. Good luck! ",
		}
		for l, i in lines { write_text(s, 0, 2 + i, l, false, .Black) }
		write_text_centered(s, 22, "SPACE to start", true, .Orange)
	case .In_Play: draw_play(core)
	case .Enemies: draw_enemies(core)
	case .Inventory: draw_inventory(core)
	case .Ground_Inventory: draw_ground(core)
	case .Equipment: draw_equipment(core)
	case .Interact_Item, .Equipment_Detail: draw_item_menu_prompt(core)
	case .Message: draw_message(core)
	case .Map: draw_map(core)
	case .Status: draw_status(core)
	case .Dead: draw_dead(core)
	case .Notice:
		centered_header(s, "Notice")
		write_text(s, 0, 2, string(core.notice[:core.notice_len]), false, .Black)
	}
}

// ---- credits --------------------------------------------------------------------------------------------------------

CREDITS_TEXT :: []string{
	"Special thanks go to the following people:", "",
	"Kordanor:", "https://www.youtube.com/c/KordanorsGamingLair", "",
	"Zooperdan:", "https://zooperdan.itch.io/", "",
	"Vermux:", "https://vurmux.itch.io/urizen-onebit-tilesets", "",
	"Lorc:", "https://game-icons.net/1x1/lorc/cultist.html", "",
	"The music was generated with Abundant Music.",
}

// Greedy word wrap to the screen width; a word longer than a row (an address) is cut at the row's end.
wrap_lines :: proc(text: string, out: ^[dynamic]string) {
	if text == "" { append(out, ""); return }
	rest := text
	for len(rest) > 0 {
		if len(rest) <= CELL_COLUMNS { append(out, rest); return }
		cut := CELL_COLUMNS
		if sp := strings.last_index_byte(rest[:CELL_COLUMNS + 1], ' '); sp > 0 { cut = sp }
		append(out, rest[:cut])
		rest = strings.trim_left(rest[cut:], " ")
	}
}

CREDITS_ROWS :: 20
credit_lines :: proc() -> []string {
	lines := make([dynamic]string, context.temp_allocator)
	for t in CREDITS_TEXT { wrap_lines(t, &lines) }
	return lines[:]
}

draw_credits :: proc(core: ^Core) {
	s := &core.screen
	centered_header(s, "Credits")
	lines := credit_lines()
	core.credits_top = clamp(core.credits_top, 0, max(len(lines) - CREDITS_ROWS, 0))
	for i in 0 ..< CREDITS_ROWS {
		if at := core.credits_top + i; at < len(lines) { write_text(s, 0, 2 + i, lines[at], false, .Black) }
	}
	write_text_centered(s, 22, "Up/Down: scroll", false, .Blue)
}
