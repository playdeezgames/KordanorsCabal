package main

import "core:fmt"
import "kc:game"

// The player of the recorded fights: STR 7, DEX 2, INF 1, WIL 2, POW 1, HP 3, MP 3, Mana 1, with the dagger wielded.
fighter_core :: proc(seed: u64) -> ^game.Core {
	c := dungeon_core()
	w := &c.world
	w.rng = {}
	game.rng_seed(&w.rng, seed)
	p := game.player_character(w)
	p.stats[.Dexterity], p.stats[.Willpower], p.stats[.Power], p.stats[.HP], p.stats[.MP], p.stats[.Mana] = 2, 2, 1, 3, 3, 1
	dagger, _ := game.carried_item_of_type(w, w.player.character, .Dagger)
	game.item_put_on(w, dagger, w.player.character, .Weapon)
	return c
}

screens_match :: proc(c: ^game.Core, name: string) -> bool {
	ref := reference(name)
	want := to_screen(&ref.cells)
	for row in 0 ..< game.CELL_ROWS { for col in 0 ..< game.CELL_COLUMNS { if !same_look(want[row][col], c.screen[row][col]) { return false } } }
	return true
}

// The dice cannot be set, so the recorded fights are found by trying seeds until the same rolls come up; the check is that the
// text, the line breaks and the follow-up screens are exactly the recorded ones.
test_combat_golden :: proc(t: ^T) {
	defer fake_reset()
	found_fight, found_death := false, false
	for seed in u64(1) ..= 40000 {
		if found_fight && found_death { break }
		if !found_fight {
			c := fighter_core(seed)
			button(c, game.NEUTRAL_TURN_FIGHT)
			step(c)
			if screens_match(c, "50-fight-message") {
				press(c, .Confirm) // the next message: the goblin that is left hits back
				step(c)
				if screens_match(c, "51-after-fight") {
					found_fight = true
					fmt.printf("    (fight screens 50 and 51 reproduced with seed %d)\n", seed)
				}
			}
			free_core(c)
		}
		if !found_death {
			c := fighter_core(seed)
			game.character_get(&c.world, c.world.player.character).stats[.Wounds] = 99
			button(c, game.NEUTRAL_TURN_FIGHT)
			step(c)
			if screens_match(c, "70-fight-while-dying") {
				press(c, .Confirm) // the second message is skipped: the dead do not get hit again
				expect_eq(t, c.state, game.UI_State.Dead)
				step(c)
				expect_like_vb(t, c, "71-after-first-message")
				press(c, .Confirm)
				expect_eq(t, c.state, game.UI_State.Title)
				found_death = true
			}
			free_core(c)
		}
	}
	expect(t, found_fight, "the recorded fight (screens 50, 51) was reproduced")
	expect(t, found_death, "the recorded death (70, 71, 72) was reproduced")
}

test_combat_screens :: proc(t: ^T) {
	defer fake_reset()
	c := fighter_core(1); defer free_core(c)
	w := &c.world
	button(c, game.NEUTRAL_GROUND_ENEMIES)
	expect_eq(t, c.state, game.UI_State.Enemies)
	step(c)
	expect_like_vb(t, c, "41-enemies")
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.In_Play)

	// level up: the button, the page and the cancel
	game.player_character(w).stats[.Unassigned] = 1
	game.player_character(w).stats[.Strength], game.player_character(w).stats[.Dexterity] = 7, 2
	step(c)
	text := string(c.screen[21][0:8][0].glyph == u8(game.Glyph.L) ? "level" : "other")
	expect_eq(t, text, "level") // "Level up!" replaces Status
	button(c, game.NEUTRAL_STATUS)
	expect_eq(t, c.state, game.UI_State.Level_Up)
	step(c)
	expect_like_vb(t, c, "63-level-up")
	press(c, .Cancel)
	expect_eq(t, c.state, game.UI_State.In_Play)
	button(c, game.NEUTRAL_STATUS)
	pick(c, 1) // a point to strength: the last one ends the screen
	expect_eq(t, c.state, game.UI_State.In_Play)
	expect_eq(t, game.character_get(w, w.player.character).stats[.Strength], i32(8))
	expect_eq(t, game.character_get(w, w.player.character).stats[.Unassigned], i32(0))
}

