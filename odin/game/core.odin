package game

// The portable game core. At this stage it only shows the title screen (a port of TitleScreenProcessor and MenuProcessor),
// to prove the whole pipeline: content generation, rendering, input, services, tests, web and native builds.
// The rest of the game is ported system by system (see PORT.md).

Title_Item :: enum { Start, Continue, Instructions, Options, About, Quit }
TITLE_LABELS := [Title_Item]string{.Start = "Start", .Continue = "Continue", .Instructions = "Instructions", .Options = "Options", .About = "About", .Quit = "Quit"}
TITLE_FIRST_ROW :: 14

Core :: struct {
	services: Services,
	screen:   Screen,
	frame:    [FRAME_WIDTH * FRAME_HEIGHT]u32,
	selected: Title_Item,
}

core_init :: proc(core: ^Core, services: Services) {
	core^ = {}
	core.services = services
}

// Port of TitleScreenProcessor.ShowPrompt + MenuProcessor.UpdateBuffer.
draw_title :: proc(core: ^Core) {
	s := &core.screen
	screen_fill(s, GLYPH_SPACE, false, .Blue)
	fill_cells(s, 0, 0, CELL_COLUMNS, 5, glyph_of('*'), true, .Blue)
	fill_cells(s, 1, 1, CELL_COLUMNS - 2, 3, GLYPH_SPACE, true, .Blue)
	write_text(s, 0, 2, "*  Kordanor's Cabal  *", true, .Blue)
	write_text(s, 0, 6, "A Game in VB.NET About", false, .Black)
	write_text(s, 0, 7, "Lookin' Like a Dungeon", false, .Black)
	write_text(s, 0, 8, " Crawler Written  for ", false, .Black)
	write_text(s, 0, 9, "      the VIC-20      ", false, .Black)
	write_text(s, 0, 11, "   A Production  of   ", false, .Black)
	write_text(s, 0, 12, "   TheGrumpyGameDev   ", false, .Black)
	write_text(s, 0, CELL_ROWS - 1, "Controls: Arrows/Space", false, .Blue)
	for item in Title_Item {
		label := TITLE_LABELS[item]
		write_text_centered(s, TITLE_FIRST_ROW + int(item), label, item == core.selected, .Orange)
	}
}

core_step :: proc(core: ^Core, input: Step_Input, out: ^Step_Output) {
	for e in input.events {
		#partial switch e.kind {
		case .Command:
			#partial switch e.command {
			case .Up: core.selected = Title_Item((int(core.selected) + len(Title_Item) - 1) % len(Title_Item))
			case .Down: core.selected = Title_Item((int(core.selected) + 1) % len(Title_Item))
			}
		case .Tap:
			if row := int(e.row) - TITLE_FIRST_ROW; row >= 0 && row < len(Title_Item) && e.col >= 0 && e.col < CELL_COLUMNS { core.selected = Title_Item(row) }
		}
	}
	draw_title(core)
	rasterize(&core.screen, &core.frame)
	out.frame = &core.frame
	out.frame_changed = true
	out.sfx_count = 0
	out.sfx_volume = 0.5
	out.music_volume = 0.5
	out.quit_requested = false
	free_all(context.temp_allocator)
}
