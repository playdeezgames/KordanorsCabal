package main

import "core:encoding/json"
import "core:fmt"
import "core:reflect"

SAVE_FORMAT :: "kordanors-cabal-save"
SAVE_VERSION :: 1


Save_Location :: struct {
	type:    Location_Type,
	level:   Dungeon_Level,
	column:  u8,
	row:     u8,
	feature: Feature_Type,
	visited: bool,
	routes:  [][3]i32, // [direction id, destination location id, route type id]
}

Save_Character :: struct {
	type:     Character_Type,
	location: Location_ID,
	stats:    [][2]i32, // [stat id, value] for non-zero stats, in stat order (ids are the stable VB ids; Odin cannot marshal enum-keyed maps and map order is not deterministic)
}

Save_World :: struct {
	item_seq:   u32,
	rng_state:  [4]u64,
	player:     Player_State,
	locations:  []Save_Location, // index i is location id i + 1
	characters: Saved_Pool(Save_Character),
	items:      Saved_Pool(Item),
}

Save_Summary :: struct { place: Dungeon_Level, hp: i32, xp: i32 }

Save_File :: struct {
	format:  string,
	version: int,
	summary: Save_Summary,
	world:   Save_World,
}

// ---- world -> file -------------------------------------------------------------------------------

to_save_file :: proc(w: ^World, allocator := context.allocator) -> Save_File {
	context.allocator = allocator
	sw: Save_World
	sw.item_seq = w.item_seq
	sw.rng_state = w.rng_state
	sw.player = w.player

	sw.locations = make([]Save_Location, int(w.location_count) - 1)
	for i in 1 ..< int(w.location_count) {
		l := &w.locations[i]
		routes: [dynamic][3]i32
		for dir in Direction {
			if r := l.routes[dir]; r.to != 0 { append(&routes, [3]i32{i32(dir), i32(r.to), i32(r.type)}) }
		}
		sw.locations[i - 1] = {l.type, l.level, l.column, l.row, l.feature, l.visited, routes[:]}
	}

	sw.characters.used_top = w.characters.used_top
	sw.characters.free_head = w.characters.free_head
	sw.characters.slots = make([dynamic]Slot(Save_Character), 0, int(w.characters.used_top))
	for i in 0 ..< int(w.characters.used_top) {
		s := w.characters.slots[i]
		out := Slot(Save_Character){gen = s.gen, alive = s.alive, next_free = s.next_free}
		if s.alive {
			out.value = {s.value.type, s.value.location, {}}
			pairs: [dynamic][2]i32
			for st in Stat {
				if v := s.value.stats[st]; v != 0 { append(&pairs, [2]i32{i32(st), v}) }
			}
			out.value.stats = pairs[:]
		}
		append(&sw.characters.slots, out)
	}
	sw.items = pool_to_saved(&w.items)

	summary: Save_Summary
	if p, ok := pool_get(&w.characters, w.player.character); ok {
		summary = {w.locations[p.location].level, p.stats[.HP] - p.stats[.Wounds], p.stats[.XP]}
	}
	return {SAVE_FORMAT, SAVE_VERSION, summary, sw}
}

save_to_json :: proc(w: ^World, pretty := false, allocator := context.allocator) -> ([]byte, json.Marshal_Error) {
	context.allocator = allocator
	f := to_save_file(w)
	return json.marshal(f, {pretty = pretty, use_enum_names = true, spec = .JSON})
}

// ---- file -> world (all failure paths return an error string; never crashes on bad data) ----------

Load_Error :: enum { None, Parse, Format, Version, Shape, Invariant }

valid_enum :: proc(v: $T) -> bool { return reflect.enum_value_has_name(v) }

