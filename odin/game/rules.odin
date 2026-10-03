package game

// Game rules that the in-play screens need, ported from the KordanorsCabal.Game classes (CharacterMovement, CharacterEncumbrance,
// CharacterHealth, LocationFactions and friends). They are plain procedures over the World; the UI sequences them.

// ---- messages and sounds -------------------------------------------------------------------------------------------

// Queues a message made of `lines`. A message that does not fit is cut; a full queue drops the new message (the original's
// queue was unbounded, but sixteen unread messages cannot happen in play).
message_add :: proc(w: ^World, sfx: Sfx, lines: ..string) {
	q := &w.messages
	if q.count == MESSAGE_QUEUE_LENGTH { return }
	m := &q.items[(q.first + q.count) % MESSAGE_QUEUE_LENGTH]
	m^ = {sfx = sfx}
	for line, i in lines {
		if i > 0 && m.len < MESSAGE_CAPACITY { m.text[m.len] = '\n'; m.len += 1 }
		n := min(len(line), MESSAGE_CAPACITY - m.len)
		copy(m.text[m.len:], line[:n])
		m.len += n
	}
	q.count += 1
}
message_head :: proc(w: ^World) -> ^Message { return w.messages.count > 0 ? &w.messages.items[w.messages.first] : nil }
message_pop :: proc(w: ^World) {
	if w.messages.count == 0 { return }
	w.messages.first = (w.messages.first + 1) % MESSAGE_QUEUE_LENGTH
	w.messages.count -= 1
}

world_play :: proc(w: ^World, sfx: Sfx) {
	if w.sfx_count < SFX_QUEUE_LENGTH {
		w.sfx_queue[w.sfx_count] = sfx
		w.sfx_count += 1
	}
}

// ---- statistics as the rules see them -----------------------------------------------------------------------------------
// A character's statistic is its own value plus the buffs of everything it wears (CharacterStatistics.GetStatistic). Changes are
// written to the own value only: the original wrote the buffed value back, so wearing an amulet while assigning a point made the
// buff permanent (a defect, recorded in docs/dead-code-audit.md).

buff_total :: proc(w: ^World, id: Character_ID, s: Stat) -> (total: i32) {
	for item_id in items_worn(w, id) { total += ITEM_TYPES[item_get(w, item_id).type].buffs[s] }
	return
}
stat_of :: proc(w: ^World, id: Character_ID, s: Stat) -> i32 { return character_get(w, id).stats[s] + buff_total(w, id, s) }

health_current :: proc(w: ^World, id: Character_ID) -> i32 { return max(0, stat_of(w, id, .HP) - stat_of(w, id, .Wounds)) }
is_dead :: proc(w: ^World, id: Character_ID) -> bool { return health_current(w, id) <= 0 }
mp_current :: proc(w: ^World, id: Character_ID) -> i32 { return max(0, stat_of(w, id, .MP) - stat_of(w, id, .Stress)) }
mana_current :: proc(w: ^World, id: Character_ID) -> i32 { return max(0, stat_of(w, id, .Mana) - stat_of(w, id, .Fatigue)) }
// "Has willpower" in the original means the character type has a Willpower statistic at all.
is_demoralized :: proc(w: ^World, id: Character_ID) -> bool { return stat_of(w, id, .Willpower) > 0 && mp_current(w, id) <= 0 }

// Sets the current hit points (Health.Current = n writes Wounds = HP - n).
set_health :: proc(w: ^World, id: Character_ID, n: i32) {
	c := character_get(w, id)
	c.stats[.Wounds] = stat_clamped(.Wounds, stat_of(w, id, .HP) - n)
}
lose_hit_point :: proc(w: ^World, id: Character_ID) { set_health(w, id, health_current(w, id) - 1) }
// Healing is a negative change of Wounds (never below zero).
heal :: proc(w: ^World, id: Character_ID, points: i32) { stat_add(character_get(w, id), .Wounds, -points) }

// ---- encumbrance -----------------------------------------------------------------------------------------------------

encumbrance_current :: proc(w: ^World, id: Character_ID) -> (total: i32) {
	for item_id in w.item_order {
		it := &w.items[item_id]
		if (it.holder == .Carried || it.holder == .Equipped) && it.character == id { total += ITEM_TYPES[it.type].stats.encumbrance }
	}
	return
}
encumbrance_maximum :: proc(w: ^World, id: Character_ID) -> i32 { return stat_of(w, id, .Base_Lift) + stat_of(w, id, .Bonus_Lift) * stat_of(w, id, .Strength) }
is_encumbered :: proc(w: ^World, id: Character_ID) -> bool { return encumbrance_current(w, id) > encumbrance_maximum(w, id) }

