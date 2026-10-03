package main

import "core:fmt"
import "core:strings"

report :: proc(name: string, ok: bool, detail: string = "") {
	fmt.printf("%-30s %s %s\n", name, ok ? "PASS" : "FAIL", detail)
}

run_spike :: proc() {
	// Persistence across runs: counter survives a page reload / process restart.
	prev, had := storage_get("spike:counter")
	n := 0
	if had { n = int(strings.count(prev, "x")) }
	fmt.printf("previous runs: %d\n", n)
	storage_set("spike:counter", strings.repeat("x", n + 1, context.temp_allocator))

	// Round trip with awkward content (quotes, newline, multibyte, NUL-free binary-ish).
	original := "{\"name\":\"Kordanor \\\"the\\\" Bold\\n\",\"gold\":12.5,\"note\":\"héllo — ✓ 日本語 🗡\"}"
	report("set", storage_set("spike:save1", original))
	got, ok := storage_get("spike:save1")
	report("get matches (utf-8)", ok && got == original, fmt.tprintf("(%d bytes)", len(original)))

	// Overwrite and remove.
	storage_set("spike:save1", "short")
	got, ok = storage_get("spike:save1")
	report("overwrite", ok && got == "short")
	storage_remove("spike:save1")
	_, ok = storage_get("spike:save1")
	report("remove -> missing", !ok)
	_, ok = storage_get("spike:never-existed")
	report("missing key -> not ok", !ok)

	// Size scaling: find the largest value that stores and reads back (up to 8 MB of ASCII).
	largest := 0
	for size := 64 * 1024; size <= 8 * 1024 * 1024; size *= 2 {
		blob := strings.repeat("a", size, context.temp_allocator)
		if !storage_set("spike:big", blob) { break }
		back, got_ok := storage_get("spike:big", context.temp_allocator)
		if !got_ok || len(back) != size { break }
		largest = size
	}
	storage_remove("spike:big")
	fmt.printf("largest ASCII value stored: %d KB\n", largest / 1024)
	free_all(context.temp_allocator)
}
