package game

// Fixed-capacity generational pool. Slot 0 is reserved so the zero Handle means "none".
Handle :: distinct u64 // low 32 bits: slot index, high 32 bits: generation

handle_make :: proc(index, gen: u32) -> Handle { return Handle(u64(gen) << 32 | u64(index)) }
handle_index :: proc(h: Handle) -> u32 { return u32(u64(h) & 0xFFFF_FFFF) }
handle_gen :: proc(h: Handle) -> u32 { return u32(u64(h) >> 32) }

Slot :: struct($T: typeid) {
	gen:       u32,
	alive:     bool,
	next_free: u32, // valid while !alive
	value:     T,
}

Pool :: struct($T: typeid, $N: int) {
	slots:     [N]Slot(T),
	free_head: u32, // 0 = empty free list (slot 0 is never free)
	used_top:  u32, // slots below this index have been handed out at least once
	count:     int,
}

pool_init :: proc(p: ^Pool($T, $N)) {
	p^ = {}
	p.used_top = 1
}

pool_create :: proc(p: ^Pool($T, $N), value: T) -> (h: Handle, ok: bool) {
	index: u32
	if p.free_head != 0 {
		index = p.free_head
		p.free_head = p.slots[index].next_free
	} else if int(p.used_top) < N {
		index = p.used_top
		p.used_top += 1
	} else {
		return 0, false // full
	}
	s := &p.slots[index]
	s.alive = true
	s.value = value
	p.count += 1
	return handle_make(index, s.gen), true
}

pool_get :: proc(p: ^Pool($T, $N), h: Handle) -> (^T, bool) {
	index := handle_index(h)
	if index == 0 || int(index) >= N { return nil, false }
	s := &p.slots[index]
	if !s.alive || s.gen != handle_gen(h) { return nil, false }
	return &s.value, true
}

pool_destroy :: proc(p: ^Pool($T, $N), h: Handle) -> bool {
	if _, ok := pool_get(p, h); !ok { return false }
	index := handle_index(h)
	s := &p.slots[index]
	s.alive = false
	s.gen += 1
	s.value = {}
	s.next_free = p.free_head
	p.free_head = index
	p.count -= 1
	return true
}

// Iteration in slot order (deterministic).
pool_next :: proc(p: ^Pool($T, $N), cursor: ^u32) -> (h: Handle, v: ^T, ok: bool) {
	for cursor^ < p.used_top {
		i := cursor^
		cursor^ += 1
		if i != 0 && p.slots[i].alive {
			return handle_make(i, p.slots[i].gen), &p.slots[i].value, true
		}
	}
	return
}

// Save form: every slot below used_top, so generations (and thus stale-handle behavior) and the
// exact free-list reuse order survive a save/load.
Saved_Pool :: struct($T: typeid) {
	used_top:  u32,
	free_head: u32,
	slots:     [dynamic]Slot(T),
}

pool_to_saved :: proc(p: ^Pool($T, $N), allocator := context.allocator) -> Saved_Pool(T) {
	out := Saved_Pool(T){used_top = p.used_top, free_head = p.free_head}
	out.slots = make([dynamic]Slot(T), 0, int(p.used_top), allocator)
	for i in 0 ..< int(p.used_top) { append(&out.slots, p.slots[i]) }
	return out
}

pool_from_saved :: proc(p: ^Pool($T, $N), saved: Saved_Pool(T)) -> bool {
	if int(saved.used_top) > N || int(saved.used_top) != len(saved.slots) { return false }
	p^ = {}
	p.used_top = saved.used_top
	p.free_head = saved.free_head
	for s, i in saved.slots {
		p.slots[i] = s
		if s.alive { p.count += 1 }
	}
	return true
}
