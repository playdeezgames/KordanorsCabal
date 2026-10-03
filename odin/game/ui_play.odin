package game

// The in-play screen and its neighbours: the hub with its ten buttons (InPlayProcessor, ModeProcessor and the Neutral, Turn and
// Move modes), the message pages, the map, the status page and the death page. Combat, items, spells and the townspeople are
// ported in later steps; their buttons are drawn as in the original but do nothing yet (marked TODO(step N)).

import "core:fmt"
import "core:strings"

BUTTON_COUNT :: 10
BUTTON_WIDTH :: 11
BUTTON_FIRST_ROW :: 18

// Button numbers, left column 0 to 4 and right column 5 to 9. What each does depends on the mode.
NEUTRAL_TURN_FIGHT :: 0
NEUTRAL_INTERACT :: 1
NEUTRAL_GROUND_ENEMIES :: 2
NEUTRAL_STATUS :: 3
NEUTRAL_MAP :: 4
NEUTRAL_MOVE_RUN :: 5
NEUTRAL_INVENTORY :: 6
NEUTRAL_EQUIPMENT :: 7
NEUTRAL_SPELLS :: 8
NEUTRAL_MENU :: 9

TURN_LEFT :: 0
TURN_AROUND :: 1
TURN_RIGHT :: 5
TURN_CANCEL :: 6

MOVE_FORWARD :: 0
MOVE_LEFT :: 1
MOVE_UP :: 2
MOVE_IN :: 3
MOVE_CANCEL :: 4
MOVE_BACKWARD :: 5
MOVE_RIGHT :: 6
MOVE_DOWN :: 7
MOVE_OUT :: 8

button_position :: proc(i: int) -> (col, row: int) { return (i / 5) * BUTTON_WIDTH, BUTTON_FIRST_ROW + i % 5 }

reset_buttons :: proc(core: ^Core) {
	core.button = 0
	core.button_depth = 0
}
push_button :: proc(core: ^Core, to: int) {
	if core.button_depth < BUTTON_STACK_DEPTH {
		core.button_stack[core.button_depth] = core.button
		core.button_depth += 1
	}
	core.button = to
}
pop_button :: proc(core: ^Core) {
	if core.button_depth > 0 {
		core.button_depth -= 1
		core.button = core.button_stack[core.button_depth]
	} else {
		core.button = 0
	}
}

push_state :: proc(core: ^Core, s: UI_State) {
	if core.stack_len < UI_STACK_DEPTH {
		core.stack[core.stack_len] = s
		core.stack_len += 1
	}
}
// Where to go after the messages; without anything pushed (the original would throw) the in-play screen.
pop_state :: proc(core: ^Core) -> UI_State {
	if core.stack_len == 0 { return .In_Play }
	core.stack_len -= 1
	return core.stack[core.stack_len]
}

// ---- the button bank -------------------------------------------------------------------------------------------------