test_combat_rules :: proc(t: ^T) {
	defer fake_reset()
	c := fighter_core(77); defer free_core(c)
	w := &c.world
	player := w.player.character
	me := game.character_get(w, player)

	// experience: a level doubles the goal, gives a point and heals
	me.stats[.XP], me.stats[.XP_Goal], me.stats[.Wounds] = 9, 10, 2
	expect(t, game.add_xp(w, player, 1), "level")
	expect_eq(t, me.stats[.XP], i32(0))
	expect_eq(t, me.stats[.XP_Goal], i32(20))
	expect_eq(t, me.stats[.Unassigned], i32(1))
	expect_eq(t, me.stats[.Wounds], i32(0))
	expect(t, !game.add_xp(w, player, 3), "no level")

	// damage limits: a dagger allows 1, a dagger and chainmail 3, nothing worn the unarmed limit
	expect_eq(t, game.maximum_damage(w, player), i32(1))
	mail := game.give_new_item(w, player, .Chainmail)
	game.equip_item(w, player, mail)
	expect_eq(t, game.maximum_damage(w, player), i32(3))
	expect_eq(t, game.determine_damage(w, player, 9), i32(3))
	expect_eq(t, game.determine_damage(w, player, -4), i32(0))
	expect_eq(t, game.attack_dice(w, player), i32(7 + 2)) // strength plus the dagger
	expect_eq(t, game.defend_dice(w, player), i32(2 + 2)) // dexterity plus the chainmail
	expect_eq(t, game.negative_influence(w, player), i32(0))
	me.stats[.Drunkenness] = 1
	expect_eq(t, game.negative_influence(w, player), i32(-1))
	me.stats[.Drunkenness] = 0

	// dice: successes on sixes, about one in six
	hits := 0
	for _ in 0 ..< 6000 { hits += int(game.roll_successes(w, 1)) }
	expect(t, hits > 850 && hits < 1150, "a die succeeds one time in six")
	expect_eq(t, game.roll_successes(w, -3), i32(0))

	// wear: the dagger breaks after ten points of wear, and says so
	broken := game.wear_weapons(w, player, 10)
	expect_eq(t, len(broken), 1)
	expect(t, len(broken) == 1 && broken[0] == .Dagger, "the dagger broke")
	_, still := game.item_in_slot(w, player, .Weapon)
	expect(t, !still, "and is gone")

	// killing: the goblin's pack and loot fall to the floor, the killer is paid
	enemy, _ := game.first_enemy(w, player)
	game.give_new_item(w, enemy, .Potion)
	money := me.stats[.Money]
	here := me.location
	lines := make([dynamic]string, context.temp_allocator)
	sfx := game.kill(w, player, enemy, &lines)
	expect_eq(t, sfx, game.Sfx.Enemy_Death)
	expect(t, game.character_get(w, enemy) == nil, "the goblin is gone")
	expect(t, lines[0] == "You kill Goblin!", "the first line")
	expect(t, me.stats[.Money] > money, "paid")
	on_floor := game.items_on_ground(w, here)
	has_potion := false
	for id in on_floor { if game.item_get(w, id).type == .Potion { has_potion = true } }
	expect(t, has_potion, "the dead goblin's potion lies on the floor")

	// panic: everything is dropped, the money halved, the player wakes in the square
	town := game.create_location(w, .Town_Square)
	_ = town
	me.stats[.Money] = 11
	game.panic_flight(w, player)
	expect_eq(t, me.stats[.Money], i32(5))
	expect_eq(t, game.location_get(w, me.location).type, game.Location_Type.Town_Square)
	expect_eq(t, len(game.items_in_pack(w, player)), 0)
	expect_eq(t, len(game.items_worn(w, player)), 0)
	ok, why := game.world_validate(w)
	expect(t, ok, why)
}

test_combat_actions :: proc(t: ^T) {
	defer fake_reset()
	// fighting many times: the world stays valid and the messages are well formed
	for seed in u64(1) ..= 60 {
		c := fighter_core(seed); defer free_core(c)
		w := &c.world
		for round in 0 ..< 40 {
			if game.is_dead(w, w.player.character) || !game.can_fight(w, w.player.character) { break }
			game.fight(w, w.player.character)
			for w.messages.count > 0 { game.message_pop(w) }
			ok, why := game.world_validate(w)
			expect(t, ok, why)
		}
	}

	// running either gets away (and turns the player) or is punished with a counter attack
	ran, failed := 0, 0
	for seed in u64(1) ..= 200 {
		c := fighter_core(seed); defer free_core(c)
		w := &c.world
		here := game.character_get(w, w.player.character).location
		game.run(w, w.player.character)
		m := game.message_head(w)
		if string(m.text[:m.len]) == "You successfully ran!" {
			ran += 1
			expect(t, game.character_get(w, w.player.character).location != here, "moved")
		} else {
			failed += 1
			expect_eq(t, string(m.text[:m.len]), "You fail to run!")
		}
	}
	expect(t, ran > 0 && failed > 0, "both outcomes happen (two of four directions are open)")

	// intimidation needs a lone enemy
	c := fighter_core(5); defer free_core(c)
	w := &c.world
	player := w.player.character
	expect(t, !game.can_do_intimidation(w, player), "two goblins back each other up")
	enemy, _ := game.first_enemy(w, player)
	game.destroy_character(w, enemy)
	expect(t, game.can_do_intimidation(w, player), "one goblin can be frightened")
	game.character_get(w, player).stats[.Influence] = 20 // overwhelming
	game.character_get(w, game.first_enemy_id(w, player)).stats[.MP] = 1
	game.do_intimidation(w, player) // an overwhelming roll (20 dice against 2) wins with near certainty and demoralizes a 1 MP goblin
	m := game.message_head(w)
	expect(t, m != nil, "the roll is reported")

	// the earth shard immobilizes: the next counter attack only says so
	c2 := fighter_core(9); defer free_core(c2)
	w2 := &c2.world
	game.character_get(w2, w2.player.character).stats[.Power] = 40
	foe, _ := game.first_enemy(w2, w2.player.character)
	game.stat_add(game.character_get(w2, foe), .Immobilization, 2)
	for w2.messages.count > 0 { game.message_pop(w2) }
	game.counter_attacks(w2, w2.player.character)
	head := game.message_head(w2)
	expect(t, head != nil && string(head.text[:head.len]) == "Counter-attack 1/2:\nGoblin is immobilized!", "immobilized enemies skip their turn")
	expect_eq(t, game.character_get(w2, foe).stats[.Immobilization], i32(1))
}
