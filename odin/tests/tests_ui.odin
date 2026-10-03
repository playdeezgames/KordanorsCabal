package main

import "base:runtime"
import "core:fmt"
import "core:strings"
import "kc:game"

// ---- fake services: an in-memory store and recorders ------------------------------------------------------------------

Fake :: struct {
	store:     map[string]string,
	downloads: [dynamic][2]string,
	picks:     int,
	entropy:   u64,
}
fake: Fake

fake_get :: proc(key: string, allocator: runtime.Allocator) -> (string, bool) {
	v, ok := fake.store[key]
	if !ok { return "", false }
	return strings.clone(v, allocator), true
}
fake_set :: proc(key, value: string) -> bool {
	if old_key, old_value := delete_key(&fake.store, key); old_key != "" { delete(old_key); delete(old_value) }
	fake.store[strings.clone(key)] = strings.clone(value)
	return true
}
fake_remove :: proc(key: string) {
	if old_key, old_value := delete_key(&fake.store, key); old_key != "" { delete(old_key); delete(old_value) }
}
fake_download :: proc(filename, text: string) { append(&fake.downloads, [2]string{strings.clone(filename), strings.clone(text)}) }
fake_pick :: proc() { fake.picks += 1 }
fake_entropy :: proc() -> u64 { return fake.entropy }
fake_log :: proc(message: string) {}

fake_services :: proc() -> game.Services {
	return {fake_get, fake_set, fake_remove, fake_download, fake_pick, fake_entropy, fake_log}
}
fake_reset :: proc() {
	for k, v in fake.store { delete(k); delete(v) }
	delete(fake.store)
	for d in fake.downloads { delete(d[0]); delete(d[1]) }
	delete(fake.downloads)
	fake = {entropy = 424242}
}

TITLE_ROW :: 14

make_core :: proc() -> ^game.Core {
	c := new(game.Core)
	game.core_init(c, fake_services())
	return c
}
free_core :: proc(c: ^game.Core) {
	game.core_destroy(c)
	free(c)
}
step :: proc(c: ^game.Core, events: ..game.Input_Event) -> game.Step_Output {
	out: game.Step_Output
	game.core_step(c, {dt = 0.016, events = events}, &out)
	return out
}
press :: proc(c: ^game.Core, commands: ..game.Command) {
	for cmd in commands { step(c, {kind = .Command, command = cmd}) }
}
// Moves the menu cursor to `index` and confirms.
pick :: proc(c: ^game.Core, index: int) {
	m, _ := game.menu_of(c, c.state)
	_ = m
	state := c.state
	for tries := 0; c.cursors[c.state] != index && tries < 20; tries += 1 { press(c, .Down) }
	assert(c.state == state, "pick: the screen changed while moving the cursor")
	press(c, .Confirm)
}
tap :: proc(c: ^game.Core, col, row: int, precise: bool) {
	step(c, {kind = .Tap, col = i16(col), row = i16(row), precise = precise})
}

// From the in-play screen to the game menu: select the last button and press it.
game_menu :: proc(c: ^game.Core) {
	assert(c.state == .In_Play)
	for tries := 0; c.button != game.NEUTRAL_MENU && tries < 12; tries += 1 { press(c, .Down) }
	press(c, .Confirm)
}

// Cells that look the same are equal: an empty, non-inverted cell shows the paper whatever its hue.
same_look :: proc(a, b: game.Cell) -> bool {
	if a.glyph == game.GLYPH_SPACE && !a.inverted && b.glyph == game.GLYPH_SPACE && !b.inverted { return true }
	return a == b
}

reference :: proc(name: string) -> ^Reference_Screen {
	for &r in REFERENCE_SCREENS { if r.name == name { return &r } }
	return nil
}

// Compares the screen with a recorded VB frame, ignoring the listed rows (where the port deliberately differs).
expect_like_vb :: proc(t: ^T, c: ^game.Core, name: string, skip: ..int, loc := #caller_location) {
	ref := reference(name)
	if ref == nil { expect(t, false, name, loc); return }
	want := to_screen(&ref.cells)
	bad := 0
	for row in 0 ..< game.CELL_ROWS {
		skipped := false
		for r in skip { if r == row { skipped = true } }
		if skipped { continue }
		differs := false
		for col in 0 ..< game.CELL_COLUMNS { if !same_look(want[row][col], c.screen[row][col]) { differs = true } }
		if differs {
			bad += 1
			fmt.printf("    row %d of %s differs\n", row, name)
			for col in 0 ..< game.CELL_COLUMNS { if !same_look(want[row][col], c.screen[row][col]) { fmt.printf("      col %d: want %v got %v\n", col, want[row][col], c.screen[row][col]) } }
		}
	}
	expect_eq(t, bad, 0, loc)
}

