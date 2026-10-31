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

### Experiment: spreading the gear (2026-10-29, task #313)

Changed the spawn table so the kit is found over the levels instead of on levels I and II (daggers I-II, shields and helmets I-III, shortswords II-III, chainmail II-III, brodesodes III-IV, platemail IV-V; total gear found rises a little so broken pieces can be replaced). Then tried higher smith prices (chainmail 120 or 150, brodesode 220 or 300, platemail 500 or 600) in the simulator only.

| Run | Dies on level I / II / III / IV / V | Kit at entry to level II | Finish alive |
|---|---|---|---|
| Before (baseline) | 3 / 3 / 10 / 7 / 3 % | chainmail 99%, brodesode 96% | 75% |
| New spawn table | 6 / 6 / 11 / 5 / 2 % | chainmail 99%, brodesode 97% | 73% |
| + prices 120 / 220 / 500 | 4 / 1 / 5 / 1 / 3 % | chainmail 99%, brodesode 52% | 86% |
| + prices 150 / 300 / 600 | 5 / 8 / 3 / 2 / 5 % | chainmail 96%, brodesode 3% | 80% |

**Finding: gear is not the lever.** The player buys the kit with the money of level I (706 gold of kills), not from the floor, and even when the shop is made much dearer the careful player is no safer or less safe than before: differences are within noise (80 to 150 runs, a few percent). A careful player skips what is dangerous, so what he wears matters little. Gear spread is kept because it makes the dungeon feel less front-loaded and replaces broken pieces, but difficulty has to come from the monsters (#314) and from attrition (#316), the two things a careful player cannot walk around. The prices were not changed.

## Experiment: a cooldown on the healer (2026-10-29)

`heal_cooldown` in `tools/analysis/campaign.py` (steps between two free heals; a room costs 6 steps, a town trip 24). 150 runs each, share of runs that die on the level:

| Cooldown | I | II | III | IV | V | Finish alive |
|---|---|---|---|---|---|---|
| 0 (before) | 6% | 6% | 11% | 5% | 2% | 73% |
| 40 steps | 10% | 9% | 16% | 12% | 8% | 56% |
| 80 steps | 15% | 18% | 14% | 5% | 4% | 55% |

Targets: I 10-20, II 8-15, III 10-18, IV 12-22, V 20-30. A cooldown of 40 puts levels I to III inside the targets and IV nearly; V is still low until the monsters scale (#314). 80 overshoots II and the extra pressure just sends the player home sooner. The game uses 40 (`HEAL_COOLDOWN`).

## Experiment: scaling the monsters (#314, 2026-10-30)

Runs of 100 (60 for the first), 40-step heal cooldown. Share of runs that die on the level:

| Scenario | I | II | III | IV | V | Finish alive |
|---|---|---|---|---|---|---|
| Cooldown only (before) | 10% | 9% | 16% | 12% | 8% | 56% |
| Heavy shift to elites/acolytes/bishops/priests (II to V) | 9% | 29% | 22% | 0%* | 0%* | 50% |
| Same plus mental attacks on bishops, priests, leaders | 7% | 46% | 67% | -* | -* | 17% |
| Milder shift | 8% | 14% | 10% | 10% | 3% | 62% |
| Milder shift, more IV and V | 6% | 9% | 8% | 10% | 3% | 69% |
| Same, bishop and priest damage limit 4, bishop attacks mentally half the time (**applied**) | 3% | 12% | 15% | 8% | 3% | 64% |

\* Too few runs got there, or the careful player skipped to the boss.

Findings: levels II and III respond to the monster mix and are now on target. IV and V do not: a careful player leaves dangerous rooms alone and goes to the boss, so more elites and bishops barely change his odds, and level V deaths do not happen at the boss either (Kordanor's room). Mental attacks barely register for a policy that avoids panic rooms but are lethal when forced (66% dead on III with every high-INF type mental). The lever for IV and V is the bosses and what stands between the key and the boss (#320, #321), not more trash. Also fixed: a panic loop in campaign.py (run counts as dead after 4 panics on one level).

Applied: spawn counts for goblin, elite, acolyte, bishop, priest on II to V; Bishop and Priest damage limit 3 to 4; Bishop gains a mental attack (weight 1 of 2).

## Experiment: boss variety (#320, 2026-10-31)

Each boss room is now its own type, so a boss can differ from the trash that fills the level, and has guards (new type Cult Guard, spawned only in boss rooms):

| Level | Boss | Guards | Trick |
|---|---|---|---|
| I | Acolyte Warden: STR 7, DEX 3, HP 5 | 1 | tougher than any acolyte |
| II | Bone Priest: STR 6, INF 4, HP 4 | 2 | mental attack one time in four |
| III | Flame Bishop: INF 6, HP 5, damage limit 4 | 2 | mental attack half the time |
| IV | Cabal Leader: STR 9, HP 8 | 4 | mental attack one time in four |
| V | Kordanor: STR 10, HP 14, damage limit 5 | 6 | the hardest fight of the dungeon |

250 runs of the campaign model, share dying on each level (last full run, with one more guard on I and a Bone Priest of HP 5, II and I slightly easier now):

| I | II | III | IV | V | Finish alive |
|---|---|---|---|---|---|
| 20% | 20% | 19% | 18% | 10% | 38% |

Targets: I 10-20, II 8-15, III 10-18, IV 12-22, V 20-30; the finish rate they imply is about 41%. 53% of level I deaths and 70 to 90% of IV and V deaths now happen at the boss, which is the point: the bosses are the set pieces. Noise between runs of the same settings is several points. Level V is still under its target; level identities (#321) are the next lever. Applied after that run: one guard on level I instead of two, Bone Priest HP 5 to 4. Not re-simulated.

## Experiment: armour raises the defence cap (#315, 2026-10-31)

The cap on the player's defence hits (Base Maximum Defend) was 1 for the whole game, so every armour die past about three was wasted. Armour can now raise it through an item buff (data only, no engine change): Chainmail +1, Platemail +2. `def_Chainmail` and `def_Platemail` in `campaign.py` model it. 150 runs, share dying on each level:

| Setting | I | II | III | IV | V | Finish alive |
|---|---|---|---|---|---|---|
| No growth (250 runs, boss run) | 20% | 20% | 19% | 18% | 10% | 38% |
| Chainmail +1, Platemail +2 (**applied**) | 7% | 8% | 11% | 19% | 11% | 55% |
| Platemail +1 only | 11% | 14% | 19% | 13% | 6% | 50% |
| Platemail +2 only | identical to the line above: the model player never wears platemail | | | | | |

Findings: chainmail is the item that matters (it arrives on level II); the model player rarely buys platemail, so its cap bonus cannot be judged here. Levels I and II differ between rows by noise alone (the cap does not touch them before chainmail). With the growth the model player finishes 55% of runs against about 41% implied by the targets, so the late game is somewhat easier again; the levers left are the level identities and the wandering threat (#321, #322) and a live playtest.

## Retune after defence growth (2026-10-31)

150 runs each, with chainmail +1 and platemail +2, share dying on each level:

| Candidate | I | II | III | IV | V | Finish alive |
|---|---|---|---|---|---|---|
| Before retune | 7% | 8% | 11% | 19% | 11% | 55% |
| Warden HP 6, Kordanor HP 16 STR 11, 8 guards on V | 11% | 5% | 12% | 17% | 7% | 57% |
| Warden HP 6, Kordanor HP 18 STR 12, 8 guards, 55 bishops and 60 priests on V (**applied**) | 14% | 9% | 15% | 11% | 20% | 47% |
| Warden with 2 guards, Kordanor HP 16 STR 11, 8 guards | 15% | 9% | 14% | 19% | 15% | 46% |

The applied set puts every level inside its target except IV, one point under; the model player finishes 47% of runs against about 41% implied by the targets, and level V now kills 20%. Remaining difference is within the noise of the runs; a live playtest is the next judge.
