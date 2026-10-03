package main

// The small slice of the game API the event handlers need. In the real port these are the character, inventory,
// location and combat modules; here they are the simplest versions that make the handlers meaningful.
import "core:fmt"
import "core:strings"

// ---- randomness ------------------------------------------------------------------------------------------
rng_next :: proc(w: ^World) -> u64 {
	w.rng += 0x9E3779B97F4A7C15
	z := w.rng
	z = (z ~ (z >> 30)) * 0xBF58476D1CE4E5B9
	z = (z ~ (z >> 27)) * 0x94D049BB133111EB
	return z ~ (z >> 31)
}
rng_range :: proc(w: ^World, lo, hi: int) -> int { return lo + int(rng_next(w) % u64(hi - lo + 1)) }
roll :: proc(w: ^World, count, sides: int) -> int { total := 0; for _ in 0 ..< count { total += rng_range(w, 1, sides) }; return total }
pick_weighted :: proc(w: ^World, weights: []int) -> int {
	sum := 0; for x in weights { sum += x }
	r := rng_range(w, 0, sum - 1)
	for x, i in weights { r -= x; if r < 0 { return i } }
	return len(weights) - 1
}

// ---- entities --------------------------------------------------------------------------------------------
char_get :: proc(w: ^World, h: Handle) -> (^Character, bool) { return pool_get(&w.characters, h) }
item_get :: proc(w: ^World, h: Handle) -> (^Item, bool) { return pool_get(&w.items, h) }

stat :: proc(c: ^Character, s: Stat) -> i32 { return c.stats[s] }
change_stat :: proc(c: ^Character, s: Stat, delta: i32) { c.stats[s] = max(0, c.stats[s] + delta) }
health_current :: proc(c: ^Character) -> i32 { return max(0, c.stats[.HP] - c.stats[.Wounds]) } // stand-in for CharacterHealth
mana_current :: proc(c: ^Character) -> i32 { return max(0, c.stats[.Mana] - c.stats[.Fatigue]) }  // stand-in for CharacterMana
is_player :: proc(w: ^World, h: Handle) -> bool { return h == w.player }

create_character :: proc(w: ^World, type: Character_Type, loc: Location_ID, stats: [Stat]i32) -> (Handle, bool) {
	return pool_create(&w.characters, Character{type = type, location = loc, stats = stats})
}
create_item :: proc(w: ^World, type: Item_Type) -> (Handle, bool) {
	w.item_seq += 1
	return pool_create(&w.items, Item{type = type, seq = w.item_seq})
}
destroy_item :: proc(w: ^World, h: Handle) { pool_destroy(&w.items, h) }
// Destroying a character destroys what it carries or wears (task 17).
destroy_character :: proc(w: ^World, h: Handle) {
	if h == w.player { return }
	cur: u32 = 0
	for ih, it in pool_next(&w.items, &cur) { if (it.holder == .Carried || it.holder == .Equipped) && it.holder_character == h { pool_destroy(&w.items, ih) } }
	pool_destroy(&w.characters, h)
}
give_item :: proc(w: ^World, ch: Handle, ih: Handle) {
	if it, ok := item_get(w, ih); ok { w.item_seq += 1; it.holder, it.holder_character, it.holder_location, it.seq = .Carried, ch, 0, w.item_seq }
}
drop_item :: proc(w: ^World, loc: Location_ID, ih: Handle) {
	if it, ok := item_get(w, ih); ok { w.item_seq += 1; it.holder, it.holder_character, it.holder_location, it.seq = .On_Ground, 0, loc, w.item_seq }
}
first_carried_of_type :: proc(w: ^World, ch: Handle, type: Item_Type) -> (Handle, bool) {
	cur: u32 = 0
	for ih, it in pool_next(&w.items, &cur) { if it.holder == .Carried && it.holder_character == ch && it.type == type { return ih, true } }
	return 0, false
}
// Stand-in for Factions.FirstEnemy: another live non-player character in the same location.
first_enemy :: proc(w: ^World, ch: Handle) -> (Handle, bool) {
	me, ok := char_get(w, ch); if !ok { return 0, false }
	cur: u32 = 0
	for h, c in pool_next(&w.characters, &cur) { if h != ch && c.location == me.location && c.type != .N00b { return h, true } }
	return 0, false
}
can_be_bribed_with :: proc(w: ^World, enemy: Handle, type: Item_Type) -> bool {
	e, ok := char_get(w, enemy); return ok && type in BRIBES[e.type]
}

// Stand-in for PhysicalCombat.DoDamage / Kill: wounds, then removal with a reward line.
damage_and_maybe_kill :: proc(w: ^World, by: Handle, enemy: Handle, damage: int, lines: ^[dynamic]string) -> Sfx {
	e, ok := char_get(w, enemy); if !ok { return .None }
	change_stat(e, .Wounds, i32(damage))
	if health_current(e) > 0 { return .None }
	append(lines, fmt.tprintf("%v dies!", e.type))
	destroy_character(w, enemy)
	return .Enemy_Death
}

// ---- messages and sounds ---------------------------------------------------------------------------------
// Only the player has a message queue (VB: PlayerCharacter.EnqueueMessage; other characters ignore messages).
say :: proc(w: ^World, who: Handle, sfx: Sfx, lines: ..string) {
	if !is_player(w, who) { return }
	if w.msg_count == MAX_MESSAGES { return } // full: drop the newest (a debug build would assert)
	m := &w.messages[(w.msg_head + w.msg_count) % MAX_MESSAGES]
	m^ = {sfx = sfx}
	b := strings.builder_from_bytes(m.text[:])
	for l, i in lines { if i > 0 { strings.write_byte(&b, '\n') }; strings.write_string(&b, l) }
	m.size = u16(strings.builder_len(b))
	w.msg_count += 1
}
message_text :: proc(m: ^Message) -> string { return string(m.text[:m.size]) }
play :: proc(w: ^World, sfx: Sfx) { if w.sfx_count < MAX_SFX { w.sfx[w.sfx_count] = sfx; w.sfx_count += 1 } }
