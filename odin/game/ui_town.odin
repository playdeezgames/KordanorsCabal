package game

// The townspeople (the nine *ModeProcessor classes under Townsfolk) and the shoppe and spell lists. A townsperson is a player
// mode: the in-play screen shows the feature's name, what they say (it depends on which button is selected), and a button bank
// of their own. Texts and positions are those of the original.

import "core:fmt"

TOWN_HELLO :: 0
TOWN_GOODBYE :: 9

// Button numbers in the town banks.
ELDER_PEP :: 1
ELDER_CABAL :: 2
INN_QUEST :: 1
INN_PRICES :: 6
INN_BUY :: 7
DRUNK_BEER :: 5
CHICKEN_FEED :: 5
MARKET_GAMBLE :: 5
MARKET_PRICES :: 6
MARKET_BUY :: 7
MAGE_OFFERS :: 1
MAGE_SELL :: 2
MAGE_RESTORE :: 5
MAGE_PRICES :: 6
MAGE_BUY :: 7
SMITH_OFFERS :: 1
SMITH_SELL :: 2
SMITH_REPAIR :: 3
SMITH_PRICES :: 6
SMITH_BUY :: 7
CONSTABLE_BOUNTIES :: 1
HEALER_HEAL :: 1
HEALER_PRICES :: 6
HEALER_BUY :: 7

is_town_mode :: proc(m: Player_Mode) -> bool { return m >= .Elder && m <= .Healer }

feature_here :: proc(w: ^World) -> Feature_Type {
	return location_get(w, character_get(w, w.player.character).location).feature
}
feature_name :: proc(f: Feature_Type) -> string { return FEATURE_TYPES[f].name }

shoppe_of_mode :: proc(m: Player_Mode) -> Shoppe_Type {
	#partial switch m {
	case .BlackMage: return .Magic
	case .Blacksmith: return .Blacksmith
	case .InnKeeper: return .Innkeeper
	case .Healer: return .Healer
	case .BlackMarket: return .Black_Market
	}
	return .None
}

can_gamble :: proc(w: ^World, who: Character_ID) -> bool { return stat_of(w, who, .Money) >= 5 }

// ---- the button banks -------------------------------------------------------------------------------------------------

town_titles :: proc(core: ^Core, titles: ^[BUTTON_COUNT]string) {
	w := &core.world
	who := w.player.character
	titles[TOWN_HELLO], titles[TOWN_GOODBYE] = "Hello!", "Good-bye"
	switch w.player.mode {
	case .Elder: titles[ELDER_PEP], titles[ELDER_CABAL] = "Pep talk", "The Cabal"
	case .InnKeeper:
		titles[INN_PRICES], titles[INN_BUY] = "Prices", "Buy"
		if quest_active(w, .Cellar_Rats) { titles[INN_QUEST] = "Quest Done!" } else if quest_can_accept(w, who, .Cellar_Rats) { titles[INN_QUEST] = "Do Quest!" }
	case .TownDrunk: if has_item_type(w, who, .Beer) { titles[DRUNK_BEER] = "Give Beer" }
	case .Chicken: if has_item_type(w, who, .kottbulle) || has_item_type(w, who, .kottbulle_35) { titles[CHICKEN_FEED] = "Feed" }
	case .BlackMarket:
		titles[MARKET_PRICES], titles[MARKET_BUY] = "Prices", "Buy"
		if can_gamble(w, who) { titles[MARKET_GAMBLE] = "Gamble" }
	case .BlackMage:
		titles[MAGE_OFFERS], titles[MAGE_SELL], titles[MAGE_PRICES], titles[MAGE_BUY] = "Offers", "Sell", "Prices", "Buy"
		if mana_current(w, who) < stat_of(w, who, .Mana) { titles[MAGE_RESTORE] = "Restore" }
	case .Blacksmith:
		titles[SMITH_OFFERS], titles[SMITH_SELL], titles[SMITH_PRICES], titles[SMITH_BUY] = "Offers", "Sell", "Prices", "Buy"
		if len(items_to_repair(w, who, .Blacksmith)) > 0 { titles[SMITH_REPAIR] = "Repair" }
	case .Constable: titles[CONSTABLE_BOUNTIES] = "Bounties"
	case .Healer:
		titles[HEALER_PRICES], titles[HEALER_BUY] = "Prices", "Buy"
		if stat_of(w, who, .Wounds) > 0 { titles[HEALER_HEAL] = "Heal me!" }
	case .None, .Neutral, .Turn, .Move:
	}
}

