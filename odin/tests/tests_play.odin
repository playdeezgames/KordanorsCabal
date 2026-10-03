package main

import "core:fmt"
import "kc:game"

// A core with a freshly generated world, past the finalize and prolog screens, standing in the town square facing east,
// with nothing unassigned (the setup of the recorded town screens).
town_core :: proc(seed: u64) -> ^game.Core {
	c := make_core()
	step(c)
	game.start_game(c, seed)
	for c.state == .Finalize_Character { pick(c, 1) }
	press(c, .Confirm) // prolog
	assert(c.state == .In_Play)
	p := game.player_character(&c.world)
	p.stats[.Unassigned] = 0
	c.world.player.facing = .East
	return c
}

test_play_town_golden :: proc(t: ^T) {
	defer fake_reset()
	c := town_core(77); defer free_core(c)
	step(c)
	expect_like_vb(t, c, "11-in-play-town-square")
	// status: the stats the recorded screen showed
	p := game.player_character(&c.world)
	p.stats[.Strength], p.stats[.Dexterity], p.stats[.Influence], p.stats[.Willpower] = 7, 2, 1, 2
	p.stats[.Power], p.stats[.HP], p.stats[.MP], p.stats[.Mana] = 1, 3, 3, 1
	for c.button != game.NEUTRAL_STATUS { press(c, .Down) }
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Status)
	expect_like_vb(t, c, "12-status", 22) // the last line shows the seed, new in the port
	expect(t, c.screen[22][0].glyph == u8(game.Glyph.S), "seed line")
	press(c, .Cancel)
	expect_eq(t, c.state, game.UI_State.In_Play)
	// turn
	for c.button != game.NEUTRAL_TURN_FIGHT { press(c, .Up) }
	press(c, .Confirm)
	expect_eq(t, c.world.player.mode, game.Player_Mode.Turn)
	expect_like_vb(t, c, "13-turn-mode")
	press(c, .Down) // Around
	press(c, .Confirm)
	expect_eq(t, c.world.player.mode, game.Player_Mode.Neutral)
	expect_eq(t, c.world.player.facing, game.Direction.West)
	expect_eq(t, c.button, 0) // the position before Turn is restored
	// move
	for c.button != game.NEUTRAL_MOVE_RUN { press(c, .Right) }
	press(c, .Confirm)
	expect_eq(t, c.world.player.mode, game.Player_Mode.Move)
	c.world.player.facing = .East
	step(c)
	expect_like_vb(t, c, "14-move-mode")
	press(c, .Cancel)
	expect_eq(t, c.world.player.mode, game.Player_Mode.Neutral)
	expect_eq(t, c.button, 5)
}

test_play_dungeon_golden :: proc(t: ^T) {
	defer fake_reset()
	c := make_core(); defer free_core(c)
	step(c)
	w := &c.world
	game.world_init(w, 1)
	c.has_world = true
	a, b, d := game.create_location(w, .Dungeon), game.create_location(w, .Dungeon), game.create_location(w, .Dungeon)
	for id in ([]game.Location_ID{a, b, d}) { game.location_get(w, id).level = .Level_I }
	game.location_get(w, a).routes[.East] = {b, .Route_2}
	game.location_get(w, a).routes[.South] = {d, .Route_2}
	player := game.create_character(w, .N00b, a)
	w.player = {character = player, mode = .Neutral, facing = .East}
	game.player_character(w).stats[.Influence] = 1
	game.player_character(w).stats[.Unassigned] = 0
	game.create_character(w, .Goblin, a)
	game.create_character(w, .Goblin, a)
	game.item_put_on_ground(w, game.create_item(w, .FE_Key), a)
	game.item_put_on_ground(w, game.create_item(w, .Helmet), a)
	for type in ([]game.Item_Type{.Potion, .Dagger, .Platemail, .Book_of_Holy_Bolt}) { game.item_put_in_pack(w, game.create_item(w, type), player) }
	ok, why := game.world_validate(w)
	expect(t, ok, why)
	c.state = .In_Play
	step(c)
	expect_like_vb(t, c, "40-dungeon-with-enemy")
}

