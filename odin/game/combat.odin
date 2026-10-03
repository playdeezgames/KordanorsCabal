package game

// Fighting: a port of CharacterPhysicalCombat, CharacterMentalCombat, CharacterEquipment's wear rules and CharacterAdvancement.
// Everything the player does here ends in messages (D18); the callers show them.
//
// Dice: a "die" in this game succeeds on a six (the original rolled 1d6 divided by 6), so a roll of n dice is the number of
// sixes among n dice.

import "core:fmt"

roll_successes :: proc(w: ^World, dice: i32) -> (total: i32) {
	for _ in 0 ..< max(dice, 0) { if roll(&w.rng, Dice{1, 6}) == 6 { total += 1 } }
	return
}

// Drunk, high or chafing: one die fewer on every roll.
negative_influence :: proc(w: ^World, id: Character_ID) -> i32 {
	return stat_of(w, id, .Drunkenness) > 0 || stat_of(w, id, .Highness) > 0 || stat_of(w, id, .Chafing) > 0 ? -1 : 0
}

attack_dice :: proc(w: ^World, id: Character_ID) -> (dice: i32) {
	dice = stat_of(w, id, .Strength)
	for item_id in items_worn(w, id) { dice += ITEM_TYPES[item_get(w, item_id).type].stats.attack_dice }
	return
}
defend_dice :: proc(w: ^World, id: Character_ID) -> (dice: i32) {
	dice = stat_of(w, id, .Dexterity)
	for item_id in items_worn(w, id) { dice += ITEM_TYPES[item_get(w, item_id).type].stats.defend_dice }
	return
}
roll_attack :: proc(w: ^World, id: Character_ID) -> i32 { return roll_successes(w, attack_dice(w, id) + negative_influence(w, id)) }
roll_defend :: proc(w: ^World, id: Character_ID) -> i32 {
	return min(roll_successes(w, defend_dice(w, id) + negative_influence(w, id)), stat_of(w, id, .Base_Maximum_Defend))
}
roll_influence :: proc(w: ^World, id: Character_ID) -> i32 { return roll_successes(w, stat_of(w, id, .Influence) + negative_influence(w, id)) }
roll_willpower :: proc(w: ^World, id: Character_ID) -> i32 { return roll_successes(w, stat_of(w, id, .Willpower) + negative_influence(w, id)) }
// The dice of a spell: power plus the level known (nothing if the spell is not known).
roll_spell :: proc(w: ^World, id: Character_ID, spell: Spell_Type) -> i32 {
	level := w.player.spells[spell]
	return level == 0 ? 0 : roll_successes(w, stat_of(w, id, .Power) + level)
}
roll_power :: proc(w: ^World, id: Character_ID) -> i32 { return roll_successes(w, stat_of(w, id, .Power) + negative_influence(w, id)) }

// The most one blow can do: the worn items' limits added together if any has one, else the unarmed limit.
maximum_damage :: proc(w: ^World, id: Character_ID) -> i32 {
	total, any_limit := i32(0), false
	for item_id in items_worn(w, id) {
		if m, ok := ITEM_TYPES[item_get(w, item_id).type].stats.max_damage.?; ok { total += m; any_limit = true }
	}
	return any_limit ? total : stat_of(w, id, .Unarmed_Maximum_Damage)
}
determine_damage :: proc(w: ^World, id: Character_ID, value: i32) -> i32 { return clamp(value, 0, maximum_damage(w, id)) }
do_damage :: proc(w: ^World, id: Character_ID, damage: i32) { stat_add(character_get(w, id), .Wounds, damage) }

// ---- wear -----------------------------------------------------------------------------------------------------------

