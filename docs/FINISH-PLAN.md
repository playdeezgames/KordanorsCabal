# Plan to finish Kordanor's Cabal

Written 2026-10-28 by cabalist. Built from `docs/TODO.md`, `docs/difficulty-analysis.md` and `docs/game-reference.md`. Not playtested live: the analysis already quantifies the ramp, and a live playtest is more useful after the retune (see "Playtest loop"). Tasks live in coxswain, project `kordanors-cabal`.

## Diagnosis

The tension comes from one thing: **a starter with no gear is in danger**. Everything that makes the later game safe is cheap and unlimited.

| Cause | Evidence |
|---|---|
| Gear is dumped on levels I and II | 14 daggers, 10 shortswords, 3.5 chainmail, 7 brodesodes, 3.5 platemail by the end of II; kill money buys the whole late kit mid level II |
| Healing is free and instant | Marten heals everything for nothing; 10 potions per level |
| Monsters do not scale | Levels I to III differ by about 0.1 HP per fight; strength only steps up at IV and V |
| Defence is capped at 1 | Armour dice past about 3 are wasted, so loot stops mattering |
| Stats barely move | 7 points in the whole game; Influence, Willpower, MP almost never matter |
| Bosses I to III are ordinary creatures | Same stats as the trash that fills later levels |
| Nothing new happens in II to IV | Same loop (find key, open dead end, kill, loot, go home to heal) on every level; no new mechanic, no story beat, no goal that is visible |
| No ending | The orb is never made; Kordanor and the horns are unreachable |

So the sag is two problems at once: **danger drops** (economy and scaling) and **novelty drops** (no mid-game hooks). Fix both, and give the player a goal to walk toward (the four shards and the twist).

## Design target

A tension curve to tune against. Death chance per occupied room for a player carrying the kit that fits the level (kits A to D from the analysis):

| Level | Expected kit | Target death/room | Target HP lost per room | Feel |
|---|---|---|---|---|
| I | A to B | 5 to 10% (starters still risky) | 1 to 2 | scrappy, careful |
| II | B | 6 to 10% | 1 to 2 | first real choices (retreat or push) |
| III | B to C | 6 to 10% | 2 | mental pressure appears |
| IV | C | 8 to 12% | 2 to 3 | resource drain matters |
| V | C to D | 10 to 15% | 3 | deadly; a bad room can end a run |
| Moon | D | 10 to 20% | 3 to 4 | optional, lethal |
| Bosses | | each stands out: 15 to 30% at the right kit | | set pieces |

Today the equipped numbers are 0 to 5% at every level, so the middle is 3 to 10 times too safe. The simulators are the gate: `tools/analysis/difficulty.py` (single fights) and `tools/analysis/campaign.py` (whole levels with carried HP, wear, money, potions, healing).

**Correction (2026-10-29).** A per-room death chance of 5 to 15% cannot be a target: a level holds 60 to 100 fights, so it would kill every run. The campaign model measures *the chance that a level kills the player*, and its targets are I 10-20%, II 8-15%, III 10-18%, IV 12-22%, V 20-30%. **These are not calibrated to real play.** The model player is mechanical (no spells, no intimidation, imperfect judgement), and its baseline says the opposite of the owner's experience for levels II to V (see `docs/difficulty-analysis.md`, "Campaign model"). The live playtest (#325) must calibrate it before the targets are trusted; until then the model is for comparing changes, not for absolute numbers.

## Phases

Order matters. Make the game completable first, because the ending defines what the difficulty is climbing toward. Then restore tension, then add mid-game interest on top of a game that is hard enough to be worth adding to.

### Phase 1: Completable (the spine)

