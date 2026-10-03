package game

// The item screens: inventory, the menu for one item, the ground, the worn equipment and one worn slot (InventoryProcessor,
// InteractItemProcessor, GroundInventoryProcessor, EquipmentProcessor, EquipmentDetailProcessor).

import "core:fmt"

LIST_HILITE_ROW :: 10 // the selected entry of a scrolling list is always drawn on this row
INVENTORY_FIRST_ROW :: 2
GROUND_FIRST_ROW :: 1
LIST_LAST_ROW :: 21

// Shows the queued messages and then continues at `back` (the original's "push the state, return Message" pair). With nothing
// queued the messages screen is skipped.
show_messages_then :: proc(core: ^Core, back: UI_State) {
	push_state(core, back)
	if core.world.messages.count > 0 { enter_state(core, .Message) } else { enter_state(core, pop_state(core)) }
}

list_count :: proc(core: ^Core) -> int {
	w := &core.world
	player := w.player.character
	#partial switch core.state {
	case .Inventory: return len(item_groups(w, player))
	case .Ground_Inventory: return len(items_on_ground(w, character_get(w, player).location))
	case .Equipment: return len(items_worn(w, player)) + 1 // plus "Go Back"
	case .Shoppe_Offers, .Shoppe_Prices, .Shoppe_Buy, .Shoppe_Sell, .Shoppe_Repair, .Spell_List: return len(shoppe_list_rows(core).labels)
	}
	return 0
}

list_command :: proc(core: ^Core, c: Command) {
	count := list_count(core)
	town_list := is_shoppe_list(core.state) || core.state == .Spell_List
	if count == 0 && !town_list { enter_state(core, .In_Play); return }
	switch c {
	case .Up: if count > 0 { core.list_cursor = (core.list_cursor + count - 1) % count }
	case .Down: if count > 0 { core.list_cursor = (core.list_cursor + 1) % count }
	case .Cancel:
		if is_shoppe_list(core.state) { core.world.player.shoppe = .None }
		enter_state(core, .In_Play)
	case .Confirm: list_confirm(core)
	case .Left, .Right, .None:
	}
}

list_confirm :: proc(core: ^Core) {
	w := &core.world
	player := w.player.character
	#partial switch core.state {
	case .Inventory:
		groups := item_groups(w, player)
		if core.list_cursor >= len(groups) { return }
		g := groups[core.list_cursor]
		core.interact_item = g.items[pick_index(&w.rng, len(g.items))] // any one of the equal items, as the original chose
		enter_state(core, .Interact_Item)
	case .Ground_Inventory:
		items := items_on_ground(w, character_get(w, player).location)
		if core.list_cursor >= len(items) { return }
		pick_up(w, player, items[core.list_cursor])
		remaining := len(items) - 1
		if remaining > 0 { core.list_cursor %= remaining } else { enter_state(core, .In_Play) }
	case .Shoppe_Offers, .Shoppe_Prices, .Shoppe_Buy, .Shoppe_Sell, .Shoppe_Repair, .Spell_List: shoppe_list_confirm(core)
	case .Equipment:
		if core.list_cursor == 0 { enter_state(core, .In_Play); return }
		worn := items_worn(w, player)
		if core.list_cursor > len(worn) { return }
		core.equip_slot = item_get(w, worn[core.list_cursor - 1]).slot
		enter_state(core, .Equipment_Detail)
	}
}

// A tap on a list row: it selects that entry (a precise tap also confirms it); tapping the selected row confirms.
list_tap :: proc(core: ^Core, row: int, precise: bool) {
	count := list_count(core)
	if count == 0 { return }
	index: int
	if core.state == .Equipment {
		index = row - 1 // "Go Back" is row 1, the slots follow
		if index < 0 || index >= count { return }
	} else {
		first := core.state == .Inventory ? INVENTORY_FIRST_ROW : GROUND_FIRST_ROW
		if core.state == .Shoppe_Offers || core.state == .Shoppe_Prices { first = 1 } else if is_shoppe_list(core.state) || core.state == .Spell_List { first = 2 }
		if row < first || row > LIST_LAST_ROW { return }
		index = row - LIST_HILITE_ROW + core.list_cursor
		if index < 0 || index >= count { return }
	}
	was := core.list_cursor
	core.list_cursor = index
	if precise || was == index { list_confirm(core) }
}

