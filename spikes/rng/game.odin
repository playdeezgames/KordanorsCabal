package main

import "core:encoding/json"
import "core:fmt"

report :: proc(name: string, ok: bool, detail: string = "") {
	fmt.printf("%-56s %s %s\n", name, ok ? "PASS" : "FAIL", detail)
}

run_spike :: proc() {
	// --- known answers for splitmix64 from seed 0 (the published reference sequence) ---
	x: u64 = 0
	kat := [3]u64{0xE220A8397B1DCDAF, 0x6E789E6AA1B965F4, 0x06C45D188009454F}
	ok_kat := true
	for want in kat { if got := splitmix64(&x); got != want { ok_kat = false } }
	report("splitmix64 matches the reference outputs", ok_kat)
	ref := Rng{s = {1, 2, 3, 4}} // published xoshiro256** test vector for state {1, 2, 3, 4}
	report("xoshiro256** matches the reference outputs", rng_u64(&ref) == 11520 && rng_u64(&ref) == 0 && rng_u64(&ref) == 1509978240 && rng_u64(&ref) == 1215971899390074240)

	// --- identical sequences on every target: print the first draws and a checksum of a million ---
	r: Rng
	rng_seed(&r, 12345)
	fmt.printf("seed 12345: %x %x %x\n", rng_u64(&r), rng_u64(&r), rng_u64(&r))
	sum: u64 = 0
	rng_seed(&r, 777)
	for _ in 0 ..< 1_000_000 { sum = sum * 31 + rng_u64(&r) }
	fmt.printf("checksum of 1,000,000 draws (seed 777): %x\n", sum)

	// --- unbiased bounded values: 3 buckets, 3,000,000 draws ---
	rng_seed(&r, 99)
	counts: [3]int
	for _ in 0 ..< 3_000_000 { counts[rng_range(&r, 0, 2)] += 1 }
	worst := 0.0
	for c in counts { d := abs(f64(c) - 1_000_000) / 1_000_000; worst = max(worst, d) }
	report("range 0..2 is uniform to within 0.5 percent", worst < 0.005, fmt.tprint(counts))

	// --- dice: 3d6 mean 10.5, 2d4 range ---
	rng_seed(&r, 5)
	total, lo, hi := 0, 99, 0
	for _ in 0 ..< 200_000 { v := roll(&r, Dice{3, 6}); total += v; lo = min(lo, v); hi = max(hi, v) }
	mean := f64(total) / 200_000
	report("3d6: range 3..18 and mean about 10.5", lo == 3 && hi == 18 && abs(mean - 10.5) < 0.05, fmt.tprintf("(mean %.3f)", mean))

	// --- weighted choice: the real Magic Egg table (task 18) ---
	weights := []int{500, 8, 4, 250, 1000, 125, 64, 1, 2, 125, 64, 16, 8, 1}
	sumw := 0
	for w in weights { sumw += w }
	rng_seed(&r, 31337)
	picks: [14]int
	N :: 2_000_000
	for _ in 0 ..< N { picks[pick_weighted(&r, weights)] += 1 }
	max_err := 0.0
	for w, i in weights { expect := f64(N) * f64(w) / f64(sumw); if w >= 100 { max_err = max(max_err, abs(f64(picks[i]) - expect) / expect) } }
	report("weighted table: frequent entries within 1 percent", max_err < 0.01, fmt.tprintf("(worst %.4f)", max_err))
	report("weighted table: rare entries are produced", picks[7] > 0 && picks[13] > 0, fmt.tprintf("(1/%d -> %d, %d)", sumw, picks[7], picks[13]))

	// --- state can be saved and restored (JSON, as in the save file) ---
	rng_seed(&r, 2024)
	for _ in 0 ..< 1000 { rng_u64(&r) }
	data, err := json.marshal(r.s)
	restored: Rng
	uerr := json.unmarshal(data, &restored.s)
	same := true
	for _ in 0 ..< 1000 { if rng_u64(&r) != rng_u64(&restored) { same = false } }
	report("saved state continues the same sequence", err == nil && uerr == nil && same, string(data))

	// --- Prim's maze: structure and cross-platform determinism ---
	structural := true
	for seed in 1 ..= 200 {
		rng_seed(&r, u64(seed))
		m := maze_generate(&r)
		if maze_door_count(&m) != MAZE_COLS * MAZE_ROWS - 1 || maze_reachable(&m) != MAZE_COLS * MAZE_ROWS { structural = false }
	}
	report("200 mazes: 120 doors and every cell reachable", structural)
	rng_seed(&r, 1)
	m1 := maze_generate(&r)
	rng_seed(&r, 1)
	m2 := maze_generate(&r)
	report("same seed -> same maze", maze_hash(&m1) == maze_hash(&m2))
	fmt.printf("maze hash seed 1: %x\n", maze_hash(&m1))
	// dead ends (cells with one door) per maze: the VB game places a boss and keys on them (task 9)
	rng_seed(&r, 4)
	total_dead_ends := 0
	for _ in 0 ..< 100 {
		m := maze_generate(&r)
		for y in 0 ..< MAZE_ROWS { for x in 0 ..< MAZE_COLS {
			doors := 0
			if m.open_east[y][x] { doors += 1 }
			if x > 0 && m.open_east[y][x - 1] { doors += 1 }
			if m.open_south[y][x] { doors += 1 }
			if y > 0 && m.open_south[y - 1][x] { doors += 1 }
			if doors == 1 { total_dead_ends += 1 }
		} }
	}
	fmt.printf("average dead ends per 11 x 11 maze: %.1f (the VB game gives 208 FE keys over 5 levels, about 42 each)\n", f64(total_dead_ends) / 100)
}