// Wears one random worn item that wears out and passes `is_kind`, `amount` times; returns the types of those that broke.
@(private)
wear_random :: proc(w: ^World, id: Character_ID, amount: i32, armor: bool) -> []Item_Type {
	broken := make([dynamic]Item_Type, context.temp_allocator)
	for _ in 0 ..< max(amount, 0) {
		candidates := make([dynamic]Item_ID, context.temp_allocator)
		for item_id in items_worn(w, id) {
			it := item_get(w, item_id)
			stats := ITEM_TYPES[it.type].stats
			if _, durable := durability_maximum(it); !durable { continue }
			if (armor ? stats.defend_dice : stats.attack_dice) > 0 { append(&candidates, item_id) }
		}
		if len(candidates) == 0 { continue }
		victim := candidates[pick_index(&w.rng, len(candidates))]
		if wear_out(item_get(w, victim), 1) {
			append(&broken, item_get(w, victim).type)
			destroy_item(w, victim)
		}
	}
	return broken[:]
}
wear_weapons :: proc(w: ^World, id: Character_ID, amount: i32) -> []Item_Type { return wear_random(w, id, amount, false) }
wear_armor :: proc(w: ^World, id: Character_ID, amount: i32) -> []Item_Type { return wear_random(w, id, amount, true) }

// ---- experience -----------------------------------------------------------------------------------------------------

// Adds experience; true when it brings a level: the goal doubles, a point to assign appears, and wounds, stress and fatigue go.
add_xp :: proc(w: ^World, id: Character_ID, xp: i32) -> bool {
	c := character_get(w, id)
	stat_add(c, .XP, xp)
	goal := c.stats[.XP_Goal]
	if goal == 0 { return false } // monsters have no goal
	if c.stats[.XP] >= goal {
		stat_add(c, .XP, -goal)
		stat_add(c, .XP_Goal, goal)
		stat_add(c, .Unassigned, 1)
		c.stats[.Wounds], c.stats[.Stress], c.stats[.Fatigue] = 0, 0, 0
		return true
	}
	return false
}

// ---- killing ---------------------------------------------------------------------------------------------------------

// The victim drops what it carries and one item from its loot table, and is removed.
drop_loot :: proc(w: ^World, victim: Character_ID) {
	here := character_get(w, victim).location
	for id in items_in_pack(w, victim) { item_put_on_ground(w, id, here) }
	for id in items_worn(w, victim) { item_put_on_ground(w, id, here) }
	table := CHARACTER_TYPES[character_get(w, victim).type].loot
	weights: [MAX_LOOT]int
	for e, i in table { weights[i] = e.weight }
	total := 0
	for wt in weights { total += wt }
	if total == 0 { return }
	if type := table[pick_weighted(&w.rng, weights[:])].item; type != .None { item_put_on_ground(w, create_item(w, type), here) }
}

// Kills `victim`, rewarding `killer`. Appends the lines to tell the player and returns the sound.
kill :: proc(w: ^World, killer, victim: Character_ID, lines: ^[dynamic]string) -> Sfx {
	def := &CHARACTER_TYPES[character_get(w, victim).type]
	append(lines, fmt.tprintf("You kill %s!", def.name))
	if money := i32(roll(&w.rng, def.money_dice)); money > 0 {
		append(lines, fmt.tprintf("You get %d money!", money))
		stat_add(character_get(w, killer), .Money, money)
	}
	if def.xp_value > 0 {
		append(lines, fmt.tprintf("You get %d XP!", def.xp_value))
		if add_xp(w, killer, def.xp_value) { append(lines, "You level up!") }
	}
	drop_loot(w, victim)
	destroy_character(w, victim)
	return .Enemy_Death
}

// ---- the player's actions --------------------------------------------------------------------------------------------

