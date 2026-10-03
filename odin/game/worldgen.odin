package game

// New-game world generation: a port of World.Start and its helpers. Everything procedural happens here once; the result is
// stored in the world and never regenerated. The random draws are NOT in the same order as the VB game (different generator),
// so tests check structure (counts, a spanning tree per level, reachable bosses, one key per locked door), not exact output.

MAZE_COLS :: 11
MAZE_ROWS :: 11
MOON_COLS :: 11
MOON_ROWS :: 11

// What the numbered route types of the data mean (they have no names there).
ROUTE_TOWN :: Route_Type.Route_1
ROUTE_CORRIDOR :: Route_Type.Route_2
ROUTE_STAIRS :: Route_Type.Route_3
ROUTE_LOCKED :: Route_Type.Route_4 // the FE door; Route_5 .. Route_9 are the boss doors of the five levels
ROUTE_PORTAL :: Route_Type.Route_10
ROUTE_MOON :: Route_Type.Route_11

DUNGEON_LEVELS := [5]Dungeon_Level{.Level_I, .Level_II, .Level_III, .Level_IV, .Level_V}
BOSS_DOORS := [5]Route_Type{.Route_5, .Route_6, .Route_7, .Route_8, .Route_9}
BOSS_KEYS := [5]Item_Type{.CU_Key, .AG_Key, .AU_Key, .PT_Key, .Elemental_Orb}

// ---- the maze (randomized Prim's algorithm: a spanning tree) --------------------------------------------------------

Maze :: struct {
	open_east, open_south: [MAZE_ROWS][MAZE_COLS]bool, // a door between a cell and its east or south neighbour is open
}

MAZE_STEPS := [4][2]int{{0, -1}, {1, 0}, {0, 1}, {-1, 0}} // N, E, S, W

maze_generate :: proc(r: ^Rng) -> (m: Maze) {
	inside, in_frontier: [MAZE_ROWS][MAZE_COLS]bool
	frontier := make([dynamic][2]int, context.temp_allocator)

	add_neighbours :: proc(c, row: int, inside, in_frontier: ^[MAZE_ROWS][MAZE_COLS]bool, frontier: ^[dynamic][2]int) {
		for d in MAZE_STEPS {
			nc, nr := c + d[0], row + d[1]
			if nc < 0 || nr < 0 || nc >= MAZE_COLS || nr >= MAZE_ROWS || inside[nr][nc] || in_frontier[nr][nc] { continue }
			in_frontier[nr][nc] = true
			append(frontier, [2]int{nc, nr})
		}
	}
	sc, sr := rng_range(r, 0, MAZE_COLS - 1), rng_range(r, 0, MAZE_ROWS - 1)
	inside[sr][sc] = true
	add_neighbours(sc, sr, &inside, &in_frontier, &frontier)
	for len(frontier) > 0 {
		i := pick_index(r, len(frontier))
		cell := frontier[i]
		ordered_remove(&frontier, i)
		in_frontier[cell.y][cell.x] = false
		options: [4][2]int
		n := 0
		for d in MAZE_STEPS {
			nc, nr := cell.x + d[0], cell.y + d[1]
			if nc >= 0 && nr >= 0 && nc < MAZE_COLS && nr < MAZE_ROWS && inside[nr][nc] { options[n] = {nc, nr}; n += 1 }
		}
		to := options[pick_index(r, n)]
		if to.x == cell.x + 1 { m.open_east[cell.y][cell.x] = true } else if to.x == cell.x - 1 { m.open_east[to.y][to.x] = true }
		else if to.y == cell.y + 1 { m.open_south[cell.y][cell.x] = true } else { m.open_south[to.y][to.x] = true }
		inside[cell.y][cell.x] = true
		add_neighbours(cell.x, cell.y, &inside, &in_frontier, &frontier)
	}
	return
}

