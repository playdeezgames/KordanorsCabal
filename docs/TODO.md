# To do

**Mode: design and plan only.** Nothing below is to be implemented until the owner says so; the items are notes for later.

Open work on Kordanor's Cabal, collected from the port. Newest findings first. (`PORT.md` has the full history and the reasons.)

## Content that is unfinished

- [ ] **Lore texts.** 25 lore texts exist (they are the contents of the 25 Notes spawned in every game); only #1 to #7 are written. #8 to #25 are placeholders whose title and text both read `Lore #N`. Source: `Lores` table in `src/KordanorsCabal/boilerplate.db`; after editing, run `tools/gen/gen_content.py` (the longest text must stay under 768 bytes, the message buffer).
- [ ] **The Level V boss room cannot be reached; the Elemental Orb was meant to be crafted.** The door needs the **Elemental Orb**, which nothing creates. Owner's recollection (2026-10-03): it was going to be **crafted from the four shards, probably by the Elder**; the project was abandoned before that was built. The data fits: the Air (Level I), Earth (Level II) and Water (Level IV) Shards spawn in boss rooms, and the **Fire Shard (Level III) has a spawn count but an empty place list, which is very likely the same boss-room rule with the places lost** (Level III's boss room is where a Bishop stands). To finish: give the Fire Shard `Dungeon_Boss` on Level III, add an Elder button (for example *Make the Orb*, shown when the pack holds all four shards) that swaps the four for an Elemental Orb with a message, and decide what the Horns of Kordanor do at the Elder (the ending). Until then Kordanor and the horns are unreachable and there is no ending.
  - **Plan (not started, owner said to only note it):** (1) data: give the Fire Shard `Dungeon_Boss` on Level III in `boilerplate.db` and regenerate; (2) Elder: a new button *Make the Orb*, shown when the pack holds Air, Earth, Fire and Water Shards, which destroys the four and gives an Elemental Orb with a message (a new `Action` and `Check` in the content enums, handled in `events.odin`, button in `ui_town.odin`); (3) the Level V door already spends the orb (route lock rule); (4) **open design question:** the ending, i.e. what handing the Horns of Kordanor to the Elder does (a message page, a score, a restart?).
- [ ] **Items that cannot be obtained** (decision review 5, deferred): Membership Card, Ring of HP, Amulet of DEX. Proposal on file in `PORT.md`.
- [ ] **Amulet of STR** has no buff in the data (the other amulets give +1 to their statistic).
- [ ] **Lotion never runs out** (it has no durability in the data; the code that would use it up never applies).
- [ ] **Spawn rules with a count but no place:** the Fire Shard (Level III, see the orb item above) and the Amulet of Mana (Level V) never appear; the Amulet of Mana (Level II) has places but no count.
- [ ] **Credits.** Add the sound-effect sources and the wording for the music ("generated with Abundant Music", seed 2645320710) to the Credits screen.

## Release

- [ ] **itch.io:** create a draft HTML5 page, upload `build/kordanors-cabal-html5.zip` (`tools/ship.sh`, add `--push` for `butler`), and test in the itch iframe: loading, the start key/tap, sound and music, saves surviving a reload, export download and import picker, fullscreen, the rotate prompt.
- [ ] **Real devices:** Android Chrome and iPhone Safari: finger accuracy on the small menu rows, fullscreen and landscape lock, audio unlock, file transfer.
- [ ] **Theme for iPhones:** provide `MinorTheme.mp3` or `.m4a` (only Ogg exists), and listen to the loop seam.
- [ ] **Continue speed:** loading a 470 KB save takes about 300 ms in wasm on the dev machine (saving 100 ms). If it feels slow on a phone, write a hand-made reader instead of Odin's JSON unmarshal.
- [ ] **Remove `src/` and `tools/vb-oracle`** when you decide the VB reference is no longer needed (they were kept on purpose; decision review 10).

## Ideas that came up

- Word wrap for long messages (the original cuts at column 22 even inside words, kept on purpose for fidelity; decision review 7).
- Slot labels that show level, HP and XP (the save stores a summary for this; the screens still show `Slot N` as in the original).
- A mouse/keyboard hint on the title screen for touch devices.