fight :: proc(w: ^World, who: Character_ID) {
	enemy, has := first_enemy(w, who)
	if !has { return }
	lines := make([dynamic]string, context.temp_allocator)
	name := CHARACTER_TYPES[character_get(w, enemy).type].name
	attack := roll_attack(w, who)
	append(&lines, fmt.tprintf("You roll an attack of %d.", attack))
	defend := roll_defend(w, enemy)
	append(&lines, fmt.tprintf("%s rolls a defend of %d.", name, defend))
	wear_armor(w, enemy, attack)
	sfx := Sfx.None
	if result := attack - defend; result <= 0 {
		append(&lines, "You miss!")
		sfx = .Miss
	} else {
		damage := determine_damage(w, who, result)
		append(&lines, fmt.tprintf("You do %d damage!", damage))
		do_damage(w, enemy, damage)
		for type in wear_weapons(w, who, damage) { append(&lines, fmt.tprintf("Yer %s breaks!", ITEM_TYPES[type].name)) }
		if is_dead(w, enemy) {
			sfx = kill(w, who, enemy, &lines)
		} else {
			sfx = .Enemy_Hit
			append(&lines, fmt.tprintf("%s has %d HP left.", name, health_current(w, enemy)))
		}
	}
	say(w, sfx, ..lines[:])
	counter_attacks(w, who)
}

// Running picks a random compass direction (and turns the player that way, as the original did); it works if that way is open.
run :: proc(w: ^World, who: Character_ID) {
	if !can_fight(w, who) { return }
	cardinal := [4]Direction{.North, .East, .South, .West}
	w.player.facing = cardinal[pick_index(&w.rng, 4)]
	if can_move(w, who, w.player.facing) {
		say(w, .None, "You successfully ran!")
		move_character(w, who, w.player.facing)
		return
	}
	say(w, .None, "You fail to run!")
	counter_attacks(w, who)
}

do_intimidation :: proc(w: ^World, who: Character_ID) {
	if !can_do_intimidation(w, who) { say(w, .None, "You cannot intimidate at this time!"); return }
	enemy, _ := first_enemy(w, who)
	name := CHARACTER_TYPES[character_get(w, enemy).type].name
	lines := make([dynamic]string, context.temp_allocator)
	influence := roll_influence(w, who)
	append(&lines, fmt.tprintf("You roll %d influence.", influence))
	willpower := roll_willpower(w, enemy)
	append(&lines, fmt.tprintf("%s rolls %d willpower.", name, willpower))
	if influence > willpower {
		stat_add(character_get(w, enemy), .Stress, 1)
		append(&lines, fmt.tprintf("%s loses 1 MP!", name))
		if is_demoralized(w, enemy) {
			append(&lines, fmt.tprintf("%s runs away!", name))
			destroy_character(w, enemy)
		}
	} else {
		append(&lines, fmt.tprintf("%s is not intimidated.", name))
	}
	say(w, .None, ..lines[:])
	counter_attacks(w, who)
}

// ---- the enemies hit back ---------------------------------------------------------------------------------------------

counter_attacks :: proc(w: ^World, who: Character_ID) {
	enemies := enemies_of(w, who)
	for enemy, i in enemies {
		if character_get(w, enemy) == nil || character_get(w, who) == nil { continue } // gone since the list was made
		counter_attack(w, who, enemy, i + 1, len(enemies))
	}
}

counter_attack :: proc(w: ^World, who, enemy: Character_ID, index, count: int) {
	if is_dead(w, who) { return }
	name := CHARACTER_TYPES[character_get(w, enemy).type].name
	header := fmt.tprintf("Counter-attack %d/%d:", index, count)
	if stat_of(w, enemy, .Immobilization) > 0 { // checks the enemy, not the player as the original did (see the audit)
		stat_add(character_get(w, enemy), .Immobilization, -1)
		say(w, .None, header, fmt.tprintf("%s is immobilized!", name))
		return
	}
	weights: [len(Attack_Type)]int
	for t in Attack_Type { weights[t] = CHARACTER_TYPES[character_get(w, enemy).type].attack_weights[t] }
	weights[Attack_Type.None] = 0
	total := 0
	for wt in weights { total += wt }
	if total == 0 { return }
	switch Attack_Type(pick_weighted(&w.rng, weights[:])) {
	case .Physical: physical_counter_attack(w, who, enemy, header)
	case .Mental: mental_counter_attack(w, who, enemy, header)
	case .None:
	}
}

