#+build js
package main

// Browser: odin.js calls _start (main) then step() every animation frame.
main :: proc() {
	game_init()
}

@(export)
step :: proc(dt: f64) -> (keep_going: bool) {
	return game_step(dt)
}
