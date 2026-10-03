package game

// The save file (task 14, amended by decision review 1): one JSON text, `{format, version, summary, world}`. Entities are
// arrays of records in creation order, every reference is the id's text. Odin's JSON cannot write enum-keyed maps and ignores
// omitempty, so the file shape is its own set of plain structs, converted to and from the World here. The loader trusts
// nothing: every enum, id and reference is checked, and the result must pass world_validate.

import "core:encoding/json"
import "core:fmt"
import "core:reflect"
import "core:strconv"
import "core:strings"

SAVE_FORMAT :: "kordanors-cabal-save"
SAVE_VERSION :: 1
SAVE_MAX_LOCATIONS :: 4096
SAVE_MAX_CHARACTERS :: 8192
SAVE_MAX_ITEMS :: 16384

Save_Route :: struct { d: Direction, to: string, t: Route_Type }
Save_Location :: struct {
	id:      string,
	type:    Location_Type,
	level:   Dungeon_Level,
	column:  u8,
	row:     u8,
	feature: Feature_Type,
	visited: bool,
	routes:  []Save_Route,
}
Save_Character :: struct {
	id:       string,
	type:     Character_Type,
	location: string,
	stats:    [][2]i32, // [stat id, value] for the non-zero statistics
}
Save_Item :: struct {
	id:     string,
	type:   Item_Type,
	wear:   i32,
	lore:   u8,
	holder: Item_Holder,
	owner:  string, // the character id when Carried or Equipped, the location id when On_Ground
	slot:   Equip_Slot,
	placed: u32,
}
Save_Player :: struct {
	character:   string,
	mode:        Player_Mode,
	facing:      Direction,
	shoppe:      Shoppe_Type,
	quests:      []Quest_Type, // accepted and not yet completed
	completions: [][2]i32,     // [quest id, times completed]
	spells:      [][2]i32,     // [spell id, level]
}
Save_World :: struct {
	seed:       string, // 16 hex digits
	rng:        string, // 64 hex digits: the four state words
	placed:     u32,
	player:     Save_Player,
	locations:  []Save_Location,
	characters: []Save_Character,
	items:      []Save_Item,
}
Save_Summary :: struct { place: Dungeon_Level, hp: i32, xp: i32 } // for the slot labels, readable without building a world
Save_File :: struct {
	format:  string,
	version: int,
	summary: Save_Summary,
	world:   Save_World,
}

// ---- world -> text -------------------------------------------------------------------------------------------------

