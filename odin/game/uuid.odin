package game

// Entity ids (decision D9, amended in review 1): characters, items and locations are keyed by 16-byte UUIDs drawn from the
// world's seeded generator, so a seed always produces the same ids. They are never reused. In memory they are plain bytes;
// in the save file they are the canonical 36-character lower-case text, because JSON has no byte arrays.
Character_ID :: distinct [16]u8
Item_ID :: distinct [16]u8
Location_ID :: distinct [16]u8

UUID_TEXT_LEN :: 36

// A version 4 UUID from two draws of the world generator.
uuid_draw :: proc(r: ^Rng) -> (id: [16]u8) {
	a, b := rng_u64(r), rng_u64(r)
	for i in 0 ..< 8 {
		id[i] = u8(a >> (uint(i) * 8))
		id[8 + i] = u8(b >> (uint(i) * 8))
	}
	id[6] = (id[6] & 0x0F) | 0x40 // version 4
	id[8] = (id[8] & 0x3F) | 0x80 // RFC 4122 variant
	return
}

uuid_text :: proc(id: [16]u8) -> (text: [UUID_TEXT_LEN]u8) {
	hex := "0123456789abcdef"
	at := 0
	for b, i in id {
		if i == 4 || i == 6 || i == 8 || i == 10 { text[at] = '-'; at += 1 }
		text[at] = hex[b >> 4]
		text[at + 1] = hex[b & 15]
		at += 2
	}
	return
}

// Allocates the text with `allocator` (the save writer needs strings that outlive the call).
uuid_string :: proc(id: $T, allocator := context.allocator) -> string where size_of(T) == 16 {
	text := uuid_text(transmute([16]u8)id)
	return string(clone_bytes(text[:], allocator))
}

clone_bytes :: proc(b: []u8, allocator := context.allocator) -> []u8 {
	out := make([]u8, len(b), allocator)
	copy(out, b)
	return out
}

// Strict: exactly the canonical form uuid_text writes (lower-case hex, dashes at 8, 13, 18, 23). The loader uses this, so a
// hand-edited save with a malformed id is rejected instead of silently becoming some other id.
uuid_parse :: proc(text: string) -> (id: [16]u8, ok: bool) {
	if len(text) != UUID_TEXT_LEN { return {}, false }
	at, nibbles := 0, 0
	for i in 0 ..< len(text) {
		c := text[i]
		if i == 8 || i == 13 || i == 18 || i == 23 {
			if c != '-' { return {}, false }
			continue
		}
		v: u8
		switch c {
		case '0' ..= '9': v = c - '0'
		case 'a' ..= 'f': v = c - 'a' + 10
		case: return {}, false
		}
		if nibbles % 2 == 0 { id[at] = v << 4 } else { id[at] |= v; at += 1 }
		nibbles += 1
	}
	return id, true
}
