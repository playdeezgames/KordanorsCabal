package main

import "core:fmt"
import "kc:game"

button :: proc(c: ^game.Core, index: int) {
	assert(c.state == .In_Play)
	for tries := 0; c.button != index && tries < 12; tries += 1 { press(c, .Down) }
	press(c, .Confirm)
}

test_items_screens_golden :: proc(t: ^T) {
	defer fake_reset()
	c := dungeon_core(); defer free_core(c)
	w := &c.world
	button(c, game.NEUTRAL_INVENTORY)
	expect_eq(t, c.state, game.UI_State.Inventory)
	step(c)
	expect_like_vb(t, c, "42-inventory")
	press(c, .Down) // Dagger
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Interact_Item)
	step(c)
	expect_like_vb(t, c, "43-interact-item")
	pick(c, 3) // Equip
	expect_eq(t, c.state, game.UI_State.Message)
	step(c)
	expect_like_vb(t, c, "44-equip-message")
	press(c, .Confirm) // back to the inventory
	expect_eq(t, c.state, game.UI_State.Inventory)
	press(c, .Cancel)
	step(c)
	expect_like_vb(t, c, "45-in-play-with-equipment")
	button(c, game.NEUTRAL_EQUIPMENT)
	expect_eq(t, c.state, game.UI_State.Equipment)
	step(c)
	expect_like_vb(t, c, "46-equipment")
	press(c, .Confirm) // Go Back
	expect_eq(t, c.state, game.UI_State.In_Play)

	// the book teaches Holy Bolt
	button(c, game.NEUTRAL_INVENTORY)
	press(c, .Confirm) // Book of Holy Bolt: either of the two
	pick(c, 2) // Use
	expect_eq(t, c.state, game.UI_State.Message)
	step(c)
	expect_like_vb(t, c, "48-use-potion-message")
	expect_eq(t, w.player.spells[.Holy_Bolt], i32(1))
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Inventory)
	press(c, .Cancel)

	// the map: this place, two exits, with enemies in it
	button(c, game.NEUTRAL_MAP)
	expect_eq(t, c.state, game.UI_State.Map)
	step(c)
	expect_like_vb(t, c, "49-map")
}

test_items_ground_golden :: proc(t: ^T) {
	defer fake_reset()
	c := town_core(41); defer free_core(c)
	w := &c.world
	square := game.player_character(w).location
	for type in ([]game.Item_Type{.Potion, .Dagger}) { game.item_put_on_ground(w, game.create_item(w, type), square) }
	step(c)
	expect_like_vb(t, c, "60-town-with-ground-items", 3, 4, 19, 20, 21, 22) // the recorded run had Spells and Inventory too
	button(c, game.NEUTRAL_GROUND_ENEMIES)
	expect_eq(t, c.state, game.UI_State.Ground_Inventory)
	step(c)
	expect_like_vb(t, c, "61-ground-inventory")
	press(c, .Confirm) // pick up the potion
	expect_eq(t, len(game.items_in_pack(w, w.player.character)), 1)
	expect_eq(t, len(game.items_on_ground(w, square)), 1)
	expect_eq(t, c.state, game.UI_State.Ground_Inventory)
	press(c, .Confirm) // and the dagger: nothing left, so back to the game
	expect_eq(t, c.state, game.UI_State.In_Play)
	expect_eq(t, len(game.items_in_pack(w, w.player.character)), 2)
}