// The intermediate strings and arrays come from the temporary allocator (valid until the next free_all); the returned text is
// allocated with `allocator` and belongs to the caller.
world_save :: proc(w: ^World, pretty := false, allocator := context.allocator) -> ([]byte, json.Marshal_Error) {
	ta := context.temp_allocator
	sw: Save_World
	sw.seed = fmt.aprintf("%016x", w.seed, allocator = ta)
	sw.rng = fmt.aprintf("%016x%016x%016x%016x", w.rng.s[0], w.rng.s[1], w.rng.s[2], w.rng.s[3], allocator = ta)
	sw.placed = w.placed

	p := &w.player
	sp := Save_Player{character = uuid_string(p.character, ta), mode = p.mode, facing = p.facing, shoppe = p.shoppe}
	quests := make([dynamic]Quest_Type, ta)
	completions := make([dynamic][2]i32, ta)
	for q in Quest_Type {
		if q in p.quests_active { append(&quests, q) }
		if p.quest_completions[q] != 0 { append(&completions, [2]i32{i32(q), p.quest_completions[q]}) }
	}
	spells := make([dynamic][2]i32, ta)
	for s in Spell_Type { if p.spells[s] != 0 { append(&spells, [2]i32{i32(s), p.spells[s]}) } }
	sp.quests, sp.completions, sp.spells = quests[:], completions[:], spells[:]
	sw.player = sp

	sw.locations = make([]Save_Location, len(w.location_order), ta)
	for id, i in w.location_order {
		l := &w.locations[id]
		routes := make([dynamic]Save_Route, ta)
		for d in Direction { if r := l.routes[d]; r.to != {} { append(&routes, Save_Route{d, uuid_string(r.to, ta), r.type}) } }
		sw.locations[i] = {uuid_string(id, ta), l.type, l.level, l.column, l.row, l.feature, l.visited, routes[:]}
	}
	sw.characters = make([]Save_Character, len(w.character_order), ta)
	for id, i in w.character_order {
		c := &w.characters[id]
		pairs := make([dynamic][2]i32, ta)
		for s in Stat { if v := c.stats[s]; v != 0 { append(&pairs, [2]i32{i32(s), v}) } }
		sw.characters[i] = {uuid_string(id, ta), c.type, uuid_string(c.location, ta), pairs[:]}
	}
	sw.items = make([]Save_Item, len(w.item_order), ta)
	for id, i in w.item_order {
		it := &w.items[id]
		owner: string
		#partial switch it.holder {
		case .Carried, .Equipped: owner = uuid_string(it.character, ta)
		case .On_Ground: owner = uuid_string(it.location, ta)
		}
		sw.items[i] = {uuid_string(id, ta), it.type, it.wear, it.lore, it.holder, owner, it.slot, it.placed}
	}

	summary: Save_Summary
	if c, ok := &w.characters[p.character]; ok {
		summary = {w.locations[c.location].level, c.stats[.HP] - c.stats[.Wounds], c.stats[.XP]}
	}
	file := Save_File{SAVE_FORMAT, SAVE_VERSION, summary, sw}
	return json.marshal(file, {pretty = pretty, use_enum_names = true, spec = .JSON}, allocator)
}

// ---- text -> world -------------------------------------------------------------------------------------------------

Load_Error :: enum { None, Parse, Format, Version, Shape, Invariant }

valid_enum :: proc(v: $T) -> bool { return reflect.enum_value_has_name(v) }

// Reads just the summary near the start of the text, for slot labels; nothing else is parsed or checked, so a slot that
// passes this can still fail world_load. ok is false for anything that does not start like a save of this version.
save_peek_summary :: proc(text: string, parse_allocator := context.allocator) -> (s: Save_Summary, ok: bool) {
	head := text[:min(len(text), 512)]
	if !strings.has_prefix(head, `{"format":"` + SAVE_FORMAT + `","version":1,"summary":`) { return {}, false }
	start := strings.index(head, `"summary":`) + len(`"summary":`)
	end := strings.index_byte(head[start:], '}')
	if end < 0 { return {}, false }
	if json.unmarshal(transmute([]byte)head[start:start + end + 1], &s, .JSON, parse_allocator) != nil { return {}, false }
	return s, valid_enum(s.place)
}

