#+build !js
package main

import "base:runtime"
import "core:strings"
import "core:log"
import "core:testing"

// A fake platform: in-memory storage and recorded calls. This is what the Services struct buys (task 15).
Fake :: struct {
	store:       map[string]string,
	downloads:   [dynamic][2]string,
	pick_calls:  int,
	logs:        int,
}
fake: Fake

fake_services :: proc() -> Services {
	fake = {}
	return {
		storage_get = proc(key: string, allocator: runtime.Allocator) -> (string, bool) { v, ok := fake.store[key]; return v, ok },
		// Strings passed to services are only borrowed for the call: a platform that keeps one must copy it.
		storage_set = proc(key, value: string) -> bool { fake.store[strings.clone(key)] = strings.clone(value); return true },
		storage_remove = proc(key: string) { delete_key(&fake.store, key) },
		download_text = proc(filename, text: string) { append(&fake.downloads, [2]string{strings.clone(filename), strings.clone(text)}) },
		request_file_pick = proc() { fake.pick_calls += 1 },
		entropy = proc() -> u64 { return 0xDEADBEEF },
		log = proc(message: string) { fake.logs += 1 },
	}
}

step :: proc(core: ^Core, events: ..Input_Event) -> Step_Output {
	out: Step_Output
	core_step(core, {dt = 0.016, events = events}, &out)
	return out
}
cmd :: proc(c: Command) -> Input_Event { return {kind = .Command, command = c} }
tap :: proc(col, row: i16) -> Input_Event { return {kind = .Tap, col = col, row = row, precise = true} }
finger :: proc(col, row: i16) -> Input_Event { return {kind = .Tap, col = col, row = row, precise = false} }

@(test)
confirm_runs_selected_item :: proc(t: ^testing.T) {
	core := new(Core); defer free(core)
	core_init(core, fake_services())
	out := step(core, cmd(.Confirm))
	testing.expect_value(t, core.counter, 1)
	testing.expect_value(t, out.sfx_count, 1)
	testing.expect_value(t, out.sfx[0], Sfx.Player_Hit)
	testing.expect(t, out.frame != nil && out.frame_changed)
}

@(test)
tap_on_menu_row_selects_and_activates :: proc(t: ^testing.T) {
	core := new(Core); defer free(core)
	core_init(core, fake_services())
	step(core, tap(3, MENU_FIRST_ROW + i16(Menu_Item.Seed)))
	testing.expect_value(t, core.selected, int(Menu_Item.Seed))
	step(core, tap(30, MENU_FIRST_ROW)) // outside the grid horizontally: ignored
	step(core, tap(-1, MENU_FIRST_ROW)) // in the left border: ignored
	testing.expect_value(t, core.selected, int(Menu_Item.Seed))
	testing.expect_value(t, core.counter, 0)
}

@(test)
save_load_export_import_round_trip :: proc(t: ^testing.T) {
	core := new(Core); defer free(core)
	core_init(core, fake_services())
	step(core, cmd(.Confirm), cmd(.Confirm), cmd(.Confirm)) // counter = 3
	step(core, tap(0, MENU_FIRST_ROW + i16(Menu_Item.Save)))
	testing.expect_value(t, fake.store[SLOT_KEY], `{"format":"spike","counter":3}`)
	step(core, tap(0, MENU_FIRST_ROW + i16(Menu_Item.Export)))
	testing.expect_value(t, len(fake.downloads), 1)
	testing.expect_value(t, fake.downloads[0][0], "kordanors-cabal-slot1.json")
	// importing a file replaces the slot; loading reads it back
	step(core, tap(0, MENU_FIRST_ROW + i16(Menu_Item.Import)))
	testing.expect_value(t, fake.pick_calls, 1)
	step(core, Input_Event{kind = .File_Text, text = `{"format":"spike","counter":41}`})
	step(core, tap(0, MENU_FIRST_ROW + i16(Menu_Item.Load)))
	testing.expect_value(t, core.counter, 41)
}

@(test)
bad_imports_leave_the_slot_untouched :: proc(t: ^testing.T) {
	core := new(Core); defer free(core)
	core_init(core, fake_services())
	fake.store[SLOT_KEY] = strings.clone(`{"format":"spike","counter":7}`)
	step(core, Input_Event{kind = .File_Text, text = `not json`})
	step(core, Input_Event{kind = .File_Text, text = `{"format":"other","counter":1}`})
	step(core, Input_Event{kind = .File_Cancelled})
	testing.expect_value(t, fake.store[SLOT_KEY], `{"format":"spike","counter":7}`)
}

