package game

// Event dispatch (decision D18). Content names behaviours with the Check and Action enums and the code implements each with a
// switch. There is no event bus: callers invoke `check` and `perform` directly, and rules report to the player through the
// message queue in the world. A handler that finds a handle gone does nothing or says "You cannot use that now!"; the original
// dereferenced a missing enemy in several places.
//
// Every check and every action is ported.

import "core:fmt"

Event_Context :: struct {
	world:     ^World,
	character: Character_ID, // who does it
	item:      Item_ID,      // the item concerned, if any
	location:  Location_ID,  // the place concerned, if any
}

say :: proc(w: ^World, sfx: Sfx, lines: ..string) { message_add(w, sfx, ..lines) }

first_enemy :: proc(w: ^World, who: Character_ID) -> (Character_ID, bool) {
	enemies := enemies_of(w, who)
	if len(enemies) == 0 { return {}, false }
	return enemies[0], true
}
// The id of the first enemy, or the zero id (which names no character).
first_enemy_id :: proc(w: ^World, who: Character_ID) -> Character_ID {
	id, _ := first_enemy(w, who)
	return id
}
can_be_bribed_with :: proc(w: ^World, enemy: Character_ID, type: Item_Type) -> bool {
	return type in CHARACTER_TYPES[character_get(w, enemy).type].bribes
}
in_dungeon :: proc(w: ^World, who: Character_ID) -> bool {
	return LOCATION_TYPES[location_get(w, character_get(w, who).location).type].is_dungeon
}
has_item_type :: proc(w: ^World, who: Character_ID, type: Item_Type) -> bool {
	_, has := carried_item_of_type(w, who, type)
	return has
}

// Gives the character a new item of the type, in the pack.
give_new_item :: proc(w: ^World, who: Character_ID, type: Item_Type) -> Item_ID {
	id := create_item(w, type)
	item_put_in_pack(w, id, who)
	return id
}

// ---- spells -------------------------------------------------------------------------------------------------------

spell_next_level :: proc(w: ^World, spell: Spell_Type) -> i32 { return w.player.spells[spell] + 1 }
can_learn_spell :: proc(w: ^World, who: Character_ID, spell: Spell_Type) -> bool {
	next := spell_next_level(w, spell)
	if next > SPELL_TYPES[spell].maximum_level { return false }
	return stat_of(w, who, .Power) >= SPELL_TYPES[spell].required_power[next]
}
learn_spell :: proc(w: ^World, who: Character_ID, spell: Spell_Type) {
	name := SPELL_TYPES[spell].name
	if !can_learn_spell(w, who, spell) {
		say(w, .None, fmt.tprintf("You cannot learn %s at this time!", name))
		return
	}
	next := spell_next_level(w, spell)
	say(w, .None, fmt.tprintf("You now know %s at level %d.", name, next))
	w.player.spells[spell] = next
}

// ---- checks (pure reads) --------------------------------------------------------------------------------------------

check :: proc(name: Check, ctx: Event_Context) -> bool {
	w, who := ctx.world, ctx.character
	if character_get(w, who) == nil { return false }
	enemy, has_enemy := first_enemy(w, who)
	dungeon := in_dungeon(w, who)
	mana := mana_current(w, who)
	switch name {
	case .None: return false
	case .Always_True: return true
	case .Is_In_Dungeon: return dungeon
	case .Has_Bong: return has_item_type(w, who, .Bong)
	case .Is_Fighting_Undead: return has_enemy && CHARACTER_TYPES[character_get(w, enemy).type].is_undead
	case .Can_Use_Beer: return !has_enemy || can_be_bribed_with(w, enemy, .Beer)
	case .Can_Use_Rotten_Egg: return has_enemy && can_be_bribed_with(w, enemy, .Rotten_Egg)
	case .Can_Use_Pr0n: return !has_enemy || can_be_bribed_with(w, enemy, .Pr0n_Scroll)
	case .Can_Use_Bottle: return has_enemy && can_be_bribed_with(w, enemy, .Empty_Bottle)
	case .Can_Use_Air_Shard, .Can_Use_Water_Shard: return dungeon && mana > 0
	case .Can_Use_Earth_Shard, .Can_Use_Fire_Shard: return dungeon && has_enemy && mana > 0
	case .Can_Learn_Holy_Bolt: return can_learn_spell(w, who, .Holy_Bolt)
	case .Can_Learn_Purify: return can_learn_spell(w, who, .Purify)
	case .Character_Can_Cast_Holy_Bolt: return has_enemy && CHARACTER_TYPES[character_get(w, enemy).type].is_undead && mana > 0
	case .Character_Can_Cast_Purify: return mana > 0
	case .Character_Can_Accept_Cellar_Rats_Quest: return .Cellar_Rats not_in w.player.quests_active
	case .Character_Can_Complete_Cellar_Rats_Quest: return has_item_type(w, who, .Rat_Tail)
	}
	return false
}

