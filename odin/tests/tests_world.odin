package main

import "core:fmt"
import "core:mem"
import "kc:game"

generated :: proc(seed: u64) -> game.World {
	w: game.World
	game.world_init(&w, seed)
	assert(game.world_generate(&w))
	return w
}

test_uuid :: proc(t: ^T) {
	r: game.Rng
	game.rng_seed(&r, 1)
	id := game.uuid_draw(&r)
	text := game.uuid_text(id)
	s := string(text[:])
	expect_eq(t, len(s), 36)
	expect_eq(t, s[14], u8('4')) // version 4
	expect(t, s[19] == '8' || s[19] == '9' || s[19] == 'a' || s[19] == 'b', "variant nibble")
	back, ok := game.uuid_parse(s)
	expect(t, ok && back == id, "round trip")
	_, ok = game.uuid_parse("00000000-0000-4000-8000-00000000000G")
	expect(t, !ok, "non-hex")
	_, ok = game.uuid_parse("00000000-0000-4000-8000-0000000000AB")
	expect(t, !ok, "upper case is not canonical")
	_, ok = game.uuid_parse("0000000000004000800000000000000000")
	expect(t, !ok, "no dashes")
	_, ok = game.uuid_parse("")
	expect(t, !ok, "empty")
	other := game.uuid_draw(&r)
	expect(t, other != id, "two draws differ")
}

test_world_entities :: proc(t: ^T) {
	w: game.World
	game.world_init(&w, 99)
	defer game.world_destroy(&w)
	town := game.create_location(&w, .Town_Square)
	cellar := game.create_location(&w, .Cellar)
	hero := game.create_character(&w, .N00b, town)
	w.player.character = hero
	rat := game.create_character(&w, .Rat, cellar)
	expect_eq(t, game.character_get(&w, rat).stats[.Strength], i32(2)) // from the Rat's initial statistics
	expect_eq(t, game.character_get(&w, rat).stats[.Unarmed_Maximum_Damage], i32(1))

	a, b, c := game.create_item(&w, .Dagger), game.create_item(&w, .Potion), game.create_item(&w, .Shield)
	game.item_put_on_ground(&w, a, cellar)
	game.item_put_in_pack(&w, b, rat)
	game.item_put_in_pack(&w, c, rat)
	game.item_put_in_pack(&w, a, rat) // moving: pack order follows placement, so the dagger is now last
	packed := game.items_in_pack(&w, rat)
	expect_eq(t, len(packed), 3)
	expect(t, packed[0] == b && packed[1] == c && packed[2] == a, "pack in placement order")
	expect_eq(t, len(game.items_on_ground(&w, cellar)), 0)

	game.item_put_on(&w, c, rat, .Shield)
	expect_eq(t, len(game.items_in_pack(&w, rat)), 2)
	worn, found := game.item_in_slot(&w, rat, .Shield)
	expect(t, found && worn == c, "item in slot")
	ok, why := game.world_validate(&w)
	expect(t, ok, why)

	expect(t, !game.destroy_character(&w, hero), "the player cannot be destroyed")
	expect(t, game.destroy_character(&w, rat), "destroy the rat")
	expect(t, game.character_get(&w, rat) == nil, "gone")
	expect(t, game.item_get(&w, a) == nil && game.item_get(&w, b) == nil && game.item_get(&w, c) == nil, "what it held is gone too")
	expect(t, !game.destroy_character(&w, rat), "second destroy refused")
	expect_eq(t, len(w.item_order), 0)
	ok, why = game.world_validate(&w)
	expect(t, ok, why)

	// ids are never reused, even for the same slot count
	seen := make(map[game.Item_ID]bool, context.temp_allocator)
	for _ in 0 ..< 500 {
		id := game.create_item(&w, .Potion)
		expect(t, id not_in seen, "id reused")
		seen[id] = true
		game.destroy_item(&w, id)
	}
}

route_total :: proc(w: ^game.World) -> (n: int) {
	for id in w.location_order { n += game.route_count(game.location_get(w, id)) }
	return
}

