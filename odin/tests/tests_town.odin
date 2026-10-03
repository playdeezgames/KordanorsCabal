package main

import "core:fmt"
import "kc:game"

// Moves the player to the feature's place and talks to them (the Interact button), like the recorded scenes.
talk_to :: proc(c: ^game.Core, f: game.Feature_Type) {
	w := &c.world
	for id in w.location_order {
		if game.location_get(w, id).feature == f {
			game.player_character(w).location = id
			game.location_get(w, id).visited = true
		}
	}
	w.player.mode = .Neutral
	c.state = .In_Play
	game.reset_buttons(c)
	step(c)
	button(c, game.NEUTRAL_INTERACT)
	step(c)
}

test_town_golden :: proc(t: ^T) {
	defer fake_reset()
	c := town_core(21); defer free_core(c)
	scenes := [?]struct { f: game.Feature_Type, name: string }{
		{.Zooperdan_the_Elder, "21-elder"}, {.Graham_the_Innkeeper, "22-innkeeper"}, {.Yermom_the_Drunk, "23-town-drunk"},
		{.Sander_the_Chicken, "24-chicken"}, {.Honest_Dan, "25-black-market"}, {.Marcus_the_Black_Mage, "26-black-mage"},
		{.Samuli_the_Blacksmith, "27-blacksmith"}, {.Nihilist_Healer_Marten, "28-healer"}, {.David_the_Constable, "29-constable"},
	}
	for s in scenes {
		talk_to(c, s.f)
		expect_like_vb(t, c, s.name)
		press(c, .Cancel) // Red leaves the conversation
	}
	// the shoppe screens of the black mage with 50 money, a potion and a book in the pack
	talk_to(c, .Marcus_the_Black_Mage)
	game.player_character(&c.world).stats[.Money] = 50
	game.give_new_item(&c.world, c.world.player.character, .Potion)
	game.give_new_item(&c.world, c.world.player.character, .Book_of_Holy_Bolt)
	button(c, game.MAGE_OFFERS)
	step(c)
	expect_like_vb(t, c, "30-shoppe-offers")
	press(c, .Cancel)
	button(c, game.MAGE_PRICES)
	step(c)
	expect_like_vb(t, c, "31-shoppe-prices")
	press(c, .Cancel)
	button(c, game.MAGE_SELL)
	step(c)
	expect_like_vb(t, c, "32-shoppe-sell")
	press(c, .Cancel)
	button(c, game.MAGE_BUY)
	step(c)
	expect_like_vb(t, c, "33-shoppe-buy")
}

test_town_spells_golden :: proc(t: ^T) {
	defer fake_reset()
	c := town_core(3); defer free_core(c)
	w := &c.world
	w.player.spells[.Holy_Bolt] = 1
	button(c, game.NEUTRAL_SPELLS)
	expect_eq(t, c.state, game.UI_State.Spell_List)
	step(c)
	expect_like_vb(t, c, "67-spell-list")
	// no enemy here, so the bolt cannot be cast: the list shows it in red and casting says so
	expect_eq(t, c.screen[10][5].hue, game.Hue.Red)
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Message)
	step(c)
	m := game.message_head(w)
	expect_eq(t, string(m.text[:m.len]), "You cannot cast Holy Bolt now.")
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Spell_List)
}

