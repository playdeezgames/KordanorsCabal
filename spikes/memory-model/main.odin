package main

import "base:runtime"
import "core:fmt"
import "core:mem"

// ---- stack probe: recursion with a 1 KiB frame; JS calls it with growing depth until the module traps ----
@(export) stack_probe :: proc "c" (depth: i32) -> i32 {
	buf: [1024]u8
	buf[0] = u8(depth)
	if depth == 0 { return i32(buf[0]) }
	r := stack_probe(depth - 1)
	return r + i32(buf[depth % 1024])
}

// ---- heap probes ------------------------------------------------------------------------------------
@(export) alloc_megabytes :: proc "c" (n: i32) -> i32 {
	context = runtime.default_context()
	blocks := make([]rawptr, n)
	defer delete(blocks)
	for i in 0 ..< n { p, _ := mem.alloc(1024 * 1024); blocks[i] = p; (cast(^u8)p)^ = 1 }
	for i in 0 ..< n { mem.free(blocks[i]) }
	return n
}
// Many temporary strings per frame, freed each frame: memory must stay flat.
@(export) temp_churn :: proc "c" (frames: i32) -> i32 {
	context = runtime.default_context()
	total := 0
	for _ in 0 ..< frames {
		for j in 0 ..< 200 { s := fmt.tprintf("line %d with some text to format %s", j, "abcdefghijklmnopqrstuvwxyz"); total += len(s) }
		free_all(context.temp_allocator)
	}
	return i32(total)
}
// The same without ever freeing the temp allocator: how fast does it grow?
@(export) temp_leak :: proc "c" (frames: i32) -> i32 {
	context = runtime.default_context()
	for _ in 0 ..< frames { for j in 0 ..< 200 { _ = fmt.tprintf("line %d with some text to format %s", j, "abcdefghijklmnopqrstuvwxyz") } }
	return 0
}
// A fixed arena sized up front, used the way a save load would use it.
@(export) arena_roundtrip :: proc "c" (size_kb: i32) -> i32 {
	context = runtime.default_context()
	buf := make([]byte, int(size_kb) * 1024)
	defer delete(buf)
	arena: mem.Arena
	mem.arena_init(&arena, buf)
	a := mem.arena_allocator(&arena)
	n := 0
	for {
		p, err := mem.alloc(4096, allocator = a)
		if err != nil || p == nil { break }
		n += 1
	}
	return i32(n)
}

big_global: [256 * 1024]u8 // static data (lives in the wasm data segment, no allocation)

main :: proc() {
	big_global[0] = 1
	fmt.println("memory model spike started; static buffer", len(big_global), "bytes")
}
