#+build !js
package main

import "core:testing"

@(test)
step_counts_frames :: proc(t: ^testing.T) {
	game = {}
	for _ in 0 ..< 4 { testing.expect(t, game_step(0.016)) }
	testing.expect(t, !game_step(0.016))
	testing.expect_value(t, game.frames, 5)
}