// Builds `w` (uninitialised, or empty) from the text. On any error `w` is left empty and nothing leaks. `parse_allocator`
// receives the parsed intermediate form and may be an arena that the caller discards afterwards (decision D19); the world's own
// storage comes from `world_allocator`.
world_load :: proc(w: ^World, data: []byte, parse_allocator := context.allocator, world_allocator := context.allocator) -> (err: Load_Error, msg: string) {
	f: Save_File
	if uerr := json.unmarshal(data, &f, .JSON, parse_allocator); uerr != nil { return .Parse, "not valid JSON" }
	if f.format != SAVE_FORMAT { return .Format, "not a Kordanor's Cabal save" }
	if f.version != SAVE_VERSION { return .Version, fmt.tprintf("unsupported save version %d", f.version) }
	sw := &f.world
	if len(sw.locations) > SAVE_MAX_LOCATIONS || len(sw.characters) > SAVE_MAX_CHARACTERS || len(sw.items) > SAVE_MAX_ITEMS { return .Shape, "too many entities" }

	world_init(w, 0, world_allocator)
	fail :: proc(w: ^World, e: Load_Error, m: string) -> (Load_Error, string) {
		world_destroy(w)
		return e, m
	}

	seed, seed_ok := strconv.parse_u64_of_base(sw.seed, 16)
	if !seed_ok || len(sw.seed) != 16 || len(sw.rng) != 64 { return fail(w, .Shape, "seed or generator state") }
	w.seed = seed
	for i in 0 ..< 4 {
		word, ok := strconv.parse_u64_of_base(sw.rng[i * 16:(i + 1) * 16], 16)
		if !ok { return fail(w, .Shape, "generator state") }
		w.rng.s[i] = word
	}
	if w.rng.s == {} { return fail(w, .Shape, "generator state is all zero") }
	w.placed = sw.placed

	// pass 1: ids (so references can be checked against the full set)
	for sl in sw.locations {
		id, ok := uuid_parse(sl.id)
		if !ok || Location_ID(id) in w.locations { return fail(w, .Shape, "location id") }
		if !valid_enum(sl.type) || sl.type == .None || !valid_enum(sl.level) || !valid_enum(sl.feature) { return fail(w, .Shape, "location enum") }
		if sl.column >= MAZE_COLS || sl.row >= MAZE_ROWS { return fail(w, .Shape, "location position") }
		w.locations[Location_ID(id)] = {type = sl.type, level = sl.level, column = sl.column, row = sl.row, feature = sl.feature, visited = sl.visited}
		append(&w.location_order, Location_ID(id))
	}
	for sc in sw.characters {
		id, ok := uuid_parse(sc.id)
		if !ok || Character_ID(id) in w.characters { return fail(w, .Shape, "character id") }
		if !valid_enum(sc.type) || sc.type == .None { return fail(w, .Shape, "character type") }
		at, at_ok := uuid_parse(sc.location)
		if !at_ok || Location_ID(at) not_in w.locations { return fail(w, .Shape, "character location") }
		c := Character{type = sc.type, location = Location_ID(at)}
		for pair in sc.stats {
			s := Stat(pair[0])
			if pair[0] < 1 || pair[0] > 255 || !valid_enum(s) { return fail(w, .Shape, "unknown statistic") }
			if pair[1] < STATS[s].minimum || pair[1] > STATS[s].maximum { return fail(w, .Shape, "statistic out of range") }
			c.stats[s] = pair[1]
		}
		w.characters[Character_ID(id)] = c
		append(&w.character_order, Character_ID(id))
	}
	// pass 2: routes and items, now that every id is known
	for sl, i in sw.locations {
		l := &w.locations[w.location_order[i]]
		for r in sl.routes {
			to, ok := uuid_parse(r.to)
			if !ok || Location_ID(to) not_in w.locations || !valid_enum(r.d) || r.d == .None || !valid_enum(r.t) || r.t == .None { return fail(w, .Shape, "route") }
			if l.routes[r.d].to != {} { return fail(w, .Shape, "two routes in one direction") }
			l.routes[r.d] = {Location_ID(to), r.t}
		}
	}
	for si in sw.items {
		id, ok := uuid_parse(si.id)
		if !ok || Item_ID(id) in w.items { return fail(w, .Shape, "item id") }
		if !valid_enum(si.type) || si.type == .None || !valid_enum(si.holder) || si.holder == .None || !valid_enum(si.slot) { return fail(w, .Shape, "item enum") }
		if si.wear < 0 || int(si.lore) > LORE_COUNT { return fail(w, .Shape, "item values") }
		owner, owner_ok := uuid_parse(si.owner)
		if !owner_ok { return fail(w, .Shape, "item owner") }
		it := Item{type = si.type, wear = si.wear, lore = si.lore, holder = si.holder, placed = si.placed}
		switch si.holder {
		case .None: unreachable()
		case .On_Ground:
			if Location_ID(owner) not_in w.locations { return fail(w, .Shape, "item location") }
			it.location = Location_ID(owner)
		case .Carried, .Equipped:
			if Character_ID(owner) not_in w.characters { return fail(w, .Shape, "item carrier") }
			it.character = Character_ID(owner)
			if si.holder == .Equipped {
				if si.slot == .None { return fail(w, .Shape, "equipped item without a slot") }
				it.slot = si.slot
			}
		}
		w.items[Item_ID(id)] = it
		append(&w.item_order, Item_ID(id))
	}

	sp := &sw.player
	pid, pid_ok := uuid_parse(sp.character)
	if !pid_ok || Character_ID(pid) not_in w.characters { return fail(w, .Invariant, "the save has no live player") }
	if !valid_enum(sp.mode) || !valid_enum(sp.facing) || !valid_enum(sp.shoppe) { return fail(w, .Shape, "player enum") }
	w.player = {character = Character_ID(pid), mode = sp.mode, facing = sp.facing, shoppe = sp.shoppe}
	for q in sp.quests {
		if !valid_enum(q) || q == .None { return fail(w, .Shape, "quest") }
		w.player.quests_active += {q}
	}
	for pair in sp.completions {
		q := Quest_Type(pair[0])
		if pair[0] < 1 || pair[0] > 255 || !valid_enum(q) || pair[1] < 0 { return fail(w, .Shape, "quest completion") }
		w.player.quest_completions[q] = pair[1]
	}
	for pair in sp.spells {
		s := Spell_Type(pair[0])
		if pair[0] < 1 || pair[0] > 255 || !valid_enum(s) || pair[1] < 0 || pair[1] > SPELL_TYPES[s].maximum_level { return fail(w, .Shape, "spell") }
		w.player.spells[s] = pair[1]
	}
	if ok, why := world_validate(w); !ok { return fail(w, .Invariant, why) }
	return .None, ""
}