play_buttons :: proc(core: ^Core) -> (titles: [BUTTON_COUNT]string) {
	w := &core.world
	player := w.player.character
	switch w.player.mode {
	case .Turn:
		titles[TURN_CANCEL], titles[TURN_LEFT], titles[TURN_RIGHT], titles[TURN_AROUND] = "Cancel", "Left", "Right", "Around"
	case .Move:
		f := w.player.facing
		titles[MOVE_CANCEL] = "Cancel"
		if can_move(w, player, DIRECTIONS[f].previous) { titles[MOVE_LEFT] = "Left" }
		if can_move(w, player, DIRECTIONS[f].next) { titles[MOVE_RIGHT] = "Right" }
		if can_move(w, player, f) { titles[MOVE_FORWARD] = "Forward" }
		if can_move(w, player, DIRECTIONS[f].opposite) { titles[MOVE_BACKWARD] = "Backward" }
		if can_move(w, player, .Up) { titles[MOVE_UP] = "Up" }
		if can_move(w, player, .Down) { titles[MOVE_DOWN] = "Down" }
		if can_move(w, player, .In) { titles[MOVE_IN] = "In" }
		if can_move(w, player, .Out) { titles[MOVE_OUT] = "Out" }
	case .Neutral, .None, .Elder, .InnKeeper, .TownDrunk, .Chicken, .BlackMarket, .BlackMage, .Blacksmith, .Constable, .Healer:
		// The townspeople's modes are ported in step 6; until then they draw the neutral bank.
		here := location_get(w, character_get(w, player).location)
		fight := can_fight(w, player)
		titles[NEUTRAL_TURN_FIGHT] = fight ? "FIGHT!" : "Turn..."
		titles[NEUTRAL_MOVE_RUN] = fight ? "RUN!" : "Move..."
		titles[NEUTRAL_MENU] = "Game Menu"
		for s in Spell_Type { if w.player.spells[s] != 0 { titles[NEUTRAL_SPELLS] = "Spells" } }
		if can_map(w, player) { titles[NEUTRAL_MAP] = "Map" }
		if len(items_worn(w, player)) > 0 { titles[NEUTRAL_EQUIPMENT] = "Equipment" }
		titles[NEUTRAL_STATUS] = character_get(w, player).stats[.Unassigned] == 0 ? "Status" : "Level up!"
		if can_do_intimidation(w, player) {
			titles[NEUTRAL_INTERACT] = "Intimidate!"
		} else if here.feature != .None {
			titles[NEUTRAL_INTERACT] = "Interact..."
		}
		if fight {
			titles[NEUTRAL_GROUND_ENEMIES] = fmt.tprintf("Enemies(%d)", len(enemies_of(w, player)))
		} else if len(items_on_ground(w, character_get(w, player).location)) > 0 {
			titles[NEUTRAL_GROUND_ENEMIES] = "Ground..."
		}
		if len(items_in_pack(w, player)) > 0 { titles[NEUTRAL_INVENTORY] = "Inventory" }
	}
	return
}

// ---- input -----------------------------------------------------------------------------------------------------------

play_command :: proc(core: ^Core, c: Command) {
	switch c {
	case .Down: core.button = (core.button + 1) % BUTTON_COUNT
	case .Up: core.button = (core.button + BUTTON_COUNT - 1) % BUTTON_COUNT
	case .Left, .Right: core.button = (core.button + BUTTON_COUNT / 2) % BUTTON_COUNT
	case .Confirm: handle_button(core, core.button)
	case .Cancel: // the original left the pushed button position on its stack here (a leak); the port restores it
		if core.world.player.mode == .Turn || core.world.player.mode == .Move { pop_button(core) }
		core.world.player.mode = .Neutral
	case .None:
	}
}

play_tap :: proc(core: ^Core, col, row: int, precise: bool) {
	for i in 0 ..< BUTTON_COUNT {
		c, r := button_position(i)
		if row == r && col >= c && col < c + BUTTON_WIDTH {
			was := core.button
			core.button = i
			if precise || was == i { handle_button(core, i) }
			return
		}
	}
}

