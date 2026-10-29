package game

// The ending (docs/FINISH-PLAN.md, phase 1). Bring the Horns of Kordanor to the Elder and he asks for them. Handing them over is
// always possible; keeping them is possible only for a player who has read enough Notes to know better. Either way the Elder is
// Zooperdan, and the fight is fought in the Town Square with the ordinary combat, no running and no shops. At half HP he
// changes (Zooperdan_Unbound) and adds mental attacks, which kill a demoralized player. Killing him ends the game: the ending
// screens (The_End) show an epilogue for the answer given.

import "core:math/bits"

LORES_FOR_REFUSAL :: 10 // notes read before keeping the horns is offered

ELDER_HORNS :: 5
ELDER_REFUSE :: 6

is_finale_boss :: proc(t: Character_Type) -> bool { return t == .Zooperdan || t == .Zooperdan_Unbound }

finale_boss_here :: proc(w: ^World, who: Character_ID) -> bool {
	for e in enemies_of(w, who) { if is_finale_boss(character_get(w, e).type) { return true } }
	return false
}

lores_read_count :: proc(w: ^World) -> int { return int(bits.count_ones(w.player.lores_read)) }

can_hand_over_horns :: proc(w: ^World, who: Character_ID) -> bool { return has_item_type(w, who, .Horns_of_Kordanor) }
can_refuse_horns :: proc(w: ^World, who: Character_ID) -> bool {
	return can_hand_over_horns(w, who) && lores_read_count(w) >= LORES_FOR_REFUSAL
}

// The Elder's answer when a button about the horns is selected.
elder_horns_speech :: proc(button: int) -> string {
	if button == ELDER_REFUSE { return "Keep them? Why would you do that? Surely you do not distrust an old man?" }
	return "The horns! At last! Give them to me, my friend, and let there be peace."
}

// Begins the fight: the twist, then Zooperdan stands in the Town Square. A refusing player meets him weaker.
begin_finale :: proc(core: ^Core, how: Finale) {
	w := &core.world
	who := w.player.character
	pop_button(core)
	w.player.mode = .Neutral
	w.player.finale = how
	switch how {
	case .None: return
	case .Handed:
		if horns, has := carried_item_of_type(w, who, .Horns_of_Kordanor); has { destroy_item(w, horns) }
		message_add(w, .None, "Zooperdan takes the horns in trembling hands. \"At last! Centuries I searched, and the kin hid them well.\"")
		message_add(w, .None, "\"Did you never wonder why the Cabal sat in that church? They did not keep Kordanor captive. They kept him SAFE. From me.\"")
		message_add(w, .None, "The old man's face splits and he is gone. \"And you, errand-runner, know too much. Let us finish this.\"")
	case .Refused:
		message_add(w, .None, "You close your hand around the horns. \"I have read the notes, Zooperdan. The Cabal kept Kordanor safe from YOU.\"")
		message_add(w, .None, "The old man's smile slips. \"Clever. Then I will take them from your corpse.\"")
	}
	boss := create_character(w, .Zooperdan, character_get(w, who).location)
	if how == .Refused { // without the horns' power he is weaker
		c := character_get(w, boss)
		c.stats[.HP], c.stats[.Strength], c.stats[.Dexterity], c.stats[.Influence] = 10, 7, 4, 4
		c.stats[.Unarmed_Maximum_Damage], c.stats[.Base_Maximum_Defend] = 3, 4
	}
	show_messages_then(core, .In_Play)
}

// Called before the enemies strike back: at half HP Zooperdan drops the last of his disguise.
finale_phase :: proc(w: ^World, who: Character_ID) {
	for e in enemies_of(w, who) {
		c := character_get(w, e)
		if c.type == .Zooperdan && health_current(w, e) * 2 <= stat_of(w, e, .HP) && !is_dead(w, e) {
			c.type = .Zooperdan_Unbound
			say(w, .None, "Zooperdan tears off the last of his disguise.", "\"I do not need a body to break you!\"")
		}
	}
}

// ---- the ending screens --------------------------------------------------------------------------------------------

END_TITLES := [Finale]string{.None = "", .Handed = "The Broken Seal", .Refused = "The Vigil"}
END_PAGES := [Finale][]string{
	.None = {},
	.Handed = {
		"Zooperdan falls and the horns clatter in the dust. But you handed them over, and in doing so broke the last seal. Far below, in the dark, something old begins to wake.",
		"The villagers pay you well and ask no questions. You sleep badly. Every night the church bell rings, though the church has no bell.",
	},
	.Refused = {
		"Zooperdan falls. The horns are warm in your hands, and the dungeon below falls silent, as if it were breathing out.",
		"You carry them back to the church and take up the watch the kin once kept: not to keep the captive within, but to keep those that seek his release without.",
		"The Cabal of Kordanor has a new member. The vigil goes on.",
	},
}

// Leaves the world for the ending screens once the death messages are done.
start_ending :: proc(core: ^Core) {
	core.ending = core.world.player.ending
	core.ending_page = 0
	abandon_world(core)
	enter_state(core, .The_End)
}

end_command :: proc(core: ^Core, c: Command) {
	if c != .Confirm { return }
	core.ending_page += 1
	if core.ending_page >= len(END_PAGES[core.ending]) { enter_state(core, .Title) }
}

draw_end :: proc(core: ^Core) {
	s := &core.screen
	centered_header(s, END_TITLES[core.ending])
	pages := END_PAGES[core.ending]
	if core.ending_page < len(pages) {
		text := wrap22(pages[core.ending_page])
		for i := 0; i * 22 < len(text); i += 1 { write_text(s, 0, 2 + i, text[i * 22:min(len(text), i * 22 + 22)], false, .Black) }
	}
	last := core.ending_page + 1 >= len(pages)
	write_text_centered(s, 22, last ? "THE END" : "SPACE", true, .Orange)
}
