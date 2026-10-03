package main

// Event dispatch (task 18). Content names behaviors with the Check and Action enums; the code implements each with a switch.
// There is no event bus: callers invoke `check` or `perform` directly, and rules report to the player through the
// message and sound queues in the world.
import "core:fmt"

// Everything a handler may need, by value. Unused handles are zero. Handlers validate the handles they use.
Event_Context :: struct {
	world:     ^World,
	character: Handle,
	item:      Handle,
	location:  Location_ID,
}

todo :: proc(ctx: Event_Context, name: any) { ctx.world.todo_hits += 1 } // a handler not yet ported (spike bookkeeping)

// ---- helpers shared by handlers ---------------------------------------------------------------------------------
item_name :: proc(it: ^Item) -> string { return it.lore != 0 ? LORE_ITEM_NAME[it.lore] : ITEM_NAME[it.type] }

// The strike sequence shared by Holy Bolt, Holy Water and the Fire Shard: damage, death, message, counter attacks.
strike :: proc(ctx: Event_Context, enemy: Handle, intro: string, damage: int) {
	w := ctx.world
	lines: [dynamic]string
	append(&lines, intro, fmt.tprintf("You do %d damage!", damage))
	sfx := damage_and_maybe_kill(w, ctx.character, enemy, damage, &lines)
	say(w, ctx.character, sfx, ..lines[:])
}

// ---- checks (pure reads) --------------------------------------------------------------------------------------
check :: proc(name: Check, ctx: Event_Context) -> bool {
	w := ctx.world
	me, ok := char_get(w, ctx.character)
	#partial switch name {
	case .None: return false
	case .Always_True: return true
	case .Is_In_Dungeon: return ok && w.locations[me.location].type in IS_DUNGEON
	case .Has_Bong:
		_, found := first_carried_of_type(w, ctx.character, .Bong)
		return ok && found
	case .Can_Use_Fire_Shard:
		_, has_enemy := first_enemy(w, ctx.character)
		return ok && w.locations[me.location].type in IS_DUNGEON && has_enemy && mana_current(me) > 0
	case .Can_Use_Beer:
		enemy, has_enemy := first_enemy(w, ctx.character)
		return ok && (!has_enemy || can_be_bribed_with(w, enemy, .Beer))
	}
	todo(ctx, name)
	return false
}