interact_item_activate :: proc(core: ^Core, index: int) {
	w := &core.world
	player := w.player.character
	id := core.interact_item
	if item_get(w, id) == nil { enter_state(core, .Inventory); return }
	switch index {
	case 0: enter_state(core, .Inventory)
	case 1:
		drop_item(w, player, id)
		enter_state(core, .Inventory) // leaves for the in-play screen when the pack is empty
	case 2:
		if item_use(w, player, id) {
			show_messages_then(core, .Inventory)
		} else {
			message_add(w, .None, "You cannot use that now!")
			show_messages_then(core, .Interact_Item)
		}
	case 3:
		equip_item(w, player, id)
		show_messages_then(core, .Inventory)
	}
}

// ---- drawing ----------------------------------------------------------------------------------------------------------

draw_list_rows :: proc(core: ^Core, first_row: int, label: proc(core: ^Core, index: int) -> string, count: int) {
	s := &core.screen
	for row in first_row ..= LIST_LAST_ROW {
		index := row - LIST_HILITE_ROW + core.list_cursor
		if index < 0 || index >= count { continue }
		selected := index == core.list_cursor
		fill_cells(s, 0, row, CELL_COLUMNS, 1, GLYPH_SPACE, selected, .Black)
		write_text_centered(s, row, label(core, index), selected, .Black)
	}
}

draw_inventory :: proc(core: ^Core) {
	s := &core.screen
	w := &core.world
	player := w.player.character
	centered_header(s, "Inventory")
	write_text_centered(s, 1, fmt.tprintf("Encumbrance: %d/%d", encumbrance_current(w, player), encumbrance_maximum(w, player)), false, .Red)
	groups := item_groups(w, player)
	draw_list_rows(core, INVENTORY_FIRST_ROW, proc(core: ^Core, i: int) -> string {
		g := item_groups(&core.world, core.world.player.character)[i]
		return fmt.tprintf("%s(%d)", g.name, len(g.items))
	}, len(groups))
	write_text_centered(s, CELL_ROWS - 1, "Arrows, Space, Esc", false, .Black)
}

draw_ground :: proc(core: ^Core) {
	s := &core.screen
	w := &core.world
	centered_header(s, "On the Ground")
	items := items_on_ground(w, character_get(w, w.player.character).location)
	draw_list_rows(core, GROUND_FIRST_ROW, proc(core: ^Core, i: int) -> string {
		w := &core.world
		return item_name(item_get(w, items_on_ground(w, character_get(w, w.player.character).location)[i]))
	}, len(items))
	write_text_centered(s, CELL_ROWS - 1, "Arrows, Space, Esc", false, .Black)
}

draw_equipment :: proc(core: ^Core) {
	s := &core.screen
	w := &core.world
	centered_header(s, "Equipment")
	write_text(s, 0, 1, "Go Back", core.list_cursor == 0, .Black)
	for id, i in items_worn(w, w.player.character) {
		it := item_get(w, id)
		row := i + 2
		selected := core.list_cursor == i + 1
		slot_text := fmt.tprintf("%s: ", EQUIP_SLOT_NAME[it.slot])
		write_text(s, 0, row, slot_text, selected, .Black)
		hue := Hue.Black
		if m, has := durability_maximum(it); has { // worn-down gear shows red then yellow
			cur, _ := durability_current(it)
			if cur <= 0 || f32(m) / f32(cur) >= 4 { hue = .Red } else if f32(m) / f32(cur) >= 2 { hue = .Yellow }
		}
		write_text(s, len(slot_text), row, item_name(it), selected, hue)
	}
	write_text_centered(s, CELL_ROWS - 1, "Arrows, Space, Esc", false, .Black)
}

// The first lines of the Interact_Item and Equipment_Detail menus.
draw_item_menu_prompt :: proc(core: ^Core) {
	s := &core.screen
	w := &core.world
	fill_cells(s, 0, 0, CELL_COLUMNS, 1, GLYPH_SPACE, true, .Blue)
	#partial switch core.state {
	case .Interact_Item:
		it := item_get(w, core.interact_item)
		if it == nil { return }
		write_text_centered(s, 1, fmt.tprintf("Encumbrance: %d", ITEM_TYPES[it.type].stats.encumbrance), false, .Black)
		write_text_centered(s, 0, item_name(it), true, .Blue)
	case .Equipment_Detail:
		id, has := item_in_slot(w, w.player.character, core.equip_slot)
		if !has { return }
		it := item_get(w, id)
		write_text_centered(s, 0, EQUIP_SLOT_NAME[core.equip_slot], true, .Blue)
		write_text(s, 0, 1, fmt.tprintf("Item: %s", item_name(it)), false, .Black)
		if cur, ok := durability_current(it); ok {
			m, _ := durability_maximum(it)
			write_text(s, 0, 2, fmt.tprintf("Durability: %d/%d", cur, m), false, .Black)
		}
	}
}
