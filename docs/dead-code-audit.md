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

## Reachability checks that came back clean

- Every statistic type is referenced by code or content; every route type is used or part of a lock; all character types are
  spawned or referenced by code (`N00b`, `Rat`); the 25 lore texts match the 25 notes spawned per game.
