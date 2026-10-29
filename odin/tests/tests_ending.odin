package main

import "kc:game"

// Fights Zooperdan to the end with a hand-picked player; returns the ending screen state.
finale_setup :: proc(seed: u64, notes: int) -> ^game.Core {
	c := town_core(seed)
	w := &c.world
	who := w.player.character
	game.give_new_item(w, who, .Horns_of_Kordanor)
	for i in 0 ..< notes { w.player.lores_read |= u32(1) << u32(i) }
	return c
}

test_ending_handed :: proc(t: ^T) {
	defer fake_reset()
	c := finale_setup(31, 0); defer free_core(c)
	w := &c.world
	who := w.player.character
	// without enough notes the horns can only be handed over
	talk_to(c, .Zooperdan_the_Elder)
	expect(t, game.can_hand_over_horns(w, who), "the horns can be handed over")
	expect(t, !game.can_refuse_horns(w, who), "but not kept, with no notes read")
	button(c, game.ELDER_REFUSE)
	expect_eq(t, c.state, game.UI_State.In_Play)
	expect_eq(t, w.player.finale, game.Finale.None)

	button(c, game.ELDER_HORNS)
	expect_eq(t, c.state, game.UI_State.Message)
	for c.state == .Message { press(c, .Confirm) }
	expect_eq(t, w.player.finale, game.Finale.Handed)
	expect(t, !game.has_item_type(w, who, .Horns_of_Kordanor), "Zooperdan took the horns")
	enemies := game.enemies_of(w, who)
	expect_eq(t, len(enemies), 1)
	boss := enemies[0]
	expect_eq(t, game.character_get(w, boss).type, game.Character_Type.Zooperdan)
	expect_eq(t, game.stat_of(w, boss, .HP), i32(14))

	// no shops, no running
	titles := game.play_buttons(c)
	expect(t, titles[game.NEUTRAL_INTERACT] == "", "no talking to townspeople during the fight")
	w.player.facing = .East
	for _ in 0 ..< 10 {
		game.player_character(w).stats[.Wounds] = 0
		game.run(w, who)
		for game.message_head(w) != nil { game.message_pop(w) }
	}
	expect_eq(t, game.character_get(w, who).location, game.character_get(w, boss).location)

	// at half HP he changes and gains mental attacks
	game.character_get(w, boss).stats[.Wounds] = 7
	game.counter_attacks(w, who)
	expect_eq(t, game.character_get(w, boss).type, game.Character_Type.Zooperdan_Unbound)
	for game.message_head(w) != nil { game.message_pop(w) }

	// a demoralized player dies to him instead of waking up
	game.player_character(w).stats[.Wounds] = 0
	pc := game.player_character(w)
	pc.stats[.Willpower], pc.stats[.MP], pc.stats[.Stress] = 1, 1, 0
	for _ in 0 ..< 200 { if !game.is_dead(w, who) { game.mental_counter_attack(w, who, boss, "x") }; for game.message_head(w) != nil { game.message_pop(w) } }
	expect(t, game.is_dead(w, who), "his mind attacks end the fight for a broken player")

	// killing him ends the game with the ending for the answer given
	game.player_character(w).stats[.Wounds] = 0
	game.character_get(w, boss).stats[.Wounds] = 13
	pc.stats[.Strength] = 40
	for c.state != .The_End && !game.is_dead(w, who) {
		c.state = .In_Play
		game.fight_action(c, .Fight)
		for c.state == .Message { press(c, .Confirm) }
		if c.state == .The_End { break } // the world is gone
		game.player_character(w).stats[.Wounds] = 0
	}
	expect_eq(t, c.state, game.UI_State.The_End)
	expect_eq(t, c.ending, game.Finale.Handed)
	for _ in 0 ..< 5 { if c.state == .The_End { press(c, .Confirm) } }
	expect_eq(t, c.state, game.UI_State.Title)
}

test_ending_refused :: proc(t: ^T) {
	defer fake_reset()
	c := finale_setup(32, game.LORES_FOR_REFUSAL); defer free_core(c)
	w := &c.world
	who := w.player.character
	talk_to(c, .Zooperdan_the_Elder)
	expect(t, game.can_refuse_horns(w, who), "enough notes read: the horns can be kept")
	button(c, game.ELDER_REFUSE)
	for c.state == .Message { press(c, .Confirm) }
	expect_eq(t, w.player.finale, game.Finale.Refused)
	expect(t, game.has_item_type(w, who, .Horns_of_Kordanor), "the player keeps the horns")
	boss := game.enemies_of(w, who)[0]
	expect_eq(t, game.stat_of(w, boss, .HP), i32(10))
	expect_eq(t, game.stat_of(w, boss, .Strength), i32(7))

	// the fight survives a save and load
	data, ok := game.world_save(w)
	expect(t, ok == nil, "saves")
	defer delete(data)
	var: game.World
	err, _ := game.world_load(&var, data, context.temp_allocator)
	expect(t, err == .None, "loads")
	expect_eq(t, var.player.finale, game.Finale.Refused)
	expect_eq(t, var.player.lores_read, w.player.lores_read)
	game.world_destroy(&var)
}
