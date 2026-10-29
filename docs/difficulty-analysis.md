# Difficulty ramp of the dungeon levels

An estimate made on 2026-10-03 from the game's own rules and data, by simulation (`tools/analysis/difficulty.py`; re-run it after changing
the content). Design notes only: nothing in the game was changed.

## Method and assumptions

- Fights use the real rules: a die counts on a six; damage = attack hits - defence hits; the **player's defence is capped at 1** (Base
  Maximum Defend 1, and no way to raise it); damage is capped by the worn items' limits (dagger 1, shortsword 2, brodesode 3, chainmail +2);
  every enemy in the room counter-attacks after each player attack.
- Levels are rebuilt as the game builds them: 11 x 11 maze (about 42 dead ends and 78 corridor rooms), each creature type placed where its
  spawn rule allows (dead ends hold badgers, elites, malcontents; corridors the rest; the boss room one boss).
- Four player *kits* stand for how the character might look, because stats barely grow (see below) and gear is what changes:
  **A** starter: dagger, no armour, STR 4, DEX 2, HP 5. **B** early: shortsword, shield, helmet, STR 5, HP 6. **C** mid: brodesode, chainmail,
  shield, helmet, STR 6, DEX 3, HP 8. **D** late: brodesode, platemail, shield, helmet, STR 8, DEX 3, WIL 3, HP 10.
- The player fights every occupied room from full HP, never flees, and does not use potions, spells or shards. Weapon breakage, hunger and
  repair costs are not modelled. The kits are assumptions, not measurements of real play.

## What the levels contain

| Level | Creatures | XP | Money | Occupied rooms | Enemies per occupied room |
|---|---|---|---|---|---|
| I | 150 | 213 | 706 | 86 | 1.7 |
| II | 233 | 319 | 1,064 | 94 | 2.5 |
| III | 266 | 460 | 1,316 | 101 | 2.6 |
| IV | 206 | 415 | 1,395 | 95 | 2.2 |
| V | 131 | 445 | 1,372 | 80 | 1.6 |
| Moon | 100 | 300 | 0 | 68 | 1.5 |

New creature types per level: I badgers, malcontents, goblin elites, bats, goblins, skeletons, snakes (acolyte boss); II adds acolytes,
zombies, priests (priest boss); III more acolytes and priests (bishop boss); IV bishops (cabal leader boss); V bishops and priests only
(Kordanor). Almost everything from level II on has STR 6, HP 2 to 4 and a damage limit of 3; only the Cabal Leader, Kordanor and the Moon people
(STR 8, limit 4) are a step up.

## Results

**Average danger of one fight against a creature of the level** (HP lost per solo fight; weighted by how many of each creature spawn):

| Level | Kit A | Kit B | Kit C | Kit D |
|---|---|---|---|---|
| I | 0.89 | 0.28 | 0.13 | 0.09 |
| II | 0.69 | 0.21 | 0.10 | 0.07 |
| III | 0.84 | 0.29 | 0.14 | 0.09 |
| IV | 1.32 | 0.51 | 0.22 | 0.17 |
| V | 2.44 | 1.02 | 0.51 | 0.36 |
| Moon | 4.13 | 2.13 | 1.16 | 0.88 |

**Chance of dying in an occupied room** (all enemies of the room at once, starting at full HP):

| Level | Kit A | Kit B | Kit C | Kit D |
|---|---|---|---|---|
| I | 23% | 4.8% | 1.4% | 0% |
| II | 37% | 9.8% | 1.4% | 0.4% |
| III | 44% | 16% | 5.0% | 0.8% |
| IV | 46% | 15% | 3.6% | 1.2% |
| V | 54% | 19% | 3.8% | 1.0% |
| Moon | 77% | 35% | 8.8% | 2.3% |

**Bosses** (solo; HP lost / chance of dying):

| Boss | Kit A | Kit B | Kit C | Kit D |
|---|---|---|---|---|
| I Acolyte (HP 2) | 2.0 / 11% | 0.6 / 0% | 0.3 / 0% | 0.2 / 0% |
| II Priest (HP 3) | 3.4 / 38% | 1.4 / 2% | 0.7 / 0% | 0.5 / 0% |
| III Bishop (HP 4) | 4.1 / 60% | 1.9 / 4% | 1.0 / 0% | 0.8 / 0% |
| IV Cabal Leader (HP 5) | 4.9 / 93% | 3.6 / 27% | 2.1 / 2% | 1.7 / 0% |
| V Kordanor (HP 10) | 5.0 / 100% | 5.6 / 82% | 4.6 / 20% | 4.1 / 4% |

**Progress of the character.** Killing everything gives 213 / 532 / 992 / 1,407 / 1,852 cumulative XP; with the XP goal doubling each level (10, 20, 40, ...)
that is only **4, 5, 6, 7, 7 level-ups** after levels I to V, and each gives one point. So the character's stats grow by about 7 points in the whole game
on top of the 8 at the start. **Gear** is the real growth: levels I and II alone drop on the floor an average of 14 daggers, 10 shortswords, 10 shields, 10 helmets,
3.5 chainmail, 7 brodesodes and 3.5 platemail, and kill money (706 on level I) pays for the whole of kit D (375) in the middle of level II. No platemail
or brodesode appears after level III, and there are 10 potions per level plus a free healer.

## Verdict

**The ramp is front-loaded and then flat; it is not a good ramp as it stands.**

1. **The hardest stretch is the first minutes** (kit A: a quarter to a half of the rooms are lethal on every level). A new character with a dagger is in real danger from
   the first fights, and a player who does not buy or find gear soon will die.