load_from_json :: proc(data: []byte, w: ^World, parse_allocator := context.allocator) -> (err: Load_Error, msg: string) {
	f: Save_File
	if uerr := json.unmarshal(data, &f, .JSON, parse_allocator); uerr != nil { return .Parse, fmt.tprint(uerr) }
	if f.format != SAVE_FORMAT { return .Format, "not a Kordanor's Cabal save" }
	if f.version != SAVE_VERSION { return .Version, fmt.tprintf("unsupported version %d", f.version) }
	sw := &f.world
	if len(sw.locations) + 1 > MAX_LOCATIONS { return .Shape, "too many locations" }
	if int(sw.characters.used_top) != len(sw.characters.slots) || int(sw.characters.used_top) > CHAR_CAP || sw.characters.used_top < 1 { return .Shape, "character pool" }
	if int(sw.items.used_top) != len(sw.items.slots) || int(sw.items.used_top) > ITEM_CAP || sw.items.used_top < 1 { return .Shape, "item pool" }

	w^ = {}
	w.location_count = u32(len(sw.locations)) + 1
	for sl, i in sw.locations {
		if !valid_enum(sl.type) || !valid_enum(sl.level) || !valid_enum(sl.feature) { return .Shape, "location enum" }
		l := &w.locations[i + 1]
		l^ = {type = sl.type, level = sl.level, column = sl.column, row = sl.row, feature = sl.feature, visited = sl.visited}
		for r in sl.routes {
			dir, rtype := Direction(r[0]), Route_Type(r[2])
			if r[0] < 1 || r[0] > 255 || r[2] < 0 || r[2] > 255 || !valid_enum(dir) || !valid_enum(rtype) || r[1] < 1 || int(r[1]) >= int(w.location_count) { return .Shape, "route" }
			l.routes[dir] = {Location_ID(r[1]), rtype}
		}
	}
	w.characters.used_top = sw.characters.used_top
	w.characters.free_head = sw.characters.free_head
	for s, i in sw.characters.slots {
		d := &w.characters.slots[i]
		d.gen, d.alive, d.next_free = s.gen, s.alive, s.next_free
		if s.alive {
			if i == 0 || !valid_enum(s.value.type) || int(s.value.location) >= int(w.location_count) || s.value.location == 0 { return .Shape, "character" }
			d.value.type, d.value.location = s.value.type, s.value.location
			for pair in s.value.stats {
				st := Stat(pair[0])
				if pair[0] < 1 || pair[0] > 255 || !valid_enum(st) { return .Shape, "unknown stat id" }
				d.value.stats[st] = pair[1]
			}
			w.characters.count += 1
		}
	}
	if !pool_from_saved(&w.items, sw.items) { return .Shape, "item pool restore" }
	w.item_seq = sw.item_seq
	w.rng_state = sw.rng_state
	w.player = sw.player
	if ok, why := world_validate(w); !ok { return .Invariant, why }
	return .None, ""
}

// ---- invariants -------------------------------------------------------------------------------

world_validate :: proc(w: ^World) -> (ok: bool, why: string) {
	if _, alive := pool_get(&w.characters, w.player.character); !alive { return false, "player is not a live character" }
	if !valid_enum(w.player.mode) || !valid_enum(w.player.facing) || !valid_enum(w.player.shoppe) { return false, "player enum" }
	// free lists must only visit dead slots below used_top and terminate
	for pool_kind in 0 ..< 2 {
		top, head := w.characters.used_top, w.characters.free_head
		if pool_kind == 1 { top, head = w.items.used_top, w.items.free_head }
		steps := 0
		for i := head; i != 0; {
			if i >= top { return false, "free list leaves pool" }
			alive := pool_kind == 0 ? w.characters.slots[i].alive : w.items.slots[i].alive
			if alive { return false, "free list contains live slot" }
			i = pool_kind == 0 ? w.characters.slots[i].next_free : w.items.slots[i].next_free
			steps += 1
			if steps > int(top) { return false, "free list cycle" }
		}
	}
	cur: u32 = 0
	equipped: map[u64]bool
	defer delete(equipped)
	for _, it in pool_next(&w.items, &cur) {
		if !valid_enum(it.type) || it.type == .None || !valid_enum(it.holder) || !valid_enum(it.equip_slot) { return false, "item enum" }
		switch it.holder {
		case .None: return false, "item has no holder"
		case .Carried, .Equipped:
			if _, alive := pool_get(&w.characters, it.holder_character); !alive { return false, "item holder character is gone" }
			if it.holder == .Equipped {
				key := u64(it.holder_character) << 8 | u64(it.equip_slot)
				if key in equipped { return false, "two items in one equip slot" }
				equipped[key] = true
			}
		case .On_Ground:
			if it.holder_location == 0 || int(it.holder_location) >= int(w.location_count) { return false, "item on invalid location" }
		}
	}
	cur = 0
	for _, ch in pool_next(&w.characters, &cur) {
		if ch.location == 0 || int(ch.location) >= int(w.location_count) { return false, "character on invalid location" }
	}
	return true, ""
}