1. **Fire Shard on Level III.** Give it `Dungeon_Boss` in `boilerplate.db`, regenerate. Also fix the other broken spawn rules (Amulet of Mana V has no place, Amulet of Mana II has no count).
2. **Make the Orb.** Elder button *Make the Orb* when the pack holds all four shards; the four are consumed, an Elemental Orb appears, with a message. New Action and Check in the content enums, `events.odin`, `ui_town.odin`.
3. **Elder as quest tracker.** His *The Cabal* speech changes with progress (no shards, N of 4, orb made, horns in hand). This is the visible goal from level I, and it costs only text.
4. **The ending.** Hand over the Horns: the twist (Zooperdan, not the cult, is the villain), then the Zooperdan fight in town as the final boss using normal combat (new character type, big stats, several phases if cheap, a parting shot), victory screen, credits, return to title. Decide: can the player refuse? Recommend yes, two endings, because refusing is cheap (a different message page) and replay value is real.
   - **Decided 2026-10-28 (owner):** two endings, gated by lore read; the final fight has two phases and no escape.
     - *Hand over the horns* (always offered): Zooperdan drops the mask, takes the horns and fights at full power. Bad ending: Kordanor's prison is open, epilogue says so even if you win.
     - *Refuse* (offered only after the player has read at least N Notes, N to be tuned, about 10 of 25): the Elder reveals himself without the horns and fights weaker. Good ending: you inherit the vigil.
     - Same Town Square fight both ways, so the difference is one stat block, one epilogue text and a lore counter. No running and no shops during it; items and spells work.
     - Phase 1 is physical. At half HP phase 2 adds mental attacks (high Influence), so MP and Willpower finally matter. Starting numbers against kit D: about 14 HP, STR 9, DEX 4, WIL 4 (weaker variant roughly 10 HP, STR 7); tune with the simulator.
     - Death in the fight uses the normal death flow. Victory shows the epilogue, credits, then the title; the slot is flagged completed. Not decided yet: whether Kordanor can be spoken to, and whether new game keeps anything.
