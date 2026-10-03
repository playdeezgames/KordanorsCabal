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

## Story and ending (design notes from the owner, 2026-10-03; not built)

- **The twist.** When the player gives the **Horns of Kordanor** to the Elder, they learn that the cult was **not keeping Kordanor captive; it was keeping him safe from Zooperdan** (the Elder, the quest giver). Giving the horns to Zooperdan turns him into the **final boss**, and the player must fight him.
- **Fits the existing text.** Lore #1 says the kin kept vigil "not to keep the captive within, but to keep those that seek his release without". Lore #7 (*Z's Epistle*, signed "-Z": "I have arrived... I cannot enter yer realm for your many wards of protection, but I will find a way to send another to fetch what I need") is Zooperdan, who sent the player. Lore #5 (*Keep him in prison, for the one seeking his death shall overtake all*) and #6 point the same way. The Elder's *The Cabal* speech ("slay this foul fiend and bring to me his horns") is the lie. The 18 missing lore texts are the natural place to unfold the reveal.
- **How it could use what exists (ideas, not decisions):** the fight uses the ordinary combat system: a new character type *Zooperdan* (enemy: the player, big stats, a sprite, a parting shot), created in the town square when the horns are handed over; the Elder's feature stays, and his *Interact...* turns into the fight while he stands there. Because the Elder is the one place the player must come back to, the whole ending happens in town.
- **Open questions:** Zooperdan's stats and whether he has several phases; what the player learns in the dialogue and whether there is a choice (hand over or refuse); the victory and defeat screens; what happens to the town afterwards (a credits roll, then back to the title?); whether Kordanor, if spared, should be able to be spoken to; whether the player can still wander the dungeon after the horns are given.

## Difficulty and balance (ideas from `docs/difficulty-analysis.md`; design only, nothing decided or built)

Finding: the ramp is front-loaded and then flat (a starter is in danger, gear removes the danger by mid level II, monster strength only rises at levels IV and V, the first three bosses are ordinary creatures). Ideas:

- [ ] **Spread the gear across the levels.** Move chainmail and shortswords off level I; keep brodesodes to levels III to IV and platemail to IV to V, so the kit grows with the dungeon.
- [ ] **Scale the monsters.** Put elites and acolytes on level II, bishops on level III; give levels IV and V the Cabal Leader's mental attacks (high Influence); give strong creatures damage limits above the player's damage reduction.
- [ ] **Let defence grow.** The player's defence is capped at 1 for the whole game (Base Maximum Defend), so armour dice past about three are wasted; raise the cap with a stat, with armour, or with levels.
- [ ] **Make bosses different.** The bosses of levels I to III are single ordinary creatures; give them more HP, a guard or an ability. The Cabal Leader and Kordanor already feel like bosses.
- [ ] **Vary the density per level.** A quiet first level, the busiest in the middle, a few dangerous creatures at the bottom (levels II and III are densest now, level V emptiest).
- [ ] **Add attrition.** Limit free healing (a cost or a cooldown), fewer potions, or hunger that bites sooner.
- [ ] **Take the early cliff off.** A better start (a dagger and a few coins), or weaker level I dead-end groups; a starter with a dagger loses a quarter to a half of the rooms.
- [ ] **Make statistics matter.** Points are scarce (7 level-ups in a whole game) and only Malcontents use mental attacks, so Influence, Willpower and MP rarely decide anything.
- [ ] **Re-run the analysis** (`python3 tools/analysis/difficulty.py`) after any of the changes above.

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