handle_button :: proc(core: ^Core, button: int) {
	w := &core.world
	player := w.player.character
	switch w.player.mode {
	case .Turn:
		f := w.player.facing
		turned := true
		switch button {
		case TURN_CANCEL:
		case TURN_AROUND: w.player.facing = DIRECTIONS[f].opposite
		case TURN_LEFT: w.player.facing = DIRECTIONS[f].previous
		case TURN_RIGHT: w.player.facing = DIRECTIONS[f].next
		case: turned = false
		}
		if turned { pop_button(core); w.player.mode = .Neutral }
	case .Move:
		f := w.player.facing
		switch button {
		case MOVE_CANCEL: pop_button(core); w.player.mode = .Neutral
		case MOVE_DOWN: do_move(core, .Down)
		case MOVE_UP: do_move(core, .Up)
		case MOVE_IN: do_move(core, .In)
		case MOVE_OUT: do_move(core, .Out)
		case MOVE_FORWARD: do_move(core, f)
		case MOVE_BACKWARD: do_move(core, DIRECTIONS[f].opposite)
		case MOVE_LEFT: do_move(core, DIRECTIONS[f].previous)
		case MOVE_RIGHT: do_move(core, DIRECTIONS[f].next)
		}
	case .Neutral, .None, .Elder, .InnKeeper, .TownDrunk, .Chicken, .BlackMarket, .BlackMage, .Blacksmith, .Constable, .Healer:
		fight := can_fight(w, player)
		switch button {
		case NEUTRAL_TURN_FIGHT:
			if fight { fight_action(core, .Fight) } else { push_button(core, 0); w.player.mode = .Turn }
		case NEUTRAL_MOVE_RUN:
			if fight { fight_action(core, .Run) } else { push_button(core, 0); w.player.mode = .Move }
		case NEUTRAL_INTERACT:
			if can_do_intimidation(w, player) { fight_action(core, .Intimidate) } /* TODO(step 6): interact */
		case NEUTRAL_GROUND_ENEMIES:
			if fight { enter_state(core, .Enemies) } else { enter_state(core, .Ground_Inventory) }
		case NEUTRAL_MENU: enter_state(core, .Game_Menu)
		case NEUTRAL_INVENTORY: enter_state(core, .Inventory)
		case NEUTRAL_MAP: if can_map(w, player) { enter_state(core, .Map) }
		case NEUTRAL_EQUIPMENT: enter_state(core, .Equipment)
		case NEUTRAL_STATUS:
			if character_get(w, player).stats[.Unassigned] != 0 { enter_state(core, .Level_Up) } else { enter_state(core, .Status) }
		case NEUTRAL_SPELLS: /* TODO(step 6) */
		}
	}
}

Fight_Action :: enum { Fight, Run, Intimidate }

// The player acts against the enemies here; the messages come first, then the in-play screen again (or the death page).
fight_action :: proc(core: ^Core, action: Fight_Action) {
	w := &core.world
	switch action {
	case .Fight: fight(w, w.player.character)
	case .Run: run(w, w.player.character)
	case .Intimidate: do_intimidation(w, w.player.character)
	}
	show_messages_then(core, .In_Play)
}

do_move :: proc(core: ^Core, d: Direction) {
	w := &core.world
	player := w.player.character
	if !can_move(w, player, d) { return }
	pop_button(core)
	w.player.mode = .Neutral
	if move_character(w, player, d) {
		message_add(w, .None, "You take damage from starvation!")
		if is_dead(w, player) { enter_state(core, .Dead); return }
		push_state(core, .In_Play)
		enter_state(core, .Message)
	}
}

message_command :: proc(core: ^Core, c: Command) {
	if c != .Confirm && c != .Cancel { return }
	w := &core.world
	message_pop(w)
	if w.messages.count > 0 { enter_state(core, .Message); return }
	if is_dead(w, w.player.character) { enter_state(core, .Dead); return }
	enter_state(core, pop_state(core))
}

// ---- drawing: the hub ----------------------------------------------------------------------------------------------

put_cell :: proc(s: ^Screen, col, row: int, glyph: Glyph, inverted: bool, hue: Hue) {
	if col >= 0 && col < CELL_COLUMNS && row >= 0 && row < CELL_ROWS { s[row][col] = {u8(glyph), hue, inverted} }
}
fill_glyph :: proc(s: ^Screen, col, row, w, h: int, glyph: Glyph, hue: Hue) { fill_cells(s, col, row, w, h, u8(glyph), false, hue) }

show_header :: proc(s: ^Screen, title: string) {
	fill_cells(s, 0, 0, CELL_COLUMNS, 1, GLYPH_SPACE, true, .Blue)
	write_text_centered(s, 0, title, true, .Blue)
}

direction_abbreviations :: proc(l: ^Location) -> string {
	b := strings.builder_make(context.temp_allocator)
	first := true
	for d in Direction {
		if l.routes[d].to == {} { continue }
		if !first { strings.write_byte(&b, ',') }
		strings.write_string(&b, DIRECTIONS[d].abbreviation)
		first = false
	}
	return strings.to_string(b)
}

ROUTE_TEXT_HUE := [Route_Type]Hue{.Route_5 = .Green, .Route_7 = .Yellow, .Route_4 = .Black, .Route_8 = .Cyan, .Route_6 = .Purple, .None = .Blue, .Route_1 = .Blue, .Route_2 = .Blue, .Route_3 = .Blue, .Route_9 = .Blue, .Route_10 = .Blue, .Route_11 = .Blue}