// ---- actions --------------------------------------------------------------------------------------------------------
perform :: proc(name: Action, ctx: Event_Context) {
	w := ctx.world
	me, ok := char_get(w, ctx.character)
	#partial switch name {
	case .None:
		return

	case .Drink_Potion:
		if !ok { return }
		heal := roll(w, 2, 4)
		change_stat(me, .Wounds, -i32(heal))
		if bottle, made := create_item(w, .Empty_Bottle); made { give_item(w, ctx.character, bottle) }
		say(w, ctx.character, .None, fmt.tprintf("Potion heals up to %d HP!", heal), fmt.tprintf("You now have %d HP!", health_current(me)))

	case .Eat_Food:
		if !ok { return }
		change_stat(me, .Wounds, -1)
		me.stats[.Hunger] = 0
		say(w, ctx.character, .None, "Food heals up to 1 HP!", fmt.tprintf("You now have %d HP!", health_current(me)))

	case .Purify_Food:
		if it, found := item_get(w, ctx.item); found { it.type = .kottbulle } // fresh food again; events are derived from the type

	case .Use_Town_Portal:
		if !ok { return }
		here := &w.locations[me.location]
		town, _ := find_location_of_type(w, .Town_Square)
		there := &w.locations[town]
		here.routes[.Out] = {town, 10} // replaces any route in that direction (VB: DestroyRoute then Create)
		there.routes[.In] = {me.location, 10}
		say(w, ctx.character, .None, "A portal opens before you!")

	case .Use_Fire_Shard:
		enemy, found := first_enemy(w, ctx.character)
		if !ok || !found { say(w, ctx.character, .None, "You cannot use that now!"); return } // VB: null reference
		me.stats[.Fatigue] += 1
		e, _ := char_get(w, enemy)
		strike(ctx, enemy, fmt.tprintf("You use %s on %s!", ITEM_NAME[.Fire_Shard], CHARACTER_NAME[e.type]), roll(w, 3, 4))

	case .Use_Beer:
		if !ok { return }
		if enemy, found := first_enemy(w, ctx.character); found && can_be_bribed_with(w, enemy, .Beer) {
			e, _ := char_get(w, enemy)
			say(w, ctx.character, .None, fmt.tprintf("You give %s the %s, and they wander off to get drunk.", CHARACTER_NAME[e.type], ITEM_NAME[.Beer]))
			destroy_character(w, enemy)
			return
		}
		me.stats[.Stress] = 0
		me.stats[.Drunkenness] += 10
		if bottle, made := create_item(w, .Empty_Bottle); made { give_item(w, ctx.character, bottle) }
		say(w, ctx.character, .None, "You drink the beer, and suddenly feel braver!")

	case .Use_Magic_Egg:
		if !ok { return }
		weights := [14]int{500, 8, 4, 250, 1000, 125, 64, 1, 2, 125, 64, 16, 8, 1}
		kinds := [14]Item_Type{.Beer, .Brodesode, .Chainmail, .Dagger, .kottbulle, .Helmet, .Holy_Water, .Moon_Portal, .Platemail, .Potion, .Shield, .Shortsword, .Town_Portal, .Trousers}
		if made, created := create_item(w, kinds[pick_weighted(w, weights[:])]); created {
			it, _ := item_get(w, made)
			say(w, ctx.character, .None, fmt.tprintf("You crack open the %s and find %s inside!", ITEM_NAME[.Magic_Egg], item_name(it)))
			give_item(w, ctx.character, made)
		}

	case .Food_Decay:
		if it, found := item_get(w, ctx.item); found && roll(w, 1, 3) == 1 { it.type = .kottbulle_35 }

	case .Rotten_Food_Decay:
		it, found := item_get(w, ctx.item)
		if !found || roll(w, 1, 2) != 1 { return }
		if roll(w, 1, 2) == 1 && it.holder == .On_Ground {
			create_character(w, .Rat, it.holder_location, rat_stats()) // a full pool just skips the rat (task 17)
		}
		destroy_item(w, ctx.item)

	case .Read_Note:
		it, found := item_get(w, ctx.item)
		if !ok || !found { return }
		if it.lore == 0 { it.lore = u8(unassigned_lore(w)) }
		say(w, ctx.character, .None, LORE_TEXT[it.lore])

	case .Location_Decay_Items:
		cur: u32 = 0
		for ih, it in pool_next(&w.items, &cur) {
			if it.holder == .On_Ground && it.holder_location == ctx.location { item_decay(w, ih) }
		}

	case .Character_Accept_Cellar_Rats_Quest:
		if !ok { return }
		say(w, ctx.character, .None, "You accept the quest!")
		w.quest_active[.Cellar_Rats] = 1
		cellar, _ := find_location_of_type(w, .Cellar)
		for _ in 0 ..< w.quest_done[.Cellar_Rats] + 1 { create_character(w, .Rat, cellar, rat_stats()) }

	case:
		todo(ctx, name)
	}
}

// ---- item events: the port of ItemEvents plus the use rule of CharacterItems ----------------------------------------
item_can_use :: proc(w: ^World, character, item: Handle) -> bool {
	it, ok := item_get(w, item)
	if !ok || ITEM_EVENTS[it.type].can_use == .None { return false }
	return check(ITEM_EVENTS[it.type].can_use, {world = w, character = character, item = item})
}
// Returns whether the item was used. Single-use items are destroyed afterwards.
item_use :: proc(w: ^World, character, item: Handle) -> bool {
	it, ok := item_get(w, item)
	if !ok || !item_can_use(w, character, item) { return false }
	type := it.type
	perform(ITEM_EVENTS[type].use, {world = w, character = character, item = item})
	if type in SINGLE_USE { destroy_item(w, item) } // after the event: it may itself have destroyed or changed the item
	return true
}
item_decay :: proc(w: ^World, item: Handle) {
	if it, ok := item_get(w, item); ok { perform(ITEM_EVENTS[it.type].decay, {world = w, item = item, location = it.holder_location}) }
}
location_decay_items :: proc(w: ^World, loc: Location_ID) { perform(.Location_Decay_Items, {world = w, location = loc}) }

// ---- small world queries used above -------------------------------------------------------------------------------------
find_location_of_type :: proc(w: ^World, t: Location_Type) -> (Location_ID, bool) {
	for i in 1 ..< len(w.locations) { if w.locations[i].type == t { return Location_ID(i), true } }
	return 0, false
}
rat_stats :: proc() -> [Stat]i32 { return #partial {.Strength = 1, .Dexterity = 1, .HP = 1, .Unarmed_Maximum_Damage = 1, .Base_Maximum_Defend = 1} }
// A lore text no live item shows yet; if all are taken, any (the VB code would throw).
unassigned_lore :: proc(w: ^World) -> int {
	taken: [LORE_COUNT + 1]bool
	cur: u32 = 0
	for _, it in pool_next(&w.items, &cur) { taken[it.lore] = true }
	free: [LORE_COUNT]int
	n := 0
	for l in 1 ..= LORE_COUNT { if !taken[l] { free[n] = l; n += 1 } }
	if n == 0 { return rng_range(w, 1, LORE_COUNT) }
	return free[rng_range(w, 0, n - 1)]
}