// ---- actions --------------------------------------------------------------------------------------------------------

perform :: proc(name: Action, ctx: Event_Context) {
	w, who := ctx.world, ctx.character
	#partial switch name { // the actions that need nobody to act: item decay and purifying
	case .None, .Purify_Food, .Food_Decay, .Rotten_Food_Decay, .Location_Decay_Items:
	case: if character_get(w, who) == nil { return } // a stale or missing actor: do nothing
	}
	switch name {
	case .None:

	case .Drink_Potion:
		if character_get(w, who) == nil { return }
		points := i32(roll(&w.rng, Dice{2, 4}))
		heal(w, who, points)
		give_new_item(w, who, .Empty_Bottle)
		say(w, .None, fmt.tprintf("Potion heals up to %d HP!", points), fmt.tprintf("You now have %d HP!", health_current(w, who)))

	case .Eat_Food:
		heal(w, who, 1)
		character_get(w, who).stats[.Hunger] = 0
		say(w, .None, "Food heals up to 1 HP!", fmt.tprintf("You now have %d HP!", health_current(w, who)))

	case .Use_Rotten_Food:
		if roll(&w.rng, Dice{1, 2}) == 1 {
			c := character_get(w, who)
			c.stats[.Hunger] /= 2
			c.stats[.Food_Poisoning] = 10
			say(w, .None, "Food was rotten!", "You got food poisoning!")
		} else {
			heal(w, who, 1)
			character_get(w, who).stats[.Hunger] = 0
			say(w, .None, "Food heals up to 1 HP!", fmt.tprintf("You now have %d HP!", health_current(w, who)))
		}

	case .Purify_Food:
		if it := item_get(w, ctx.item); it != nil { it.type = .kottbulle } // fresh again; every property comes from the type

	case .Food_Decay:
		if it := item_get(w, ctx.item); it != nil && roll(&w.rng, Dice{1, 3}) == 1 { it.type = .kottbulle_35 }

	case .Rotten_Food_Decay:
		it := item_get(w, ctx.item)
		if it == nil || roll(&w.rng, Dice{1, 2}) != 1 { return }
		if roll(&w.rng, Dice{1, 2}) == 1 && it.holder == .On_Ground { create_character(w, .Rat, it.location) }
		destroy_item(w, ctx.item)

	case .Location_Decay_Items:
		for id in items_on_ground(w, ctx.location) { item_decay(w, id) }

	case .Read_Note:
		it := item_get(w, ctx.item)
		if it == nil { return }
		if it.lore == 0 { it.lore = u8(unassigned_lore(w)) }
		say(w, .None, LORES[it.lore].text)

	case .Learn_Holy_Bolt: learn_spell(w, who, .Holy_Bolt)
	case .Learn_Purify: learn_spell(w, who, .Purify)

	case .Use_Town_Portal:
		squares := locations_of_type(w, .Town_Square)
		if len(squares) == 0 { return }
		open_portal(w, character_get(w, who).location, squares[0])
	case .Use_Moon_Portal:
		moon := locations_of_type(w, .Moon)
		if len(moon) == 0 { return }
		open_portal(w, character_get(w, who).location, moon[pick_index(&w.rng, len(moon))])

	case .Use_Air_Shard:
		c := character_get(w, who)
		here := location_get(w, c.location)
		same_level := make([dynamic]Location_ID, context.temp_allocator)
		for id in locations_of_type(w, .Dungeon) { if location_get(w, id).level == here.level { append(&same_level, id) } }
		if len(same_level) == 0 { say(w, .None, "You cannot use that now!"); return }
		set_mana(w, who, mana_current(w, who) - 1)
		to := same_level[pick_index(&w.rng, len(same_level))]
		move_to(w, who, to)
		say(w, .None, fmt.tprintf("You use the %s and suddenly find yerself somewhere else!", ITEM_TYPES[.Air_Shard].name))

	case .Use_Water_Shard:
		set_mana(w, who, mana_current(w, who) - 1)
		character_get(w, who).stats[.Wounds] = 0
		say(w, .None, fmt.tprintf("You use %s to heal yer wounds!", ITEM_TYPES[.Water_Shard].name))

	case .Use_Herb:
		missing := stat_of(w, who, .Mana) - mana_current(w, who)
		set_mana(w, who, stat_of(w, who, .Mana))
		stat_add(character_get(w, who), .Highness, 10)
		say(w, .None, fmt.tprintf("You use yer %s to smoke yer %s.", ITEM_TYPES[.Bong].name, ITEM_TYPES[.Herb].name), fmt.tprintf("You gain %d %s.", missing, STATS[.Mana].name))

	case .Use_Magic_Egg:
		// type, weight (the original's table, in order)
		table := [14]struct { type: Item_Type, weight: int }{
			{.Beer, 500}, {.Brodesode, 8}, {.Chainmail, 4}, {.Dagger, 250}, {.kottbulle, 1000}, {.Helmet, 125}, {.Holy_Water, 64},
			{.Moon_Portal, 1}, {.Platemail, 2}, {.Potion, 125}, {.Shield, 64}, {.Shortsword, 16}, {.Town_Portal, 8}, {.Trousers, 1},
		}
		weights: [14]int
		for e, i in table { weights[i] = e.weight }
		id := give_new_item(w, who, table[pick_weighted(&w.rng, weights[:])].type)
		say(w, .None, fmt.tprintf("You crack open the %s and find %s inside!", ITEM_TYPES[.Magic_Egg].name, item_name(item_get(w, id))))

	case .Use_Beer:
		if enemy, has := first_enemy(w, who); has && can_be_bribed_with(w, enemy, .Beer) {
			say(w, .None, fmt.tprintf("You give %s the %s, and they wander off to get drunk.", CHARACTER_TYPES[character_get(w, enemy).type].name, ITEM_TYPES[.Beer].name))
			destroy_character(w, enemy)
			return
		}
		c := character_get(w, who)
		c.stats[.Stress] = 0 // the original set the current MP to the maximum, which is no stress
		stat_add(c, .Drunkenness, 10)
		give_new_item(w, who, .Empty_Bottle)
		say(w, .None, "You drink the beer, and suddenly feel braver!")

	case .Use_Rotten_Egg:
		if enemy, has := first_enemy(w, who); has && can_be_bribed_with(w, enemy, .Rotten_Egg) {
			say(w, .None, fmt.tprintf("You give %s the %s, and they quickly wander off with a seeming great purpose.", CHARACTER_TYPES[character_get(w, enemy).type].name, ITEM_TYPES[.Rotten_Egg].name))
			destroy_character(w, enemy)
			return
		}
		say(w, .None, "You cannot use that now!")

	case .Use_Bottle:
		enemy, has := first_enemy(w, who)
		if !has { say(w, .None, "You cannot use that now!"); return }
		say(w, .None, fmt.tprintf("You give the %s to the %s, and it wanders off happily.", ITEM_TYPES[.Empty_Bottle].name, CHARACTER_TYPES[character_get(w, enemy).type].name))
		destroy_character(w, enemy)

	case .Use_Pr0n:
		use_pr0n(w, who)

	case .Use_Holy_Water:
		enemy, has := first_enemy(w, who)
		if !has { say(w, .None, "You cannot use that now!"); return }
		damage := i32(roll(&w.rng, Dice{1, 4}))
		strike(w, who, enemy, damage, fmt.tprintf("%s deals %d HP to %s!", ITEM_TYPES[.Holy_Water].name, damage, CHARACTER_TYPES[character_get(w, enemy).type].name))
	case .Use_Fire_Shard:
		enemy, has := first_enemy(w, who)
		if !has { say(w, .None, "You cannot use that now!"); return }
		character_get(w, who).stats[.Fatigue] += 1
		damage := i32(roll(&w.rng, Dice{3, 4}))
		strike(w, who, enemy, damage, fmt.tprintf("You use %s on %s!", ITEM_TYPES[.Fire_Shard].name, CHARACTER_TYPES[character_get(w, enemy).type].name), fmt.tprintf("You do %d damage!", damage))
	case .Use_Earth_Shard:
		enemy, has := first_enemy(w, who)
		if !has { say(w, .None, "You cannot use that now!"); return }
		character_get(w, who).stats[.Fatigue] += 1
		name := CHARACTER_TYPES[character_get(w, enemy).type].name
		turns := roll_power(w, who)
		stat_add(character_get(w, enemy), .Immobilization, turns)
		say(w, .None, fmt.tprintf("You use %s on %s!", ITEM_TYPES[.Earth_Shard].name, name), fmt.tprintf("You immobilize %s for %d turns!", name, turns))
		counter_attacks(w, who)
	case .Character_Cast_Holy_Bolt:
		if !check(.Character_Can_Cast_Holy_Bolt, ctx) { say(w, .None, fmt.tprintf("You cannot cast %s now!", SPELL_TYPES[.Holy_Bolt].name)); return }
		enemy, _ := first_enemy(w, who)
		character_get(w, who).stats[.Fatigue] += 1
		damage := roll_spell(w, who, .Holy_Bolt)
		strike(w, who, enemy, damage, fmt.tprintf("You cast %s on %s!", SPELL_TYPES[.Holy_Bolt].name, CHARACTER_TYPES[character_get(w, enemy).type].name), fmt.tprintf("You do %d damage!", damage))
	case .Character_Cast_Purify:
		for id in items_in_pack(w, who) { item_purify(w, id) }
		for id in items_worn(w, who) { item_purify(w, id) }
		character_get(w, who).stats[.Fatigue] += 1
		say(w, .None, "You purify yer inventory!")

	case .Character_Accept_Cellar_Rats_Quest:
		say(w, .None, "You accept the quest!")
		w.player.quests_active += {.Cellar_Rats}
		cellars := locations_of_type(w, .Cellar)
		if len(cellars) == 0 { return }
		for _ in 0 ..< w.player.quest_completions[.Cellar_Rats] + 1 { create_character(w, .Rat, cellars[0]) } // one more rat each time

	case .Character_Complete_Cellar_Rats_Quest:
		say(w, .None, "You complete the quest!")
		paid := 0
		for id in items_in_pack(w, who) {
			if paid == 10 { break }
			if item_get(w, id).type == .Rat_Tail { stat_add(character_get(w, who), .Money, 1); destroy_item(w, id); paid += 1 }
		}
		w.player.quests_active -= {.Cellar_Rats}
		w.player.quest_completions[.Cellar_Rats] += 1
	}
}

