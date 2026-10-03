package game

// The 22 x 23 cell screen and its software rasterizer. A port of SPLORR.UI (Hue, PatternCell, PatternBuffer, Renderer):
// verified bit for bit against the VB renderer on 54 recorded screens (tests/reference_test.odin).

Hue :: enum u8 { Black, White, Red, Cyan, Purple, Green, Blue, Yellow, Orange, Light_Orange, Pink, Light_Cyan, Light_Purple, Light_Green, Light_Blue, Light_Yellow }

// Palette from HueUtility.vb, stored as 0xAABBGGRR so the bytes in memory are R, G, B, A.
PALETTE := [Hue]u32{
	.Black = 0xFF000000, .White = 0xFFFFFFFF,
	.Red = 0xFF262D77, .Cyan = 0xFFDCD485, .Purple = 0xFFB45FA8, .Green = 0xFF4A9E55, .Blue = 0xFF8B3442, .Yellow = 0xFF71CCBD,
	.Orange = 0xFF4A73A8, .Light_Orange = 0xFF87B2E9, .Pink = 0xFF6268B6, .Light_Cyan = 0xFFFFFFC5, .Light_Purple = 0xFFF59DE9,
	.Light_Green = 0xFF87DF92, .Light_Blue = 0xFFCA707E, .Light_Yellow = 0xFFB0FFFF,
}

Cell :: struct { glyph: u8, hue: Hue, inverted: bool }
Screen :: [CELL_ROWS][CELL_COLUMNS]Cell

GLYPH_SPACE :: 32 // the Space glyph's index in the VB Pattern enum

glyph_bitmaps := GLYPHS
ascii_glyph := ASCII_GLYPH

// Lowercase letters use the uppercase glyphs; characters without a glyph show as a space.
glyph_of :: proc(ch: u8) -> u8 {
	if ch < 128 && ascii_glyph[ch] != 255 { return ascii_glyph[ch] }
	return GLYPH_SPACE
}

screen_fill :: proc(s: ^Screen, glyph: u8, inverted: bool, hue: Hue) {
	for r in 0 ..< CELL_ROWS { for c in 0 ..< CELL_COLUMNS { s[r][c] = {glyph, hue, inverted} } }
}
fill_cells :: proc(s: ^Screen, col, row, w, h: int, glyph: u8, inverted: bool, hue: Hue) {
	for r in row ..< min(row + h, CELL_ROWS) { for c in col ..< min(col + w, CELL_COLUMNS) { if r >= 0 && c >= 0 { s[r][c] = {glyph, hue, inverted} } } }
}
// Writes text left to right; like the VB WriteText it wraps to the next row at the right edge and to the top at the bottom.
write_text :: proc(s: ^Screen, col, row: int, text: string, inverted: bool, hue: Hue) {
	c, r := col, row
	for i in 0 ..< len(text) {
		s[r][c] = {glyph_of(text[i]), hue, inverted}
		c += 1
		if c >= CELL_COLUMNS { c = 0; r += 1 }
		if r >= CELL_ROWS { r = 0 }
	}
}
write_text_centered :: proc(s: ^Screen, row: int, text: string, inverted: bool, hue: Hue) {
	write_text(s, (CELL_COLUMNS - len(text)) / 2, row, text, inverted, hue)
}

// Port of Renderer.Update: border hue Cyan, screen hue White, bit 0 of each bitmap byte is the leftmost pixel.
rasterize :: proc(s: ^Screen, frame: ^[FRAME_WIDTH * FRAME_HEIGHT]u32) {
	border, paper := PALETTE[.Cyan], PALETTE[.White]
	for y in 0 ..< FRAME_HEIGHT {
		for x in 0 ..< FRAME_WIDTH {
			sx, sy := x - BORDER_X, y - BORDER_Y
			if sx < 0 || sy < 0 || sx >= CELL_COLUMNS * CELL_SIZE || sy >= CELL_ROWS * CELL_SIZE {
				frame[y * FRAME_WIDTH + x] = border
				continue
			}
			cell := s[sy / CELL_SIZE][sx / CELL_SIZE]
			bit := (glyph_bitmaps[cell.glyph & 127][sy % CELL_SIZE] >> uint(sx % CELL_SIZE)) & 1 == 1
			frame[y * FRAME_WIDTH + x] = (bit != cell.inverted) ? PALETTE[cell.hue] : paper
		}
	}
}