// Is the door out of (column,row) in cardinal direction `d` open?
maze_open :: proc(m: ^Maze, column, row: int, d: Direction) -> bool {
	switch d {
	case .North: return row > 0 && m.open_south[row - 1][column]
	case .East: return column < MAZE_COLS - 1 && m.open_east[row][column]
	case .South: return row < MAZE_ROWS - 1 && m.open_south[row][column]
	case .West: return column > 0 && m.open_east[row][column - 1]
	case .None, .Up, .Down, .In, .Out: return false
	}
	return false
}

// ---- helpers -----------------------------------------------------------------------------------------------------

@(private)
connect :: proc(w: ^World, from: Location_ID, d: Direction, type: Route_Type, to: Location_ID) {
	location_get(w, from).routes[d] = {to, type}
}

// A two-way route: `d` out of `from`, the opposite direction back.
@(private)
connect_both :: proc(w: ^World, from: Location_ID, d: Direction, type: Route_Type, to: Location_ID) {
	connect(w, from, d, type, to)
	connect(w, to, DIRECTIONS[d].opposite, type, from)
}

@(private)
pick_one :: proc(w: ^World, list: []$T) -> (T, bool) {
	if len(list) == 0 { return {}, false }
	return list[pick_index(&w.rng, len(list))], true
}

// ---- generation --------------------------------------------------------------------------------------------------

// Fills an initialised, empty world with a new game. Returns false only if the content makes generation impossible (a bug in
// the data, caught by a test), never for random reasons.
world_generate :: proc(w: ^World) -> bool {
	create_town(w) or_return
	create_dungeon(w) or_return
	create_moon(w)
	create_features(w) or_return
	create_player(w) or_return
	return true
}

@(private)
create_town :: proc(w: ^World) -> bool {
	centre := create_location(w, .Town_Square)
	north, north_east, east, south_east := create_location(w, .Town), create_location(w, .Town), create_location(w, .Town), create_location(w, .Town)
	south, south_west, west, north_west := create_location(w, .Town), create_location(w, .Town), create_location(w, .Town), create_location(w, .Town)

	connect_both(w, centre, .North, ROUTE_TOWN, north)
	connect_both(w, centre, .East, ROUTE_TOWN, east)
	connect_both(w, centre, .South, ROUTE_TOWN, south)
	connect_both(w, centre, .West, ROUTE_TOWN, west)
	connect_both(w, north_west, .East, ROUTE_TOWN, north)
	connect_both(w, north_west, .South, ROUTE_TOWN, west)
	connect_both(w, south_west, .East, ROUTE_TOWN, south)
	connect_both(w, south_west, .North, ROUTE_TOWN, west)
	connect_both(w, north_east, .West, ROUTE_TOWN, north)
	connect_both(w, north_east, .South, ROUTE_TOWN, east)
	connect_both(w, south_east, .North, ROUTE_TOWN, east)
	connect_both(w, south_east, .West, ROUTE_TOWN, south)

	// the church entrance hangs off a random town location, in a cardinal direction that is still free
	town_location := pick_one(w, locations_of_type(w, .Town)) or_return
	entrance := create_location(w, .Church_Entrance)
	free_directions := make([dynamic]Direction, context.temp_allocator)
	for d in Direction {
		if DIRECTIONS[d].is_cardinal && location_get(w, town_location).routes[d].to == {} { append(&free_directions, d) }
	}
	d := pick_one(w, free_directions[:]) or_return
	connect_both(w, town_location, d, ROUTE_TOWN, entrance)
	return true
}

@(private)
create_dungeon :: proc(w: ^World) -> bool {
	entrance := pick_one(w, locations_of_type(w, .Church_Entrance)) or_return
	from := entrance
	for level, i in DUNGEON_LEVELS {
		from = create_dungeon_level(w, from, level, BOSS_KEYS[i], BOSS_DOORS[i]) or_return
	}
	return true
}