// ---- who is where --------------------------------------------------------------------------------------------------

// `who` regards x as an enemy when x's type lists who's type among its enemies (CharacterType.Combat.IsEnemy).
is_enemy_of :: proc(w: ^World, x, who: Character_ID) -> bool {
	return character_get(w, who).type in CHARACTER_TYPES[character_get(w, x).type].enemies
}
// The characters at `who`'s location that treat `who` as an enemy, in creation order.
enemies_of :: proc(w: ^World, who: Character_ID) -> []Character_ID {
	out := make([dynamic]Character_ID, context.temp_allocator)
	for id in characters_at(w, character_get(w, who).location) { if id != who && is_enemy_of(w, id, who) { append(&out, id) } }
	return out[:]
}
allies_of :: proc(w: ^World, who: Character_ID) -> []Character_ID {
	out := make([dynamic]Character_ID, context.temp_allocator)
	for id in characters_at(w, character_get(w, who).location) { if !is_enemy_of(w, id, who) { append(&out, id) } }
	return out[:]
}
can_fight :: proc(w: ^World, who: Character_ID) -> bool { return len(enemies_of(w, who)) > 0 }

// Can `who` frighten the first enemy here? Needs influence, and an enemy that has willpower and is not backed by a crowd.
can_do_intimidation :: proc(w: ^World, who: Character_ID) -> bool {
	if stat_of(w, who, .Influence) <= 0 { return false }
	enemies := enemies_of(w, who)
	if len(enemies) == 0 { return false }
	target := enemies[0]
	return stat_of(w, target, .Willpower) > 0 && len(allies_of(w, target)) <= len(enemies_of(w, target))
}

can_map :: proc(w: ^World, who: Character_ID) -> bool {
	return LOCATION_TYPES[location_get(w, character_get(w, who).location).type].can_map
}

has_stairs :: proc(l: ^Location) -> bool {
	for r in l.routes { if r.to != {} && r.type == ROUTE_STAIRS { return true } }
	return false
}

// The first carried item of the type, if any.
carried_item_of_type :: proc(w: ^World, owner: Character_ID, type: Item_Type) -> (Item_ID, bool) {
	for id in items_in_pack(w, owner) { if w.items[id].type == type { return id, true } }
	return {}, false
}

// ---- movement --------------------------------------------------------------------------------------------------------

can_move :: proc(w: ^World, who: Character_ID, d: Direction) -> bool {
	if d == .None || is_encumbered(w, who) { return false }
	c := character_get(w, who)
	r, ok := route_at(location_get(w, c.location), d)
	if !ok { return false }
	if key := ROUTE_TYPES[r.type].unlock_item; key != .None { // locked: needs the key in the pack
		if _, has := carried_item_of_type(w, who, key); !has { return false }
	}
	if LOCATION_TYPES[location_get(w, r.to).type].requires_mp && is_demoralized(w, who) { return false }
	return true
}

// Moves one step. Returns true when the walk starved the character of a hit point (the caller tells the player).
move_character :: proc(w: ^World, who: Character_ID, d: Direction) -> (starved: bool) {
	if !can_move(w, who, d) { return false }
	c := character_get(w, who)
	from := location_get(w, c.location)
	route := from.routes[d]

	stat_add(c, .Hunger, max(c.stats[.Highness] / 2 + c.stats[.Food_Poisoning] / 2, 1))
	stat_add(c, .Drunkenness, -1)
	stat_add(c, .Highness, -1)
	stat_add(c, .Food_Poisoning, -1)
	stat_add(c, .Chafing, -1)

	if key := ROUTE_TYPES[route.type].unlock_item; key != .None { // spend the key, the door stays open
		if item, has := carried_item_of_type(w, who, key); has { destroy_item(w, item) }
		from.routes[d].type = ROUTE_TYPES[route.type].unlocks
		world_play(w, .Unlock_Door)
	}
	location_decay_items(w, c.location)
	if ROUTE_TYPES[route.type].is_single_use { from.routes[d] = {} }
	c = character_get(w, who) // the lookups above may not move the map, but the rule is: no pointer across a call that can change it
	c.location = route.to
	if who == w.player.character { location_get(w, route.to).visited = true }

	if c.stats[.Hunger] == STATS[.Hunger].maximum {
		c.stats[.Hunger] /= 2
		lose_hit_point(w, who)
		return true
	}
	return false
}
