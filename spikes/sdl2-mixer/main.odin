package main

import "core:fmt"
import SDL "vendor:sdl2"
import Mix "vendor:sdl2/mixer"

main :: proc() {
	SDL.Init({.AUDIO})
	defer SDL.Quit()
	if Mix.OpenAudio(44100, Mix.DEFAULT_FORMAT, 2, 2048) != 0 {
		fmt.eprintln("OpenAudio failed:", SDL.GetError())
		return
	}
	defer Mix.CloseAudio()
	Mix.Init({.OGG})
	wav := Mix.LoadWAV("../../src/KordanorsCabal/Content/LevelUp.wav")
	mus := Mix.LoadMUS("../../src/KordanorsCabal/Content/MinorTheme.ogg")
	fmt.println("wav loaded:", wav != nil, "ogg loaded:", mus != nil)
	if wav == nil || mus == nil { fmt.eprintln(SDL.GetError()) }
}
