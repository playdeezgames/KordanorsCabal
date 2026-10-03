package main

import "core:encoding/json"
import "core:fmt"

Character :: struct {
	name:      string,
	hp:        int,
	items:     [dynamic]Handle, // owner-side inventory
}
Item :: struct {
	kind: int,
}

CHAR_CAP :: 8
ITEM_CAP :: 16

World :: struct {
	characters: Pool(Character, CHAR_CAP),
	items:      Pool(Item, ITEM_CAP),
	player:     Handle,
}


report :: proc(name: string, ok: bool, detail: string = "") {
	fmt.printf("%-34s %s %s\n", name, ok ? "PASS" : "FAIL", detail)
}

run_spike :: proc() {
	w: World
	pool_init(&w.characters)
	pool_init(&w.items)

	// create, resolve, destroy, stale detection
	a, _ := pool_create(&w.characters, Character{name = "Kordanor", hp = 10})
	b, _ := pool_create(&w.characters, Character{name = "Rat", hp = 2})
	c, _ := pool_create(&w.characters, Character{name = "Bat", hp = 3})
	w.player = a
	_, ok := pool_get(&w.characters, b)
	report("resolve live handle", ok)
	_, zero_ok := pool_get(&w.characters, 0)
	report("zero handle is none", !zero_ok)
	report("destroy", pool_destroy(&w.characters, b))
	_, ok = pool_get(&w.characters, b)
	report("stale handle rejected", !ok)
	report("double destroy rejected", !pool_destroy(&w.characters, b))
	d, _ := pool_create(&w.characters, Character{name = "Snake", hp = 4})
	report("slot reused, new generation", handle_index(d) == handle_index(b) && handle_gen(d) == handle_gen(b) + 1)
	_, ok = pool_get(&w.characters, b)
	report("old handle still stale after reuse", !ok)

	// exhaustion
	for i in 0 ..< CHAR_CAP { pool_create(&w.characters, Character{hp = i}) }
	_, full_ok := pool_create(&w.characters, Character{})
	report("full pool reports failure", !full_ok, fmt.tprintf("(count %d of %d usable)", w.characters.count, CHAR_CAP - 1))

	// iteration order is slot order
	cursor: u32 = 0
	order: [dynamic]string
	defer delete(order)
	for h, v in pool_next(&w.characters, &cursor) { append(&order, v.name != "" ? v.name : "-") ; _ = h }
	report("iteration deterministic", len(order) == w.characters.count && order[0] == "Kordanor", fmt.tprint(order[:min(4, len(order))]))

	// handle through JSON (packed u64, fixed array pool)
	i1, _ := pool_create(&w.items, Item{kind = 7})
	append(&a_char(&w).items, i1)
	data, err := json.marshal(w.player)
	back: Handle
	uerr := json.unmarshal(data, &back)
	report("Handle JSON round trip", err == nil && uerr == nil && back == w.player, string(data))

	// a handle with a generation survives as one number
	big := handle_make(5, 3000000000)
	data2, _ := json.marshal(big)
	back2: Handle
	json.unmarshal(data2, &back2)
	report("packed handle with big gen", back2 == big, string(data2))

	fmt.printf("sizeof Character slot: %d bytes, Item slot: %d bytes, World: %d bytes\n", size_of(Slot(Character)), size_of(Slot(Item)), size_of(World))
	_ = c
	save_load_check()
}

a_char :: proc(w: ^World) -> ^Character {
	c, _ := pool_get(&w.characters, w.player)
	return c
}

save_load_check :: proc() {
	w: World
	pool_init(&w.characters)
	hs: [5]Handle
	for i in 0 ..< 5 { hs[i], _ = pool_create(&w.characters, Character{name = "x", hp = i}) }
	pool_destroy(&w.characters, hs[1])
	pool_destroy(&w.characters, hs[3])
	saved := pool_to_saved(&w.characters)
	data, err := json.marshal(saved)
	report("pool marshal", err == nil, fmt.tprintf("(%d bytes)", len(data)))
	loaded: Saved_Pool(Character)
	uerr := json.unmarshal(data, &loaded)
	w2: World
	ok := pool_from_saved(&w2.characters, loaded)
	report("pool unmarshal + restore", uerr == nil && ok && w2.characters.count == 3)
	_, live := pool_get(&w2.characters, hs[2])
	_, stale := pool_get(&w2.characters, hs[1])
	report("live ok / stale rejected after load", live && !stale)
	n1, _ := pool_create(&w.characters, Character{})
	n2, _ := pool_create(&w2.characters, Character{})
	report("same reuse order after load", n1 == n2, fmt.tprintf("(%x)", u64(n1)))
}