physical_counter_attack :: proc(w: ^World, who, enemy: Character_ID, header: string) {
	name := CHARACTER_TYPES[character_get(w, enemy).type].name
	lines := make([dynamic]string, context.temp_allocator)
	append(&lines, header)
	attack := roll_attack(w, enemy)
	append(&lines, fmt.tprintf("%s rolls an attack of %d.", name, attack))
	for type in wear_armor(w, who, attack) { append(&lines, fmt.tprintf("Yer %s breaks!", ITEM_TYPES[type].name)) }
	defend := roll_defend(w, who)
	append(&lines, fmt.tprintf("You roll a defend of %d.", defend))
	sfx := Sfx.None
	if result := attack - defend; result <= 0 {
		append(&lines, fmt.tprintf("%s misses!", name))
		sfx = .Miss
	} else {
		damage := determine_damage(w, enemy, result)
		append(&lines, fmt.tprintf("%s does %d damage!", name, damage))
		do_damage(w, who, damage)
		wear_weapons(w, enemy, damage)
		if is_dead(w, who) {
			sfx = .Player_Death
			append(&lines, fmt.tprintf("%s kills you!", name))
			if shot := parting_shot(w, enemy); shot != "" { append(&lines, fmt.tprintf("%s says \"%s\"", name, shot)) }
		} else {
			sfx = .Player_Hit
			append(&lines, fmt.tprintf("You have %d HP left.", health_current(w, who)))
		}
	}
	say(w, sfx, ..lines[:])
}

mental_counter_attack :: proc(w: ^World, who, enemy: Character_ID, header: string) {
	name := CHARACTER_TYPES[character_get(w, enemy).type].name
	lines := make([dynamic]string, context.temp_allocator)
	append(&lines, header)
	append(&lines, fmt.tprintf("%s attempts to intimidate you!", name))
	influence := roll_influence(w, enemy)
	append(&lines, fmt.tprintf("%s rolls influence of %d.", name, influence))
	willpower := roll_willpower(w, who)
	append(&lines, fmt.tprintf("You roll willpower of %d.", willpower))
	sfx := Sfx.None
	if influence - willpower <= 0 {
		append(&lines, fmt.tprintf("%s fails to intimidate you!", name))
		sfx = .Miss
	} else {
		append(&lines, fmt.tprintf("%s adds 1 stress!", name))
		stat_add(character_get(w, who), .Stress, 1)
		if is_demoralized(w, who) {
			append(&lines, fmt.tprintf("%s completely demoralizes you and you drop everything and run away!", name))
			panic_flight(w, who)
		} else {
			append(&lines, fmt.tprintf("You have %d MP left.", mp_current(w, who)))
		}
	}
	say(w, sfx, ..lines[:])
}

// Demoralized: everything worn and carried is dropped, half the money is lost, and the player wakes in the town square.
panic_flight :: proc(w: ^World, who: Character_ID) {
	for id in items_worn(w, who) { item_put_in_pack(w, id, who) }
	here := character_get(w, who).location
	for id in items_in_pack(w, who) { item_put_on_ground(w, id, here) }
	character_get(w, who).stats[.Money] /= 2
	if squares := locations_of_type(w, .Town_Square); len(squares) > 0 { move_to(w, who, squares[0]) }
}

parting_shot :: proc(w: ^World, enemy: Character_ID) -> string {
	table := CHARACTER_TYPES[character_get(w, enemy).type].parting_shots
	weights: [MAX_PARTING_SHOTS]int
	total := 0
	for e, i in table { weights[i] = e.weight; total += e.weight }
	if total == 0 { return "" }
	return table[pick_weighted(&w.rng, weights[:])].text
}

// ---- the strike sequence shared by Holy Bolt, Holy Water and the Fire Shard -------------------------------------------

strike :: proc(w: ^World, who, enemy: Character_ID, damage: i32, intro: ..string) {
	lines := make([dynamic]string, context.temp_allocator)
	append(&lines, ..intro)
	do_damage(w, enemy, damage)
	sfx := Sfx.None
	if is_dead(w, enemy) { sfx = kill(w, who, enemy, &lines) }
	say(w, sfx, ..lines[:])
	counter_attacks(w, who)
}