test_worldgen_shape :: proc(t: ^T) {
	for seed in u64(1) ..= 5 {
		w := generated(seed)
		defer game.world_destroy(&w)
		expect_eq(t, len(w.locations), 737) // 5 x 121 dungeon + 121 moon + 9 town + church entrance + cellar
		expect_eq(t, route_total(&w), 1722)
		ok, why := game.world_validate(&w)
		expect(t, ok, why)

		// the number of monsters is fixed by the data: every spawn count, plus the player
		want := 1
		for type in game.Character_Type { for level in game.Dungeon_Level { want += game.CHARACTER_TYPES[type].spawn_count[level] } }
		expect_eq(t, len(w.characters), want)

		counts: [game.Location_Type]int
		for id in w.location_order { counts[game.location_get(&w, id).type] += 1 }
		expect_eq(t, counts[.Town_Square], 1)
		expect_eq(t, counts[.Town], 8)
		expect_eq(t, counts[.Church_Entrance], 1)
		expect_eq(t, counts[.Cellar], 1)
		expect_eq(t, counts[.Dungeon_Boss], 5)
		expect_eq(t, counts[.Moon], 121)

		// the features are each on one distinct location of the right type; the innkeeper's cellar hangs below
		features: [game.Feature_Type]int
		for id in w.location_order {
			l := game.location_get(&w, id)
			if l.feature != .None {
				features[l.feature] += 1
				expect(t, game.FEATURE_TYPES[l.feature].location_type == l.type, "feature on the wrong kind of place")
				if l.feature == .Graham_the_Innkeeper {
					below, has := game.route_at(l, .Down)
					expect(t, has && game.location_get(&w, below.to).type == .Cellar, "cellar below the inn")
				}
			}
		}
		for f in game.Feature_Type { if f != .None { expect_eq(t, features[f], 1) } }

		// the player: in the town square, visited, alive, with all eight unassigned points rolled or kept
		p := game.player_character(&w)
		expect(t, p != nil && p.type == .N00b, "player")
		expect_eq(t, game.location_get(&w, p.location).type, game.Location_Type.Town_Square)
		expect(t, game.location_get(&w, p.location).visited, "start is visited")
		spent := p.stats[.Strength] + p.stats[.Dexterity] + p.stats[.Influence] + p.stats[.Willpower] + p.stats[.Power] + p.stats[.Unassigned]
		expect_eq(t, spent, i32(1 + 1 + 1 + 1 + 1 + 8)) // the starting values plus the eight points, wherever they went
		expect(t, game.DIRECTIONS[w.player.facing].is_cardinal, "faces a cardinal direction")
		expect_eq(t, w.player.mode, game.Player_Mode.Neutral)
	}
}

// Walks the routes from `start` and returns how many locations can be reached (ignoring locks).
reachable :: proc(w: ^game.World, start: game.Location_ID) -> int {
	seen := make(map[game.Location_ID]bool, context.temp_allocator)
	stack := make([dynamic]game.Location_ID, context.temp_allocator)
	append(&stack, start)
	seen[start] = true
	for len(stack) > 0 {
		id := pop(&stack)
		for r in game.location_get(w, id).routes {
			if r.to != {} && r.to not_in seen { seen[r.to] = true; append(&stack, r.to) }
		}
	}
	return len(seen)
}