test_town_shops :: proc(t: ^T) {
	defer fake_reset()
	c := town_core(8); defer free_core(c)
	w := &c.world
	who := w.player.character
	me := game.player_character(w)

	// buying: the blacksmith's dagger costs 5; only what you can afford is listed
	me.stats[.Money] = 12
	talk_to(c, .Samuli_the_Blacksmith)
	button(c, game.SMITH_BUY)
	rows := game.shoppe_list_rows(c)
	for label in rows.labels { expect(t, label != "Platemail(250)", "too dear to list") }
	expect(t, len(rows.labels) >= 2, "a dagger and a helmet at least")
	names := rows.labels[0]
	expect_eq(t, names, "Dagger(5)")
	press(c, .Confirm)
	expect_eq(t, me.stats[.Money], i32(7))
	expect_eq(t, len(game.items_in_pack(w, who)), 1)
	press(c, .Cancel)
	expect_eq(t, c.state, game.UI_State.In_Play)
	expect_eq(t, w.player.shoppe, game.Shoppe_Type.None)

	// selling: the smith pays 1 for a dagger, and the screen empties into the game
	button(c, game.SMITH_SELL)
	expect_eq(t, len(game.shoppe_list_rows(c).labels), 1)
	press(c, .Confirm)
	expect_eq(t, me.stats[.Money], i32(8))
	expect_eq(t, c.state, game.UI_State.In_Play)

	// repair: a half-worn dagger costs half of 2, a worn out shield 6 x 7/10 rounded up
	dagger, shield := game.give_new_item(w, who, .Dagger), game.give_new_item(w, who, .Shield)
	game.item_get(w, dagger).wear = 5
	game.item_get(w, shield).wear = 7
	expect_eq(t, game.repair_cost(game.item_get(w, dagger), .Blacksmith), i32(1))
	expect_eq(t, game.repair_cost(game.item_get(w, shield), .Blacksmith), i32(5)) // 7 * 6 / 10 = 4.2, up to 5
	expect_eq(t, len(game.items_to_repair(w, who, .Blacksmith)), 2)
	me.stats[.Money] = 3
	button(c, game.SMITH_REPAIR)
	expect_eq(t, c.state, game.UI_State.Shoppe_Repair)
	press(c, .Down) // the shield costs 5: too much, nothing happens
	press(c, .Confirm)
	expect_eq(t, me.stats[.Money], i32(3))
	expect_eq(t, c.state, game.UI_State.Shoppe_Repair)
	press(c, .Up)
	press(c, .Confirm) // the dagger for 1
	expect_eq(t, me.stats[.Money], i32(2))
	expect_eq(t, game.item_get(w, dagger).wear, i32(0))
	press(c, .Cancel)

	// the healer
	me.stats[.Wounds] = 2
	talk_to(c, .Nihilist_Healer_Marten)
	button(c, game.HEALER_HEAL)
	expect_eq(t, me.stats[.Wounds], i32(0))

	// the black mage restores mana
	me.stats[.Mana], me.stats[.Fatigue] = 3, 2
	talk_to(c, .Marcus_the_Black_Mage)
	button(c, game.MAGE_RESTORE)
	expect_eq(t, c.state, game.UI_State.Message)
	expect_eq(t, game.mana_current(w, who), i32(3))
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.In_Play)

	// the elder's pep talk lifts a broken spirit to 1 MP
	me.stats[.MP], me.stats[.Stress] = 3, 3
	talk_to(c, .Zooperdan_the_Elder)
	button(c, game.ELDER_PEP)
	expect_eq(t, game.mp_current(w, who), i32(1))
	press(c, .Confirm)

	// the constable pays 10 per membership card
	for _ in 0 ..< 3 { game.give_new_item(w, who, .Membership_Card) }
	me.stats[.Money] = 0
	talk_to(c, .David_the_Constable)
	button(c, game.CONSTABLE_BOUNTIES)
	expect_eq(t, me.stats[.Money], i32(30))
	press(c, .Confirm)

	// the drunk takes the beer and returns a bottle; the chicken eats food and sometimes lays an egg
	game.give_new_item(w, who, .Beer)
	talk_to(c, .Yermom_the_Drunk)
	button(c, game.DRUNK_BEER)
	_, still_beer := game.carried_item_of_type(w, who, .Beer)
	expect(t, !still_beer, "the beer is gone")
	_, has_bottle := game.carried_item_of_type(w, who, .Empty_Bottle)
	expect(t, has_bottle, "and an empty bottle is in the pack")
	press(c, .Confirm)
	eggs := 0
	for _ in 0 ..< 60 {
		game.give_new_item(w, who, .kottbulle)
		talk_to(c, .Sander_the_Chicken)
		button(c, game.CHICKEN_FEED)
		press(c, .Confirm)
	}
	for id in game.items_in_pack(w, who) { if game.item_get(w, id).type == .Magic_Egg { eggs += 1 } }
	expect(t, eggs >= 1 && eggs <= 25, "about one feeding in six lays an egg")

	// two-up: win 15 or lose 5
	me.stats[.Money] = 20
	wins, losses := 0, 0
	for _ in 0 ..< 40 {
		before := me.stats[.Money]
		talk_to(c, .Honest_Dan)
		button(c, game.MARKET_GAMBLE)
		if me.stats[.Money] == before + 15 { wins += 1 } else if me.stats[.Money] == before - 5 { losses += 1 }
		press(c, .Confirm)
		if me.stats[.Money] < 5 { me.stats[.Money] = 20 }
	}
	expect_eq(t, wins + losses, 40)
	expect(t, wins > 2 && losses > 15, "a quarter of the games win")
}

test_town_quest_spells :: proc(t: ^T) {
	defer fake_reset()
	c := town_core(15); defer free_core(c)
	w := &c.world
	who := w.player.character
	me := game.player_character(w)
	cellar := game.locations_of_type(w, .Cellar)[0]
	rats_in_cellar := proc(w: ^game.World, cellar: game.Location_ID) -> int { return len(game.characters_at(w, cellar)) }

	// accept: one rat the first time, two the second
	talk_to(c, .Graham_the_Innkeeper)
	button(c, game.INN_QUEST) // "Do Quest!"
	expect(t, game.quest_active(w, .Cellar_Rats), "accepted")
	expect_eq(t, rats_in_cellar(w, cellar), 1)
	press(c, .Confirm)
	// completing pays for the tails, then clears the quest and counts it
	for _ in 0 ..< 12 { game.give_new_item(w, who, .Rat_Tail) }
	me.stats[.Money] = 0
	talk_to(c, .Graham_the_Innkeeper)
	button(c, game.INN_QUEST) // "Quest Done!"
	expect_eq(t, me.stats[.Money], i32(10)) // at most ten tails
	expect(t, !game.quest_active(w, .Cellar_Rats), "completed")
	expect_eq(t, w.player.quest_completions[.Cellar_Rats], i32(1))
	press(c, .Confirm)
	talk_to(c, .Graham_the_Innkeeper)
	button(c, game.INN_QUEST)
	expect_eq(t, rats_in_cellar(w, cellar), 1 + 2) // the first rat is still down there, and now two more
	press(c, .Confirm)

	// purify: spoilt food is made fresh again and a point of fatigue is paid
	rotten := game.give_new_item(w, who, .kottbulle_35)
	me.stats[.Mana], me.stats[.Fatigue] = 2, 0
	w.player.spells[.Purify] = 1
	expect(t, game.can_cast(w, who, .Purify), "mana is enough")
	game.cast_spell(w, who, .Purify)
	expect_eq(t, game.item_get(w, rotten).type, game.Item_Type.kottbulle)
	expect_eq(t, me.stats[.Fatigue], i32(1))
	me.stats[.Fatigue] = 2
	expect(t, !game.can_cast(w, who, .Purify), "no mana, no spell")
}