test_items_rules :: proc(t: ^T) {
	defer fake_reset()
	c := dungeon_core(); defer free_core(c)
	w := &c.world
	player := w.player.character
	// everything the pack holds can be found again, sorted, grouped
	groups := game.item_groups(w, player)
	expect_eq(t, len(groups), 4)
	expect_eq(t, groups[0].name, "Book of Holy Bolt")
	expect_eq(t, len(groups[0].items), 2)
	expect_eq(t, groups[3].name, "Potion")
	expect_eq(t, game.encumbrance_current(w, player), i32(45))
	expect_eq(t, game.encumbrance_maximum(w, player), i32(120))

	// durability, wear and a broken item
	dagger, _ := game.carried_item_of_type(w, player, .Dagger)
	it := game.item_get(w, dagger)
	cur, has := game.durability_current(it)
	expect(t, has && cur == 10, "a new dagger has 10 durability")
	for _ in 0 ..< 9 { expect(t, !game.wear_out(it, 1), "not broken yet") }
	expect(t, game.wear_out(it, 1), "broken at zero")
	_, has = game.durability_current(game.item_get(w, game.give_new_item(w, player, .Potion)))
	expect(t, !has, "potions do not wear")

	// equipping: the slot is chosen, the old item goes back, a second ring takes the second hand
	dagger2 := game.give_new_item(w, player, .Dagger)
	game.equip_item(w, player, dagger)
	game.equip_item(w, player, dagger2) // replaces the first
	worn, found := game.item_in_slot(w, player, .Weapon)
	expect(t, found && worn == dagger2, "the newer dagger is wielded")
	expect(t, game.item_get(w, dagger).holder == .Carried, "the old one is back in the pack")
	ring1, ring2 := game.give_new_item(w, player, .Ring_of_HP), game.give_new_item(w, player, .Ring_of_HP)
	game.equip_item(w, player, ring1)
	game.equip_item(w, player, ring2)
	expect_eq(t, game.item_get(w, ring1).slot, game.Equip_Slot.LHand)
	expect_eq(t, game.item_get(w, ring2).slot, game.Equip_Slot.RHand)
	// worn items buff the statistic: two rings of HP add 2 to the maximum
	expect_eq(t, game.stat_of(w, player, .HP), game.character_get(w, player).stats[.HP] + 2)
	game.unequip(w, player, .LHand)
	expect_eq(t, game.stat_of(w, player, .HP), game.character_get(w, player).stats[.HP] + 1)
	// things that cannot be worn say so
	potion := game.give_new_item(w, player, .Potion)
	for w.messages.count > 0 { game.message_pop(w) }
	game.equip_item(w, player, potion)
	head := game.message_head(w)
	expect(t, head != nil && string(head.text[:head.len]) == "You cannot equip Potion!", "cannot equip")
	expect_eq(t, game.item_get(w, potion).holder, game.Item_Holder.Carried)
	ok, why := game.world_validate(w)
	expect(t, ok, why)
}

// Runs one action as the player and returns the text of the messages it queued.
run_action :: proc(w: ^game.World, action: game.Action, item: game.Item_ID = {}) -> string {
	for w.messages.count > 0 { game.message_pop(w) }
	game.perform(action, {world = w, character = w.player.character, item = item})
	if m := game.message_head(w); m != nil { return fmt.tprintf("%s", string(m.text[:m.len])) }
	return ""
}