test_play_movement :: proc(t: ^T) {
	defer fake_reset()
	c := town_core(5); defer free_core(c)
	w := &c.world
	player := w.player.character
	square := game.player_character(w).location
	expect(t, game.can_move(w, player, .North), "the square has a north exit")
	hunger := game.player_character(w).stats[.Hunger]
	// Move... Forward (facing east)
	for c.button != game.NEUTRAL_MOVE_RUN { press(c, .Right) }
	press(c, .Confirm)
	expect_eq(t, c.world.player.mode, game.Player_Mode.Move)
	press(c, .Confirm) // button 0 is Forward after the push
	expect_eq(t, c.world.player.mode, game.Player_Mode.Neutral)
	here := game.player_character(w).location
	expect(t, here != square, "moved")
	expect(t, game.location_get(w, here).visited, "the new place is visited")
	expect_eq(t, game.player_character(w).stats[.Hunger], hunger + 1)
	expect_eq(t, c.button, 5)
	// and back: the opposite direction leads home
	expect(t, game.move_character(w, player, .West) == false, "no starvation yet")
	expect_eq(t, game.player_character(w).location, square)
	// a wall does nothing
	for d in game.Direction {
		if d != .None && game.location_get(w, square).routes[d].to == {} {
			expect(t, !game.can_move(w, player, d), "no route, no move")
			expect(t, !game.move_character(w, player, d), "nothing happens")
		}
	}
	expect_eq(t, game.player_character(w).location, square)
}

test_play_rules :: proc(t: ^T) {
	defer fake_reset()
	c := town_core(9); defer free_core(c)
	w := &c.world
	player := w.player.character
	// nobody else is about, so that no fight is on offer; the beginner is weak (maximum load 60)
	others := make([dynamic]game.Character_ID, context.temp_allocator)
	for id in w.character_order { if id != player { append(&others, id) } }
	for id in others { game.destroy_character(w, id) }
	game.player_character(w).stats[.Strength] = 1
	// find a locked door and stand in front of it
	door_from: game.Location_ID
	door_dir: game.Direction
	for id in w.location_order {
		for d in game.Direction {
			if game.location_get(w, id).routes[d].type == .Route_4 && door_from == {} { door_from, door_dir = id, d }
		}
	}
	expect(t, door_from != {}, "the world has a locked door")
	game.player_character(w).location = door_from
	w.player.facing = door_dir
	target := game.location_get(w, door_from).routes[door_dir].to
	expect(t, !game.can_move(w, player, door_dir), "locked without the key")
	key := game.create_item(w, .FE_Key)
	game.item_put_in_pack(w, key, player)
	expect(t, game.can_move(w, player, door_dir), "the key opens it")
	expect(t, !game.move_character(w, player, door_dir), "no starvation")
	expect_eq(t, game.player_character(w).location, target)
	expect(t, game.item_get(w, key) == nil, "the key is spent")
	expect_eq(t, game.location_get(w, door_from).routes[door_dir].type, game.Route_Type.Route_2)
	expect_eq(t, w.sfx_count, 1) // the unlock sound waits for the core to collect it
	expect_eq(t, w.sfx_queue[0], game.Sfx.Unlock_Door)
	step(c)
	expect_eq(t, w.sfx_count, 0)

	// encumbrance: two suits of platemail are too much for a beginner
	for _ in 0 ..< 2 { game.item_put_in_pack(w, game.create_item(w, .Platemail), player) }
	expect(t, game.is_encumbered(w, player), "encumbered")
	for d in game.Direction { expect(t, !game.can_move(w, player, d), "encumbered characters cannot move") }
	for id in game.items_in_pack(w, player) { game.destroy_item(w, id) }
	expect(t, !game.is_encumbered(w, player), "relieved")

	// hunger: at 99 the next step starves one hit point, halves the hunger and shows a message
	game.player_character(w).location = door_from
	game.player_character(w).stats[.Hunger] = 99
	game.player_character(w).stats[.HP] = 5
	game.player_character(w).stats[.Wounds] = 0
	w.player.facing = door_dir
	for c.button != game.NEUTRAL_MOVE_RUN { press(c, .Right) }
	press(c, .Confirm)
	press(c, .Confirm) // Forward
	expect_eq(t, c.state, game.UI_State.Message)
	expect_eq(t, game.player_character(w).stats[.Hunger], i32(50))
	expect_eq(t, game.health_current(game.player_character(w)), i32(4))
	step(c)
	m := game.message_head(w)
	expect(t, m != nil && string(m.text[:m.len]) == "You take damage from starvation!", "message text")
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.In_Play)

	// and death: the last hit point ends the game
	game.player_character(w).stats[.Hunger] = 99
	game.player_character(w).stats[.Wounds] = 4 // one point left
	game.player_character(w).location = door_from
	game.location_get(w, door_from).routes[door_dir] = {target, .Route_2}
	for c.button != game.NEUTRAL_MOVE_RUN { press(c, .Right) }
	press(c, .Confirm)
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Dead)
	step(c)
	expect(t, c.screen[0][0].glyph == u8(game.Glyph.Y), "yer dead")
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Title)
	expect(t, !c.has_world, "the world is gone")
}