// Slots 1 and 2 hold a game (what the reference frames show); only the label text matters to the screens.
put_fake_saves :: proc(c: ^game.Core) {
	w: game.World
	game.world_init(&w, 5)
	assert(game.world_generate(&w))
	data, _ := game.world_save(&w)
	fake_set("kc:slot1", string(data))
	fake_set("kc:slot2", string(data))
	delete(data)
	game.world_destroy(&w)
}

test_ui_golden :: proc(t: ^T) {
	defer fake_reset()
	c := make_core(); defer free_core(c)
	put_fake_saves(c)
	step(c)
	// the title: the original six lines are unchanged; the port adds "Start with seed..." in the menu
	expect_like_vb(t, c, "01-title", 14, 15, 16, 17, 18, 19, 20)
	pick(c, 3)
	expect_like_vb(t, c, "02-instructions")
	press(c, .Cancel)
	pick(c, 5)
	expect_like_vb(t, c, "03-about", 20, 21, 22) // the last lines now point at the credits screen
	press(c, .Cancel)
	pick(c, 4)
	expect_like_vb(t, c, "04-options", 6, 7, 8, 9) // no Screen Size item any more
	pick(c, 1)
	expect_like_vb(t, c, "06-options-sfx-volume")
	press(c, .Cancel)
	pick(c, 2)
	expect_like_vb(t, c, "07-options-mux-volume")
	press(c, .Cancel)
	pick(c, 0)
	press(c, .Down)
	pick(c, 6)
	expect_like_vb(t, c, "08-confirm-quit")
	press(c, .Cancel)
	pick(c, 2)
	expect_like_vb(t, c, "09-load-game", 11) // the extra "Import a file..." item
}

test_ui_title_input :: proc(t: ^T) {
	defer fake_reset()
	c := make_core(); defer free_core(c)
	step(c)
	title := c.state
	press(c, .Down)
	expect_eq(t, c.cursors[.Title], 1)
	press(c, .Up, .Up)
	expect_eq(t, c.cursors[.Title], 6) // wraps
	tap(c, 5, TITLE_ROW + 4, false) // a finger tap selects first
	expect_eq(t, c.cursors[.Title], 4)
	expect_eq(t, c.state, title)
	tap(c, -1, TITLE_ROW, true) // the border is ignored
	tap(c, 3, 40, true)
	expect_eq(t, c.cursors[.Title], 4)
	expect_eq(t, c.state, title)
	tap(c, 5, TITLE_ROW + 4, false) // the second tap on the selected item confirms
	expect_eq(t, c.state, game.UI_State.Options)
	tap(c, 11, 5, true) // a mouse click selects and confirms at once: Go Back
	expect_eq(t, c.state, game.UI_State.Title)
	// quit needs two confirmations
	pick(c, 6)
	expect_eq(t, c.state, game.UI_State.Confirm_Quit)
	out := step(c)
	expect(t, !out.quit_requested, "not yet")
	pick(c, 1)
	out = step(c)
	expect(t, out.quit_requested, "quit after Yes")
}

test_ui_config :: proc(t: ^T) {
	defer fake_reset()
	c := make_core(); defer free_core(c)
	out := step(c)
	expect_eq(t, out.sfx_volume, f32(0.5))
	pick(c, 4); pick(c, 1) // Options, SFX Volume
	expect_eq(t, c.cursors[.Sfx_Volume], 5) // starts on the current value
	pick(c, 8)
	expect_eq(t, c.state, game.UI_State.Options)
	out = step(c)
	expect_eq(t, out.sfx_volume, f32(0.8))
	pick(c, 2); pick(c, 0)
	out = step(c)
	expect_eq(t, out.music_volume, f32(0))
	// a new core reads what was stored
	c2 := make_core(); defer free_core(c2)
	out = step(c2)
	expect_eq(t, out.sfx_volume, f32(0.8))
	expect_eq(t, out.music_volume, f32(0))
	// damaged config falls back to the defaults
	fake_set("kc:config", `{"sfx_volume": 7}`)
	c3 := make_core(); defer free_core(c3)
	out = step(c3)
	expect_eq(t, out.sfx_volume, f32(0.5))
}