// ---- what they say ----------------------------------------------------------------------------------------------------

draw_town :: proc(core: ^Core) {
	s := &core.screen
	w := &core.world
	who := w.player.character
	show_header(s, feature_name(feature_here(w)))
	say_line :: proc(s: ^Screen, text: string) { write_text(s, 0, 1, text, false, .Black) }
	b := core.button
	switch w.player.mode {
	case .Elder:
		switch b {
		case TOWN_GOODBYE: say_line(s, "Fare well!")
		case ELDER_PEP: say_line(s, "If you find yerself disheartened, I'm always here to help!")
		case ELDER_CABAL: say_line(s, "Deep within the cata- combs beneath the ab- andoned church there  is a demon named Kord-anor! Slay this foul  fiend and bring to me his horns, so that    there can be a lastingpeace!")
		case: say_line(s, "Hello, my friend! Staya while, and listen!")
		}
	case .InnKeeper:
		switch b {
		case TOWN_GOODBYE: say_line(s, "@#$% off, you %$#^!")
		case INN_QUEST: say_line(s, quest_active(w, .Cellar_Rats) ? "How the rat killing going?" : "Do a guy a favor and kill the rats in the cellar? I'll pay 1 money for 1 rat tail, up to 10 rat tails.")
		case: say_line(s, "@#$% You!")
		}
	case .TownDrunk: say_line(s, "*HIC*")
	case .Chicken: say_line(s, "MOO! I'm a cow!")
	case .BlackMarket:
		if b == TOWN_GOODBYE {
			say_line(s, "Off with ye then!")
		} else if b == MARKET_GAMBLE && can_gamble(w, who) {
			say_line(s, "Play two-up for 5 money! Try yer luck! Flip two coins, and if both are head, you win 15! Otherwise, I take yer 5!")
			write_text(s, 0, 8, fmt.tprintf("You have %d money.", stat_of(w, who, .Money)), false, .Black)
		} else {
			say_line(s, "'Allo! 'Allo!")
			write_text(s, 0, 2, "Would you like to buy a lovely pair of trousers?", false, .Black)
		}
	case .BlackMage: say_line(s, b == TOWN_GOODBYE ? "Fare well!" : "Big Black Mage Blessings To You!")
	case .Blacksmith: say_line(s, b == TOWN_GOODBYE ? "Watch out for the moon people! They'll come down from the moon and tie yer shoes together you know!" : "Hullo therrrre! What can I do for ye?")
	case .Constable:
		switch b {
		case TOWN_GOODBYE: say_line(s, "Fare well!")
		case CONSTABLE_BOUNTIES: say_line(s, "We've been plagued by a group of malcontents! I will give 10 money for proof that a malcontent has been dealt with.")
		case: say_line(s, "Brint it, trolls!")
		}
	case .Healer:
		switch b {
		case TOWN_GOODBYE: say_line(s, "Fare well!")
		case HEALER_HEAL: say_line(s, stat_of(w, who, .Wounds) > 0 ? "Any time yer ready!" : "What ails you, my friend?")
		case: say_line(s, "What ails you, my friend?")
		}
	case .None, .Neutral, .Turn, .Move:
	}
}

// ---- what the buttons do ------------------------------------------------------------------------------------------------

