package main

// A tiny portable test kit (task 22). `core:testing` needs threads and an OS, so it does not exist on js_wasm32; tests are
// plain procedures written against this kit, and are run (a) by `odin test` through one wrapper and (b) as a wasm program
// under node. Same test code, both targets, so platform-dependent behaviour (32-bit int, memory) is caught too.
import "base:intrinsics"
import "base:runtime"
import "core:fmt"
import "core:mem"

T :: struct {
	name:     string,
	failures: int,
	checks:   int,
}

Test_Case :: struct {
	name: string,
	run:  proc(t: ^T),
}

expect :: proc(t: ^T, condition: bool, message := "", loc := #caller_location) {
	t.checks += 1
	if !condition {
		t.failures += 1
		fmt.printf("    FAIL %s:%d %s\n", loc.file_path, loc.line, message)
	}
}
expect_eq :: proc(t: ^T, got, want: $V, loc := #caller_location) where intrinsics.type_is_comparable(V) {
	t.checks += 1
	if got != want {
		t.failures += 1
		fmt.printf("    FAIL %s:%d got %v, want %v\n", loc.file_path, loc.line, got, want)
	}
}

// Runs every case with a tracking allocator so that a leak is a failure. Returns the number of failed cases.
run_all :: proc(cases: []Test_Case) -> int {
	failed_cases, total_checks := 0, 0
	for c in cases {
		track: mem.Tracking_Allocator
		mem.tracking_allocator_init(&track, context.allocator)
		old := context.allocator
		context.allocator = mem.tracking_allocator(&track)
		t := T{name = c.name}
		c.run(&t)
		free_all(context.temp_allocator)
		context.allocator = old
		leaked := len(track.allocation_map)
		if leaked > 0 {
			t.failures += 1
			fmt.printf("    FAIL %s leaked %d allocation(s), %d bytes\n", c.name, leaked, track.current_memory_allocated)
		}
		mem.tracking_allocator_destroy(&track)
		total_checks += t.checks
		fmt.printf("%s %s (%d checks)\n", t.failures == 0 ? "ok  " : "FAIL", c.name, t.checks)
		if t.failures > 0 { failed_cases += 1 }
	}
	if failed_cases == 0 { fmt.printf("TESTS PASSED: %d cases, %d checks\n", len(cases), total_checks) } else { fmt.printf("TESTS FAILED: %d of %d cases\n", failed_cases, len(cases)) }
	return failed_cases
}
_ :: runtime