// Plays through new-game screens; returns once the in-play placeholder shows.
begin_game :: proc(c: ^game.Core, with_seed := false) {
	if with_seed { pick(c, 1); press(c, .Confirm) } else { pick(c, 0) }
	if c.state == .Finalize_Character {
		for c.state == .Finalize_Character { pick(c, 1) } // put every point into Strength
	}
}

test_ui_new_game :: proc(t: ^T) {
	defer fake_reset()
	c := make_core(); defer free_core(c)
	step(c)
	out: game.Step_Output
	press(c, .Confirm) // Start
	expect(t, c.has_world, "the world exists")
	expect_eq(t, c.world.seed, u64(424242))
	p := game.player_character(&c.world)
	expect(t, p != nil, "player")
	if c.state == .Finalize_Character {
		// match the recorded screen: set the stats it showed (Unassigned 3, STR 4, DEX 2, INF 1, WIL 2, POW 1, HP 3, MP 3, Mana 1)
		p.stats[.Strength], p.stats[.Dexterity], p.stats[.Influence], p.stats[.Willpower] = 4, 2, 1, 2
		p.stats[.Power], p.stats[.HP], p.stats[.MP], p.stats[.Mana], p.stats[.Unassigned] = 1, 3, 3, 1, 3
		step(c)
		expect_like_vb(t, c, "09b-finalize-character")
		before := p.stats[.Strength] + p.stats[.Dexterity] + p.stats[.Influence] + p.stats[.Willpower] + p.stats[.Power] + p.stats[.HP] + p.stats[.MP] + p.stats[.Mana]
		pick(c, 1)
		expect_eq(t, p.stats[.Unassigned], i32(2))
		pick(c, 6)
		pick(c, 8)
		expect_eq(t, c.state, game.UI_State.Prolog) // the last point moves on
		after := p.stats[.Strength] + p.stats[.Dexterity] + p.stats[.Influence] + p.stats[.Willpower] + p.stats[.Power] + p.stats[.HP] + p.stats[.MP] + p.stats[.Mana]
		expect_eq(t, after, before + 3)
	} else {
		expect_eq(t, c.state, game.UI_State.Prolog) // the rolls used every point
	}
	out = step(c)
	expect(t, out.sfx_count == 0, "the character-creation sound plays once, on the step it happened")
	expect_like_vb(t, c, "10-prolog")
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.In_Play)
	game_menu(c)
	expect_like_vb(t, c, "15-game-menu")
}

test_ui_seed :: proc(t: ^T) {
	defer fake_reset()
	worlds: [2]u64
	for round in 0 ..< 2 {
		c := make_core()
		step(c)
		pick(c, 1)
		expect_eq(t, c.state, game.UI_State.Seed_Entry)
		// type 000000042: move to the last digit, 2 up, previous digit, 4 up
		press(c, .Left, .Up, .Up, .Left, .Up, .Up, .Up, .Up)
		expect_eq(t, game.seed_value(c), u64(42))
		press(c, .Confirm)
		expect_eq(t, c.world.seed, u64(42))
		worlds[round] = u64(c.world.player.character[0]) | u64(c.world.player.character[1]) << 8 | u64(c.world.player.character[2]) << 16 | u64(c.world.player.character[3]) << 24
		free_core(c)
	}
	expect(t, worlds[0] == worlds[1], "the same typed seed gives the same player id")
	// Cancel goes back; Down wraps below zero
	c := make_core(); defer free_core(c)
	step(c)
	pick(c, 1)
	press(c, .Down)
	expect_eq(t, c.seed_digits[0], u8(9))
	press(c, .Cancel)
	expect_eq(t, c.state, game.UI_State.Title)
}

