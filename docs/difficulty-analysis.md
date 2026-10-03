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