draw_play :: proc(core: ^Core) {
	s := &core.screen
	w := &core.world
	player := w.player.character
	here := location_get(w, character_get(w, player).location)
	mode := w.player.mode
	if LOCATION_TYPES[here.type].is_dungeon {
		draw_dungeon(core, here)
	} else {
		show_header(s, LOCATION_TYPES[here.type].name)
		write_text(s, 0, 1, fmt.tprintf("Facing: %s", DIRECTIONS[w.player.facing].name), false, .Black)
		write_text(s, 0, 2, fmt.tprintf("Exits: %s", direction_abbreviations(here)), false, .Black)
		#partial switch mode {
		case .Turn: write_text(s, 0, 4, "Turn which way?", false, .Purple)
		case .Move: write_text(s, 0, 4, "Move which way?", false, .Purple)
		case:
			if here.feature != .None { write_text(s, 0, 3, fmt.tprintf("You see: %s", FEATURE_TYPES[here.feature].name), false, .Black) }
			if is_encumbered(w, player) { write_text(s, 0, 5, "You are encumbered!", false, .Red) }
			for r in here.routes { if r.to != {} && r.type == ROUTE_PORTAL { write_text(s, 0, 7, "There is a portal here!", false, .Purple); break } }
		}
	}
	titles := play_buttons(core)
	for i in 0 ..< BUTTON_COUNT {
		c, r := button_position(i)
		inverted := i == core.button
		fill_cells(s, c, r, BUTTON_WIDTH, 1, GLYPH_SPACE, inverted, .Orange)
		write_text(s, c, r, titles[i], inverted, .Orange)
	}
}

draw_sprite :: proc(s: ^Screen, col, row: int, sprite: ^Sprite) {
	for line, y in sprite.rows {
		for i in 0 ..< len(line) {
			switch line[i] {
			case '#': put_cell(s, col + i, row + y, .Space, true, sprite.hue)
			case ' ': put_cell(s, col + i, row + y, .Space, false, sprite.hue)
			}
		}
	}
}

