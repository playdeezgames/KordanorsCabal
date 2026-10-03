# Dead code and defects found in the VB game

Collected during the Odin port (PORT.md, tasks 6 to 30). Rule D13: do not port what the game does not need; fix defects
instead of copying them. This list is the record of what was dropped or fixed and why. "Confirm" marks items where only the
author can say whether the thing was meant to exist.

## Not ported (dead or unused)

| What | Evidence | Decision |
|------|----------|----------|
| Item event 5, "add to inventory" (`ItemEvents.AddToInventoryEventId`, call in `Inventory.Add`) | no row in `ItemTypeEvents` uses event id 5 | dropped |
| Commands `Yellow`, `Start`, `Back`, `NextItem`, `PreviousItem` | mapped to keys in `CommandUtility`, handled by no screen | dropped; `Green` and `Blue` merged into `Confirm`, `Red` is `Cancel` |
| `DateTimeOffset.Now` start/elapsed lines in `World.CreateDungeonLevel` | values never used | dropped |
| `Renderer.BorderHue` / `ScreenHue` as settable properties | never assigned; always Cyan and White | constants |
| Options screen "Screen Size" | window scale; the web page scales itself | dropped |
| `KordanorsCabal.Scaffolder`, `KordanorsCabal.Descaffolder`, `boilerplate.db` as a runtime file, `_boilerplate.db` | SQL is gone (D1); `_boilerplate.db` is an older schema | dropped; `boilerplate.db` stays only as the source for the content generators and the oracle |
| All `KordanorsCabal.Data.Tests` (155 facts) and `KordanorsCabal.Game.Tests` (245 facts) | mock-interaction tests of a data layer that no longer exists | not ported (D22) |
| The interface/wrapper layer (`I*` interfaces, `BaseThingie`, `SubcharacterBase`, ~45 view classes) | views over IDs, mirrors for mocking | replaced by plain procedures over the world (D9) |
| Static UI state hooks (`Get/SetCurrent...` delegates, `PushUIState`/`PopUIState` delegates, `ModeProcessor.Buttons` statics) | wiring between `Root` and the screens | replaced by an explicit UI struct |
| 17 members never referenced outside their declaration: `AllDungeonLevels`, `CombineGenerator`, `DecayActionName`, `LoadContent`, `MakeBooleanGenerator`, `MakeDictionary`, `MakeHashSet`, `MakeList`, `MaximumRoll`, `Primitive`, `PurifyActionName`, `QuestDescriptors`, `ReadForCharacterType`, `ReadForStatisticValue`, `ReadValues`, `UseActionName`, `ValidateDice` | static scan of all non-test VB source (declarations vs uses, interface and implementation counted together) | not ported |
| Dice syntax beyond `XdY` (`+`, `*`, `/`, negative counts) in `RNG.RollDice` | every dice string in the content is plain `XdY` | `Dice{count, sides}` only |

## Content that cannot be obtained (confirm)

Three item types exist in the content and have display entries, but no spawn rule, loot table, shop, bribe, lock or code path
ever creates them:

- **Membership Card** (id 32): only *checked* by the constable's screen (`ConstableModeProcessor`), never given.
- **Ring of HP** (id 46) and **Amulet of DEX** (id 49): equipment with a stat buff and a UI glyph, never obtainable.

Default: keep the three rows (they cost nothing) and port the constable check; do not invent a way to obtain them.
**Deferred by the owner (review 5): decide when finishing the game.** Proposal on file: Amulet of DEX as a 1d1 spawn on Level II (the one level without an amulet), Membership Card at weight 2 in the Malcontent loot table (about 13 cards over a game), Ring of HP as a 1d1 spawn on the Moon.
All other item types, character types, statistics, route types, spells and quests are reachable.

## Defects (fixed in the port rather than copied)

| Defect | Where | Port behaviour |
|--------|-------|----------------|
| Rotten food keeps the fresh food's events and stats, so it never rots further, eats as fresh food and never spawns rats | `FoodDecay` writes only the item's type column; per-item copies of stats and events stay | everything derived from the current type (D15) |
| Continue / Save screens throw (`no such table: Players`) when a save slot file is missing or empty | slot check loads each slot file into the live world | slot existence is a storage lookup (D14/D15) |
| Load Game slot labels stay stale for the rest of the session | `Validated` flag never reset on the Load screen | labels come from stored summaries |
| Pending messages survive a new game or load | static queue not cleared | queues live in the world and are reset (D18/D13) |
| A sound is played from inside the draw routine of the message screen | `MessageProcessor.UpdateBuffer` | sound raised when the message first appears |
| The elemental orb flickers because drawing calls the game random generator | `ElementalOrbUIDescriptor.DisplayHue` | cosmetic function of the frame counter, separate from the game generator |
| Null reference when a fight item is used with no enemy present (Fire/Earth/Water Shard, Bottle, Holy Water) | handlers assume the `Can...` check ran | handlers check and say "You cannot use that now!" |
| `ReadNote` throws when all 25 lore texts are taken | `RNG.FromEnumerable` on an empty set | falls back to a random lore |
| Unmapped characters in text throw `KeyNotFoundException` | `PatternBuffer.WriteText` | drawn as a space |
| `World.Start` can throw if a spawn candidate list is empty or a `Single()` assumption fails (one cellar, one town square) | generation | asserted in tests; generation invariants (task 22) |
| Popping an empty UI stack throws | `Root.PopUIState` | defined fallback to the in-play screen |
| Combat and shop messages lose words at the 22-column wrap (hard wrap inside words, e.g. `6 H`/`P!`) | `MessageProcessor` | kept as is: it is how the game looks (reference frames); word wrap is a possible later polish |
| Changing a statistic while wearing a buffing item writes the buffed value back, so an amulet's bonus becomes permanent | `CharacterStatistics.ChangeStatistic` reads `GetStatistic` (base plus buffs) and stores the sum | changes apply to the base value only; buffs are added when read (port step 4) |
| `ChangeStatistic` on a statistic the character has no row for does nothing | same method; every real character has a row (possibly 0) for the statistics that change | the port keeps all statistics in a full array, so no such case exists |
| Red in Turn or Move mode leaves the saved button position on the stack | `ModeProcessor` push without pop | Red restores it like Cancel (port step 3) |
| The Earth Shard does nothing: counter attacks test the *player's* immobilization instead of the enemy's, so an immobilized enemy still hits | `CharacterPhysicalCombat.DoCounterAttack` / `IsImmobilized` | the enemy's own immobilization is tested and counted down (port step 5) |
| Running changes the direction the player faces, even when the run fails | `CharacterPhysicalCombat.Run` assigns `Movement.Direction` before testing the move | kept (the player turns toward where they tried to run) |

## Content questions for the owner (found while porting items; nothing was changed)

- **Amulet of STR has no buff in the data** (the other amulets have +1 to their statistic); it spawns on Level IV and does nothing when worn.
- **Lotion never runs out**: the Pr0n Scroll code wears the lotion down and replaces an empty one with an empty bottle, but the Lotion item type has no maximum durability, so the wear does nothing and the "You ran out that bottle" branch can never run. Ported as it behaved.

## Reachability checks that came back clean

- Every statistic type is referenced by code or content; every route type is used or part of a lock; all character types are
  spawned or referenced by code (`N00b`, `Rat`); the 25 lore texts match the 25 notes spawned per game.
