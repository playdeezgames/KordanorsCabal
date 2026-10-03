#+build !js
package main

import "core:os"

main :: proc() {
	run_spike()
	os.exit(0)
}