test_worldgen_levels :: proc(t: ^T) {
	for seed in u64(10) ..= 14 {
		w := generated(seed)
		defer game.world_destroy(&w)
		start := game.locations_of_type(&w, .Town_Square)[0]
		expect_eq(t, reachable(&w, start), 9 + 1 + 1 + 5 * 121) // everything but the moon, via the church and the stairs

		keys: [game.Item_Type]int
		for id in w.item_order {
			it := game.item_get(&w, id)
			if it.holder == .On_Ground { keys[it.type] += 1 }
		}
		for level, i in game.DUNGEON_LEVELS {
			doors := 0 // corridor doors that are locked, and the boss door
			locked, bosses := 0, 0
			dead_ends := 0
			cells := 0
			for id in w.location_order {
				l := game.location_get(&w, id)
				if l.level != level { continue }
				cells += 1
				count := game.route_count(l)
				// a corridor tree has 120 edges; stairs add to the level's first and last rooms only
				if l.type == .Dungeon_Dead_End || l.type == .Dungeon_Boss {
					dead_ends += 1
					if l.type == .Dungeon_Boss { bosses += 1 }
				}
				for r in l.routes {
					if r.type == .Route_4 { locked += 1 }
					if r.type == game.BOSS_DOORS[i] { doors += 1 }
				}
				_ = count
			}
			expect_eq(t, cells, 121)
			expect_eq(t, bosses, 1)
			expect_eq(t, doors, 1)
			expect_eq(t, locked, dead_ends - 1) // every dead end but the boss room is behind a FE door
			expect(t, dead_ends >= 2, "a maze has dead ends")
		}
		// FE keys: one per locked door on each level, plus whatever the data scatters (FE keys also spawn through their own rule: none here)
		total_locked := 0
		for id in w.location_order { for r in game.location_get(&w, id).routes { if r.type == .Route_4 { total_locked += 1 } } }
		expect_eq(t, keys[.FE_Key], total_locked)
		expect_eq(t, keys[.CU_Key] + keys[.AG_Key] + keys[.AU_Key] + keys[.PT_Key], 4)
		expect_eq(t, keys[.Note], 25)

		// 120 corridors per level, each written both ways (240 routes), no loops: a tree
		corridors := 0
		for id in w.location_order { for r in game.location_get(&w, id).routes { if r.type == .Route_2 || r.type == .Route_4 || (r.type >= .Route_5 && r.type <= .Route_9) { corridors += 1 } } }
		expect_eq(t, corridors, 5 * 240)
	}
}

// Everything that can differ between two worlds, as one text: the save itself (it holds the ids, so ids must match too).
world_text :: proc(w: ^game.World) -> string {
	data, err := game.world_save(w, false, context.temp_allocator)
	assert(err == nil)
	return string(data)
}

test_worldgen_determinism :: proc(t: ^T) {
	a, b, c := generated(42), generated(42), generated(43)
	defer { game.world_destroy(&a); game.world_destroy(&b); game.world_destroy(&c) }
	ta, tb, tc := world_text(&a), world_text(&b), world_text(&c)
	expect(t, ta == tb, "same seed, different world")
	expect(t, ta != tc, "different seeds, same world")
}

test_save_roundtrip :: proc(t: ^T) {
	w := generated(7)
	defer game.world_destroy(&w)
	// make the world a little less fresh: some wear, a worn item, a spell, a quest, wounds
	player := w.player.character
	dagger := game.create_item(&w, .Dagger)
	game.item_put_on(&w, dagger, player, .Weapon)
	game.item_get(&w, dagger).wear = 3
	game.stat_add(game.player_character(&w), .Wounds, 1)
	w.player.spells[.Holy_Bolt] = 1
	w.player.quests_active += {.Cellar_Rats}
	w.player.quest_completions[.Cellar_Rats] = 4
	w.player.shoppe = .Healer

	data, err := game.world_save(&w)
	expect(t, err == nil, "save")
	defer delete(data)
	fmt.printf("    (a new game saves as %d bytes)\n", len(data))
	loaded: game.World
	lerr, msg := game.world_load(&loaded, data, context.temp_allocator)
	expect_eq(t, lerr, game.Load_Error.None)
	if lerr != .None { fmt.println("   ", msg); return }
	defer game.world_destroy(&loaded)
	again, err2 := game.world_save(&loaded)
	expect(t, err2 == nil, "re-save")
	defer delete(again)
	expect(t, string(data) == string(again), "re-saving a loaded world must give identical text")
	expect_eq(t, loaded.player.spells[.Holy_Bolt], i32(1))
	expect(t, .Cellar_Rats in loaded.player.quests_active, "quest")
	expect_eq(t, loaded.player.quest_completions[.Cellar_Rats], i32(4))
	expect_eq(t, game.item_get(&loaded, dagger).wear, i32(3))
	expect_eq(t, game.item_get(&loaded, dagger).slot, game.Equip_Slot.Weapon)
	expect_eq(t, len(loaded.characters), len(w.characters))
	expect(t, loaded.character_order[100] == w.character_order[100], "creation order survives")
	summary, ok := game.save_peek_summary(string(data), context.temp_allocator)
	expect(t, ok && summary.place == .None, "summary reads without a world")
}