// ---- invariants -----------------------------------------------------------------------------------------------------

// Everything that must hold in any world, fresh or loaded or after play. Returns the first violation.
world_validate :: proc(w: ^World) -> (ok: bool, why: string) {
	if len(w.location_order) != len(w.locations) || len(w.character_order) != len(w.characters) || len(w.item_order) != len(w.items) {
		return false, "an order list and its map disagree"
	}
	if _, alive := &w.characters[w.player.character]; !alive { return false, "the player is not a live character" }
	if !valid_enum(w.player.mode) || !valid_enum(w.player.facing) || !valid_enum(w.player.shoppe) { return false, "player enum" }
	for id in w.location_order {
		l, found := &w.locations[id]
		if !found { return false, "location order lists a missing location" }
		for d in Direction {
			if r := l.routes[d]; r.to != {} && r.to not_in w.locations { return false, "route to a missing location" }
		}
	}
	for id in w.character_order {
		c, found := &w.characters[id]
		if !found { return false, "character order lists a missing character" }
		if c.location not_in w.locations { return false, "character at a missing location" }
	}
	worn := make(map[[17]u8]bool, context.temp_allocator)
	for id in w.item_order {
		it, found := &w.items[id]
		if !found { return false, "item order lists a missing item" }
		if it.type == .None || !valid_enum(it.type) { return false, "item type" }
		switch it.holder {
		case .None: return false, "item has no holder"
		case .On_Ground:
			if it.location not_in w.locations { return false, "item on a missing location" }
		case .Carried:
			if it.character not_in w.characters { return false, "item carried by a missing character" }
		case .Equipped:
			if it.character not_in w.characters { return false, "item worn by a missing character" }
			if it.slot == .None { return false, "worn item without a slot" }
			key: [17]u8
			copy(key[:16], it.character[:])
			key[16] = u8(it.slot)
			if key in worn { return false, "two items in one slot" }
			worn[key] = true
		}
	}
	return true, ""
}

