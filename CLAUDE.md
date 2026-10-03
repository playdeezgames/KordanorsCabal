# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Kordanor's Cabal is a VB.NET (.NET 6 / netstandard2.1) dungeon-crawler game rendered with MonoGame (DesktopGL) in a VIC-20/PETSCII style. All source lives in `src/` (solution: `src/KordanorsCabal.sln`). `pub-linux/` and `pub-windows/` are committed publish output; don't edit them by hand.

## Commands

Run from `src/`:

```bash
dotnet build KordanorsCabal.sln
dotnet run --project KordanorsCabal/KordanorsCabal.vbproj      # run the game
dotnet test                                                    # all tests (xUnit + Moq + Shouldly)
dotnet test KordanorsCabal.Game.Tests --filter "FullyQualifiedName~WorldShould"   # one class/test
```

`shippit.sh` (repo root) publishes self-contained linux-x64/win-x64 builds to `pub-*`, pushes them to itch.io with `butler`, and commits everything as "shipped it!". That's why many commits have that message and why `pub-*` binaries are tracked.

## Architecture

Layered projects, each depending only downward:

- **SPLORR.Data / SPLORR.Game / SPLORR.UI** – reusable engine-ish libraries (SQLite `Store` wrapper, RNG/maze generation, framebuffer/pattern/sprite rendering). Game-agnostic.
- **KordanorsCabal.Schema** – SQLite schema (`Scaffolder.vb`), seed data (`Populator.vb` plus per-domain populators under `Item/`, `Character/`, etc.), and `Constants/Tables.vb` + `Columns.vb` name constants used everywhere in SQL.
- **KordanorsCabal.Data** – `I*Data` interfaces and implementations over the SQLite store; `IWorldData` is the root. All persistent game state lives in the database.
- **KordanorsCabal.Game** – domain logic. `World.FromWorldData(IWorldData)` builds `IWorld`; characters, items, locations, features, routes are wrapped as "thingies" (`IBaseThingie`) on top of Data interfaces. Events (`Events.vb`) and sfx are raised from here.
- **KordanorsCabal.UI** – game screens/state (`MainProcessor`, `InPlay/`, `Boilerplate/`), sprites, and `StaticWorldData` which opens `boilerplate.db`.
- **KordanorsCabal** – MonoGame host (`Program.vb`, `Root.vb`), content (audio/sprites), and the shipped `boilerplate.db`.

### The boilerplate.db workflow

`boilerplate.db` is a pre-populated SQLite database copied into the game output; at runtime the game works against it (`StaticWorldData`). Two tools regenerate things around it:

- **KordanorsCabal.Scaffolder** (`dotnet run --project KordanorsCabal.Scaffolder -- <path/to/db>`) deletes and recreates the DB using `Schema`'s `Scaffold` + `Populate`.
- **KordanorsCabal.Descaffolder** (run in the directory containing a `boilerplate.db`) does the reverse: reads an existing DB and *generates* `Tables.vb`, `Columns.vb`, `Scaffold.vb`, `Populate.vb` into the current directory, which are then copied into `KordanorsCabal.Schema`. Generated files are overwritten by this process, so prefer changing the DB and re-descaffolding over hand-editing them when the change is a schema/seed one (check `git log` for how prior schema changes were done; `Scaffolder.vb`/`Populator.vb` in Schema are hand-maintained).

### Tests

- `KordanorsCabal.Game.Tests` mocks `IWorldData`/`I*Data` with Moq and uses a `WithX(Sub(mocks, subject) ...)` helper pattern ending in `VerifyNoOtherCalls()`, so any new call to a data interface inside a method under test must be set up/verified in its tests.
- `KordanorsCabal.Data.Tests` tests the Data layer against SQLite.

## Notes

- README has a TODO: "IItem subobjectification" (splitting `IItem` into sub-objects).
- `README.md` otherwise is the stream log and music seed; `sourcematerial/` holds reference art/PDFs.

## Odin port

A rewrite in Odin (`js_wasm32` + native SDL2) is in progress: infrastructure and step 1 (world, generation, save/load) are built; the game systems are being ported in the order given in `PORT.md`. See `PORT.md` for decisions, the prerequisite task list, and the findings log. After every step of the port, log its findings in `PORT.md` before moving on.

### Odin port: layout and commands (task 24)

```
odin/game/            package game: the portable core (rendering, input, services interface, RNG, UUID-keyed world, generation, save/load, UI shell screens, in-play hub, movement, items, events and combat; later townsfolk and spells)
odin/platform/web/    package main for js_wasm32 + page/ (index.html, platform.js, layout.js, storage.js and their node tests)
odin/platform/native/ package main for the SDL2 development build
odin/tests/           portable test kit + all test cases; runs natively (odin test) and as wasm under node
tools/                build.sh, test.sh, serve.sh, run_wasm_node.js; gen/ (generators for font, reference data); vb-oracle/ (headless VB driver)
docs/reference/vb/    recorded screens of the original game (text, exact cells, PNG, frame hash)
spikes/               throwaway experiments, kept as history until parity
src/                  the original VB.NET game: the reference until the port reaches parity
```

```bash
tools/build.sh all     # web + native into build/
tools/test.sh          # everything; must pass before finishing a change
tools/serve.sh         # serve build/web on :8080 (add ?touch=1 to force the touch layout)
```
Imports use the collection `kc` (`import "kc:game"`, pass `-collection:kc=odin`). Rules (see PORT.md): no `core:os` in shared code; platform differences by `#+build` files; the core allocates nothing that outlives a step except its fixed state; tests must not leak (the kit checks).