test_save_continues :: proc(t: ^T) {
	w := generated(2024)
	defer game.world_destroy(&w)
	data, _ := game.world_save(&w)
	defer delete(data)
	loaded: game.World
	lerr, _ := game.world_load(&loaded, data, context.temp_allocator)
	expect_eq(t, lerr, game.Load_Error.None)
	if lerr != .None { return }
	defer game.world_destroy(&loaded)
	// the same actions on both give the same ids and the same random numbers
	for _ in 0 ..< 50 {
		a, b := game.create_character(&w, .Rat, w.location_order[0]), game.create_character(&loaded, .Rat, loaded.location_order[0])
		expect(t, a == b, "next id differs after load")
		expect_eq(t, game.rng_u64(&w.rng), game.rng_u64(&loaded.rng))
	}
}

test_save_rejections :: proc(t: ^T) {
	w := generated(5)
	defer game.world_destroy(&w)
	good, _ := game.world_save(&w)
	defer delete(good)

	try :: proc(t: ^T, name: string, text: string, want: game.Load_Error) {
		loaded: game.World
		err, _ := game.world_load(&loaded, transmute([]byte)text, context.temp_allocator)
		if err != want { fmt.printf("    FAIL %s: got %v want %v\n", name, err, want); t.failures += 1 }
		t.checks += 1
		if err != .None { expect(t, len(loaded.locations) == 0 && len(loaded.characters) == 0, "failed load left things behind") } else { game.world_destroy(&loaded) }
	}
	try(t, "empty", "", .Parse)
	try(t, "not json", "hello", .Parse)
	try(t, "wrong format", `{"format":"something else","version":1}`, .Format)
	try(t, "future version", `{"format":"kordanors-cabal-save","version":2}`, .Version)
	try(t, "no player", `{"format":"kordanors-cabal-save","version":1,"world":{"seed":"0000000000000001","rng":"0000000000000001000000000000000200000000000000030000000000000004"}}`, .Invariant)
	try(t, "bad seed", `{"format":"kordanors-cabal-save","version":1,"world":{"seed":"zz"}}`, .Shape)
	text := string(good)
	// a character that points at a location that is not there
	idx := 0
	for i in 0 ..< len(text) - 20 { if text[i:i + 14] == `"characters":[` { idx = i; break } }
	expect(t, idx > 0, "found the character list")
	bad := fmt.tprintf("%s%s%s", text[:idx + 14], `{"id":"00000000-0000-4000-8000-000000000001","type":"Rat","location":"00000000-0000-4000-8000-0000000000ff","stats":[]},`, text[idx + 14:])
	try(t, "dangling location", bad, .Shape)
	try(t, "the real thing still loads", text, .None)
}

test_save_fuzz :: proc(t: ^T) {
	w := generated(11)
	defer game.world_destroy(&w)
	good, _ := game.world_save(&w)
	defer delete(good)
	r: game.Rng
	game.rng_seed(&r, 123)
	accepted, rejected := 0, 0
	buf := make([]byte, len(good))
	defer delete(buf)
	arena_mem := make([]byte, 8 * 1024 * 1024)
	defer delete(arena_mem)
	for round in 0 ..< 60 {
		copy(buf, good)
		data := buf
		switch round % 3 {
		case 0: // flip some digits
			for _ in 0 ..< 5 {
				at := game.pick_index(&r, len(buf))
				if buf[at] >= '0' && buf[at] <= '9' { buf[at] = '0' + u8(game.pick_index(&r, 10)) }
			}
		case 1: // random bytes
			for _ in 0 ..< 3 { buf[game.pick_index(&r, len(buf))] = u8(game.pick_index(&r, 256)) }
		case 2: // truncate
			data = buf[:game.pick_index(&r, len(buf))]
		}
		arena: mem.Arena
		mem.arena_init(&arena, arena_mem)
		loaded: game.World
		err, _ := game.world_load(&loaded, data, mem.arena_allocator(&arena))
		if err == .None {
			accepted += 1
			ok, why := game.world_validate(&loaded)
			expect(t, ok, why)
			again, merr := game.world_save(&loaded)
			expect(t, merr == nil, "an accepted world can be saved again")
			delete(again)
			game.world_destroy(&loaded)
		} else {
			rejected += 1
		}
	}
	fmt.printf("    (fuzz: %d accepted, %d rejected)\n", accepted, rejected)
	expect(t, rejected > 0, "some mutations must be caught")
}
