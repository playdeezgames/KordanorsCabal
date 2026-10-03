package game

// =============================================================================================
// The platform interface (task 15). Everything the portable core exchanges with a platform.
// The core never imports a platform package; platforms never reach into core state.
//
//   platform -> core:  Input_Event list per frame, dt, and Services supplied once at init
//   core -> platform:  Step_Output per frame (pixels, sounds, volumes, quit flag)
//                      and synchronous calls through Services (storage, files, entropy, log)
// =============================================================================================

import "base:runtime"

// ---- fixed geometry (task 8) -----------------------------------------------------------------
CELL_COLUMNS :: 22
CELL_ROWS :: 23
CELL_SIZE :: 8
BORDER_X :: 16
BORDER_Y :: 28
FRAME_WIDTH :: BORDER_X * 2 + CELL_COLUMNS * CELL_SIZE // 208
FRAME_HEIGHT :: BORDER_Y * 2 + CELL_ROWS * CELL_SIZE   // 240
FRAME_STRETCH_X :: 2 // the VIC-20 look (D10): the platform shows the frame twice as wide as tall

// ---- input -----------------------------------------------------------------------------------
// The six commands the game really uses (task 12; Green and Blue always meant the same thing).
Command :: enum u8 { None, Up, Down, Left, Right, Confirm, Cancel }

Input_Kind :: enum u8 {
	None,
	Command,        // a keyboard key, gamepad button, or on-screen control button, already mapped
	Tap,            // pointer/touch press on the paper: col,row are CELL coordinates (may be outside the grid).
	                // `precise` is true for a mouse or pen. A finger is not precise (cells are only about 11 to 17 CSS px
	                // tall on a phone), so the core treats an imprecise tap as "select" first and "confirm" on the selected item.
	File_Text,      // result of request_file_pick: `text` holds the file contents (valid for this step only)
	File_Cancelled, // the user dismissed the file picker, or reading failed
}

Input_Event :: struct {
	kind:     Input_Kind,
	command:  Command,
	col, row: i16,
	precise:  bool,
	text:     string,
}

// ---- output ----------------------------------------------------------------------------------
// Matches the 8 values of the VB Sfx enum.
Sfx :: enum u8 { None, Character_Creation, Enemy_Death, Enemy_Hit, Level_Up, Miss, Player_Death, Player_Hit, Unlock_Door }
MAX_SFX_PER_STEP :: 16

Step_Output :: struct {
	frame:           ^[FRAME_WIDTH * FRAME_HEIGHT]u32, // RGBA bytes in memory order R,G,B,A (little-endian u32 0xAABBGGRR)
	frame_changed:   bool,
	sfx:             [MAX_SFX_PER_STEP]Sfx,
	sfx_count:       int,
	sfx_volume:      f32, // 0..1, always valid
	music_volume:    f32, // 0..1, always valid
	quit_requested:  bool,
}

Step_Input :: struct {
	dt:     f64, // seconds since the previous step (presentation only; the game is turn-based)
	events: []Input_Event,
}

// ---- services (platform functions the core calls synchronously) ---------------------------------
// Supplied once to core_init, so the core has no platform imports and tests can pass fakes.
Services :: struct {
	storage_get:       proc(key: string, allocator: runtime.Allocator) -> (value: string, ok: bool),
	storage_set:       proc(key, value: string) -> bool, // false on quota or when storage is unavailable
	storage_remove:    proc(key: string),
	download_text:     proc(filename, text: string), // export: offer text to the user as a file
	request_file_pick: proc(),                       // import: result arrives later as File_Text / File_Cancelled
	entropy:           proc() -> u64,                // for seeding; not reproducible
	log:               proc(message: string),
}

// ---- the core's entry points (implemented in core.odin) -------------------------------------------
//   core_init :: proc(core: ^Core, services: Services)
//   core_step :: proc(core: ^Core, input: Step_Input, out: ^Step_Output)
// Rules: core_step is called once per presented frame; it must not block, must not keep pointers into
// `input` (including event text) after returning, and fills `out` completely every call.