// One level: a 11 x 11 maze, its locks and keys, its items, the stairs from `from`, and its monsters. Returns the boss room.
@(private)
create_dungeon_level :: proc(w: ^World, from: Location_ID, level: Dungeon_Level, boss_key: Item_Type, boss_door: Route_Type) -> (boss: Location_ID, ok: bool) {
	maze := maze_generate(&w.rng)
	cells := make([]Location_ID, MAZE_COLS * MAZE_ROWS, context.temp_allocator)
	for row in 0 ..< MAZE_ROWS {
		for column in 0 ..< MAZE_COLS {
			id := create_location(w, .Dungeon)
			l := location_get(w, id)
			l.level, l.column, l.row = level, u8(column), u8(row)
			cells[column + row * MAZE_COLS] = id
		}
	}
	for row in 0 ..< MAZE_ROWS {
		for column in 0 ..< MAZE_COLS {
			id := cells[column + row * MAZE_COLS]
			for d in Direction.North ..= Direction.West {
				if !maze_open(&maze, column, row, d) { continue }
				step := MAZE_STEPS[int(d) - 1]
				connect(w, id, d, ROUTE_CORRIDOR, cells[(column + step[0]) + (row + step[1]) * MAZE_COLS])
			}
			if route_count(location_get(w, id)) == 1 { location_get(w, id).type = .Dungeon_Dead_End }
		}
	}

	// Every dead end is locked behind a door (FE key) except the boss room, whose door needs the level's own key.
	dead_ends := make([dynamic]Location_ID, context.temp_allocator)
	for id in cells { if location_get(w, id).type == .Dungeon_Dead_End { append(&dead_ends, id) } }
	if len(dead_ends) == 0 { return {}, false }
	keys := make([dynamic]Item_Type, context.temp_allocator)
	for dead_end in dead_ends {
		inbound := dead_end_inbound(w, dead_end)
		inbound^ = {inbound.to, ROUTE_LOCKED}
		append(&keys, Item_Type.FE_Key)
	}
	keys[0] = boss_key
	boss = pick_one(w, dead_ends[:]) or_return
	location_get(w, boss).type = .Dungeon_Boss
	boss_inbound := dead_end_inbound(w, boss)
	boss_inbound.type = boss_door
	for key in keys { spawn_item(w, cells, level, key) }
	populate_items(w, cells, level)

	entry_candidates := make([dynamic]Location_ID, context.temp_allocator)
	for id in cells { if route_count(location_get(w, id)) > 1 { append(&entry_candidates, id) } }
	entry := pick_one(w, entry_candidates[:]) or_return
	connect_both(w, from, .Down, ROUTE_STAIRS, entry)
	populate_characters(w, cells, level)
	return boss, true
}

// The route (owned by the neighbour) that leads into a dead end.
@(private)
dead_end_inbound :: proc(w: ^World, dead_end: Location_ID) -> ^Route {
	l := location_get(w, dead_end)
	for d in Direction {
		if l.routes[d].to != {} {
			return &location_get(w, l.routes[d].to).routes[DIRECTIONS[d].opposite]
		}
	}
	return nil
}

@(private)
create_moon :: proc(w: ^World) {
	cells := make([]Location_ID, MOON_COLS * MOON_ROWS, context.temp_allocator)
	for row in 0 ..< MOON_ROWS {
		for column in 0 ..< MOON_COLS {
			id := create_location(w, .Moon)
			l := location_get(w, id)
			l.level, l.column, l.row = .The_Moon, u8(column), u8(row)
			cells[column + row * MOON_COLS] = id
		}
	}
	for row in 0 ..< MOON_ROWS {
		for column in 0 ..< MOON_COLS {
			here := cells[column + row * MOON_COLS]
			connect(w, here, .North, ROUTE_MOON, cells[column + ((row + MOON_ROWS - 1) % MOON_ROWS) * MOON_COLS])
			connect(w, here, .South, ROUTE_MOON, cells[column + ((row + 1) % MOON_ROWS) * MOON_COLS])
			connect(w, here, .East, ROUTE_MOON, cells[((column + 1) % MOON_COLS) + row * MOON_COLS])
			connect(w, here, .West, ROUTE_MOON, cells[((column + MOON_COLS - 1) % MOON_COLS) + row * MOON_COLS])
		}
	}
	populate_characters(w, cells, .The_Moon)
	populate_items(w, cells, .The_Moon)
}