test_ui_save_load :: proc(t: ^T) {
	defer fake_reset()
	c := make_core(); defer free_core(c)
	step(c)
	pick(c, 0)
	for c.state == .Finalize_Character { pick(c, 1) }
	press(c, .Confirm) // the prolog
	game_menu(c)
	expect_eq(t, c.state, game.UI_State.Game_Menu)
	seed := c.world.seed
	pick(c, 1) // Save Game
	expect_eq(t, c.state, game.UI_State.Save_Game)
	pick(c, 3) // slot 3
	expect_eq(t, c.state, game.UI_State.In_Play)
	saved, ok := fake.store["kc:slot3"]
	expect(t, ok && len(saved) > 100000, "slot 3 holds a save")
	// abandon
	game_menu(c)
	pick(c, 2)
	expect_like_vb(t, c, "17-confirm-abandon-game")
	pick(c, 1)
	expect_eq(t, c.state, game.UI_State.Title)
	expect(t, !c.has_world, "abandoned")
	// the Continue screen shows slot 3 as used and the others as empty, and loads it
	pick(c, 2)
	m, _ := game.menu_of(c, .Load_Game)
	expect_eq(t, m.labels[3], "Slot 3")
	expect_eq(t, m.labels[1], "(empty)")
	pick(c, 1) // an empty slot says so and returns
	expect_eq(t, c.state, game.UI_State.Notice)
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Load_Game)
	pick(c, 3)
	expect_eq(t, c.state, game.UI_State.In_Play)
	expect_eq(t, c.world.seed, seed)
	// a damaged slot is reported, not crashed on
	fake_set("kc:slot4", `{"format":"kordanors-cabal-save","version":1,"summary":{"place":"Level_I","hp":1,"xp":0},"world":{}}`)
	game_menu(c)
	pick(c, 2); pick(c, 1) // abandon
	pick(c, 2) // Continue
	pick(c, 4)
	expect_eq(t, c.state, game.UI_State.Notice)
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Load_Game)
}

test_ui_export_import :: proc(t: ^T) {
	defer fake_reset()
	c := make_core(); defer free_core(c)
	put_fake_saves(c)
	step(c)
	// export slot 2 from the Save screen (reached through a game)
	pick(c, 0)
	for c.state == .Finalize_Character { pick(c, 1) }
	press(c, .Confirm)
	game_menu(c)
	pick(c, 1) // Save Game
	pick(c, 6) // Export a slot...
	expect_eq(t, c.state, game.UI_State.Export_Slot)
	pick(c, 2)
	expect_eq(t, len(fake.downloads), 1)
	if len(fake.downloads) == 1 {
		expect_eq(t, fake.downloads[0][0], "kordanors-cabal-slot2.json")
		expect(t, fake.downloads[0][1] == fake.store["kc:slot2"], "the download is the slot's text")
	}
	pick(c, 5) // an empty slot
	expect_eq(t, c.state, game.UI_State.Notice)
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Export_Slot)
	press(c, .Cancel, .Cancel) // back to the game menu, then abandon to reach the title
	pick(c, 2); pick(c, 1)
	// import: ask for a file
	pick(c, 2)
	pick(c, 6) // Import a file...
	expect_eq(t, c.state, game.UI_State.Import_Wait)
	expect_eq(t, fake.picks, 1)
	step(c, {kind = .File_Cancelled})
	expect_eq(t, c.state, game.UI_State.Load_Game)
	pick(c, 6)
	step(c, {kind = .File_Text, text = "this is not a save"})
	expect_eq(t, c.state, game.UI_State.Notice)
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Load_Game)
	pick(c, 6)
	good := fake.store["kc:slot1"]
	step(c, {kind = .File_Text, text = good})
	expect_eq(t, c.state, game.UI_State.Import_Slot)
	pick(c, 5)
	expect_eq(t, c.state, game.UI_State.Notice)
	expect(t, fake.store["kc:slot5"] == good, "the file is in slot 5")
	press(c, .Confirm)
	m, _ := game.menu_of(c, .Load_Game)
	expect_eq(t, m.labels[5], "Slot 5")
	pick(c, 5) // and it loads
	expect_eq(t, c.state, game.UI_State.In_Play)
}

test_ui_credits_notice :: proc(t: ^T) {
	defer fake_reset()
	c := make_core(); defer free_core(c)
	step(c)
	pick(c, 5)
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Credits)
	press(c, .Down, .Down, .Down, .Down, .Down, .Down, .Down, .Down, .Down, .Down, .Down, .Down)
	step(c)
	lines := game.credit_lines()
	expect(t, c.credits_top == max(len(lines) - game.CREDITS_ROWS, 0), "scroll is clamped to the text")
	press(c, .Up)
	expect(t, c.credits_top >= 0, "scroll up")
	press(c, .Cancel)
	expect_eq(t, c.state, game.UI_State.About)
	press(c, .Cancel)
	expect_eq(t, c.state, game.UI_State.Title)
}
