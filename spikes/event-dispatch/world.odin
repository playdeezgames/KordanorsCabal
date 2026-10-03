package main

// Minimal world for the event spike: the shapes from tasks 14 and 17 plus the output queues from task 11/18.
import "base:runtime"
_ :: runtime

Location_ID :: distinct u32
Route_Slot :: struct { to: Location_ID, type: u8 }
Location :: struct {
	type:    Location_Type,
	level:   Dungeon_Level,
	routes:  [Direction]Route_Slot,
}
Character :: struct { type: Character_Type, location: Location_ID, stats: [Stat]i32 }
Item_Holder :: enum u8 { None, Carried, On_Ground, Equipped }
Item :: struct {
	type: Item_Type, wear: i32, lore: u8,
	holder: Item_Holder, holder_character: Handle, holder_location: Location_ID, seq: u32,
}
Sfx :: enum u8 { None, Character_Creation, Enemy_Death, Enemy_Hit, Level_Up, Miss, Player_Death, Player_Hit, Unlock_Door }

// ---- output queues (replace the static message queue and the .NET Sfx event of the VB game) ----------------
MAX_MESSAGES :: 16
MESSAGE_BYTES :: 768 // longest lore text is 417 characters
Message :: struct { sfx: Sfx, size: u16, text: [MESSAGE_BYTES]u8 } // lines separated by '\n'
MAX_SFX :: 16

World :: struct {
	locations:  [64]Location,
	characters: Pool(Character, 256),
	items:      Pool(Item, 512),
	player:     Handle,
	quest_active, quest_done: [Quest_Type]i32,
	spells:     [Spell_Type]i32,
	item_seq:   u32,
	rng:        u64, // splitmix64 state; the real generator is chosen in task 21
	messages:   [MAX_MESSAGES]Message, // ring buffer
	msg_head, msg_count: int,
	sfx:        [MAX_SFX]Sfx, // immediate sounds raised by game rules
	sfx_count:  int,
	todo_hits:  int, // handlers not ported yet (spike only)
}
