#+build !js
package main

// Native platform (development and tests). D6: this is where core:os and SDL2 are allowed.
import "core:fmt"
import "core:os"
import "core:strconv"
import "core:strings"
import SDL "vendor:sdl2"
import "kc:game"
import Mix "vendor:sdl2/mixer"

CONTENT :: "assets/" // next to the working directory; build.sh copies the game assets there
SFX_FILES := [game.Sfx]string{
	.None = "", .Character_Creation = "RollDice.wav", .Enemy_Death = "EnemyDeath.wav", .Enemy_Hit = "EnemyHit.wav", .Level_Up = "LevelUp.wav",
	.Miss = "Miss.wav", .Player_Death = "PlayerDeath.wav", .Player_Hit = "PlayerHit.wav", .Unlock_Door = "UnlockDoor.wav",
}

native_services :: proc() -> game.Services {
	return {
		storage_get = storage_get, storage_set = storage_set, storage_remove = storage_remove,
		download_text = proc(filename, text: string) {
			os.make_directory("exports")
			path := strings.concatenate({"exports/", filename}, context.temp_allocator)
			err := os.write_entire_file(path, transmute([]byte)text)
			fmt.println("export ->", path, err == nil ? "ok" : "FAILED")
		},
		request_file_pick = proc() { pick_requested = true },
		entropy = proc() -> u64 {
			if fixed_seed >= 0 { return u64(fixed_seed) } // --seed N
			return u64(SDL.GetPerformanceCounter()) * 2862933555777941757 + u64(SDL.GetTicks())
		},
		log = proc(message: string) { fmt.println(message) },
	}
}

pick_requested: bool
fixed_seed := -1 // set by --seed N

// Stand-in for a file picker: reads ./import.json, or reports "cancelled" when it does not exist.
deliver_pick :: proc(events: ^[dynamic]game.Input_Event) {
	if !pick_requested { return }
	pick_requested = false
	if data, err := os.read_entire_file("import.json", context.temp_allocator); err == nil {
		append(events, game.Input_Event{kind = .File_Text, text = string(data)})
	} else {
		append(events, game.Input_Event{kind = .File_Cancelled})
	}
}

main :: proc() {
	for arg, i in os.args { if arg == "--seed" && i + 1 < len(os.args) { if n, ok := strconv.parse_int(os.args[i + 1]); ok && n >= 0 && n < 1_000_000_000 { fixed_seed = n } } }
	if SDL.Init({.VIDEO, .EVENTS, .AUDIO}) != 0 { fmt.eprintln(SDL.GetError()); os.exit(1) }
	defer SDL.Quit()
	LOGICAL_W :: game.FRAME_WIDTH * game.FRAME_STRETCH_X
	window := SDL.CreateWindow("platform api spike", SDL.WINDOWPOS_CENTERED, SDL.WINDOWPOS_CENTERED, LOGICAL_W * 2, game.FRAME_HEIGHT * 2, {.SHOWN, .RESIZABLE})
	renderer := SDL.CreateRenderer(window, -1, {.ACCELERATED, .PRESENTVSYNC})
	SDL.RenderSetLogicalSize(renderer, LOGICAL_W, game.FRAME_HEIGHT)
	texture := SDL.CreateTexture(renderer, .ABGR8888, .STREAMING, game.FRAME_WIDTH, game.FRAME_HEIGHT) // bytes R,G,B,A in memory
	defer { SDL.DestroyTexture(texture); SDL.DestroyRenderer(renderer); SDL.DestroyWindow(window) }

	audio_ok := Mix.OpenAudio(44100, Mix.DEFAULT_FORMAT, 2, 2048) == 0
	chunks: [game.Sfx]^Mix.Chunk
	if audio_ok {
		for sfx in game.Sfx { if sfx != .None { chunks[sfx] = Mix.LoadWAV(strings.clone_to_cstring(strings.concatenate({CONTENT, SFX_FILES[sfx]}, context.temp_allocator), context.temp_allocator)) } }
		if music := Mix.LoadMUS(CONTENT + "MinorTheme.ogg"); music != nil { Mix.PlayMusic(music, -1) }
	}

	core := new(game.Core)
	game.core_init(core, native_services())
	out: game.Step_Output
	events: [dynamic]game.Input_Event
	prev := SDL.GetTicks()
	quit := false
	for !quit {
		clear(&events)
		e: SDL.Event
		for SDL.PollEvent(&e) {
			#partial switch e.type {
			case .QUIT: quit = true
			case .KEYDOWN:
				if e.key.repeat != 0 { continue }
				cmd: game.Command
				#partial switch e.key.keysym.scancode {
				case .UP, .KP_8: cmd = .Up
				case .DOWN, .KP_2: cmd = .Down
				case .LEFT, .KP_4: cmd = .Left
				case .RIGHT, .KP_6: cmd = .Right
				case .SPACE, .RETURN, .KP_ENTER, .KP_5: cmd = .Confirm
				case .ESCAPE, .BACKSPACE: cmd = .Cancel
				}
				if cmd != .None { append(&events, game.Input_Event{kind = .Command, command = cmd}) }
			case .MOUSEBUTTONDOWN, .FINGERDOWN:
				// window pixels -> logical (416 x 240) -> frame pixels (de-stretch) -> cell
				wx, wy: f32
				if e.type == .MOUSEBUTTONDOWN { wx, wy = f32(e.button.x), f32(e.button.y) } else {
					w, h: i32; SDL.GetWindowSize(window, &w, &h); wx, wy = e.tfinger.x * f32(w), e.tfinger.y * f32(h)
				}
				ww, wh, ow, oh: i32
				SDL.GetWindowSize(window, &ww, &wh)
				SDL.GetRendererOutputSize(renderer, &ow, &oh)
				px, py := wx * f32(ow) / f32(ww), wy * f32(oh) / f32(wh) // window -> output pixels (HiDPI)
				scale := min(f32(ow) / LOGICAL_W, f32(oh) / game.FRAME_HEIGHT)
				lx, ly := (px - (f32(ow) - LOGICAL_W * scale) / 2) / scale, (py - (f32(oh) - game.FRAME_HEIGHT * scale) / 2) / scale
				fx, fy := lx / game.FRAME_STRETCH_X, ly
				col, row := int((fx - game.BORDER_X) / game.CELL_SIZE), int((fy - game.BORDER_Y) / game.CELL_SIZE)
				if fx < game.BORDER_X { col = -1 }; if fy < game.BORDER_Y { row = -1 }
				append(&events, game.Input_Event{kind = .Tap, col = i16(col), row = i16(row), precise = e.type == .MOUSEBUTTONDOWN})
			}
		}
		deliver_pick(&events)
		now := SDL.GetTicks()
		game.core_step(core, {dt = f64(now - prev) / 1000, events = events[:]}, &out)
		prev = now
		if out.frame_changed { SDL.UpdateTexture(texture, nil, out.frame, game.FRAME_WIDTH * 4) }
		SDL.RenderClear(renderer)
		SDL.RenderCopy(renderer, texture, nil, nil)
		SDL.RenderPresent(renderer)
		if audio_ok {
			Mix.Volume(-1, i32(out.sfx_volume * 128)); Mix.VolumeMusic(i32(out.music_volume * 128))
			for i in 0 ..< out.sfx_count { if c := chunks[out.sfx[i]]; c != nil { Mix.PlayChannel(-1, c, 0) } }
		}
		if out.quit_requested { quit = true }
		free_all(context.temp_allocator)
	}
}