town_button :: proc(core: ^Core, button: int) {
	w := &core.world
	who := w.player.character
	mode := w.player.mode
	if button == TOWN_GOODBYE { pop_button(core); w.player.mode = .Neutral; return }
	open_shoppe :: proc(core: ^Core, shoppe: Shoppe_Type, screen: UI_State) {
		core.world.player.shoppe = shoppe
		enter_state(core, screen)
	}
	shoppe := shoppe_of_mode(mode)
	switch mode {
	case .Elder:
		if button == ELDER_PEP {
			message_add(w, .None, "Sometimes in our life we all have pain. We all have sorrow. But, if we are wise, we know that there's always tomorrow!")
			if mp_current(w, who) < 1 { character_get(w, who).stats[.Stress] = stat_of(w, who, .MP) - 1 }
			show_messages_then(core, .In_Play)
		}
	case .InnKeeper:
		switch button {
		case INN_QUEST:
			if quest_active(w, .Cellar_Rats) {
				perform(QUEST_TYPES[.Cellar_Rats].complete, {world = w, character = who})
				show_messages_then(core, .In_Play)
			} else if quest_can_accept(w, who, .Cellar_Rats) {
				perform(QUEST_TYPES[.Cellar_Rats].accept, {world = w, character = who})
				show_messages_then(core, .In_Play)
			}
		case INN_PRICES: open_shoppe(core, shoppe, .Shoppe_Prices)
		case INN_BUY: open_shoppe(core, shoppe, .Shoppe_Buy)
		}
	case .TownDrunk:
		if button == DRUNK_BEER {
			if beer, has := carried_item_of_type(w, who, .Beer); has {
				drunk := feature_name(.Yermom_the_Drunk)
				message_add(w, .None, fmt.tprintf("You give %s to %s.", ITEM_TYPES[.Beer].name, drunk),
					fmt.tprintf("%s drinks it all in one swallow, burps, and hands you %s.", drunk, ITEM_TYPES[.Empty_Bottle].name))
				destroy_item(w, beer)
				give_new_item(w, who, .Empty_Bottle)
				show_messages_then(core, .In_Play)
			}
		}
	case .Chicken:
		if button == CHICKEN_FEED { feed_chicken(core) }
	case .BlackMarket:
		switch button {
		case MARKET_GAMBLE: if can_gamble(w, who) { gamble(core) }
		case MARKET_PRICES: open_shoppe(core, shoppe, .Shoppe_Prices)
		case MARKET_BUY: open_shoppe(core, shoppe, .Shoppe_Buy)
		}
	case .BlackMage:
		switch button {
		case MAGE_OFFERS: open_shoppe(core, shoppe, .Shoppe_Offers)
		case MAGE_SELL: open_shoppe(core, shoppe, .Shoppe_Sell)
		case MAGE_PRICES: open_shoppe(core, shoppe, .Shoppe_Prices)
		case MAGE_BUY: open_shoppe(core, shoppe, .Shoppe_Buy)
		case MAGE_RESTORE:
			message_add(w, .None, fmt.tprintf("%s sparks up his %s and gives you a hit of %s.", feature_name(.Marcus_the_Black_Mage), ITEM_TYPES[.Bong].name, ITEM_TYPES[.Herb].name))
			set_mana(w, who, stat_of(w, who, .Mana))
			show_messages_then(core, .In_Play)
		}
	case .Blacksmith:
		switch button {
		case SMITH_OFFERS: open_shoppe(core, shoppe, .Shoppe_Offers)
		case SMITH_SELL: open_shoppe(core, shoppe, .Shoppe_Sell)
		case SMITH_PRICES: open_shoppe(core, shoppe, .Shoppe_Prices)
		case SMITH_BUY: open_shoppe(core, shoppe, .Shoppe_Buy)
		case SMITH_REPAIR: if len(items_to_repair(w, who, shoppe)) > 0 { open_shoppe(core, shoppe, .Shoppe_Repair) }
		}
	case .Constable:
		if button == CONSTABLE_BOUNTIES {
			cards := make([dynamic]Item_ID, context.temp_allocator)
			for id in items_in_pack(w, who) { if item_get(w, id).type == .Membership_Card { append(&cards, id) } }
			if len(cards) > 0 {
				reward := i32(len(cards)) * 10
				message_add(w, .None, fmt.tprintf("I'll be certain to file these under evidence. Here's yer reward! %d money.", reward))
				stat_add(character_get(w, who), .Money, reward)
				for id in cards { destroy_item(w, id) }
			} else {
				message_add(w, .None, "See me when you've got evidence of malcontent activity.")
			}
			show_messages_then(core, .In_Play)
		}
	case .Healer:
		switch button {
		case HEALER_HEAL: character_get(w, who).stats[.Wounds] = 0
		case HEALER_PRICES: open_shoppe(core, shoppe, .Shoppe_Prices)
		case HEALER_BUY: open_shoppe(core, shoppe, .Shoppe_Buy)
		}
	case .None, .Neutral, .Turn, .Move:
	}
}

