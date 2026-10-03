package game

// Items: names, durability, picking up, dropping, wearing (Item, Durability, Equipment, Inventory, CharacterEquipment and
// CharacterItems in the original). The events that items trigger are in events.odin.

import "core:fmt"

// The name shown for an item: a note shows the title of its lore, everything else its type's name.
item_name :: proc(it: ^Item) -> string { return it.lore != 0 ? LORES[it.lore].item_name : ITEM_TYPES[it.type].name }

// Durability left, for the types that wear out (max_durability is the data's maximum and `wear` counts up from zero).
durability_maximum :: proc(it: ^Item) -> (i32, bool) {
	m, ok := ITEM_TYPES[it.type].stats.max_durability.?
	return m, ok
}
durability_current :: proc(it: ^Item) -> (i32, bool) {
	m, ok := durability_maximum(it)
	return m - it.wear, ok
}
// Adds wear; returns true when the item is now broken (the caller destroys it).
wear_out :: proc(it: ^Item, amount: i32) -> bool {
	if _, ok := durability_maximum(it); !ok { return false }
	it.wear += amount
	cur, _ := durability_current(it)
	return cur <= 0
}

// ---- moving items around ----------------------------------------------------------------------------------------

// Takes a ground item into the pack. Already carried by `who` does nothing.
pick_up :: proc(w: ^World, who: Character_ID, id: Item_ID) {
	it := item_get(w, id)
	if it == nil || (it.holder == .Carried && it.character == who) { return }
	item_put_in_pack(w, id, who)
}

drop_item :: proc(w: ^World, who: Character_ID, id: Item_ID) {
	if item_get(w, id) == nil { return }
	item_put_on_ground(w, id, character_get(w, who).location)
}

// ---- wearing ----------------------------------------------------------------------------------------------------

can_equip :: proc(it: ^Item) -> bool { return ITEM_TYPES[it.type].equip != {} }

// Wears the item in the first free slot it fits, or in its first slot, sending what was there back to the pack. Tells the
// player what happened, as the original did.
equip_item :: proc(w: ^World, who: Character_ID, id: Item_ID) {
	it := item_get(w, id)
	if it == nil { return }
	if !can_equip(it) {
		message_add(w, .None, fmt.tprintf("You cannot equip %s!", item_name(it)))
		return
	}
	slots := ITEM_TYPES[it.type].equip
	chosen := Equip_Slot.None
	for slot in slots { // the first slot of the item's list that is empty
		if _, taken := item_in_slot(w, who, slot); !taken { chosen = slot; break }
	}
	if chosen == .None { for slot in slots { chosen = slot; break } }
	if old, has := item_in_slot(w, who, chosen); has { item_put_in_pack(w, old, who) }
	item_put_on(w, id, who, chosen)
	message_add(w, .None, fmt.tprintf("You equip %s to %s.", item_name(item_get(w, id)), EQUIP_SLOT_NAME[chosen]))
}

unequip :: proc(w: ^World, who: Character_ID, slot: Equip_Slot) {
	if id, has := item_in_slot(w, who, slot); has { item_put_in_pack(w, id, who) }
}

// ---- lists the screens show ----------------------------------------------------------------------------------------

// What the player carries, grouped by name and sorted by name (case-insensitively), each group in placement order.
Item_Group :: struct { name: string, items: []Item_ID }

item_groups :: proc(w: ^World, owner: Character_ID) -> []Item_Group {
	pack := items_in_pack(w, owner)
	groups := make([dynamic]Item_Group, context.temp_allocator)
	for id in pack {
		name := item_name(item_get(w, id))
		found := false
		for &g in groups { if g.name == name { found = true; break } }
		if !found { append(&groups, Item_Group{name = name}) }
	}
	// insertion sort by name; there are at most a few dozen
	for i in 1 ..< len(groups) {
		for j := i; j > 0 && compare_names(groups[j - 1].name, groups[j].name) > 0; j -= 1 { groups[j - 1], groups[j] = groups[j], groups[j - 1] }
	}
	for &g in groups {
		members := make([dynamic]Item_ID, context.temp_allocator)
		for id in pack { if item_name(item_get(w, id)) == g.name { append(&members, id) } }
		g.items = members[:]
	}
	return groups[:]
}

compare_names :: proc(a, b: string) -> int {
	for i in 0 ..< min(len(a), len(b)) {
		x, y := lower_ascii(a[i]), lower_ascii(b[i])
		if x != y { return x < y ? -1 : 1 }
	}
	return len(a) - len(b)
}
lower_ascii :: proc(c: u8) -> u8 { return c >= 'A' && c <= 'Z' ? c + 32 : c }
