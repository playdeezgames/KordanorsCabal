package game

import "base:runtime"

// The mutable world (decision D9): everything a save contains. Entities are plain values in maps keyed by UUID, plus an order
// list that remembers creation order, because map iteration order is not stable across a save and load but the game must
// behave identically after loading. Rules:
//  * Ids are never reused. Lookups return nil for an id that is gone; callers treat that as "it no longer exists".
//  * A pointer from a lookup is valid only until the next create (the map may move); never hold one across a call.
//  * Items know where they are; characters know where they are; nothing else records either.
//  * No strings: names come from the content tables by type.

Route :: struct {
	to:   Location_ID, // the all-zero id means "no route this way"
	type: Route_Type,
}

Location :: struct {
	type:    Location_Type,
	level:   Dungeon_Level, // .None for the town
	column:  u8,
	row:     u8,
	feature: Feature_Type,
	visited: bool,
	routes:  [Direction]Route,
}

Character :: struct {
	type:     Character_Type,
	location: Location_ID,
	stats:    [Stat]i32, // effective values: the type's initial values, defaults included, then changed by play
}

Item_Holder :: enum u8 { None, Carried, On_Ground, Equipped }

Item :: struct {
	type:      Item_Type,
	wear:      i32, // the only item statistic that changes after creation; durability left = max_durability - wear
	lore:      u8,  // which of the lore texts a note shows; 0 = none chosen yet
	holder:    Item_Holder,
	character: Character_ID, // the carrier or wearer, when holder is Carried or Equipped
	location:  Location_ID,  // where it lies, when holder is On_Ground
	slot:      Equip_Slot,   // when Equipped
	placed:    u32,          // the value of World.placed when it last moved; lists of items are shown in this order
}

Player_State :: struct {
	character:         Character_ID,
	mode:              Player_Mode,
	facing:            Direction,
	shoppe:            Shoppe_Type,
	quests_active:     bit_set[Quest_Type],
	quest_completions: [Quest_Type]i32,
	spells:            [Spell_Type]i32, // level known; 0 = not known
}

World :: struct {
	seed:            u64,
	rng:             Rng,
	placed:          u32,
	locations:       map[Location_ID]Location,
	location_order:  [dynamic]Location_ID,
	characters:      map[Character_ID]Character,
	character_order: [dynamic]Character_ID,
	items:           map[Item_ID]Item,
	item_order:      [dynamic]Item_ID,
	player:          Player_State,
}

// All storage comes from `allocator` (kept inside the maps and lists), so a world can be freed with world_destroy.
world_init :: proc(w: ^World, seed: u64, allocator := context.allocator) {
	w^ = {}
	w.seed = seed
	rng_seed(&w.rng, seed)
	w.locations = make(map[Location_ID]Location, 1024, allocator)
	w.characters = make(map[Character_ID]Character, 2048, allocator)
	w.items = make(map[Item_ID]Item, 1024, allocator)
	w.location_order = make([dynamic]Location_ID, 0, 1024, allocator)
	w.character_order = make([dynamic]Character_ID, 0, 2048, allocator)
	w.item_order = make([dynamic]Item_ID, 0, 1024, allocator)
}

world_destroy :: proc(w: ^World) {
	delete(w.locations)
	delete(w.location_order)
	delete(w.characters)
	delete(w.character_order)
	delete(w.items)
	delete(w.item_order)
	w^ = {}
}

// ---- lookups ------------------------------------------------------------------------------------------------------

location_get :: proc(w: ^World, id: Location_ID) -> ^Location { return &w.locations[id] }
character_get :: proc(w: ^World, id: Character_ID) -> ^Character { return &w.characters[id] }
item_get :: proc(w: ^World, id: Item_ID) -> ^Item { return &w.items[id] }
player_character :: proc(w: ^World) -> ^Character { return &w.characters[w.player.character] }

// ---- creation -----------------------------------------------------------------------------------------------------

@(private)
fresh_id :: proc(w: ^World, taken: proc(w: ^World, id: [16]u8) -> bool) -> [16]u8 {
	for {
		id := uuid_draw(&w.rng)
		if !taken(w, id) { return id }
	}
}

create_location :: proc(w: ^World, type: Location_Type) -> Location_ID {
	id := Location_ID(fresh_id(w, proc(w: ^World, id: [16]u8) -> bool { return Location_ID(id) in w.locations }))
	w.locations[id] = {type = type}
	append(&w.location_order, id)
	return id
}

// A character with the statistics its type starts with.
create_character :: proc(w: ^World, type: Character_Type, location: Location_ID) -> Character_ID {
	id := Character_ID(fresh_id(w, proc(w: ^World, id: [16]u8) -> bool { return Character_ID(id) in w.characters }))
	w.characters[id] = {type = type, location = location, stats = CHARACTER_TYPES[type].initial_stats}
	append(&w.character_order, id)
	return id
}

