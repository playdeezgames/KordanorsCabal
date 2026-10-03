package main

// Draft of the in-memory world (task 14 spike). Single Handle type for now (see task 17 log).
MAX_LOCATIONS :: 800
CHAR_CAP :: 2048
ITEM_CAP :: 4096

Location_ID :: distinct u32

Route_Slot :: struct { to: Location_ID, type: Route_Type }

Location :: struct {
	type:    Location_Type,
	level:   Dungeon_Level,
	column:  u8,
	row:     u8,
	feature: Feature_Type,
	visited: bool,
	routes:  [Direction]Route_Slot,
}

Character :: struct {
	type:     Character_Type,
	location: Location_ID,
	stats:    [Stat]i32,
}

Item_Holder :: enum u8 { None, Carried, On_Ground, Equipped }

Item :: struct {
	type:             Item_Type,
	wear:             i32,
	lore:             u8,
	holder:           Item_Holder,
	holder_character: Handle,
	holder_location:  Location_ID,
	equip_slot:       Equip_Slot,
	seq:              u32,
}

Player_State :: struct {
	character:         Handle,
	mode:              Player_Mode,
	facing:            Direction,
	shoppe:            Shoppe_Type,
	quests_active:     bit_set[Quest_Type; u8],
	quest_completions: [Quest_Type]i32,
	spells:            [Spell_Type]i32,
}

World :: struct {
	locations:      [MAX_LOCATIONS]Location,
	location_count: u32, // index 0 unused; valid ids are 1 ..< location_count
	characters:     Pool(Character, CHAR_CAP),
	items:          Pool(Item, ITEM_CAP),
	player:         Player_State,
	item_seq:       u32,
	rng_state:      [4]u64,
}