// Port of ModeProcessor.ShowDungeon: the view down the corridor from where the player stands.
draw_dungeon :: proc(core: ^Core, here: ^Location) {
	s := &core.screen
	w := &core.world
	player := w.player.character
	facing := w.player.facing
	k :: Hue.Black

	// the walls: diagonals into the corners, the far wall as a frame
	for i in 0 ..< 3 {
		put_cell(s, 1 + i, i, .DownwardDiagonal, false, k)
		put_cell(s, 20 - i, i, .UpwardDiagonal, false, k)
		put_cell(s, 3 - i, 15 + i, .UpwardDiagonal, false, k)
		put_cell(s, 18 + i, 15 + i, .DownwardDiagonal, false, k)
	}
	fill_glyph(s, 5, 3, 12, 1, .Horizontal1, k)
	fill_glyph(s, 5, 14, 12, 1, .Horizontal8, k)
	fill_glyph(s, 4, 4, 1, 10, .Vertical1, k)
	fill_glyph(s, 17, 4, 1, 10, .Vertical8, k)
	put_cell(s, 4, 3, .TopLeftCorner, false, k)
	put_cell(s, 17, 3, .TopRightCorner, false, k)
	put_cell(s, 4, 14, .BottomLeftCorner, false, k)
	put_cell(s, 17, 14, .BottomRightCorner, false, k)

	if r, ok := route_at(here, facing); ok { // a door straight ahead
		fill_glyph(s, 9, 6, 4, 1, .Horizontal1, k)
		fill_glyph(s, 8, 6, 1, 8, .Vertical8, k)
		fill_glyph(s, 13, 6, 1, 8, .Vertical1, k)
		put_cell(s, 13, 14, .BottomLeftCorner, false, k)
		put_cell(s, 8, 14, .BottomRightCorner, false, k)
		write_text(s, 10, 9, ROUTE_TYPES[r.type].abbreviation, false, ROUTE_TEXT_HUE[r.type])
	}
	if r, ok := route_at(here, DIRECTIONS[facing].previous); ok { // a side passage on the left
		put_cell(s, 0, 3, .DownwardDiagonal, false, k)
		put_cell(s, 1, 4, .DownwardDiagonal, false, k)
		fill_glyph(s, 1, 5, 1, 12, .Vertical8, k)
		write_text(s, 0, 9, ROUTE_TYPES[r.type].abbreviation[1:2], false, ROUTE_TEXT_HUE[r.type])
	}
	if r, ok := route_at(here, DIRECTIONS[facing].next); ok { // and on the right
		put_cell(s, 21, 3, .UpwardDiagonal, false, k)
		put_cell(s, 20, 4, .UpwardDiagonal, false, k)
		fill_glyph(s, 20, 5, 1, 12, .Vertical1, k)
		write_text(s, 21, 9, ROUTE_TYPES[r.type].abbreviation[0:1], false, ROUTE_TEXT_HUE[r.type])
	}
	if _, ok := route_at(here, .Up); ok { // a way up through the ceiling
		put_cell(s, 7, 0, .DownwardDiagonal, false, k)
		fill_glyph(s, 8, 0, 6, 1, .Horizontal8, k)
		put_cell(s, 14, 0, .UpwardDiagonal, false, k)
	}
	if _, ok := route_at(here, .Down); ok { // and down through the floor
		put_cell(s, 7, 17, .UpwardDiagonal, false, k)
		fill_glyph(s, 8, 17, 6, 1, .Horizontal1, k)
		put_cell(s, 14, 17, .DownwardDiagonal, false, k)
	}
	for d in ([]Direction{.Out, .In}) { // portals are the only routes in or out, and the only ones with a picture
		if _, ok := route_at(here, d); ok { draw_sprite(s, 5, 5, &PORTAL_SPRITE) }
	}

	for item_id in items_on_ground(w, character_get(w, player).location) {
		type := item_get(w, item_id).type
		if d := ITEM_DISPLAY[type]; d.present {
			hue := d.hue
			if d.flicker { hue = ORB_HUES[(core.ticks / 6) % len(ORB_HUES)] } // the original drew a random colour every frame
			put_cell(s, int(d.x), int(d.y), d.glyph, false, hue)
		}
	}
	if enemies := enemies_of(w, player); len(enemies) > 0 {
		sprite := &CHARACTER_SPRITES[character_get(w, enemies[0]).type]
		if sprite.rows[0] != "" { draw_sprite(s, 5, 5, sprite) }
	}

	write_text_centered(s, 1, DIRECTIONS[facing].abbreviation, false, .Purple)
	if is_encumbered(w, player) { write_text_centered(s, 2, "Encumbered", false, .Red) }
}

// ---- messages, map, status, death ------------------------------------------------------------------------------------

draw_message :: proc(core: ^Core) {
	s := &core.screen
	screen_fill(s, GLYPH_SPACE, false, .Black)
	m := message_head(&core.world)
	if m == nil { return }
	row := 0
	for line in strings.split(string(m.text[:m.len]), "\n", context.temp_allocator) {
		write_text(s, 0, row, line, false, .Black)
		row += (len(line) + CELL_COLUMNS - 1) / CELL_COLUMNS
		if row >= CELL_ROWS { break }
	}
}

draw_map :: proc(core: ^Core) {
	s := &core.screen
	w := &core.world
	player := w.player.character
	here_id := character_get(w, player).location
	level := location_get(w, here_id).level
	if level != .None {
		for id in w.location_order {
			l := location_get(w, id)
			if l.level != level || !l.visited { continue }
			hue := Hue.Purple
			if len(enemies_of_at(w, player, id)) > 0 { hue = .Pink } else if has_stairs(l) { hue = .Green } else if len(items_on_ground(w, id)) == 0 { hue = .Black }
			draw_map_cell(s, int(l.column) * 2, int(l.row) * 2, l, id == here_id, hue)
		}
	}
	write_text(s, 2, 22, "Stair", true, .Green)
	write_text(s, 8, 22, "Enemy", true, .Pink)
	write_text(s, 14, 22, "Items", true, .Purple)
}

