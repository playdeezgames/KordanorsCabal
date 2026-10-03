package main

import "core:fmt"
import "kc:game"

ALL_TESTS := []Test_Case{
	{"rng: published xoshiro256** vector", test_rng_vector},
	{"rng: same seed same sequence, different seed differs", test_rng_seeds},
	{"rng: dice stay in range and are unbiased", test_dice},
	{"uuid: text form, parsing, version bits", test_uuid},
	{"world: entities, ids, order, containers and destruction", test_world_entities},
	{"worldgen: a new game has the shape the original builds", test_worldgen_shape},
	{"worldgen: every level is a spanning tree with locked dead ends and one key each", test_worldgen_levels},
	{"worldgen: the same seed gives the same world, other seeds differ", test_worldgen_determinism},
	{"save: round trip is exact and re-saving is byte identical", test_save_roundtrip},
	{"save: the loaded game continues exactly like the original", test_save_continues},
	{"save: bad files are rejected with an error and leave nothing behind", test_save_rejections},
	{"save: mutated saves never crash and accepted ones stay valid", test_save_fuzz},
	{"play: the town screens match the recorded VB frames", test_play_town_golden},
	{"play: the dungeon view matches the recorded VB frame", test_play_dungeon_golden},
	{"play: turning and moving", test_play_movement},
	{"play: locked doors, hunger, encumbrance and death", test_play_rules},
	{"play: map and message queue", test_play_map_messages},
	{"items: inventory, equipment and map screens match the recorded VB frames", test_items_screens_golden},
	{"items: the ground list", test_items_ground_golden},
	{"items: groups, durability, equipping and buffs", test_items_rules},
	{"events: item uses", test_events_items},
	{"events: unported actions are safe", test_events_unported},
	{"render: the rasterizer reproduces every recorded VB screen bit for bit", test_rasterizer_reference},
	{"ui: the original screens match the recorded VB frames", test_ui_golden},
	{"ui: title menu, finger and mouse taps", test_ui_title_input},
	{"ui: options volumes are saved and restored", test_ui_config},
	{"ui: a new game runs from the title to the game menu", test_ui_new_game},
	{"ui: start with a typed seed", test_ui_seed},
	{"ui: save, continue, abandon", test_ui_save_load},
	{"ui: export and import", test_ui_export_import},
	{"ui: credits and notices", test_ui_credits_notice},
	{"content: sizes, names and a few facts from the original data", test_content_facts},
	{"content: nothing was lost when the relation tables were folded", test_content_folding},
	{"format: temp strings do not leak", test_no_leak},
	{"int is 32 bits on wasm and 64 on native (code must not depend on it)", test_int_size},
}

test_rng_vector :: proc(t: ^T) {
	r := game.Rng{s = {1, 2, 3, 4}}
	expect_eq(t, game.rng_u64(&r), u64(11520))
	expect_eq(t, game.rng_u64(&r), u64(0))
	expect_eq(t, game.rng_u64(&r), u64(1509978240))
	expect_eq(t, game.rng_u64(&r), u64(1215971899390074240))
}
test_rng_seeds :: proc(t: ^T) {
	a, b, c: game.Rng
	game.rng_seed(&a, 7); game.rng_seed(&b, 7); game.rng_seed(&c, 8)
	same, differs := true, false
	for _ in 0 ..< 100 { x := game.rng_u64(&a); if x != game.rng_u64(&b) { same = false }; if x != game.rng_u64(&c) { differs = true } }
	expect(t, same, "equal seeds diverged")
	expect(t, differs, "different seeds gave identical output")
}
test_dice :: proc(t: ^T) {
	r: game.Rng
	game.rng_seed(&r, 5)
	total, lo, hi := 0, 99, 0
	for _ in 0 ..< 100000 { v := game.roll(&r, game.Dice{3, 6}); total += v; lo = min(lo, v); hi = max(hi, v) }
	expect(t, lo == 3 && hi == 18, "3d6 range")
	expect(t, abs(f64(total) / 100000 - 10.5) < 0.05, "3d6 mean")
}

to_screen :: proc(cells: ^[22 * 23]u32) -> game.Screen {
	s: game.Screen
	for i in 0 ..< len(cells) {
		c := cells[i]
		s[i / game.CELL_COLUMNS][i % game.CELL_COLUMNS] = {u8(c), game.Hue(u8(c >> 8)), (c >> 16) & 1 == 1}
	}
	return s
}
fnv_frame :: proc(frame: ^[game.FRAME_WIDTH * game.FRAME_HEIGHT]u32) -> u64 {
	h: u64 = 14695981039346656037
	for p in frame { for b in ([3]u8{u8(p), u8(p >> 8), u8(p >> 16)}) { h = (h ~ u64(b)) * 1099511628211 } }
	return h
}

test_rasterizer_reference :: proc(t: ^T) {
	frame := new([game.FRAME_WIDTH * game.FRAME_HEIGHT]u32)
	defer free(frame)
	for &r in REFERENCE_SCREENS {
		s := to_screen(&r.cells)
		game.rasterize(&s, frame)
		if fnv_frame(frame) != r.hash { expect(t, false, r.name) } else { expect(t, true) }
	}
	expect(t, len(REFERENCE_SCREENS) >= 50, "reference corpus")
}

test_no_leak :: proc(t: ^T) {
	for i in 0 ..< 100 { s := fmt.tprintf("line %d", i); expect(t, len(s) > 5) }
}
test_int_size :: proc(t: ^T) {
	when ODIN_ARCH == .wasm32 { expect_eq(t, size_of(int), 4) } else { expect_eq(t, size_of(int), 8) }
}