test_play_map_messages :: proc(t: ^T) {
	defer fake_reset()
	c := town_core(3); defer free_core(c)
	w := &c.world
	player := w.player.character
	// stand in a dungeon room of level I
	entry: game.Location_ID
	for id in w.location_order { if game.location_get(w, id).level == .Level_I && entry == {} { entry = id } }
	game.player_character(w).location = entry
	game.location_get(w, entry).visited = true
	step(c)
	expect(t, game.can_map(w, player), "dungeon places can be mapped")
	for c.button != game.NEUTRAL_MAP { press(c, .Down) }
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.Map)
	step(c)
	l := game.location_get(w, entry)
	cell := c.screen[int(l.row) * 2][int(l.column) * 2]
	expect(t, cell.inverted, "the player's place is drawn inverted")
	expect(t, c.screen[22][2].glyph == u8(game.Glyph.S), "legend")
	// unvisited places are not drawn
	visited := 0
	for r in 0 ..< 22 { for col in 0 ..< 22 { if c.screen[r][col].glyph != game.GLYPH_SPACE && r < 22 { visited += 1 } } }
	expect_eq(t, visited, 4) // one place, four cells
	press(c, .Down)
	expect_eq(t, c.state, game.UI_State.Map)
	press(c, .Confirm)
	expect_eq(t, c.state, game.UI_State.In_Play)

	// the town cannot be mapped
	game.player_character(w).location = game.locations_of_type(w, .Town_Square)[0]
	expect(t, !game.can_map(w, player), "no map in town")

	// the message queue: first in, first out, bounded
	for i in 0 ..< 20 { game.message_add(w, .None, fmt.tprintf("message %d", i), "second line") }
	expect_eq(t, w.messages.count, game.MESSAGE_QUEUE_LENGTH)
	head := game.message_head(w)
	expect(t, string(head.text[:head.len]) == "message 0\nsecond line", "first message")
	game.message_pop(w)
	head = game.message_head(w)
	expect(t, string(head.text[:head.len]) == "message 1\nsecond line", "second message")
	for w.messages.count > 0 { game.message_pop(w) }
	expect(t, game.message_head(w) == nil, "empty")
	game.message_add(w, .Level_Up, "x")
	push_before := c.stack_len
	game.push_state(c, .In_Play)
	expect_eq(t, c.stack_len, push_before + 1)
	expect_eq(t, game.pop_state(c), game.UI_State.In_Play)
	expect_eq(t, game.pop_state(c), game.UI_State.In_Play) // popping an empty stack falls back instead of crashing
}