@(private)
create_features :: proc(w: ^World) -> bool {
	for feature in Feature_Type {
		if feature == .None { continue }
		free := make([dynamic]Location_ID, context.temp_allocator)
		for id in locations_of_type(w, FEATURE_TYPES[feature].location_type) {
			if location_get(w, id).feature == .None { append(&free, id) }
		}
		at := pick_one(w, free[:]) or_return
		location_get(w, at).feature = feature
		if feature == .Graham_the_Innkeeper { // the innkeeper's cellar
			cellar := create_location(w, .Cellar)
			connect_both(w, at, .Down, ROUTE_STAIRS, cellar)
		}
	}
	return true
}

@(private)
create_player :: proc(w: ^World) -> bool {
	start := pick_one(w, locations_of_type(w, .Town_Square)) or_return
	id := create_character(w, .N00b, start)
	location_get(w, start).visited = true
	w.player = {character = id, mode = .Neutral}
	cardinal := make([dynamic]Direction, context.temp_allocator)
	for d in Direction { if DIRECTIONS[d].is_cardinal { append(&cardinal, d) } }
	w.player.facing = pick_one(w, cardinal[:]) or_return

	// Two rounds of rolling for the unassigned points; the second round starts from whatever the first left unassigned.
	roll_stats(w, {{.Dexterity, 1}, {.Influence, 1}, {.Strength, 1}, {.Unassigned, 2}, {.Willpower, 1}})
	roll_stats(w, {{.Dexterity, 1}, {.Influence, 1}, {.Power, 1}, {.Strength, 1}, {.Unassigned, 1}, {.Willpower, 1}})
	return true
}

@(private)
roll_stats :: proc(w: ^World, table: []struct { stat: Stat, weight: int }) {
	c := player_character(w)
	dice := c.stats[.Unassigned]
	c.stats[.Unassigned] = 0
	weights := make([]int, len(table), context.temp_allocator)
	for entry, i in table { weights[i] = entry.weight }
	for _ in 0 ..< dice { stat_add(c, table[pick_weighted(&w.rng, weights)].stat, 1) }
}

// ---- population ---------------------------------------------------------------------------------------------------

@(private)
populate_characters :: proc(w: ^World, cells: []Location_ID, level: Dungeon_Level) {
	for type in Character_Type {
		if type == .None { continue }
		def := &CHARACTER_TYPES[type]
		count := def.spawn_count[level]
		if count == 0 { continue }
		candidates := make([dynamic]Location_ID, context.temp_allocator)
		for id in cells { if location_get(w, id).type in def.spawn_locations[level] { append(&candidates, id) } }
		if len(candidates) == 0 { continue } // the data asks for monsters where nothing allows them; a test checks the data
		for _ in 0 ..< count { create_character(w, type, pick_one(w, candidates[:]) or_continue) }
	}
}

@(private)
populate_items :: proc(w: ^World, cells: []Location_ID, level: Dungeon_Level) {
	for type in Item_Type {
		if type == .None { continue }
		for _ in 0 ..< roll(&w.rng, ITEM_TYPES[type].spawn[level].dice) { spawn_item(w, cells, level, type) }
	}
}

@(private)
spawn_item :: proc(w: ^World, cells: []Location_ID, level: Dungeon_Level, type: Item_Type) {
	allowed := ITEM_TYPES[type].spawn[level].locations
	if allowed == {} { return }
	candidates := make([dynamic]Location_ID, context.temp_allocator)
	for id in cells { if location_get(w, id).type in allowed { append(&candidates, id) } }
	where_to, ok := pick_one(w, candidates[:])
	if !ok { return }
	item_put_on_ground(w, create_item(w, type), where_to)
}
