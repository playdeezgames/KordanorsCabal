package main

import "core:encoding/json"
import "core:fmt"
import "base:runtime"
import "core:mem"

report :: proc(name: string, ok: bool, detail: string = "") {
	fmt.printf("%-40s %s %s\n", name, ok ? "PASS" : "FAIL", detail)
}

// Deterministic LCG so every run builds the same world.
Rng :: struct { s: u64 }
rng_next :: proc(r: ^Rng, n: int) -> int {
	r.s = r.s * 6364136223846793005 + 1442695040888963407
	return int((r.s >> 33) % u64(n))
}

// A world with the same shape and size as a fresh game (task 6 / 9 numbers), not real game rules.
make_world :: proc() -> ^World {
	w := new(World)
	pool_init(&w.characters)
	pool_init(&w.items)
	r := Rng{12345}
	w.location_count = 738 // ids 1..737
	for i in 1 ..< 738 {
		l := &w.locations[i]
		l.type = i == 1 ? .Town_Square : i <= 9 ? .Town : Location_Type(4 + r_pick(&r, 5))
		l.level = i <= 10 ? .None : Dungeon_Level(1 + (i - 10) / 121 % 6)
		l.column, l.row = u8(i % 11), u8(i / 11 % 11)
		if i <= 10 && r_pick(&r, 2) == 0 { l.feature = Feature_Type(1 + r_pick(&r, 9)) }
		for d in Direction.North ..= Direction.West { // ~2.3 routes per location, like the real 1,722
			if r_pick(&r, 100) < 58 {
				l.routes[d] = {Location_ID(1 + r_pick(&r, 737)), Route_Type(1 + r_pick(&r, 11))}
			}
		}
	}
	// the player
	player, _ := pool_create(&w.characters, Character{type = .N00b, location = 1})
	pc, _ := pool_get(&w.characters, player)
	pc.stats = #partial {.Strength = 4, .Dexterity = 3, .Influence = 2, .Willpower = 2, .Power = 1, .HP = 3, .MP = 3, .Mana = 1, .Unassigned = 1, .Unarmed_Maximum_Damage = 1, .Base_Maximum_Defend = 1, .XP_Goal = 10, .Base_Lift = 50, .Bonus_Lift = 10}
	w.player = {character = player, mode = .Neutral, facing = .North, quests_active = {.Cellar_Rats}, }
	w.player.quest_completions[.Cellar_Rats] = 2
	w.player.spells[.Holy_Bolt] = 1
	// 1,086 monsters with ~11 non-zero stats each, mostly unmodified
	for _ in 0 ..< 1086 {
		ct := Character_Type(1 + r_pick(&r, 15))
		if ct == .N00b { ct = .Rat }
		c := Character{type = ct, location = Location_ID(10 + r_pick(&r, 727))}
		c.stats = #partial {.Strength = i32(1 + r_pick(&r, 6)), .Dexterity = i32(1 + r_pick(&r, 6)), .HP = i32(1 + r_pick(&r, 9)), .Unarmed_Maximum_Damage = i32(1 + r_pick(&r, 4)), .Base_Maximum_Defend = i32(1 + r_pick(&r, 3)), .Immobilization = 0, .Wounds = 0}
		if r_pick(&r, 5) != 0 { c.stats[.Influence] = i32(1 + r_pick(&r, 5)); c.stats[.Willpower] = i32(1 + r_pick(&r, 5)); c.stats[.MP] = i32(r_pick(&r, 4)); c.stats[.Stress] = 0 }
		if r_pick(&r, 20) == 0 { c.stats[.Wounds] = i32(1 + r_pick(&r, 3)) }
		pool_create(&w.characters, c)
	}
	// churn: kill 150 monsters and add 100 back so free lists and generations are non-trivial
	for _ in 0 ..< 150 {
		for { // find a live monster
			idx := 2 + r_pick(&r, int(w.characters.used_top) - 2)
			s := w.characters.slots[idx]
			if s.alive { pool_destroy(&w.characters, handle_make(u32(idx), s.gen)); break }
		}
	}
	for _ in 0 ..< 100 { pool_create(&w.characters, Character{type = .Rat, location = Location_ID(10 + r_pick(&r, 727)), stats = #partial {.HP = 1, .Strength = 1}}) }
	// 362 items: carried by the player, equipped, on the ground
	for k in 0 ..< 362 {
		it := Item{type = Item_Type(1 + r_pick(&r, 53)), seq = u32(k + 1)}
		w.item_seq = u32(k + 1)
		switch r_pick(&r, 10) {
		case 0, 1: it.holder, it.holder_character = .Carried, player
		case 2: it.holder, it.holder_character, it.equip_slot = .Equipped, player, Equip_Slot(1 + k % 8); if k > 7 { it.holder, it.holder_character = .Carried, player; it.equip_slot = .None }
		case: it.holder, it.holder_location = .On_Ground, Location_ID(10 + r_pick(&r, 727))
		}
		if r_pick(&r, 20) == 0 { it.wear = i32(1 + r_pick(&r, 5)) }
		pool_create(&w.items, it)
	}
	return w
}
r_pick :: proc(r: ^Rng, n: int) -> int { return rng_next(r, n) }

worlds_equal :: proc(a, b: ^World) -> bool {
	if a.location_count != b.location_count || a.locations != b.locations && false { }
	for i in 1 ..< int(a.location_count) { if a.locations[i] != b.locations[i] { return false } }
	return a.characters == b.characters && a.items == b.items && a.player == b.player && a.item_seq == b.item_seq && a.rng_state == b.rng_state
}

run_spike :: proc() {
	w := make_world()
	defer free(w)
	ok, why := world_validate(w)
	report("generated world is valid", ok, why)
	fmt.printf("world: %d locations, %d characters (used_top %d), %d items\n", w.location_count - 1, w.characters.count, w.characters.used_top, w.items.count)

	// Memory cost of one save and one load (task 19): peak live bytes while each runs, measured with a tracking allocator.
	track :: proc(t: ^mem.Tracking_Allocator, backing: mem.Allocator) { mem.tracking_allocator_init(t, backing); t.bad_free_array.allocator = backing }
	{
		t: mem.Tracking_Allocator
		track(&t, context.allocator)
		old := context.allocator
		context.allocator = mem.tracking_allocator(&t)
		data, _ := save_to_json(w)
		fmt.printf("save: output %d bytes; peak heap during save %d bytes; allocations %d\n", len(data), t.peak_memory_allocated, t.total_allocation_count)
		t2: mem.Tracking_Allocator
		track(&t2, old)
		context.allocator = mem.tracking_allocator(&t2)
		wl := new(World)
		mem_before := t2.current_memory_allocated
		load_err, _ := load_from_json(data, wl)
		fmt.printf("load: peak heap during load %d bytes (World itself is %d bytes); live after load %d bytes; result %v\n", t2.peak_memory_allocated, size_of(World), t2.current_memory_allocated - mem_before, load_err)
		// Task 19 policy: parse into an arena that is dropped right after the world is filled.
		arena_buf := make([]byte, 2 * 1024 * 1024)
		arena: mem.Arena
		mem.arena_init(&arena, arena_buf)
		wl2 := new(World)
		live_before := t2.current_memory_allocated
		load_err2, _ := load_from_json(data, wl2, mem.arena_allocator(&arena))
		fmt.printf("load into an arena: arena used %d bytes of %d; heap outside the arena grew by %d bytes; result %v; same world: %v\n", arena.offset, len(arena_buf), t2.current_memory_allocated - live_before, load_err2, worlds_equal(wl, wl2))
		small: mem.Arena
		small_buf := make([]byte, 64 * 1024)
		mem.arena_init(&small, small_buf)
		wl3 := new(World)
		load_err3, _ := load_from_json(data, wl3, mem.arena_allocator(&small))
		fmt.printf("load into a too-small 64 KiB arena: %v (graceful, no crash)\n", load_err3)
		context.allocator = old
		free(wl); free(wl2); free(wl3); delete(arena_buf); delete(small_buf)
	}
	compact, err := save_to_json(w)
	report("marshal compact", err == nil, fmt.tprintf("(%d bytes, err=%v)", len(compact), err))
	if err != nil { return }
	{
		f := to_save_file(w)
		sz :: proc(v: $T) -> int { d, _ := json.marshal(v, {use_enum_names = true, spec = .JSON}); return len(d) }
		fmt.printf("sizes: locations %d, characters %d, items %d, player %d\n", sz(f.world.locations), sz(f.world.characters), sz(f.world.items), sz(f.world.player))
	}
	pretty, _ := save_to_json(w, true)
	fmt.printf("pretty size: %d bytes\n", len(pretty))
	fmt.println(string(compact[:min(len(compact), 700)]))

	w2 := new(World)
	defer free(w2)
	lerr, msg := load_from_json(compact, w2)
	report("load", lerr == .None, msg)
	report("round trip is exact", lerr == .None && worlds_equal(w, w2))
	// a second save of the loaded world is byte-identical
	compact2, _ := save_to_json(w2)
	same := string(compact) == string(compact2)
	report("save(load(save(w))) identical", same)
	if !same {
		a, b := string(compact), string(compact2)
		for i in 0 ..< min(len(a), len(b)) {
			if a[i] != b[i] { fmt.printf("first difference at byte %d:\n  A: %s\n  B: %s\n", i, a[max(0, i-60):min(len(a), i+60)], b[max(0, i-60):min(len(b), i+60)]); break }
		}
		fmt.println("lengths", len(a), len(b))
	}

	// after loading, the next created handles match
	h1, _ := pool_create(&w.characters, Character{type = .Rat, location = 5})
	h2, _ := pool_create(&w2.characters, Character{type = .Rat, location = 5})
	report("next handle identical after load", h1 == h2)

	// import-style validation failures
	bad := fmt.tprintf("%s", string(compact))
	try :: proc(name: string, text: string, want: Load_Error) {
		t := new(World)
		defer free(t)
		got, m := load_from_json(transmute([]u8)text, t)
		report(name, got == want, fmt.tprintf("-> %v %s", got, m))
	}
	try("rejects empty input", "", .Parse)
	try("rejects non-JSON", "hello", .Parse)
	try("rejects wrong format", `{"format":"other","version":1}`, .Format)
	try("rejects future version", `{"format":"kordanors-cabal-save","version":99}`, .Version)
	try("rejects missing player", `{"format":"kordanors-cabal-save","version":1,"world":{"locations":[],"characters":{"used_top":1,"free_head":0,"slots":[{"gen":0,"alive":false,"next_free":0,"value":{}}]},"items":{"used_top":1,"free_head":0,"slots":[{"gen":0,"alive":false,"next_free":0,"value":{}}]}}}`, .Invariant)
	_ = bad

	fuzz(compact)
}

fuzz :: proc(good: []byte) {
	counts: [Load_Error]int
	r := Rng{987}
	arena_buf := make([]byte, 64 * 1024 * 1024)
	defer delete(arena_buf)
	N :: 150
	for i in 0 ..< N {
		mutated := make([]byte, len(good))
		copy(mutated, good)
		switch i % 3 {
		case 0: mutated = mutated[:rng_next(&r, len(mutated))] // truncate
		case 1: for _ in 0 ..< 1 + rng_next(&r, 8) { mutated[rng_next(&r, len(mutated))] = byte(rng_next(&r, 256)) } // random bytes
		case 2: for _ in 0 ..< 1 + rng_next(&r, 5) { // overwrite a digit with another digit (valid JSON, wrong values)
			p := rng_next(&r, len(mutated))
			if mutated[p] >= '0' && mutated[p] <= '9' { mutated[p] = byte('0' + rng_next(&r, 10)) }
		} }
		arena: mem.Arena
		mem.arena_init(&arena, arena_buf)
		context.allocator = mem.arena_allocator(&arena)
		t := new(World)
		e, _ := load_from_json(mutated, t)
		counts[e] += 1
		if e == .None {
			// anything accepted must satisfy the invariants and survive a re-save
			ok, _ := world_validate(t)
			if !ok { fmt.println("ACCEPTED INVALID WORLD at", i) }
			_, serr := save_to_json(t)
			if serr != nil { fmt.println("accepted world cannot be saved at", i) }
		}
		free_all(context.allocator)
	}
	report("fuzz: no crashes", true, fmt.tprintf("(%d mutations: %v)", N, counts))
}