5. **Write the 18 lore texts** (#8 to #25) as breadcrumbs, 3 to 4 per level, so each level adds a piece of the reveal (see Phase 3).

### Phase 2: Restore tension (economy and scaling)

Tune in this order; re-run the simulator after each.

1. **Spread the gear.** Chainmail and shortswords off level I; brodesode on III to IV; platemail on IV to V. The kit then grows with the dungeon instead of being finished by mid II.
2. **Scale monsters.** Elites and acolytes on level II, bishops on III, Cabal-Leader-style mental attacks (high Influence) on IV and V, damage limits above the player's reduction on strong creatures. Give the bosses a real jump.
3. **Let defence grow.** Raise Base Maximum Defend with armour tier or with levelling (one extra point per two levels is enough). Armour loot starts to matter again.
4. **Attrition.** Marten's heal costs money (scaling with level or wounds) or has a cooldown measured in dungeon steps; fewer potions (10 to 4 per level); hunger bites sooner (about 60 steps per HP). Repair costs bite harder as items tier up. Mana and MP must also gate things: panic at 0 MP should hurt.
5. **Money sinks.** The Moon Portal (5,000) is the only big sink and kill money reaches 6,500 total. Add sinks that matter: Marten, repairs, a high-end stock at Samuli that only appears after the Fire Shard (for example Platemail+), Honest Dan's rare goods. Money should be tense mid game and interesting late.
6. **Take the early cliff off, narrowly.** A dagger and a few coins at the start, or weaker level I dead-end groups, so the opening stays tense without being a coin flip. Do this last and sparingly: the user likes the opening.
7. **Make stats matter.** Mental attackers on III to V make Influence, Willpower and MP decide fights; more level-up points (cheaper XP curve, not doubling every level) so builds diverge.

### Phase 3: Mid-game interest

The aim is that every level has an identity, a new decision, and a story beat. Proposed shape:

| Level | Theme | New wrinkle | Story beat |
|---|---|---|---|
| I | Cultists' outskirts | Teaches the loop; boss: Acolyte guarding the Air Shard | lore: the kin's vigil |
| II | The crypt | Undead heavy (zombies, skeletons). Holy Bolt and Purify earn their price. Skeletons cannot be intimidated, so the player must change tactics. Earth Shard boss with a guard | lore: Z's epistle, the warning |
| III | The mind | Malcontents and bishops hit MP. Intimidation and Willpower matter. Panic is a real threat and costs money. Fire Shard boss: Bishop with a guard (finally gives the shard a home) | lore: the prison is not a prison |
| IV | The hunt | Wandering enemies (see below), harsher attrition, Cabal Leader as a proper boss with mental attacks. Water Shard | lore: who is Z |
| V | The deep | Few but lethal bishops/priests, doors that matter, Kordanor behind the Orb | the choice |

Mechanics to add, in order of value for cost:

1. **Shards as active powers**, not only quest parts. Each is usable once per visit or with a cost, and each boss rewards you with its power before you need it for the Orb. The Earth Shard already uses POW. Give Air (flee safely from one fight), Fire (burst damage), Water (partial heal) a role. The player must decide whether to spend a shard or keep it for the Orb, since the Orb needs all four at once. That is a real mid-game dilemma at no engine cost beyond new actions.
2. **Wandering threat.** A dungeon clock: every N steps in a level, a creature appears in a corridor room you have already cleared, or a pack walks toward the stairs. Stops free farming and retreat-to-heal loops, and gives the empty levels something to do. Tie N to level.
3. **Boss variety.** The first three bosses get HP, a guard, or an ability: Acolyte calls an ally, Priest heals itself or casts mental attacks, Bishop spawns acolytes. Each boss must feel different from the room before it.
4. **Make the unobtainable items obtainable and use them as rewards.** Membership Card (David's bounty, from malcontents), Ring of HP, Amulet of DEX, Amulet of STR (give it a buff) as boss drops or a rare chest: a loot chase between the shards.
5. **Elder side quests**, one per level, text only plus a reward: bring N snake fangs, clear the crypt of a named skeleton, rescue a townsperson (use the Moon people). Pays in stat points (this also fixes the scarcity of points) or the defence cap.
6. **Town reacts.** After each shard, a townsperson changes: new stock, new line, a rumour that hints at the twist. Cheap, and it makes going home mean something other than healing.
7. **The Moon as an optional endgame trial.** Already the deadliest zone; reward it (the best loot, a lore cluster).

Stay within what exists. No new screens until the first four are in.

### Phase 4: Finish and ship

1. Credits screen: sound sources and the Abundant Music line (seed 2645320710).
2. Lotion durability.
3. Real-device pass (Android Chrome, iPhone Safari), iPhone music (m4a), continue speed.
4. itch.io draft page and iframe test, then ship.
5. Decide on removing `src/` and `tools/vb-oracle`.

## Playtest loop

1. After each Phase 2 change, re-run `python3 tools/analysis/difficulty.py` and compare with the target table.
2. Extend the simulator for what it ignores: weapon wear, hunger, healing cost, potions, mental attacks. Without these, attrition changes cannot be judged.
3. Use the existing soak test for crashes and timing after each content regeneration.
4. A live playtest (browser, `tools/serve.sh`) after Phase 2 and again after Phase 3, with fixed seeds so a run can be repeated. A short checklist: does the first hour stay tense, does level III hurt without full armour, does the player spend or hoard shards, is there a run-ending moment on V.

## Risks

- **Over-correcting the opening.** The user likes it; change it last.
- **Mid-game hooks that trivialise combat.** Shard powers must cost something.
- **Lore buffer.** The longest text must stay under 768 bytes.
- **Content regeneration.** Edit `boilerplate.db` and run `tools/gen/gen_content.py`, never hand-edit generated files.
- **Ending scope creep.** Ship a single fight and a text epilogue first.

## Open questions for the owner

1. Two endings (hand over or refuse) or one?
2. Is a wandering threat acceptable (it breaks the "cleared room stays clear" assumption)?
3. How hard should free healing be limited: money, cooldown, or both?
4. Do you want the early game left exactly as it is?
