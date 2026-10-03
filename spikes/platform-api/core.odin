package main

// A toy core used only to exercise the platform interface. Real renderer (task 8 port), no game rules.
import "core:encoding/json"
import "core:fmt"
import "core:strings"

Hue :: enum u8 { Black, White, Red, Cyan, Purple, Green, Blue, Yellow, Orange, Light_Orange, Pink, Light_Cyan, Light_Purple, Light_Green, Light_Blue, Light_Yellow }

// Palette from HueUtility.vb, stored as 0xAABBGGRR so the bytes in memory are R, G, B, A.
rgb :: proc(r, g, b: u32) -> u32 { return 0xFF000000 | b << 16 | g << 8 | r }
PALETTE := [Hue]u32{
	.Black = 0xFF000000, .White = 0xFFFFFFFF,
	.Red = 0xFF262D77, .Cyan = 0xFFDCD485, .Purple = 0xFFB45FA8, .Green = 0xFF4A9E55, .Blue = 0xFF8B3442, .Yellow = 0xFF71CCBD,
	.Orange = 0xFF4A73A8, .Light_Orange = 0xFF87B2E9, .Pink = 0xFF6268B6, .Light_Cyan = 0xFFFFFFC5, .Light_Purple = 0xFFF59DE9,
	.Light_Green = 0xFF87DF92, .Light_Blue = 0xFFCA707E, .Light_Yellow = 0xFFB0FFFF,
}

Cell :: struct { glyph: u8, hue: Hue, inverted: bool }

Core :: struct {
	services:     Services,
	cells:        [CELL_ROWS][CELL_COLUMNS]Cell,
	frame:        [FRAME_WIDTH * FRAME_HEIGHT]u32,
	selected:     int,
	counter:      int,
	frames:       int,
	last:         [22]u8,
	last_len:     int,
	info:         [3][22]u8, // three free text lines
	info_len:     [3]int,
	sfx_volume:   f32,
	music_volume: f32,
	pending_sfx:  [MAX_SFX_PER_STEP]Sfx,
	pending_count: int,
}

MENU_FIRST_ROW :: 6
Menu_Item :: enum { Count, Save, Load, Export, Import, Volume, Seed }
MENU_LABELS := [Menu_Item]string{.Count = "COUNT +1 (SOUND)", .Save = "SAVE SLOT", .Load = "LOAD SLOT", .Export = "EXPORT SLOT FILE", .Import = "IMPORT SLOT FILE", .Volume = "MUSIC VOLUME", .Seed = "NEW SEED"}

SLOT_KEY :: "kc:spike"
Slot_File :: struct { format: string, counter: int }

glyph_bitmaps := GLYPHS
ascii_glyph := ASCII_GLYPH

core_init :: proc(core: ^Core, services: Services) {
	core^ = {}
	core.services = services
	core.sfx_volume = 0.5
	core.music_volume = 0.5
	set_last(core, "READY")
	set_info(core, 0, "KEYS: ARROWS SPACE ESC")
	set_info(core, 1, "OR TAP A MENU ROW")
}

set_text :: proc(buf: []u8, len_out: ^int, text: string) {
	n := min(len(buf), len(text))
	copy(buf, text[:n])
	len_out^ = n
}
set_last :: proc(core: ^Core, text: string) { set_text(core.last[:], &core.last_len, text) }
set_info :: proc(core: ^Core, line: int, text: string) { set_text(core.info[line][:], &core.info_len[line], text) }
play :: proc(core: ^Core, sfx: Sfx) {
	if core.pending_count < MAX_SFX_PER_STEP { core.pending_sfx[core.pending_count] = sfx; core.pending_count += 1 }
}

core_step :: proc(core: ^Core, input: Step_Input, out: ^Step_Output) {
	core.frames += 1
	core.pending_count = 0
	for e in input.events {
		switch e.kind {
		case .None:
		case .Command:
			switch e.command {
			case .None:
			case .Up: core.selected = (core.selected + len(Menu_Item) - 1) % len(Menu_Item); set_last(core, "UP")
			case .Down: core.selected = (core.selected + 1) % len(Menu_Item); set_last(core, "DOWN")
			case .Left: set_last(core, "LEFT")
			case .Right: set_last(core, "RIGHT")
			case .Confirm: set_last(core, "CONFIRM"); activate(core, Menu_Item(core.selected))
			case .Cancel: set_last(core, "CANCEL"); play(core, .Miss)
			}
		case .Tap:
			set_last(core, fmt.tprintf("TAP %d,%d", e.col, e.row))
			if row := int(e.row) - MENU_FIRST_ROW; row >= 0 && row < len(Menu_Item) && e.col >= 0 && e.col < CELL_COLUMNS {
				confirm := e.precise || core.selected == row // a finger first selects, then a second tap on the same row confirms
				core.selected = row
				if confirm { activate(core, Menu_Item(row)) }
			}
		case .File_Text:
			import_text(core, e.text)
		case .File_Cancelled:
			set_info(core, 2, "IMPORT CANCELLED")
		}
	}
	draw(core)
	rasterize(core)

	out.frame = &core.frame
	out.frame_changed = true
	out.sfx = core.pending_sfx
	out.sfx_count = core.pending_count
	out.sfx_volume = core.sfx_volume
	out.music_volume = core.music_volume
	out.quit_requested = false
	free_all(context.temp_allocator)
}

