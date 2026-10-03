package main

import "core:fmt"
import "core:os"
import SDL "vendor:sdl2"

W :: 192
H :: 252 // roughly the game's 22x23 cell view plus border

main :: proc() {
	if SDL.Init({.VIDEO, .EVENTS}) != 0 {
		fmt.eprintln("SDL.Init failed:", SDL.GetError())
		os.exit(1)
	}
	defer SDL.Quit()

	fmt.println("video driver:", SDL.GetCurrentVideoDriver())

	window := SDL.CreateWindow("sdl2 spike", SDL.WINDOWPOS_CENTERED, SDL.WINDOWPOS_CENTERED, W * 3, H * 3, {.SHOWN, .RESIZABLE})
	if window == nil {
		fmt.eprintln("CreateWindow failed:", SDL.GetError())
		os.exit(1)
	}
	defer SDL.DestroyWindow(window)

	renderer := SDL.CreateRenderer(window, -1, {.ACCELERATED, .PRESENTVSYNC})
	if renderer == nil {
		fmt.eprintln("CreateRenderer failed:", SDL.GetError())
		os.exit(1)
	}
	defer SDL.DestroyRenderer(renderer)

	info: SDL.RendererInfo
	SDL.GetRendererInfo(renderer, &info)
	fmt.println("renderer:", info.name)

	SDL.RenderSetLogicalSize(renderer, W, H)
	tex := SDL.CreateTexture(renderer, .RGBA32, .STREAMING, W, H)
	defer SDL.DestroyTexture(tex)

	pixels: [W * H]u32
	frames := 0
	keys_seen := 0
	running := true
	for running && frames < 120 {
		e: SDL.Event
		for SDL.PollEvent(&e) {
			#partial switch e.type {
			case .QUIT: running = false
			case .KEYDOWN: keys_seen += 1
			case .FINGERDOWN, .MOUSEBUTTONDOWN: keys_seen += 1
			}
		}
		for y in 0 ..< H {
			for x in 0 ..< W {
				pixels[y * W + x] = 0xFF000000 | u32((x + frames) & 0xFF) | u32(y & 0xFF) << 8
			}
		}
		SDL.UpdateTexture(tex, nil, &pixels[0], W * 4)
		SDL.RenderClear(renderer)
		SDL.RenderCopy(renderer, tex, nil, nil)
		if frames == 10 { readback_check(renderer, window, pixels[:], frames) }
		SDL.RenderPresent(renderer)
		frames += 1
	}
	fmt.println("frames rendered:", frames, "input events:", keys_seen)
}

// Reads the rendered frame back from the renderer and compares sampled points with the source buffer.
readback_check :: proc(renderer: ^SDL.Renderer, window: ^SDL.Window, pixels: []u32, frame: int) {
	ow, oh: i32
	SDL.GetRendererOutputSize(renderer, &ow, &oh)
	out := make([]u32, int(ow) * int(oh))
	defer delete(out)
	if SDL.RenderReadPixels(renderer, nil, u32(SDL.PixelFormatEnum.RGBA32), &out[0], ow * 4) != 0 {
		fmt.eprintln("RenderReadPixels failed:", SDL.GetError())
		return
	}
	// With logical size, the image is scaled uniformly and letterboxed; compute the scale and offset.
	scale := min(f32(ow) / W, f32(oh) / H)
	off_x := (f32(ow) - W * scale) / 2
	off_y := (f32(oh) - H * scale) / 2
	fmt.println("output size:", ow, oh, "scale:", scale)
	matches, total := 0, 0
	for sy in ([]int{0, 10, 100, 200, H - 1}) {
		for sx in ([]int{0, 5, 80, 150, W - 1}) {
			px := int(off_x + (f32(sx) + 0.5) * scale)
			py := int(off_y + (f32(sy) + 0.5) * scale)
			got := out[py * int(ow) + px]
			want := pixels[sy * W + sx]
			total += 1
			if got == want { matches += 1 } else {
				fmt.printf("  mismatch at src (%d,%d): got %08x want %08x\n", sx, sy, got, want)
			}
		}
	}
	fmt.printf("readback frame %d: %d/%d sampled pixels match\n", frame, matches, total)
}