2. **Gear erases the difficulty by the middle of level II.** From then on the best kits lose about one HP a room and almost never die (0.4 to 1.2% per room, kit D).
   Levels II, III and IV are not harder than level I for an equipped character; the apparent rise in room danger (kit B: 4.8%, 9.8%, 16%) comes only from more enemies per
   room (2.5 to 2.6), not from stronger ones.
3. **Monster strength barely changes until level IV.** Levels I to III differ by about +-0.1 HP per fight; level IV doubles it and level V doubles it again (bishops and priests
   everywhere), which is the only real step. It arrives when the player is already fully equipped.
4. **Level V is the emptiest of the deep levels** (131 creatures, 1.6 per room), after the busiest two (levels II and III).
5. **Bosses of levels I to III are ordinary creatures.** The Acolyte, Priest and Bishop have the same stats as the many Acolytes, Priests and Bishops of the later levels and are
   easier than a pair of them in a room; only the Cabal Leader (27% against kit B) and Kordanor (20% against kit C) feel like bosses, and Kordanor is unreachable until the Elemental Orb exists.
6. **Statistics do not matter much.** Points are scarce (one per level), the player's defence cap of 1 makes armour dice past about three useless, and only the Malcontent uses
   mental attacks, so Influence, Willpower and MP almost never decide anything.
7. **Attrition is weak.** Healing is free and instant, potions are plentiful, and hunger takes about 100 steps per hit point.

## Ideas to improve the ramp (not decided, not built)

- Spread the gear: move chainmail and shortswords off level I, keep brodesode to levels III to IV and platemail to IV to V, so that the kit grows with the levels.
- Make monsters scale: put elites and acolytes on level II, bishops on level III, add the Cabal Leader's mental attacks (high INF) to levels IV and V, and give the strong
  creatures damage limits that exceed the player's reduction.
- Let defence grow (raise Base Maximum Defend with a stat or with armour), otherwise armour dice are wasted.
- Give bosses a real difference: more HP, a guard, or an ability; the first three bosses are single ordinary creatures.
- Make the levels differ in density (a quiet first level, busiest in the middle, a few dangerous creatures at the bottom).
- Add attrition: a limit on free healing (a cost or a cooldown), fewer potions, or hunger that bites sooner.
- Take the early cliff off: a better start (a dagger and a few coins), or weaker level I dead-end groups.


## Campaign model (2026-10-29; `tools/analysis/campaign.py`)

The first model (above) fights each room from full HP. This one plays the dungeon: HP, stress, wear, money, potions and level-ups carry from room to room; gear is placed by the spawn rules and sold or worn; the smith's prices buy upgrades and repairs; the healer is a free full heal; the player retreats to town when hurt, drinks potions and runs when a fight turns, and skips rooms that look too dangerous (leaving them, and their XP and money, behind). Rules under test are in `RULES`; run `python3 tools/analysis/campaign.py [runs]`.

Baseline (120 runs, the game as it is):

| Level | Reached | Dies on the level | Town trips per level | Potions drunk | Money at entry | Kit at entry |
|---|---|---|---|---|---|---|
| I | 100% | 19% | 22 | 3.8 | 0 | none |
| II | 81% | 34% | 31 | 7.8 | 213 | shield, helmet, chainmail, brodesode |
| III | 53% | 67% | 35 | 6.7 | 742 | full kit |
| IV | 18% | 67% | 31 | 6.4 | 1,148 | full kit |
| V | 6% | 57% | 26 | 7.4 | 1,735 | full kit |

What it shows, and what it does not:
- **The free heal is the engine of the game**: 22 to 35 trips to town per level, i.e. the player heals every two or three fights. Attrition does not exist.
- **Money has no use**: 700 to 1,700 unspent by levels III to V, with the full kit bought by the start of level II (platemail is never worth it because the defence cap makes it useless).
- **The kit is complete at level II and stats stay flat** (HP 8 to 10), so everything from III on is the same fight against bigger groups. That is the sag.
- **It disagrees with the owner on the mid game.** The model player dies on levels II to V far more than the target, while the owner finds those levels easy. The reason is probably the player: the model uses no spells (Holy Bolt against the many undead), no intimidation, and flees by the game's random direction rule (half the time it fails and costs a counter-attack). Treat the absolute numbers as a pessimistic bound and use the model for *comparisons* (does gear spreading, attrition, scaling move the table the right way?). Calibrating it against a real playthrough is task #325.

### Calibration (2026-10-29)

The owner reported: almost no deaths after level I; tactics are running to come back healed, Holy Bolt on the undead, and fighting on with potions; the character at the start of level III is character level 4 to 5 with HP 8 to 10 (the model matches that). The first model player was far too reckless (it accepted fights with up to 30% risk and had 25 trials of judgement); a careful, accurate player (`avoid_above` 2%, `push_above` 4%, 100 risk trials, heals below 80% HP) matches the report. Baseline of that player (150 runs, the game as it is):

| Level | Dies on the level | Town trips | Potions | Money at entry |
|---|---|---|---|---|
| I | 3% | 19 | 2.0 | 0 |
| II | 3% | 27 | 3.3 | 211 |
| III | 10% | 30 | 3.7 | 596 |
| IV | 7% | 25 | 1.9 | 885 |
| V | 3% | 17 | 1.1 | 1,271 |

75% finish level V alive. Reading it: the mid game is a walk because **the careful player simply skips what is dangerous and heals for free (17 to 30 trips a level)**, so attrition and tension are absent, and money (up to 1,271 unspent) has no use. Level I is too kind in the model (3%), since the model's starting HP is 6 and the real start is HP 3 plus whatever is left to assign: treat the opening as untested here. The plan's per-level targets (I 10-20%, II 8-15%, III 10-18%, IV 12-22%, V 20-30%) apply to this player.
