package game

// Static game content: the shapes of the definitions. The data (content_data.odin) and the enums (content_enums.odin) are
// generated from src/KordanorsCabal/boilerplate.db by tools/gen/gen_content.py (task 27). Every enum value equals the original
// database id, and 0 is always `None`, so a zero value means "nothing".

Transaction :: enum u8 { None, Offer, Price, Repair } // what a shoppe does with an item: buys it from you, sells it to you, repairs it
Attack_Type :: enum u8 { None, Physical, Mental }

Item_Stats :: struct {
	single_use:     bool,
	encumbrance:    i32,
	attack_dice:    i32,
	defend_dice:    i32,
	max_damage:     Maybe(i32),
	max_durability: Maybe(i32),
	offer:          Maybe(i32), // what a shoppe pays
	price:          Maybe(i32), // what a shoppe asks
	repair_price:   Maybe(i32),
}

Spawn_Rule :: struct {
	dice:      Dice,                    // how many appear on a dungeon level
	locations: bit_set[Location_Type],  // where they may be placed
}

Item_Events :: struct { purify: Action, can_use: Check, use: Action, decay: Action }

Item_Type_Def :: struct {
	name:   string,
	stats:  Item_Stats,
	equip:  bit_set[Equip_Slot],
	events: Item_Events,
	shops:  [Shoppe_Type]bit_set[Transaction],
	spawn:  [Dungeon_Level]Spawn_Rule,
	buffs:  [Stat]i32, // added to the wearer's statistics while equipped
}

MAX_LOOT :: 4          // the most loot entries any character type has in the data
MAX_PARTING_SHOTS :: 2
Loot_Entry :: struct { item: Item_Type, weight: int } // item None means "nothing drops"; weight 0 marks an unused slot
Weighted_Text :: struct { text: string, weight: int } // weight 0 marks an unused slot

Character_Type_Def :: struct {
	name:            string,
	xp_value:        i32,
	money_dice:      Dice,
	is_undead:       bool,
	initial_stats:   [Stat]i32,
	attack_weights:  [Attack_Type]int,
	bribes:          bit_set[Item_Type],
	enemies:         bit_set[Character_Type],
	loot:            [MAX_LOOT]Loot_Entry,
	parting_shots:   [MAX_PARTING_SHOTS]Weighted_Text,
	spawn_count:     [Dungeon_Level]int,
	spawn_locations: [Dungeon_Level]bit_set[Location_Type],
}

Stat_Def :: struct { name, abbreviation: string, minimum: i32, default: Maybe(i32), maximum: i32 }
Direction_Def :: struct { name, abbreviation: string, is_cardinal: bool, previous, opposite, next: Direction }
Location_Type_Def :: struct { name: string, is_dungeon, can_map, requires_mp: bool }
Feature_Type_Def :: struct { name: string, location_type: Location_Type, interaction_mode: int } // mode ids are the VB PlayerModes
Route_Type_Def :: struct { abbreviation: string, is_single_use: bool, unlocks: Route_Type, unlock_item: Item_Type }
Spell_Type_Def :: struct { name: string, maximum_level: i32, can_cast: Check, do_cast: Action, required_power: [2]i32 } // power needed for level 0 and 1
Quest_Type_Def :: struct { can_accept: Check, accept: Action, can_complete: Check, complete: Action }
Lore_Def :: struct { item_name, text: string }
