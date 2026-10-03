#+build !js
package main

import "core:os"
import "core:strings"
import "core:testing"

// `odin test` runs this one test, which runs the whole suite; `odin run` uses main.
@(test)
all_cases :: proc(t: ^testing.T) {
	testing.expect_value(t, run_all(ALL_TESTS), 0)
}

// `build/tests_native part-of-a-name` runs only the cases whose name contains the text.
main :: proc() {
	cases := ALL_TESTS
	if len(os.args) > 1 {
		picked := make([dynamic]Test_Case)
		for c in ALL_TESTS { if strings.contains(c.name, os.args[1]) { append(&picked, c) } }
		cases = picked[:]
	}
	os.exit(run_all(cases) == 0 ? 0 : 1)
}