feed_chicken :: proc(core: ^Core) {
	w := &core.world
	who := w.player.character
	candidates := make([dynamic]Item_ID, context.temp_allocator)
	for id in items_in_pack(w, who) { if item_get(w, id).type == .kottbulle || item_get(w, id).type == .kottbulle_35 { append(&candidates, id) } }
	if len(candidates) == 0 { return }
	food := candidates[pick_index(&w.rng, len(candidates))]
	type := item_get(w, food).type
	destroy_item(w, food)
	chicken := feature_name(.Sander_the_Chicken)
	if rng_range(&w.rng, 0, 5) == 0 {
		prize: Item_Type = type == .kottbulle ? .Magic_Egg : .Rotten_Egg
		food_name := type == .kottbulle ? "the food" : "the rotten food"
		message_add(w, .None, fmt.tprintf("%s eats %s and then a %s pops out!", chicken, food_name, ITEM_TYPES[prize].name))
		give_new_item(w, who, prize)
	} else {
		message_add(w, .None, fmt.tprintf("%s eats the food, and gives a satified \"moo\" in return.", chicken))
	}
	show_messages_then(core, .In_Play)
}

// Two-up: two coins, both heads wins 15, anything else loses the stake of 5.
gamble :: proc(core: ^Core) {
	w := &core.world
	who := w.player.character
	first, second := rng_range(&w.rng, 0, 1), rng_range(&w.rng, 0, 1)
	lines := make([dynamic]string, context.temp_allocator)
	append(&lines, "You flip the two coins!")
	append(&lines, fmt.tprintf("The first coin comes up %s!", first > 0 ? "heads" : "tails"))
	append(&lines, fmt.tprintf("The second coin comes up %s!", second > 0 ? "heads" : "tails"))
	if first > 0 && second > 0 {
		append(&lines, "You win and receive 15 money!")
		stat_add(character_get(w, who), .Money, 15)
	} else {
		append(&lines, "You lose and must pay 5 money!")
		stat_add(character_get(w, who), .Money, -5)
	}
	message_add(w, .None, ..lines[:])
	show_messages_then(core, .In_Play)
}

// ---- the shoppe and spell lists ---------------------------------------------------------------------------------------

is_shoppe_list :: proc(s: UI_State) -> bool {
	return s == .Shoppe_Offers || s == .Shoppe_Prices || s == .Shoppe_Buy || s == .Shoppe_Sell || s == .Shoppe_Repair
}

// The rows of a list screen and their colours.
List_Rows :: struct { labels: []string, hues: []Hue }

