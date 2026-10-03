#+build !js
package main

import "core:os"
import "core:time"

main :: proc() {
	game_init()
	prev := time.now()
	for {
		now := time.now()
		dt := time.duration_seconds(time.diff(prev, now))
		prev = now
		if !game_step(dt) { break }
		time.sleep(16 * time.Millisecond)
	}
	os.exit(0)
}