// The enemies of `who` that stand at `where` (the map asks about places other than the player's).
enemies_of_at :: proc(w: ^World, who: Character_ID, at: Location_ID) -> []Character_ID {
	out := make([dynamic]Character_ID, context.temp_allocator)
	for id in characters_at(w, at) { if id != who && is_enemy_of(w, id, who) { append(&out, id) } }
	return out[:]
}

draw_map_cell :: proc(s: ^Screen, col, row: int, l: ^Location, inverted: bool, hue: Hue) {
	n, e, so, we := l.routes[.North].to != {}, l.routes[.East].to != {}, l.routes[.South].to != {}, l.routes[.West].to != {}
	put_cell(s, col, row, n && we ? .ElbowUpLeft : n ? .Vertical5 : we ? .Horizontal5 : .ElbowDownRight, inverted, hue)
	put_cell(s, col + 1, row, n && e ? .ElbowUpRight : n ? .Vertical5 : e ? .Horizontal5 : .ElbowDownLeft, inverted, hue)
	put_cell(s, col, row + 1, so && we ? .ElbowDownLeft : so ? .Vertical5 : we ? .Horizontal5 : .ElbowUpRight, inverted, hue)
	put_cell(s, col + 1, row + 1, so && e ? .ElbowDownRight : so ? .Vertical5 : e ? .Horizontal5 : .ElbowUpLeft, inverted, hue)
}

draw_status :: proc(core: ^Core) {
	s := &core.screen
	w := &core.world
	id := w.player.character
	centered_header(s, "Status")
	line :: proc(s: ^Screen, col, row: int, stat: Stat, text: string) {
		write_text(s, col, row, fmt.tprintf("%s %s", STATS[stat].abbreviation, text), false, .Black)
	}
	v :: proc(w: ^World, id: Character_ID, stat: Stat) -> string { return fmt.tprintf("%d", stat_of(w, id, stat)) }
	line(s, 0, 1, .Strength, v(w, id, .Strength))
	line(s, 0, 2, .Dexterity, v(w, id, .Dexterity))
	line(s, 0, 3, .HP, fmt.tprintf("%d/%d", health_current(w, id), stat_of(w, id, .HP)))
	line(s, 11, 1, .Influence, v(w, id, .Influence))
	line(s, 11, 2, .Willpower, v(w, id, .Willpower))
	line(s, 11, 3, .MP, fmt.tprintf("%d/%d", mp_current(w, id), stat_of(w, id, .MP)))
	line(s, 0, 5, .Power, v(w, id, .Power))
	line(s, 0, 6, .Mana, fmt.tprintf("%d/%d", mana_current(w, id), stat_of(w, id, .Mana)))
	line(s, 11, 5, .XP, fmt.tprintf("%d/%d", stat_of(w, id, .XP), stat_of(w, id, .XP_Goal)))
	line(s, 0, 8, .Money, v(w, id, .Money))
	line(s, 0, 10, .Hunger, v(w, id, .Hunger))
	row := 11
	for stat in ([]Stat{.Highness, .Drunkenness, .Food_Poisoning, .Chafing}) {
		if stat_of(w, id, stat) > 0 { line(s, 0, row, stat, v(w, id, stat)); row += 1 }
	}
	write_text(s, 0, CELL_ROWS - 1, fmt.tprintf("Seed %09d", w.seed), false, .Purple) // new in the port (decision review 6)
}

draw_dead :: proc(core: ^Core) {
	s := &core.screen
	write_text(s, 0, 0, "yer dead!", false, .Red)
	if _, ok := item_in_slot(&core.world, core.world.player.character, .Legs); ok { write_text(s, 0, 2, "But at least you died with dignity!", false, .Black) }
}

draw_enemies :: proc(core: ^Core) {
	s := &core.screen
	w := &core.world
	centered_header(s, "Enemies")
	row := 1
	for id in enemies_of(w, w.player.character) {
		write_text(s, 0, row, fmt.tprintf("%s(%d/%d)", CHARACTER_TYPES[character_get(w, id).type].name, health_current(w, id), stat_of(w, id, .HP)), false, .Black)
		row += 1
	}
}
