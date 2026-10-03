package main

import "core:fmt"

frames := 0

main :: proc() {
	fmt.println("hello from odin js_wasm32")
}

@(export)
step :: proc(dt: f64) -> (keep_going: bool) {
	frames += 1
	if frames == 3 { fmt.println("3 frames, dt>0:", dt >= 0) }
	return frames < 5
}
