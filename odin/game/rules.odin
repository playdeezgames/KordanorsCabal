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

// ---- health and friends ----------------------------------------------------------------------------------------------

health_current :: proc(c: ^Character) -> i32 { return max(0, c.stats[.HP] - c.stats[.Wounds]) }
is_dead :: proc(c: ^Character) -> bool { return health_current(c) <= 0 }
mp_current :: proc(c: ^Character) -> i32 { return max(0, c.stats[.MP] - c.stats[.Stress]) }
mana_current :: proc(c: ^Character) -> i32 { return max(0, c.stats[.Mana] - c.stats[.Fatigue]) }
// "Has willpower" in the original means the character type has a Willpower statistic at all.
is_demoralized :: proc(c: ^Character) -> bool { return c.stats[.Willpower] > 0 && mp_current(c) <= 0 }

// Takes one hit point (the original sets Health.Current to current - 1).
lose_hit_point :: proc(c: ^Character) {
	c.stats[.Wounds] = stat_clamped(.Wounds, c.stats[.HP] - (health_current(c) - 1))
}

// ---- encumbrance -----------------------------------------------------------------------------------------------------

encumbrance_current :: proc(w: ^World, id: Character_ID) -> (total: i32) {
	for item_id in w.item_order {
		it := &w.items[item_id]
		if (it.holder == .Carried || it.holder == .Equipped) && it.character == id { total += ITEM_TYPES[it.type].stats.encumbrance }
	}
	return
}
encumbrance_maximum :: proc(c: ^Character) -> i32 { return c.stats[.Base_Lift] + c.stats[.Bonus_Lift] * c.stats[.Strength] }
is_encumbered :: proc(w: ^World, id: Character_ID) -> bool {
	return encumbrance_current(w, id) > encumbrance_maximum(character_get(w, id))
}

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
	if character_get(w, who).stats[.Influence] <= 0 { return false }
	enemies := enemies_of(w, who)
	if len(enemies) == 0 { return false }
	target := enemies[0]
	return character_get(w, target).stats[.Willpower] > 0 && len(allies_of(w, target)) <= len(enemies_of(w, target))
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
	if LOCATION_TYPES[location_get(w, r.to).type].requires_mp && is_demoralized(c) { return false }
	return true
}

// The raising of `Location.DecayItems` (the Location_Decay_Items action: food rots) belongs to the item events, ported with
// the items (PORT.md step 4). Until then nothing decays.
location_decay_items :: proc(w: ^World, l: Location_ID) {}

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
		lose_hit_point(c)
		return true
	}
	return false
}
