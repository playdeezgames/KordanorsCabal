package main

// The game's random number generator (task 21): xoshiro256** seeded through splitmix64.
// Written out here instead of using core:math/rand so that (1) the same seed gives the same sequence on every target and
// every Odin version, (2) the whole state is four u64 and can be saved, (3) bounded values have no modulo bias.

Rng :: struct { s: [4]u64 } // never all zero; a saved game stores exactly this

splitmix64 :: proc(x: ^u64) -> u64 {
	x^ += 0x9E3779B97F4A7C15
	z := x^
	z = (z ~ (z >> 30)) * 0xBF58476D1CE4E5B9
	z = (z ~ (z >> 27)) * 0x94D049BB133111EB
	return z ~ (z >> 31)
}

rng_seed :: proc(r: ^Rng, seed: u64) {
	x := seed
	for i in 0 ..< 4 { r.s[i] = splitmix64(&x) }
}

rotl :: #force_inline proc "contextless" (x: u64, k: uint) -> u64 { return (x << k) | (x >> (64 - k)) }

rng_u64 :: proc(r: ^Rng) -> u64 {
	result := rotl(r.s[1] * 5, 7) * 9
	t := r.s[1] << 17
	r.s[2] ~= r.s[0]
	r.s[3] ~= r.s[1]
	r.s[1] ~= r.s[2]
	r.s[0] ~= r.s[3]
	r.s[2] ~= t
	r.s[3] = rotl(r.s[3], 45)
	return result
}

// Uniform integer in [0, n) without modulo bias (Lemire's method with rejection).
rng_below :: proc(r: ^Rng, n: u64) -> u64 {
	if n <= 1 { return 0 }
	x := rng_u64(r)
	hi, lo := mul128(x, n)
	if lo < n {
		threshold := (0 - n) % n
		for lo < threshold { x = rng_u64(r); hi, lo = mul128(x, n) }
	}
	return hi
}
mul128 :: proc "contextless" (a, b: u64) -> (hi, lo: u64) {
	a0, a1 := a & 0xFFFFFFFF, a >> 32
	b0, b1 := b & 0xFFFFFFFF, b >> 32
	p00, p01, p10, p11 := a0 * b0, a0 * b1, a1 * b0, a1 * b1
	mid := (p00 >> 32) + (p01 & 0xFFFFFFFF) + (p10 & 0xFFFFFFFF)
	lo = (p00 & 0xFFFFFFFF) | (mid << 32)
	hi = p11 + (p01 >> 32) + (p10 >> 32) + (mid >> 32)
	return
}

// ---- the helpers the game uses (the VB RNG module: FromRange, RollXDY, FromGenerator, FromList) ----------------
rng_range :: proc(r: ^Rng, lo, hi: int) -> int { return lo + int(rng_below(r, u64(hi - lo + 1))) } // inclusive both ends
Dice :: struct { count, sides: i16 } // every dice string in the content is plain XdY (task 9)
roll :: proc(r: ^Rng, d: Dice) -> int {
	total := 0
	for _ in 0 ..< d.count { total += rng_range(r, 1, int(d.sides)) }
	return total
}
// Weighted choice: index into `weights` with probability weight / sum (VB: RNG.FromGenerator).
pick_weighted :: proc(r: ^Rng, weights: []int) -> int {
	sum := 0
	for w in weights { sum += w }
	pick := int(rng_below(r, u64(sum)))
	for w, i in weights { pick -= w; if pick < 0 { return i } }
	return len(weights) - 1
}
pick_index :: proc(r: ^Rng, count: int) -> int { return int(rng_below(r, u64(count))) }
