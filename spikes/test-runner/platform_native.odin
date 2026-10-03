#+build !js
package main

import "core:os"
import "core:testing"

// `odin test` entry: one @test that runs the whole suite on the native target.
@(test)
all_cases :: proc(t: ^testing.T) {
	failed := run_all(ALL_TESTS)
	testing.expect_value(t, failed, 0)
}

main :: proc() { os.exit(run_all(ALL_TESTS) == 0 ? 0 : 1) }
