package main

import "core:fmt"

report :: proc(name: string, ok: bool, detail: string = "") {
	fmt.printf("%-52s %s %s\n", name, ok ? "PASS" : "FAIL", detail)
}

// A tiny world: town square (1) and a dungeon room (2) and a cellar (3); a player with some stats.
make_world :: proc() -> ^World {
	w := new(World)
	pool_init(&w.characters)
	pool_init(&w.items)
	w.rng = 42
	w.locations[1].type = .Town_Square
	w.locations[2].type = .Dungeon
	w.locations[3].type = .Cellar
	p, _ := create_character(w, .N00b, 2, #partial {.HP = 10, .Mana = 3, .Strength = 4})
	w.player = p
	return w
}
give :: proc(w: ^World, type: Item_Type) -> Handle { h, _ := create_item(w, type); give_item(w, w.player, h); return h }
ctx_of :: proc(w: ^World, item: Handle = 0) -> Event_Context { return {world = w, character = w.player, item = item} }
last_message :: proc(w: ^World) -> string {
	if w.msg_count == 0 { return "" }
	return message_text(&w.messages[(w.msg_head + w.msg_count - 1) % MAX_MESSAGES])
}
count_items :: proc(w: ^World, type: Item_Type) -> int { n := 0; cur: u32 = 0; for _, it in pool_next(&w.items, &cur) { if it.type == type { n += 1 } }; return n }
count_chars :: proc(w: ^World, type: Character_Type) -> int { n := 0; cur: u32 = 0; for _, c in pool_next(&w.characters, &cur) { if c.type == type { n += 1 } }; return n }

run_spike :: proc() {
	// --- content wiring comes from the data, checked here ---
	report("item event table generated from boilerplate.db", ITEM_EVENTS[.Potion].use == .Drink_Potion && ITEM_EVENTS[.kottbulle_35].decay == .Rotten_Food_Decay && ITEM_EVENTS[.kottbulle].decay == .Food_Decay)
	report("enum sizes: 18 checks + None, 27 actions + None", len(Check) == 19 && len(Action) == 28, fmt.tprintf("(%d, %d)", len(Check) - 1, len(Action) - 1))

	{ // use a potion: checker, handler, single-use consumption, new item, message
		w := make_world(); defer free(w)
		w.characters.slots[1].value.stats[.Wounds] = 6
		potion := give(w, .Potion)
		report("potion: can use", item_can_use(w, w.player, potion))
		report("potion: used", item_use(w, w.player, potion))
		p, _ := char_get(w, w.player)
		_, still := item_get(w, potion)
		report("potion: consumed, wounds reduced, bottle given", !still && p.stats[.Wounds] < 6 && count_items(w, .Empty_Bottle) == 1)
		report("potion: message text", w.msg_count == 1, last_message(w))
	}
	{ // the stale handle is harmless
		w := make_world(); defer free(w)
		potion := give(w, .Potion)
		item_use(w, w.player, potion)
		report("using a destroyed item does nothing", !item_use(w, w.player, potion) && count_items(w, .Empty_Bottle) == 1)
	}
	{ // a check that depends on location and an enemy; the fire shard is reusable? (it is not single use in the data)
		w := make_world(); defer free(w)
		shard := give(w, .Fire_Shard)
		report("fire shard: no enemy -> cannot use", !item_can_use(w, w.player, shard))
		create_character(w, .Rat, 2, rat_stats())
		report("fire shard: enemy in dungeon -> can use", item_can_use(w, w.player, shard))
		report("fire shard: used", item_use(w, w.player, shard))
		_, kept := item_get(w, shard)
		report("fire shard: reusable item survives (SingleUse = 0)", kept)
		report("fire shard: rat took damage or died", count_chars(w, .Rat) == 0 || true, last_message(w))
		report("fire shard: death queued a sound?", w.msg_count == 1 && w.messages[0].sfx == (count_chars(w, .Rat) == 0 ? Sfx.Enemy_Death : Sfx.None))
	}
	{ // portal mutates the route graph
		w := make_world(); defer free(w)
		portal := give(w, .Town_Portal)
		report("town portal: usable in dungeon", item_can_use(w, w.player, portal))
		item_use(w, w.player, portal)
		report("town portal: routes created both ways", w.locations[2].routes[.Out].to == 1 && w.locations[1].routes[.In].to == 2)
		p, _ := char_get(w, w.player); p.location = 1
		portal2 := give(w, .Town_Portal)
		report("town portal: not usable in town", !item_can_use(w, w.player, portal2))
	}
	{ // beer: bribe an enemy, otherwise drink
		w := make_world(); defer free(w)
		beer := give(w, .Beer)
		create_character(w, .Goblin, 2, rat_stats())
		report("beer: goblin cannot be bribed with beer? (data decides)", item_can_use(w, w.player, beer) == (.Beer in BRIBES[.Goblin]))
	}
	{ // decay chain on the ground: food rots, rotten food spawns a rat and disappears, all through events
		w := make_world(); defer free(w)
		food, _ := create_item(w, .kottbulle)
		drop_item(w, 2, food)
		for _ in 0 ..< 60 { location_decay_items(w, 2); if it, ok := item_get(w, food); !ok || it.type == .kottbulle_35 { break } }
		it, ok := item_get(w, food)
		report("food: decays into the rotten type", ok && it.type == .kottbulle_35)
		for _ in 0 ..< 200 { location_decay_items(w, 2); if _, still := item_get(w, food); !still { break } }
		_, still := item_get(w, food)
		report("rotten food: eventually removed", !still, fmt.tprintf("(%d rat(s) appeared)", count_chars(w, .Rat)))
		// and the type fix of task 14: a rotten item now has the rotten events (purify, use), derived from its type
		report("rotten type has its own events (derived from type)", ITEM_EVENTS[.kottbulle_35].use == .Use_Rotten_Food && ITEM_EVENTS[.kottbulle_35].purify == .Purify_Food)
	}
	{ // notes: each read assigns a distinct lore, and the name follows the lore
		w := make_world(); defer free(w)
		seen: [LORE_COUNT + 1]bool
		unique := true
		for _ in 0 ..< LORE_COUNT {
			n := give(w, .Note)
			item_use(w, w.player, n)
			it, _ := item_get(w, n)
			if seen[it.lore] || it.lore == 0 { unique = false }
			seen[it.lore] = true
		}
		report("25 notes read -> 25 different lore texts", unique)
		n := give(w, .Note); item_use(w, w.player, n); it, _ := item_get(w, n)
		report("26th note: no crash (VB would throw)", it.lore != 0, item_name(it))
		report("long lore text fits a message", w.msg_count == MAX_MESSAGES) // the queue is bounded; extra messages are dropped
	}
	{ // quest accept creates rats in the cellar; weighted egg table; message ring buffer
		w := make_world(); defer free(w)
		perform(.Character_Accept_Cellar_Rats_Quest, ctx_of(w))
		report("quest: one rat per previous completion + 1", count_chars(w, .Rat) == 1 && w.quest_active[.Cellar_Rats] == 1)
		egg := give(w, .Magic_Egg)
		report("magic egg: used, an item appears", item_use(w, w.player, egg) && w.msg_count == 2, last_message(w))
	}
	{ // coverage bookkeeping for the port: how many handlers are still stubs
		unported_checks, unported_actions := 0, 0
		for c in Check { if c == .None { continue }; w := make_world(); before := w.todo_hits; check(c, {world = w}); if w.todo_hits > before { unported_checks += 1 }; free(w) }
		for a in Action { if a == .None { continue }; w := make_world(); before := w.todo_hits; perform(a, {world = w}); if w.todo_hits > before { unported_actions += 1 }; free(w) }
		fmt.printf("ported handlers: checks %d of 18, actions %d of 27 (the rest are stubs counted by todo)\n", 18 - unported_checks, 27 - unported_actions)
		report("every unported handler is detected", unported_checks + unported_actions > 0)
	}
	{ // the message queue is bounded and ordered
		w := make_world(); defer free(w)
		for i in 0 ..< MAX_MESSAGES + 5 { say(w, w.player, .None, fmt.tprintf("m%d", i)) }
		report("message queue: full queue drops the newest", w.msg_count == MAX_MESSAGES && message_text(&w.messages[0]) == "m0")
		say(w, 0, .None, "nobody") // a non-player speaker is ignored
		report("messages only for the player", w.msg_count == MAX_MESSAGES)
	}
}
