package main

// Port of SPLORR.Game Maze.Generate (task 9): randomized Prim's algorithm on an N x M grid. Result: a spanning tree.
MAZE_COLS :: 11
MAZE_ROWS :: 11

Maze :: struct {
	open_east, open_south: [MAZE_ROWS][MAZE_COLS]bool, // a door between a cell and its east or south neighbour is open
}

maze_generate :: proc(r: ^Rng) -> (m: Maze) {
	inside: [MAZE_ROWS][MAZE_COLS]bool
	in_frontier: [MAZE_ROWS][MAZE_COLS]bool
	frontier: [dynamic][2]int
	defer delete(frontier)

	add_neighbors :: proc(c, row: int, inside, in_frontier: ^[MAZE_ROWS][MAZE_COLS]bool, frontier: ^[dynamic][2]int) {
		for d in ([4][2]int{{0, -1}, {1, 0}, {0, 1}, {-1, 0}}) { // N, E, S, W like the VB direction table
			nc, nr := c + d[0], row + d[1]
			if nc < 0 || nr < 0 || nc >= MAZE_COLS || nr >= MAZE_ROWS || inside[nr][nc] || in_frontier[nr][nc] { continue }
			in_frontier[nr][nc] = true
			append(frontier, [2]int{nc, nr})
		}
	}
	sc, sr := rng_range(r, 0, MAZE_COLS - 1), rng_range(r, 0, MAZE_ROWS - 1)
	inside[sr][sc] = true
	add_neighbors(sc, sr, &inside, &in_frontier, &frontier)
	for len(frontier) > 0 {
		i := pick_index(r, len(frontier))
		cell := frontier[i]
		ordered_remove(&frontier, i)
		in_frontier[cell.y][cell.x] = false
		// join to a random neighbour that is already inside
		options: [4][2]int
		n := 0
		for d in ([4][2]int{{0, -1}, {1, 0}, {0, 1}, {-1, 0}}) {
			nc, nr := cell.x + d[0], cell.y + d[1]
			if nc >= 0 && nr >= 0 && nc < MAZE_COLS && nr < MAZE_ROWS && inside[nr][nc] { options[n] = {nc, nr}; n += 1 }
		}
		to := options[pick_index(r, n)]
		if to.x == cell.x + 1 { m.open_east[cell.y][cell.x] = true } else if to.x == cell.x - 1 { m.open_east[to.y][to.x] = true }
		else if to.y == cell.y + 1 { m.open_south[cell.y][cell.x] = true } else { m.open_south[to.y][to.x] = true }
		inside[cell.y][cell.x] = true
		add_neighbors(cell.x, cell.y, &inside, &in_frontier, &frontier)
	}
	return
}

maze_door_count :: proc(m: ^Maze) -> (n: int) {
	for y in 0 ..< MAZE_ROWS { for x in 0 ..< MAZE_COLS { if m.open_east[y][x] { n += 1 }; if m.open_south[y][x] { n += 1 } } }
	return
}
// Number of cells reachable from (0,0): MAZE_COLS * MAZE_ROWS when the maze is connected.
maze_reachable :: proc(m: ^Maze) -> int {
	seen: [MAZE_ROWS][MAZE_COLS]bool
	stack: [dynamic][2]int
	defer delete(stack)
	append(&stack, [2]int{0, 0}); seen[0][0] = true
	count := 0
	for len(stack) > 0 {
		c := pop(&stack)
		count += 1
		try :: proc(m: ^Maze, seen: ^[MAZE_ROWS][MAZE_COLS]bool, stack: ^[dynamic][2]int, open: bool, x, y: int) {
			if open && !seen[y][x] { seen[y][x] = true; append(stack, [2]int{x, y}) }
		}
		x, y := c.x, c.y
		if x + 1 < MAZE_COLS { try(m, &seen, &stack, m.open_east[y][x], x + 1, y) }
		if x > 0 { try(m, &seen, &stack, m.open_east[y][x - 1], x - 1, y) }
		if y + 1 < MAZE_ROWS { try(m, &seen, &stack, m.open_south[y][x], x, y + 1) }
		if y > 0 { try(m, &seen, &stack, m.open_south[y - 1][x], x, y - 1) }
	}
	return count
}
// A hash of the door layout, to compare mazes across platforms.
maze_hash :: proc(m: ^Maze) -> u64 {
	h: u64 = 14695981039346656037
	for y in 0 ..< MAZE_ROWS { for x in 0 ..< MAZE_COLS {
		bits: u64 = (m.open_east[y][x] ? 1 : 0) | (m.open_south[y][x] ? 2 : 0)
		h = (h ~ bits) * 1099511628211
	} }
	return h
}