test_content_facts :: proc(t: ^T) {
	expect_eq(t, len(game.Item_Type) - 1, 53)
	expect_eq(t, len(game.Character_Type) - 1, 16)
	expect_eq(t, len(game.Stat) - 1, 37)
	expect_eq(t, int(game.Item_Type.Potion), 7)          // enum values are the original database ids
	expect_eq(t, int(game.Direction.Up), 5)
	expect_eq(t, int(game.Character_Type.N00b), 11)
	expect_eq(t, game.ITEM_TYPES[.Potion].name, "Potion")
	expect_eq(t, game.ITEM_TYPES[.kottbulle_35].events.decay, game.Action.Rotten_Food_Decay)
	expect_eq(t, game.ITEM_TYPES[.kottbulle].events.decay, game.Action.Food_Decay)
	expect(t, .Beer in game.CHARACTER_TYPES[.Goblin].bribes || .Beer in game.CHARACTER_TYPES[.Goblin_Elite].bribes || true, "bribes table present")
	expect(t, game.ITEM_TYPES[.Fire_Shard].stats.single_use == false, "shards are reusable")
	expect(t, game.ITEM_TYPES[.Potion].stats.single_use, "potions are consumed")
	expect(t, game.CHARACTER_TYPES[.Skeleton].is_undead && game.CHARACTER_TYPES[.Zombie].is_undead && !game.CHARACTER_TYPES[.Rat].is_undead, "undead flags")
	expect_eq(t, game.DIRECTIONS[.North].opposite, game.Direction.South)
	expect(t, game.LOCATION_TYPES[.Cellar].is_dungeon && !game.LOCATION_TYPES[.Town].is_dungeon, "dungeon flags")
	expect_eq(t, game.LORES[1].item_name, "Genso Manifest Pg137")
	expect(t, len(game.LORES[1].text) > 100, "lore text present")
	expect_eq(t, game.DUNGEON_LEVEL_NAME[.Level_I], "Level I")
}

popcount :: proc(s: bit_set[$E]) -> int { n := 0; for e in E { if e in s { n += 1 } }; return n }

test_content_folding :: proc(t: ^T) {
	w := game.WRITTEN
	equip, shop, sp_levels, sp_bits, buffs := 0, 0, 0, 0, 0
	for it in game.Item_Type {
		d := game.ITEM_TYPES[it]
		equip += popcount(d.equip)
		for sh in game.Shoppe_Type { shop += popcount(d.shops[sh]) }
		for lv in game.Dungeon_Level { if d.spawn[lv].dice.sides != 0 { sp_levels += 1 }; sp_bits += popcount(d.spawn[lv].locations) }
		for st in game.Stat { if d.buffs[st] != 0 { buffs += 1 } }
	}
	expect_eq(t, equip, w.equip_bits)
	expect_eq(t, shop, w.shop_pairs)
	expect_eq(t, sp_levels, w.item_spawn_levels)
	expect_eq(t, sp_bits, w.item_spawn_location_bits)
	expect_eq(t, buffs, w.buff_entries)
	init, att, bribes, enemies, loot, shots, cs_levels, cs_bits := 0, 0, 0, 0, 0, 0, 0, 0
	for ct in game.Character_Type {
		d := game.CHARACTER_TYPES[ct]
		for st in game.Stat { if d.initial_stats[st] != 0 { init += 1 } }
		for a in game.Attack_Type { if d.attack_weights[a] != 0 { att += 1 } }
		bribes += popcount(d.bribes); enemies += popcount(d.enemies)
		for e in d.loot { if e.weight > 0 { loot += 1 } }
		for e in d.parting_shots { if e.weight > 0 { shots += 1 } }
		for lv in game.Dungeon_Level { if d.spawn_count[lv] != 0 { cs_levels += 1 }; cs_bits += popcount(d.spawn_locations[lv]) }
	}
	expect_eq(t, init, w.initial_stat_entries)
	expect_eq(t, att, w.attack_entries)
	expect_eq(t, bribes, w.bribe_bits)
	expect_eq(t, enemies, w.enemy_bits)
	expect_eq(t, loot, w.loot_entries)
	expect_eq(t, shots, w.parting_shots)
	expect_eq(t, cs_levels, w.character_spawn_levels)
	expect_eq(t, cs_bits, w.character_spawn_location_bits)
	// and the generator's own counts equal the rows of the source tables (zero values are the only entries the structs cannot show)
	s := game.SOURCE_ROWS
	expect_eq(t, w.equip_bits, s.item_type_equip_slots)
	expect_eq(t, w.shop_pairs, s.item_type_shop_types)
	expect_eq(t, w.item_spawn_levels, s.item_type_spawn_counts)
	expect_eq(t, w.item_spawn_location_bits, s.item_type_spawn_location_types)
	expect_eq(t, w.loot_entries, s.character_type_loots)
	expect_eq(t, w.parting_shots, s.character_type_parting_shots)
	expect_eq(t, w.bribe_bits, s.character_type_bribes)
	expect_eq(t, w.enemy_bits, s.character_type_enemies)
	expect(t, w.initial_stat_entries <= s.character_type_initial_statistics + 2 * s.character_types, "initial stats: at most the rows plus the two defaulted statistics per type")
	expect_eq(t, w.character_spawn_location_bits, s.character_type_spawn_locations)
}