use_pr0n :: proc(w: ^World, who: Character_ID) {
	enemy, has := first_enemy(w, who)
	if has && can_be_bribed_with(w, enemy, .Pr0n_Scroll) {
		say(w, .None, fmt.tprintf("You give %s the %s, and they quickly wander off with a seeming great purpose.", CHARACTER_TYPES[character_get(w, enemy).type].name, ITEM_TYPES[.Pr0n_Scroll].name))
		destroy_character(w, enemy)
		return
	}
	if has { say(w, .None, "Dude! now is not the time!"); return }
	points := i32(roll(&w.rng, Dice{1, 4}))
	stat_add(character_get(w, who), .Stress, -points)
	lines := make([dynamic]string, context.temp_allocator)
	append(&lines, fmt.tprintf("You make use of %s, which cheers you up by %d %s.", ITEM_TYPES[.Pr0n_Scroll].name, points, STATS[.MP].name))
	append(&lines, fmt.tprintf("You now have %d %s.", mp_current(w, who), STATS[.MP].name))
	if _, protected := carried_item_of_type(w, who, .Lotion); !protected {
		append(&lines, fmt.tprintf("You also receive 10 %s. Try %s next time.", STATS[.Chafing].name, ITEM_TYPES[.Lotion].name))
		character_get(w, who).stats[.Chafing] = 10
	} else {
		// The original wore the lotion down and swapped the empty bottle for an empty bottle item, but the lotion has no
		// durability in the data, so in the original it never ran out; kept as it was (see docs/dead-code-audit.md).
		append(&lines, fmt.tprintf("You use a bit of %s to prevent %s.", ITEM_TYPES[.Lotion].name, STATS[.Chafing].name))
	}
	say(w, .None, ..lines[:])
}

