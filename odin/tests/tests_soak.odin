package main

import "core:fmt"
import "kc:game"

// Random play: thousands of random commands and taps from the title screen on, with the world checked after every step. Nothing
// may crash, trip an assert, leak, or leave the world invalid. Saves are exercised too, through the fake storage.
soak_run :: proc(t: ^T, seed: u64, steps: int) {
	c := make_core()
	defer free_core(c)
	r: game.Rng
	game.rng_seed(&r, seed)
	fake.entropy = seed
	reached: [game.UI_State]int
	commands := [?]game.Command{.Up, .Down, .Left, .Right, .Confirm, .Confirm, .Confirm, .Cancel}
	for i in 0 ..< steps {
		reached[c.state] += 1
		event: game.Input_Event
		if c.has_world && i % 40 == 0 { // help the random player along so that it reaches the deeper screens
			w := &c.world
			me := game.player_character(w)
			me.stats[.Wounds], me.stats[.Stress], me.stats[.Fatigue], me.stats[.Hunger] = 0, 0, 0, 0
			me.stats[.Money] += 100
			type := game.Item_Type(1 + game.pick_index(&r, len(game.Item_Type) - 1))
			game.give_new_item(w, w.player.character, type)
			w.player.spells[game.Spell_Type(1 + game.pick_index(&r, 2))] = 1
			if i % 120 == 0 && c.state == .In_Play { // and visit somewhere else: the dungeon, the moon, the cellar
				there := w.location_order[game.pick_index(&r, len(w.location_order))]
				game.move_to(w, w.player.character, there)
				w.player.mode = .Neutral
			}
		}
		if c.state == .In_Play && game.pick_index(&r, 10) < 7 {
			c.button = game.pick_index(&r, game.BUTTON_COUNT)
			event = {kind = .Command, command = .Confirm}
		} else if game.pick_index(&r, 10) == 0 {
			event = {kind = .Tap, col = i16(game.pick_index(&r, 24)) - 1, row = i16(game.pick_index(&r, 25)) - 1, precise = game.pick_index(&r, 2) == 0}
		} else {
			event = {kind = .Command, command = commands[game.pick_index(&r, len(commands))]}
		}
		// Do not quit; the picker answers with a bad file.
		out := step(c, event)
		if c.state == .Import_Wait { step(c, {kind = .File_Text, text = "not a save"}) }
		if out.quit_requested { c.quit = false; game.enter_state(c, .Title) }
		if c.has_world && (c.state == .In_Play || i % 25 == 0) {
			ok, why := game.world_validate(&c.world)
			if !ok { expect(t, false, fmt.tprintf("seed %d step %d: %s", seed, i, why)); return }
		}
		if c.state == .Title && game.pick_index(&r, 6) == 0 && c.has_world { game.abandon_world(c) } // keep memory bounded
	}
	covered := 0
	for n in reached { if n > 0 { covered += 1 } }
	expect(t, covered >= 14, "the random player reached a good share of the screens")
	t.checks += 0
	fmt.printf("    (seed %d: %d of %d screens visited; never: ", seed, covered, len(game.UI_State))
	for n, st in reached { if n == 0 { fmt.printf("%v ", st) } }
	fmt.println(")")
}

test_soak :: proc(t: ^T) {
	defer fake_reset()
	for seed in u64(1) ..= 2 { soak_run(t, seed, 1500) }
}
