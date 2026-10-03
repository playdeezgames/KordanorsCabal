package main

// Example cases. The real suite lives with the code it tests; these show the style: state in, state out, no mocks.
import "core:fmt"

ALL_TESTS := []Test_Case{
	{"rng: published xoshiro256** vector", test_rng_vector},
	{"rng: same seed, same sequence; different seed differs", test_rng_seeds},
	{"rng: dice stay in range", test_dice_range},
	{"maze: spanning tree for 50 seeds", test_maze_tree},
	{"maze: fixed seed gives the exact same maze on every target", test_maze_golden},
	{"format: temp strings do not leak", test_no_leak},
	{"runner reports failures (only fails with -define:SPIKE_FAIL=true)", test_fail_on_purpose},
	{"int is 32 bits on wasm and 64 on native (so code must not rely on it)", test_int_size},
}

test_rng_vector :: proc(t: ^T) {
	r := Rng{s = {1, 2, 3, 4}}
	expect_eq(t, rng_u64(&r), u64(11520))
	expect_eq(t, rng_u64(&r), u64(0))
	expect_eq(t, rng_u64(&r), u64(1509978240))
	expect_eq(t, rng_u64(&r), u64(1215971899390074240))
}
test_rng_seeds :: proc(t: ^T) {
	a, b, c: Rng
	rng_seed(&a, 7); rng_seed(&b, 7); rng_seed(&c, 8)
	same, differs := true, false
	for _ in 0 ..< 100 { x := rng_u64(&a); if x != rng_u64(&b) { same = false }; if x != rng_u64(&c) { differs = true } }
	expect(t, same, "equal seeds diverged")
	expect(t, differs, "different seeds gave identical output")
}
test_dice_range :: proc(t: ^T) {
	r: Rng
	rng_seed(&r, 1)
	for _ in 0 ..< 10000 { v := roll(&r, Dice{2, 4}); expect(t, v >= 2 && v <= 8, "2d4 out of range") }
}
test_maze_tree :: proc(t: ^T) {
	r: Rng
	for seed in 1 ..= 50 {
		rng_seed(&r, u64(seed))
		m := maze_generate(&r)
		expect_eq(t, maze_door_count(&m), MAZE_COLS * MAZE_ROWS - 1)
		expect_eq(t, maze_reachable(&m), MAZE_COLS * MAZE_ROWS)
	}
}
test_maze_golden :: proc(t: ^T) {
	r: Rng
	rng_seed(&r, 1)
	m := maze_generate(&r)
	expect_eq(t, maze_hash(&m), u64(0xe0e232178e19fd5a)) // recorded from the first run; the same on native and wasm
}
test_no_leak :: proc(t: ^T) {
	for i in 0 ..< 100 { s := fmt.tprintf("line %d", i); expect(t, len(s) > 5) }
}
test_int_size :: proc(t: ^T) {
	when ODIN_ARCH == .wasm32 { expect_eq(t, size_of(int), 4) } else { expect_eq(t, size_of(int), 8) }
}

// A deliberately failing case, active only with -define:SPIKE_FAIL=true, to prove that the runner reports failure on both targets.
SPIKE_FAIL :: #config(SPIKE_FAIL, false)
test_fail_on_purpose :: proc(t: ^T) {
	expect(t, !SPIKE_FAIL, "failing on purpose")
	when SPIKE_FAIL { _ = make([]byte, 100) } // and leaking on purpose: the kit must report it
}
