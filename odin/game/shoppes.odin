package game

// Shoppes, repair and spells: the rules behind the townsfolk screens (ShoppeType, Repair, CharacterRepair, CharacterSpellbook).

import "core:fmt"

// ---- what a shoppe deals in --------------------------------------------------------------------------------------------

// The item types a shoppe buys from you (with what it pays), sells (with its price) or repairs (full price), in type order.
Shoppe_Entry :: struct { type: Item_Type, value: i32 }

shoppe_entries :: proc(shoppe: Shoppe_Type, transaction: Transaction) -> []Shoppe_Entry {
	out := make([dynamic]Shoppe_Entry, context.temp_allocator)
	for type in Item_Type {
		if type == .None || transaction not_in ITEM_TYPES[type].shops[shoppe] { continue }
		stats := ITEM_TYPES[type].stats
		value: Maybe(i32)
		switch transaction {
		case .Offer: value = stats.offer
		case .Price: value = stats.price
		case .Repair: value = stats.repair_price
		case .None:
		}
		append(&out, Shoppe_Entry{type, value.? or_else 0})
	}
	return out[:]
}
shoppe_offers :: proc(shoppe: Shoppe_Type) -> []Shoppe_Entry { return shoppe_entries(shoppe, .Offer) }
shoppe_prices :: proc(shoppe: Shoppe_Type) -> []Shoppe_Entry { return shoppe_entries(shoppe, .Price) }

buy_price :: proc(shoppe: Shoppe_Type, type: Item_Type) -> (i32, bool) {
	for e in shoppe_offers(shoppe) { if e.type == type { return e.value, true } }
	return 0, false
}
repair_price :: proc(shoppe: Shoppe_Type, type: Item_Type) -> (i32, bool) {
	for e in shoppe_entries(shoppe, .Repair) { if e.type == type { return e.value, true } }
	return 0, false
}

// ---- selling, buying -------------------------------------------------------------------------------------------------------

// What the player carries that the shoppe would buy, in the order carried.
items_to_sell :: proc(w: ^World, who: Character_ID, shoppe: Shoppe_Type) -> []Item_ID {
	out := make([dynamic]Item_ID, context.temp_allocator)
	for id in items_in_pack(w, who) { if _, ok := buy_price(shoppe, item_get(w, id).type); ok { append(&out, id) } }
	return out[:]
}
sell_item :: proc(w: ^World, who: Character_ID, shoppe: Shoppe_Type, id: Item_ID) {
	it := item_get(w, id)
	if it == nil { return }
	price, ok := buy_price(shoppe, it.type)
	if !ok { return }
	stat_add(character_get(w, who), .Money, price)
	destroy_item(w, id)
}

// What the shoppe sells that the player can afford.
items_to_buy :: proc(w: ^World, who: Character_ID, shoppe: Shoppe_Type) -> []Shoppe_Entry {
	out := make([dynamic]Shoppe_Entry, context.temp_allocator)
	money := stat_of(w, who, .Money)
	for e in shoppe_prices(shoppe) { if e.value <= money { append(&out, e) } }
	return out[:]
}
buy_item :: proc(w: ^World, who: Character_ID, entry: Shoppe_Entry) {
	if stat_of(w, who, .Money) < entry.value { return }
	stat_add(character_get(w, who), .Money, -entry.value)
	give_new_item(w, who, entry.type)
}

// ---- repair --------------------------------------------------------------------------------------------------------------

needs_repair :: proc(it: ^Item) -> bool {
	_, durable := durability_maximum(it)
	return durable && it.wear > 0
}

// The price of mending the item: its wear as a share of the full price, rounded up.
repair_cost :: proc(it: ^Item, shoppe: Shoppe_Type) -> i32 {
	full, _ := repair_price(shoppe, it.type)
	maximum, ok := durability_maximum(it)
	if !ok || maximum == 0 { return 0 }
	remainder: i32 = (it.wear * full) % maximum > 0 ? 1 : 0
	return it.wear * full / maximum + remainder
}

// Carried items first, then worn ones, that are worn down and that the shoppe will mend.
items_to_repair :: proc(w: ^World, who: Character_ID, shoppe: Shoppe_Type) -> []Item_ID {
	out := make([dynamic]Item_ID, context.temp_allocator)
	for id in items_in_pack(w, who) { append_if_repairable(w, &out, id, shoppe) }
	for id in items_worn(w, who) { append_if_repairable(w, &out, id, shoppe) }
	return out[:]
}
@(private)
append_if_repairable :: proc(w: ^World, out: ^[dynamic]Item_ID, id: Item_ID, shoppe: Shoppe_Type) {
	it := item_get(w, id)
	if _, will := repair_price(shoppe, it.type); will && needs_repair(it) { append(out, id) }
}
repair_item :: proc(w: ^World, who: Character_ID, shoppe: Shoppe_Type, id: Item_ID) -> bool {
	it := item_get(w, id)
	if it == nil { return false }
	cost := repair_cost(it, shoppe)
	if stat_of(w, who, .Money) < cost { return false }
	stat_add(character_get(w, who), .Money, -cost)
	it.wear = 0
	return true
}

// ---- spells --------------------------------------------------------------------------------------------------------------

// The spells the player knows, with their levels, in spell order.
known_spells :: proc(w: ^World) -> []Spell_Type {
	out := make([dynamic]Spell_Type, context.temp_allocator)
	for s in Spell_Type { if s != .None && w.player.spells[s] != 0 { append(&out, s) } }
	return out[:]
}
has_spells :: proc(w: ^World) -> bool { return len(known_spells(w)) > 0 }

can_cast :: proc(w: ^World, who: Character_ID, spell: Spell_Type) -> bool {
	return check(SPELL_TYPES[spell].can_cast, {world = w, character = who})
}
cast_spell :: proc(w: ^World, who: Character_ID, spell: Spell_Type) {
	if !can_cast(w, who, spell) {
		message_add(w, .None, fmt.tprintf("You cannot cast %s now.", SPELL_TYPES[spell].name))
		return
	}
	perform(SPELL_TYPES[spell].do_cast, {world = w, character = who})
}

// ---- quests ------------------------------------------------------------------------------------------------------------

quest_active :: proc(w: ^World, q: Quest_Type) -> bool { return q in w.player.quests_active }
quest_can_accept :: proc(w: ^World, who: Character_ID, q: Quest_Type) -> bool {
	return !quest_active(w, q) && check(QUEST_TYPES[q].can_accept, {world = w, character = who})
}
