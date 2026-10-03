#+build !js
package main

import "core:os"
import "core:testing"

// `odin test` runs this one test, which runs the whole suite; `odin run` uses main.
@(test)
all_cases :: proc(t: ^testing.T) {
	testing.expect_value(t, run_all(ALL_TESTS), 0)
}

main :: proc() { os.exit(run_all(ALL_TESTS) == 0 ? 0 : 1) }
