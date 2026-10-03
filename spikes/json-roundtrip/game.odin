package main

import "core:encoding/json"
import "core:fmt"
import "core:mem"

Item_Type_ID :: distinct int
Location_ID :: distinct int

Stat :: enum { Strength, Dexterity, Intelligence }
Flag :: enum { Undead, Invisible }
Flags :: bit_set[Flag]
Mode :: union { Shoppe_Mode, Combat_Mode }
Shoppe_Mode :: struct { shoppe_type: int }
Combat_Mode :: struct { enemy: int, hp: int }

Mode_Kind :: enum { None, Shoppe, Combat }
Kinded_Mode :: struct { kind: Mode_Kind, shoppe_type, enemy, hp: int }

Item :: struct {
	type:       Item_Type_ID,
	durability: Maybe(int),
	label:      string,
}

Character :: struct {
	name:      string,
	stats:     [Stat]int,         // enumerated array
	position:  [2]int,            // fixed array
	flags:     Flags,
	inventory: [dynamic]Item,
	equipped:  map[string]Item,   // string-keyed map
	known:     map[int]bool,      // int-keyed map
	mode:      Mode,
	gold:      f32,
	loc:       Location_ID,
}

World_State :: struct {
	version:    int,
	characters: [dynamic]Character,
	seed:       u64,
}

make_state :: proc() -> World_State {
	ws := World_State{version = 1, seed = 18446744073709551000}
	c := Character{name = "Kordanor \"the\" Bold\n", gold = 12.5, loc = 7, position = {3, 4}}
	c.stats[.Strength] = 12
	c.stats[.Intelligence] = 9
	c.flags = {.Invisible}
	append(&c.inventory, Item{type = 3, durability = 40, label = "sword"})
	append(&c.inventory, Item{type = 5, label = "potion"})
	c.equipped["hand"] = Item{type = 3, durability = 40, label = "sword"}
	c.known[42] = true
	c.known[7] = false
	c.mode = Combat_Mode{enemy = 2, hp = 17}
	append(&ws.characters, c)
	return ws
}

report :: proc(name: string, ok: bool, detail: string = "") {
	fmt.printf("%-28s %s %s\n", name, ok ? "PASS" : "FAIL", detail)
}

run_spike :: proc() {
	ws := make_state()
	data, err := json.marshal(ws, {pretty = true, use_enum_names = true, spec = .JSON})
	report("marshal", err == nil, fmt.tprintf("(%d bytes, err=%v)", len(data), err))
	if err != nil { return }
	fmt.println(string(data))

	out: World_State
	uerr := json.unmarshal(data, &out)
	report("unmarshal", uerr == nil, fmt.tprintf("(err=%v)", uerr))
	if uerr != nil { return }

	c0, c1 := ws.characters[0], out.characters[0]
	report("string w/ escapes", c0.name == c1.name)
	report("enumerated array", c0.stats == c1.stats)
	report("fixed array", c0.position == c1.position)
	report("bit_set", c0.flags == c1.flags)
	report("distinct int id", c0.loc == c1.loc && c0.inventory[0].type == c1.inventory[0].type)
	report("Maybe(int) some/none", c1.inventory[0].durability == Maybe(int)(40) && c1.inventory[1].durability == nil)
	report("dynamic array", len(c1.inventory) == 2)
	report("map[string]struct", c1.equipped["hand"].label == "sword")
	report("map[int]bool", c1.known[42] == true && c1.known[7] == false && len(c1.known) == 2)
	mode, is_combat := c1.mode.(Combat_Mode)
	report("tagged union", is_combat && mode.hp == 17, fmt.tprintf("(got %v)", c1.mode))

	// Workaround: an explicit discriminator enum plus flat fields.
	kinded := Kinded_Mode{kind = .Combat, enemy = 2, hp = 17}
	kd, _ := json.marshal(kinded)
	kback: Kinded_Mode
	json.unmarshal(kd, &kback)
	report("kind-enum workaround", kback == kinded, string(kd))
	report("f32", c0.gold == c1.gold)
	report("u64 big value", ws.seed == out.seed, fmt.tprintf("(%v vs %v)", ws.seed, out.seed))

	// Robustness: bad input must return an error, not crash.
	bad: World_State
	berr := json.unmarshal(transmute([]u8)string("{\"version\": \"nope\""), &bad)
	report("malformed json -> error", berr != nil, fmt.tprintf("(err=%v)", berr))

	// Allocator behavior: marshal/unmarshal in a growing arena, then free it all at once.
	arena: mem.Arena
	buf := make([]byte, 64 * 1024)
	mem.arena_init(&arena, buf)
	context.allocator = mem.arena_allocator(&arena)
	d2, e2 := json.marshal(ws)
	var2: World_State
	e3 := json.unmarshal(d2, &var2)
	report("arena allocator", e2 == nil && e3 == nil, fmt.tprintf("(arena used %d of %d bytes)", arena.offset, len(buf)))
}