// An item that is nowhere yet; give it a place at once with one of the item_put_* procedures.
create_item :: proc(w: ^World, type: Item_Type) -> Item_ID {
	id := Item_ID(fresh_id(w, proc(w: ^World, id: [16]u8) -> bool { return Item_ID(id) in w.items }))
	w.items[id] = {type = type}
	append(&w.item_order, id)
	return id
}

// ---- moving items -------------------------------------------------------------------------------------------------

item_put_on_ground :: proc(w: ^World, id: Item_ID, location: Location_ID) {
	it := item_get(w, id)
	w.placed += 1
	it^ = {type = it.type, wear = it.wear, lore = it.lore, holder = .On_Ground, location = location, placed = w.placed}
}
item_put_in_pack :: proc(w: ^World, id: Item_ID, owner: Character_ID) {
	it := item_get(w, id)
	w.placed += 1
	it^ = {type = it.type, wear = it.wear, lore = it.lore, holder = .Carried, character = owner, placed = w.placed}
}
item_put_on :: proc(w: ^World, id: Item_ID, owner: Character_ID, slot: Equip_Slot) {
	it := item_get(w, id)
	w.placed += 1
	it^ = {type = it.type, wear = it.wear, lore = it.lore, holder = .Equipped, character = owner, slot = slot, placed = w.placed}
}

// ---- destruction --------------------------------------------------------------------------------------------------

destroy_item :: proc(w: ^World, id: Item_ID) -> bool {
	if id not_in w.items { return false }
	delete_key(&w.items, id)
	order_remove(&w.item_order, id)
	return true
}

// Destroys what the character carries and wears too (the original drops loot first; whatever was left became unreachable).
// The player cannot be destroyed.
destroy_character :: proc(w: ^World, id: Character_ID) -> bool {
	if id not_in w.characters || id == w.player.character { return false }
	doomed := make([dynamic]Item_ID, context.temp_allocator)
	for item_id in w.item_order {
		it := &w.items[item_id]
		if (it.holder == .Carried || it.holder == .Equipped) && it.character == id { append(&doomed, item_id) }
	}
	for item_id in doomed { destroy_item(w, item_id) }
	delete_key(&w.characters, id)
	order_remove(&w.character_order, id)
	return true
}

@(private)
order_remove :: proc(order: ^[dynamic]$T, id: T) {
	for e, i in order { if e == id { ordered_remove(order, i); return } }
}

// ---- queries (results are in the temporary allocator and valid for the current step only) --------------------------

characters_at :: proc(w: ^World, location: Location_ID) -> []Character_ID {
	out := make([dynamic]Character_ID, context.temp_allocator)
	for id in w.character_order { if w.characters[id].location == location { append(&out, id) } }
	return out[:]
}

locations_of_type :: proc(w: ^World, type: Location_Type) -> []Location_ID {
	out := make([dynamic]Location_ID, context.temp_allocator)
	for id in w.location_order { if w.locations[id].type == type { append(&out, id) } }
	return out[:]
}

// Items in a container, oldest placement first (the order the original showed them in).
@(private)
items_where :: proc(w: ^World, holder: Item_Holder, character: Character_ID, location: Location_ID) -> []Item_ID {
	out := make([dynamic]Item_ID, context.temp_allocator)
	for id in w.item_order {
		it := &w.items[id]
		if it.holder != holder { continue }
		if holder == .On_Ground ? it.location == location : it.character == character { append(&out, id) }
	}
	// insertion sort by `placed`: the lists are short
	for i in 1 ..< len(out) {
		for j := i; j > 0 && w.items[out[j - 1]].placed > w.items[out[j]].placed; j -= 1 { out[j - 1], out[j] = out[j], out[j - 1] }
	}
	return out[:]
}
items_on_ground :: proc(w: ^World, location: Location_ID) -> []Item_ID { return items_where(w, .On_Ground, {}, location) }
items_in_pack :: proc(w: ^World, owner: Character_ID) -> []Item_ID { return items_where(w, .Carried, owner, {}) }
items_worn :: proc(w: ^World, owner: Character_ID) -> []Item_ID { return items_where(w, .Equipped, owner, {}) }

item_in_slot :: proc(w: ^World, owner: Character_ID, slot: Equip_Slot) -> (Item_ID, bool) {
	for id in items_worn(w, owner) { if w.items[id].slot == slot { return id, true } }
	return {}, false
}

// ---- statistics ---------------------------------------------------------------------------------------------------

stat_clamped :: proc(s: Stat, v: i32) -> i32 { return clamp(v, STATS[s].minimum, STATS[s].maximum) }
stat_set :: proc(c: ^Character, s: Stat, v: i32) { c.stats[s] = stat_clamped(s, v) }
stat_add :: proc(c: ^Character, s: Stat, delta: i32) { c.stats[s] = stat_clamped(s, c.stats[s] + delta) }

route_at :: proc(l: ^Location, d: Direction) -> (Route, bool) { return l.routes[d], l.routes[d].to != {} }
route_count :: proc(l: ^Location) -> (n: int) {
	for r in l.routes { if r.to != {} { n += 1 } }
	return
}

_ :: runtime