activate :: proc(core: ^Core, item: Menu_Item) {
	s := &core.services
	switch item {
	case .Count:
		core.counter += 1
		play(core, .Player_Hit)
	case .Save:
		text := fmt.tprintf(`{{"format":"spike","counter":%d}}`, core.counter)
		ok := s.storage_set(SLOT_KEY, text)
		set_info(core, 2, ok ? "SAVED" : "SAVE FAILED")
	case .Load:
		if text, ok := s.storage_get(SLOT_KEY, context.temp_allocator); ok {
			f: Slot_File
			if json.unmarshal(transmute([]u8)text, &f, .JSON) == nil { core.counter = f.counter; set_info(core, 2, "LOADED") } else { set_info(core, 2, "SLOT UNREADABLE") }
		} else { set_info(core, 2, "SLOT EMPTY") }
	case .Export:
		if text, ok := s.storage_get(SLOT_KEY, context.temp_allocator); ok {
			s.download_text("kordanors-cabal-slot1.json", text)
			set_info(core, 2, fmt.tprintf("EXPORTED %d BYTES", len(text)))
		} else { set_info(core, 2, "NOTHING TO EXPORT") }
	case .Import:
		s.request_file_pick()
		set_info(core, 2, "PICK A FILE...")
	case .Volume:
		core.music_volume = core.music_volume >= 1 ? 0 : core.music_volume + 0.25
		play(core, .Level_Up)
	case .Seed:
		set_info(core, 0, fmt.tprintf("SEED %X", s.entropy()))
	}
}

import_text :: proc(core: ^Core, text: string) {
	if len(text) > 2 * 1024 * 1024 { set_info(core, 2, "FILE TOO BIG"); return }
	f: Slot_File
	if json.unmarshal(transmute([]u8)text, &f, .JSON) != nil || f.format != "spike" { set_info(core, 2, "NOT A VALID FILE"); play(core, .Miss); return }
	if !core.services.storage_set(SLOT_KEY, text) { set_info(core, 2, "STORE FAILED"); return }
	set_info(core, 2, fmt.tprintf("IMPORTED %d BYTES", len(text)))
	play(core, .Unlock_Door)
}

// ---- drawing into the 22 x 23 cell grid ---------------------------------------------------------
put_text :: proc(core: ^Core, col, row: int, text: string, inverted: bool, hue: Hue) {
	for i in 0 ..< len(text) {
		c := int(col) + i
		if c >= CELL_COLUMNS || row >= CELL_ROWS { return }
		g := u8(32)
		if ch := text[i]; ch < 128 && ascii_glyph[ch] != 255 { g = ascii_glyph[ch] }
		core.cells[row][c] = {g, hue, inverted}
	}
}

draw :: proc(core: ^Core) {
	for r in 0 ..< CELL_ROWS { for c in 0 ..< CELL_COLUMNS { core.cells[r][c] = {32, .Blue, false} } }
	put_text(core, 0, 0, "PLATFORM API SPIKE   ", true, .Blue)
	put_text(core, 0, 2, fmt.tprintf("LAST: %s", string(core.last[:core.last_len])), false, .Black)
	put_text(core, 0, 3, fmt.tprintf("COUNT: %d  FRAME: %d", core.counter, core.frames), false, .Black)
	put_text(core, 0, 4, fmt.tprintf("MUSIC %d%%  SFX %d%%", int(core.music_volume * 100), int(core.sfx_volume * 100)), false, .Purple)
	for item in Menu_Item {
		label := MENU_LABELS[item]
		inverted := int(item) == core.selected
		put_text(core, 0, MENU_FIRST_ROW + int(item), "                      ", inverted, .Orange)
		put_text(core, 0, MENU_FIRST_ROW + int(item), label, inverted, .Orange)
	}
	for i in 0 ..< 3 { put_text(core, 0, 15 + i, string(core.info[i][:core.info_len[i]]), false, .Green) }
}

// Port of Renderer.Update (SPLORR.UI): border hue Cyan, screen hue White, bit 0 is the leftmost pixel.
rasterize :: proc(core: ^Core) {
	border := PALETTE[.Cyan]
	paper := PALETTE[.White]
	for y in 0 ..< FRAME_HEIGHT {
		for x in 0 ..< FRAME_WIDTH {
			sx, sy := x - BORDER_X, y - BORDER_Y
			if sx < 0 || sy < 0 || sx >= CELL_COLUMNS * CELL_SIZE || sy >= CELL_ROWS * CELL_SIZE {
				core.frame[y * FRAME_WIDTH + x] = border
				continue
			}
			cell := core.cells[sy / CELL_SIZE][sx / CELL_SIZE]
			bit := (glyph_bitmaps[cell.glyph & 127][sy % CELL_SIZE] >> uint(sx % CELL_SIZE)) & 1 == 1
			core.frame[y * FRAME_WIDTH + x] = (bit != cell.inverted) ? PALETTE[cell.hue] : paper
		}
	}
}

_ :: strings