shoppe_list_rows :: proc(core: ^Core) -> (rows: List_Rows) {
	w := &core.world
	who := w.player.character
	shoppe := w.player.shoppe
	labels := make([dynamic]string, context.temp_allocator)
	hues := make([dynamic]Hue, context.temp_allocator)
	#partial switch core.state {
	case .Shoppe_Offers: for e in shoppe_offers(shoppe) { append(&labels, fmt.tprintf("%s: %d", ITEM_TYPES[e.type].name, e.value)) }
	case .Shoppe_Prices: for e in shoppe_prices(shoppe) { append(&labels, fmt.tprintf("%s: %d", ITEM_TYPES[e.type].name, e.value)) }
	case .Shoppe_Buy: for e in items_to_buy(w, who, shoppe) { append(&labels, fmt.tprintf("%s(%d)", ITEM_TYPES[e.type].name, e.value)) }
	case .Shoppe_Sell: for id in items_to_sell(w, who, shoppe) { append(&labels, item_name(item_get(w, id))) }
	case .Shoppe_Repair:
		for id in items_to_repair(w, who, shoppe) { append(&labels, fmt.tprintf("%s(%d)", item_name(item_get(w, id)), repair_cost(item_get(w, id), shoppe))) }
	case .Spell_List:
		for s in known_spells(w) {
			append(&labels, fmt.tprintf("%s(Lvl%d)", SPELL_TYPES[s].name, w.player.spells[s]))
		}
	}
	for _ in labels { append(&hues, Hue.Black) }
	if core.state == .Spell_List { for s, i in known_spells(w) { hues[i] = can_cast(w, who, s) ? .Black : .Red } }
	return {labels[:], hues[:]}
}

draw_shoppe_list :: proc(core: ^Core) {
	s := &core.screen
	w := &core.world
	header: string
	first_row := 1
	#partial switch core.state {
	case .Shoppe_Offers: header = "Offers"
	case .Shoppe_Prices: header = "Prices"
	case .Shoppe_Buy: header, first_row = "Buy", 2
	case .Shoppe_Sell: header, first_row = "Sell", 2
	case .Shoppe_Repair: header, first_row = "Repair", 2
	case .Spell_List: header, first_row = "Spell List", 2
	}
	screen_fill(s, GLYPH_SPACE, false, .Blue)
	centered_header(s, header)
	if first_row == 2 && core.state != .Spell_List {
		write_text_centered(s, 1, fmt.tprintf("Money: %d", stat_of(w, w.player.character, .Money)), false, .Black)
	}
	rows := shoppe_list_rows(core)
	for row in first_row ..= LIST_LAST_ROW {
		index := row - LIST_HILITE_ROW + core.list_cursor
		if index < 0 || index >= len(rows.labels) { continue }
		selected := index == core.list_cursor
		fill_cells(s, 0, row, CELL_COLUMNS, 1, GLYPH_SPACE, selected, rows.hues[index])
		write_text_centered(s, row, rows.labels[index], selected, rows.hues[index])
	}
	write_text_centered(s, CELL_ROWS - 1, core.state == .Shoppe_Offers || core.state == .Shoppe_Prices ? "Arrows, Esc" : "Arrows, Space, Esc", false, .Black)
}

// What Confirm does on a shoppe or spell list.
shoppe_list_confirm :: proc(core: ^Core) {
	w := &core.world
	who := w.player.character
	shoppe := w.player.shoppe
	old := core.list_cursor
	count_before := list_count(core)
	if count_before == 0 { enter_state(core, .In_Play); return }
	#partial switch core.state {
	case .Shoppe_Buy: buy_item(w, who, items_to_buy(w, who, shoppe)[old])
	case .Shoppe_Sell: sell_item(w, who, shoppe, items_to_sell(w, who, shoppe)[old])
	case .Shoppe_Repair:
		if !repair_item(w, who, shoppe, items_to_repair(w, who, shoppe)[old]) { return } // too poor: stay
	case .Spell_List:
		cast_spell(w, who, known_spells(w)[old])
		show_messages_then(core, .Spell_List)
		return
	case: return // offers and prices are only to be read
	}
	if remaining := list_count(core); remaining > 0 { core.list_cursor = old % remaining } else { enter_state(core, .In_Play) }
}