test_events_items :: proc(t: ^T) {
	defer fake_reset()
	c := dungeon_core(); defer free_core(c)
	w := &c.world
	player := w.player.character
	me := game.character_get(w, player)
	game.create_location(w, .Town_Square)
	game.create_location(w, .Moon)

	// potion: heals, gives back the bottle
	me.stats[.HP] = 20
	me.stats[.Wounds] = 15
	potion, _ := game.carried_item_of_type(w, player, .Potion)
	expect(t, game.item_can_use(w, player, potion), "a potion can always be drunk")
	bottles := len(game.items_in_pack(w, player))
	expect(t, game.item_use(w, player, potion), "used")
	expect(t, game.item_get(w, potion) == nil, "a potion is consumed")
	expect_eq(t, len(game.items_in_pack(w, player)), bottles) // one gone, an empty bottle in its place
	expect(t, me.stats[.Wounds] >= 7 && me.stats[.Wounds] <= 13, "2d4 healed")
	_, has_bottle := game.carried_item_of_type(w, player, .Empty_Bottle)
	expect(t, has_bottle, "empty bottle")

	// food: heals one, empties the stomach
	me.stats[.Hunger] = 40
	food := game.give_new_item(w, player, .kottbulle)
	game.item_use(w, player, food)
	expect_eq(t, me.stats[.Hunger], i32(0))

	// rotten food: purifying makes it fresh, rotting makes it rotten, it is consumed either way
	fresh := game.give_new_item(w, player, .kottbulle)
	for game.item_get(w, fresh).type == .kottbulle { game.perform(.Food_Decay, {world = w, item = fresh}) }
	expect_eq(t, game.item_get(w, fresh).type, game.Item_Type.kottbulle_35)
	game.item_purify(w, fresh)
	expect_eq(t, game.item_get(w, fresh).type, game.Item_Type.kottbulle)

	// rotten food on the floor eventually vanishes, sometimes leaving a rat
	rotten := game.create_item(w, .kottbulle_35)
	game.item_put_on_ground(w, rotten, me.location)
	rats := 0
	for tries := 0; game.item_get(w, rotten) != nil && tries < 200; tries += 1 { game.location_decay_items(w, me.location) }
	expect(t, game.item_get(w, rotten) == nil, "rotten food rots away")
	_ = rats

	// the Magic Egg gives something, the Town Portal opens a way to the square, the Moon Portal one to the moon
	egg := game.give_new_item(w, player, .Magic_Egg)
	before := len(w.item_order)
	game.item_use(w, player, egg)
	expect_eq(t, len(w.item_order), before) // the egg went, a prize came
	expect(t, game.check(.Is_In_Dungeon, {world = w, character = player}), "the test place is a dungeon")
	text := run_action(w, .Use_Town_Portal)
	expect_eq(t, text, "A portal opens before you!")
	out, has_out := game.route_at(game.location_get(w, me.location), .Out)
	expect(t, has_out && out.type == game.ROUTE_PORTAL && game.location_get(w, out.to).type == .Town_Square, "portal to the square")
	back, has_back := game.route_at(game.location_get(w, out.to), .In)
	expect(t, has_back && back.to == me.location, "and back")

	// notes: each shows a different text and takes the lore's title; none left falls back to a random one
	seen := make(map[u8]bool, context.temp_allocator)
	for _ in 0 ..< game.LORE_COUNT + 1 {
		note := game.give_new_item(w, player, .Note)
		game.perform(.Read_Note, {world = w, character = player, item = note})
		n := game.item_get(w, note)
		expect(t, n.lore >= 1 && int(n.lore) <= game.LORE_COUNT, "a lore was chosen")
		seen[n.lore] = true
	}
	expect_eq(t, len(seen), game.LORE_COUNT)

	// learning: the book teaches once (level 1 is the most)
	expect(t, game.can_learn_spell(w, player, .Holy_Bolt), "can learn")
	text = run_action(w, .Learn_Holy_Bolt)
	expect_eq(t, text, "You now know Holy Bolt at level 1.")
	expect(t, !game.can_learn_spell(w, player, .Holy_Bolt), "already known")
	text = run_action(w, .Learn_Holy_Bolt)
	expect_eq(t, text, "You cannot learn Holy Bolt at this time!")

	// the shards
	me.stats[.Mana], me.stats[.Fatigue], me.stats[.Wounds] = 3, 0, 5
	text = run_action(w, .Use_Water_Shard)
	expect_eq(t, me.stats[.Wounds], i32(0))
	expect_eq(t, game.mana_current(w, player), i32(2))
	here := me.location
	text = run_action(w, .Use_Air_Shard)
	expect_eq(t, game.location_get(w, me.location).level, game.Dungeon_Level.Level_I) // somewhere on the same level
	me.location = here // back among the goblins

	// the herb refills the mana
	me.stats[.Fatigue] = 2
	text = run_action(w, .Use_Herb)
	expect_eq(t, game.mana_current(w, player), game.stat_of(w, player, .Mana))
	expect_eq(t, me.stats[.Highness], i32(10))

	// bribes: the goblins take beer or a scroll, not a bottle; the beer is kept when nobody is here
	expect(t, game.check(.Can_Use_Beer, {world = w, character = player}), "goblins can be bribed with beer")
	expect(t, !game.check(.Can_Use_Bottle, {world = w, character = player}), "not with a bottle")
	expect(t, !game.check(.Is_Fighting_Undead, {world = w, character = player}), "goblins are alive")
	crowd := len(game.enemies_of(w, player)) // two goblins, and maybe a rat the rotten food bred
	text = run_action(w, .Use_Beer)
	expect_eq(t, len(game.enemies_of(w, player)), crowd - 1) // one wandered off to get drunk
	expect(t, text != "", "and said so")
}

test_events_unported :: proc(t: ^T) {
	// the combat and quest actions are still missing: they say so rather than crash
	pending := 0
	for a in game.Action { if !game.action_ported(a) { pending += 1 } }
	expect_eq(t, pending, 7)
	defer fake_reset()
	c := dungeon_core(); defer free_core(c)
	w := &c.world
	text := run_action(w, .Use_Fire_Shard)
	expect_eq(t, text, "That does not work yet in this version.")
	// every action is safe on a world, whoever the context names: run them all and validate
	for a in game.Action {
		item := game.give_new_item(w, w.player.character, .Note)
		game.perform(a, {world = w, character = w.player.character, item = item, location = game.character_get(w, w.player.character).location})
		game.perform(a, {world = w}) // and with nobody named
		for w.messages.count > 0 { game.message_pop(w) }
		ok, why := game.world_validate(w)
		expect(t, ok, why)
	}
}
