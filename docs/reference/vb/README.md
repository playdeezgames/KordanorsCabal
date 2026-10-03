# Reference frames from the original VB.NET game

Recorded by `tools/vb-oracle` (task 23 of the Odin port, see `PORT.md`). The oracle drives the original game's UI
state machine without a window and writes, for each scene in `scenes.txt`:

| File | Content |
|------|---------|
| `NAME.txt` | the UI state and a text view of the 22 x 23 cell screen (a block character `#` is a solid cell; box-drawing glyphs are shown as their closest text character) |
| `NAME.cells` | the exact cells: one line per row, each cell `glyph.hue.inverted` using the VB `Pattern` and `Hue` enum numbers |
| `NAME.png` | the whole 208 x 240 frame, 2x wide for the VIC-20 pixel aspect, 3x scale |
| `NAME.hash` | FNV-1a 64 over the R, G, B bytes of the unscaled 208 x 240 frame, for bit-exact comparison of another renderer |

Scenes that depend on the randomly generated world (which enemy is met, what is in the shops' stock, the character's rolled
stats) differ from run to run; the text, layout and colors of the screens do not. `_log.txt` lists what was done for each
snapshot. UI states captured: 30 of 32 (not captured: `EquipmentDetail`, `ShoppeRepair`).

Re-record with: `dotnet run --project tools/vb-oracle -- <workdir> capture docs/reference/vb/scenes.txt <outdir>`, where
`<workdir>` is a directory that contains a copy of `src/KordanorsCabal/boilerplate.db`.