@(test)
frame_geometry_and_colors :: proc(t: ^testing.T) {
	core := new(Core); defer free(core)
	core_init(core, fake_services())
	out := step(core)
	f := out.frame
	testing.expect_value(t, f[0], PALETTE[.Cyan])                                   // border
	testing.expect_value(t, f[(FRAME_HEIGHT - 1) * FRAME_WIDTH + FRAME_WIDTH - 1], PALETTE[.Cyan])
	// the title row is drawn inverted blue-on-... : cell (0,0) uses the space glyph inverted => solid blue
	testing.expect_value(t, f[BORDER_Y * FRAME_WIDTH + BORDER_X], PALETTE[.Blue])
	// the body background cells are space glyphs, not inverted => paper (white)
	testing.expect_value(t, f[(BORDER_Y + 20 * CELL_SIZE) * FRAME_WIDTH + BORDER_X], PALETTE[.White])
}

@(test)
volume_cycles_and_is_reported :: proc(t: ^testing.T) {
	core := new(Core); defer free(core)
	core_init(core, fake_services())
	out := step(core)
	testing.expect_value(t, out.music_volume, 0.5)
	step(core, tap(0, MENU_FIRST_ROW + i16(Menu_Item.Volume)))
	out = step(core)
	testing.expect_value(t, out.music_volume, 0.75)
}

@(test)
finger_taps_select_first_then_confirm :: proc(t: ^testing.T) {
	core := new(Core); defer free(core)
	core_init(core, fake_services())
	step(core, finger(3, MENU_FIRST_ROW + i16(Menu_Item.Count)))   // already selected by default: confirms
	testing.expect_value(t, core.counter, 1)
	step(core, finger(3, MENU_FIRST_ROW + i16(Menu_Item.Seed)))    // a different row: only selects
	testing.expect_value(t, core.selected, int(Menu_Item.Seed))
	testing.expect_value(t, core.counter, 1)
	step(core, finger(3, MENU_FIRST_ROW + i16(Menu_Item.Count)))   // moves back, still no action
	testing.expect_value(t, core.counter, 1)
	step(core, finger(3, MENU_FIRST_ROW + i16(Menu_Item.Count)))   // second tap on the selected row confirms
	testing.expect_value(t, core.counter, 2)
}

// Compare the Odin rasterizer with the original VB renderer, bit for bit, on every recorded VB screen (task 23).
import "core:os"
import "core:strconv"

REFERENCE_DIR :: "../../docs/reference/vb/"

fnv_frame :: proc(frame: ^[FRAME_WIDTH * FRAME_HEIGHT]u32) -> u64 {
	h: u64 = 14695981039346656037
	for p in frame {
		for b in ([3]u8{u8(p), u8(p >> 8), u8(p >> 16)}) { h = (h ~ u64(b)) * 1099511628211 }
	}
	return h
}

@(test)
rasterizer_matches_the_vb_renderer_on_recorded_screens :: proc(t: ^testing.T) {
	log_data, log_err := os.read_entire_file(REFERENCE_DIR + "_log.txt", context.temp_allocator)
	testing.expect(t, log_err == nil, "reference log missing: run tools/vb-oracle")
	core := new(Core); defer free(core)
	checked := 0
	for line in strings.split_lines(string(log_data), context.temp_allocator) {
		if !strings.has_prefix(line, "snap ") { continue }
		name := strings.split(line[5:], " ", context.temp_allocator)[0]
		cells_data, e1 := os.read_entire_file(strings.concatenate({REFERENCE_DIR, name, ".cells"}, context.temp_allocator), context.temp_allocator)
		hash_data, e2 := os.read_entire_file(strings.concatenate({REFERENCE_DIR, name, ".hash"}, context.temp_allocator), context.temp_allocator)
		if e1 != nil || e2 != nil { testing.fail_now(t, name) }
		rows := strings.split_lines(strings.trim_space(string(cells_data)), context.temp_allocator)
		testing.expect_value(t, len(rows), CELL_ROWS)
		for r in 0 ..< CELL_ROWS {
			cells := strings.fields(rows[r], context.temp_allocator)
			for c in 0 ..< CELL_COLUMNS {
				parts := strings.split(cells[c], ".", context.temp_allocator)
				g, _ := strconv.parse_int(parts[0]); h, _ := strconv.parse_int(parts[1]); inv, _ := strconv.parse_int(parts[2])
				core.cells[r][c] = {u8(g), Hue(h), inv == 1}
			}
		}
		rasterize(core)
		want, _ := strconv.parse_u64_of_base(strings.trim_space(string(hash_data)), 16)
		got := fnv_frame(&core.frame)
		if got != want { testing.fail_now(t, name) }
		checked += 1
	}
	testing.expect(t, checked >= 50, "too few reference screens")
	testing.expect_value(t, checked, checked)
	log.infof("%d recorded VB screens render identically", checked)
}
