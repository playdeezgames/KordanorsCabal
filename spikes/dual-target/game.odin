package main

// Shared game code: no platform imports except core:fmt and core:math.
import "core:fmt"
import "core:math"

Game :: struct {
	frames: int,
	angle:  f64,
}

game: Game

game_init :: proc() {
	fmt.println("game_init")
}

// Returns false when the game wants to quit.
game_step :: proc(dt: f64) -> bool {
	game.frames += 1
	game.angle += dt * math.TAU
	if game.frames == 3 {
		fmt.printf("game_step: frame %d, sin=%.3f\n", game.frames, math.sin(game.angle))
	}
	return game.frames < 5
}