// A pair of portals between here and `there`, replacing whatever led out of here or in to there.
open_portal :: proc(w: ^World, here, there: Location_ID) {
	location_get(w, here).routes[.Out] = {there, ROUTE_PORTAL}
	location_get(w, there).routes[.In] = {here, ROUTE_PORTAL}
	say(w, .None, "A portal opens before you!")
}

set_mana :: proc(w: ^World, who: Character_ID, current: i32) {
	character_get(w, who).stats[.Fatigue] = stat_clamped(.Fatigue, stat_of(w, who, .Mana) - current)
}

move_to :: proc(w: ^World, who: Character_ID, to: Location_ID) {
	character_get(w, who).location = to
	if who == w.player.character { location_get(w, to).visited = true }
}

// A lore text no item shows yet; if all are taken, any (the original threw).
unassigned_lore :: proc(w: ^World) -> int {
	taken: [LORE_COUNT + 1]bool
	for id in w.item_order { taken[item_get(w, id).lore] = true }
	free: [LORE_COUNT]int
	n := 0
	for l in 1 ..= LORE_COUNT { if !taken[l] { free[n] = l; n += 1 } }
	if n == 0 { return rng_range(&w.rng, 1, LORE_COUNT) }
	return free[pick_index(&w.rng, n)]
}

// ---- items and their events -----------------------------------------------------------------------------------------

item_can_use :: proc(w: ^World, who: Character_ID, id: Item_ID) -> bool {
	it := item_get(w, id)
	if it == nil { return false }
	can := ITEM_TYPES[it.type].events.can_use
	if can == .None { return false }
	return check(can, {world = w, character = who, item = id})
}

// Uses the item if it can be used. The single-use rule is applied after the event, because the event may change or destroy the
// item itself (food rots, notes pick a lore). Returns whether the item was used.
item_use :: proc(w: ^World, who: Character_ID, id: Item_ID) -> bool {
	if !item_can_use(w, who, id) { return false }
	type := item_get(w, id).type
	perform(ITEM_TYPES[type].events.use, {world = w, character = who, item = id})
	if ITEM_TYPES[type].stats.single_use { destroy_item(w, id) }
	return true
}

item_decay :: proc(w: ^World, id: Item_ID) {
	it := item_get(w, id)
	if it == nil { return }
	perform(ITEM_TYPES[it.type].events.decay, {world = w, item = id, location = it.location})
}

location_decay_items :: proc(w: ^World, l: Location_ID) { perform(.Location_Decay_Items, {world = w, location = l}) }

// The purify spell's effect on one carried or worn item.
item_purify :: proc(w: ^World, id: Item_ID) {
	if it := item_get(w, id); it != nil { perform(ITEM_TYPES[it.type].events.purify, {world = w, item = id}) }
}
