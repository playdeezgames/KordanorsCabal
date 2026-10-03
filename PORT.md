# Odin Port: Decisions and Prerequisites

Goal: rewrite Kordanor's Cabal (currently VB.NET + MonoGame + SQLite, under `src/`) in Odin, shipping as `js_wasm32` for the browser. Status: all prerequisite tasks are done except task 28, which needs the user; the repository skeleton exists (`odin/`, `tools/`) and runs the title screen on web and native. See "Needs your attention" below for what is waiting on a person.


## Needs your attention

Written at the end of an unattended session (tasks 19 to 30). Nothing was committed, pushed, uploaded or published; everything
is in the working tree (`git status` shows the new files: `PORT.md`, `CLAUDE.md`, `docs/`, `odin/`, `spikes/`, `tools/`, and an
edited `.gitignore`).

### Blocked on you

1. **Task 28, the itch.io test.** I cannot create or reach your itch.io page. To do: create a draft HTML project page, run
   `tools/ship.sh` (writes `build/kordanors-cabal-html5.zip`, 2.7 MB; add `--push` to upload with `butler`, which needs
   `butler login`, or upload the zip by hand), then on a phone and on a desktop check inside itch's iframe: the game loads,
   the start tap works, sound and music play, `localStorage` saves survive a reload, export downloads a file and import opens
   the picker (iframes may need `allow-downloads`), fullscreen button, and the rotate prompt in portrait. Page settings to
   choose: embed size, "Mobile friendly", fullscreen button on.
2. **Real devices.** Everything touch, fullscreen, orientation lock, audio and file transfer was only exercised in a desktop
   browser engine with emulated mobile viewports (the pane and one real desktop Chrome). Unknown until a person tries it on
   Android Chrome and iPhone Safari: finger accuracy on 11 to 15 px rows (task 16), whether fullscreen + landscape lock work,
   audio unlock, the file picker modal (task 29).
3. **Music for iPhones.** The theme exists only as Ogg Vorbis (1.9 MB). I could not establish whether iPhone Safari plays Ogg,
   and this machine has no encoder. Please provide `MinorTheme.mp3` or `.m4a` (or tell me to install `ffmpeg` here), and then
   listen to whether the 2-minute theme loops without an audible gap in the browser (task 20).
4. ~~**Commit policy.**~~ Resolved: work is committed on the branch `odin-port` in logical commits.

### Decisions for you: all ten reviewed on 2026-10-03

Outcomes: (1) UUID-keyed maps for characters, items and locations (D9, D15 amended); (2) rotten food fixed; (3) phones keep the original border (D17 revised); (4) export/import on the Save and Load screens; (5) unobtainable items deferred to the end of the project; (6) visible, typeable seeds (D21 amended); (7) VB word wrap kept; (8) in-game Credits screen (music and sound credits still to be supplied); (9) two tabs ignored; (10) cleanup as above. The list below keeps the original wording with each outcome marked.

Each is recorded in the decision table above (D9 and later were adopted by me on the evidence in the logs, not chosen by you).
The ones most worth a look, in order of how hard they are to change later:

1. ~~**D9 entity model and D15 save format**~~ **Resolved in review 1:** UUID-keyed maps for characters, items and locations; see D9.
2. ~~**Rotten food fix**~~ **Resolved in review 2:** fix it. Rotten food uses its own content events (poisoning, rat spawn, disappearance); everything is derived from the item's current type.
3. ~~**D17 touch design.**~~ **Resolved in review 3:** keep the original border on phones (no cropping); side control columns and the select-then-confirm finger tap stay. Real-phone testing is still needed.
4. ~~**Export/import placement** (task 29)~~ **Resolved in review 4:** on the Save and Load screens (see the Task 29 log).
5. ~~**Three unobtainable item types**~~ **Deferred in review 5 (decide when finishing the game):** the three item types stay in the content exactly as they are and still cannot be obtained. Proposal on file for that time: Amulet of DEX as a 1d1 spawn on Level II (the only dungeon level without an amulet), Membership Card in the Malcontent loot table at weight 2 (the constable pays 10 money per card for "evidence of malcontent activity"), Ring of HP as a 1d1 spawn on the Moon.
6. ~~**Seeds.**~~ **Resolved in review 6:** players can see their seed and type one (see D21).
7. ~~**Message word-wrap.**~~ **Resolved in review 7:** keep the VB wrap exactly (cut at column 22, even inside words); the recorded reference screens stay the specification.
8. ~~**Credits.**~~ **Resolved in review 8:** an in-game Credits screen, no separate page. Design: the About screen's last line ("PLEASE SEE CREDITS.TXT FOR LINKS TO THEIR WORK!") changes to point at the new screen, and Confirm on About opens it (Cancel still returns to the title). The Credits screen shows the text of `Credits.txt` (Kordanor, Zooperdan, Vermux, Lorc, with their URLs as plain text wrapped at 22 columns, about 20 rows) and scrolls with Up and Down. URLs cannot be tapped inside the game. **Still open:** the credits mention nothing about the music (the README says it was generated with Abundant Music, seed 2645320710) or where the sound effects came from; tell me what to credit.
9. ~~**Two browser tabs**~~ **Resolved in review 9:** ignore; the last save wins silently. Export is the backup.
10. ~~**Cleanup at parity**~~ **Resolved in review 10:** at parity delete `spikes/` and the old `shippit.sh`; keep `src/` and `tools/vb-oracle` until the owner says to remove them.

### Facts you should know

- The VB game has crashes and bugs I recorded in `docs/dead-code-audit.md` (twelve entries). The two worth knowing: Continue
  crashes when no save file exists (seen in the headless driver; not confirmed in the shipped binary), and the elemental orb
  flickers because drawing uses the random generator.
- Several handlers and systems were prototyped with stand-ins (combat, factions, health, mana); the real versions must be
  ported before those handlers are trusted (task 18 log).
- `PORT.md` is long on purpose; each task has "Verified" and "Not verified" lists. The "Not verified" items are the real
  to-do list for testing.

### Suggested order for the actual port (in progress: steps 1 to 6 done, see "Port step 1" in the log)

The infrastructure exists (rendering, input, services, content, RNG, tests, builds). Each step below ends with
`tools/test.sh` passing and, where a screen exists, a bit-exact comparison with `docs/reference/vb`:
1. ~~World state, save/load (task 14 format), `world_validate`; world generation.~~ **Done** (`uuid.odin`, `world.odin`, `worldgen.odin`, `save.odin`).
2. ~~UI shell: the boilerplate screens (instructions, about, options, quit, load/save, export/import) on the menu pattern.~~ **Done** (`core.odin`, `ui_screens.odin`, `config.odin`; see "Port step 2").
3. ~~The in-play screen, movement, turning, the dungeon artwork, sprites and map.~~ **Done** (`ui_play.odin`, `rules.odin`, `ui_data.odin`; see "Port step 3").
4. ~~Items: inventory, equipment, ground, events (task 18 handlers), durability.~~ **Done** (`items.odin`, `events.odin`, `ui_items.odin`; repair comes with the blacksmith in step 6).
5. ~~Combat, death, XP and level up.~~ **Done** (`combat.odin`; see "Port step 5").
6. ~~Townsfolk and shoppes; spells and quests.~~ **Done** (`shoppes.odin`, `ui_town.odin`; see "Port step 6").
7. Polish, the real-device pass, itch.io release.

## Decisions

| # | Topic | Decision |
|---|-------|----------|
| D1 | Data store | **No SQL.** Game state is in-memory Odin structs, persisted as **JSON**. In the browser the JSON string is stored in HTML5 `localStorage`. |
| D2 | Targets | Two targets from one codebase: **`js_wasm32`** (shipping) and a **native SDL2** build (development, `odin test`, debugging). Game logic must be platform-independent; only a thin platform layer differs. |
| D3 | JS glue | **As small as possible**, but it covers general input, output, rendering, local storage, SFX and music. Everything else stays in Odin. |
| D4 | Touch/mobile | **In scope from the start.** Touch is a first-class input method, not a later add-on, so the input model and UI layout must be designed with it in mind. |
| D5 | Platform split | Platform differences are selected with Odin **`when`** blocks for code, plus **file-level `#+build` tags** for anything that needs an `import` (amended after task 3: Odin forbids `import` inside `when`). Native-only files use `#+build !js`; browser-only files use `#+build js`. |
| D6 | `core:os` | **`core:os` is restricted to the native client only** (imported only from `#+build !js` files). It must never be imported by shared game code, because it does not exist on `js_wasm32`. |
| D7 | Platforms | **macOS is out of scope.** Supported targets are `js_wasm32` (shipping), Linux and Windows (native SDL2 dev/test builds). Do not spend time verifying macOS. |
| D8 | Seed data | **A one-off exporter dumps `boilerplate.db` to Odin source** (no hand porting of the seed data). The generated Odin files are checked in as the content tables; the exporter is not part of the shipped game. |
| D9 | Entities and handles | **REVISED 2026-10-03 by the owner (decision review 1): characters, items and locations are held in hash maps keyed by UUIDs.** A UUID is 16 bytes in memory (a version-4 UUID drawn from the game's seeded generator, D21, so a seed reproduces the same ids) and a lowercase hyphenated string only in the save. Ids are never reused, so a lookup with a stale id simply fails. There are no pool capacities, generations or free lists. Anything that scans entities walks them in creation order (each entity carries a creation counter), because Odin map order is not deterministic. Code never holds a pointer into a map across anything that creates or destroys entities. Static content stays enums (0 = `None`, values = VB ids). Items record where they are (holder kind + owner id + sequence number); characters at a location are found by scanning. The earlier pool design is in the Task 17 log for history. |
| D10 | Frame and orientation | **The 2:1 horizontal stretch is part of the game's look (VIC-20 pixel aspect) and is kept on every device.** On mobile the game targets **landscape**: it requests landscape (auto-rotate) and shows a "rotate your device" prompt in portrait. How the on-screen controls work (D-pad and buttons, taps on cells, or both) is still open (task 16). |
| D11 | Save backup | **Export and import of saves as a file** is a required feature, in addition to `localStorage` slots. |
| D12 | Distribution | **itch.io HTML5** is the only distribution target. No separate site, no PWA. |
| D13 | Dead code | **Do not port what the game does not need.** Dead and unreachable VB code, unused commands and unused hooks are dropped, and each system is checked for reachability before it is ported. Behaviour that is live is ported faithfully; defects found along the way are fixed, not copied. |
| D14 | Old saves | **VB.NET `.db` saves are not imported.** SQLite stays out of scope. |
| D15 | Save format v1 | **One JSON text per save** (a slot is the whole text under `kc:slotN`; an exported file is that same text). Envelope `{format, version, summary, world}`. Entities store only per-instance data; everything derivable from content (item stats, events and names, character base data) is not saved. No maps, no unions, no omitted-field tricks; stats and routes are numeric tuples using the stable VB ids. Importing parses into a temporary world and runs `world_validate` before anything is replaced. **Amended by D9 (revised):** entities, items and locations are saved as arrays of records sorted by creation order, each with an `"id"` UUID string (never as JSON objects keyed by id, which Odin writes in nondeterministic order); the saved pools' `used_top`, `free_head` and generations no longer exist. Details in the Task 14 log. Adopted by the assistant on the evidence in the log. |
| D16 | Platform interface | **The core is a pure `core_step(input) -> output` with `Services` passed in; platforms only do I/O.** Commands: Up, Down, Left, Right, Confirm, Cancel. The platform owns layout (2:1 stretch, scaling, letterbox), key and control-button mapping, touch-to-cell conversion, audio assets and unlocking, file dialogs; the core owns the 208 x 240 RGBA frame (CPU rasterizer), sound requests, volumes, config and all game state. The authoritative text is `spikes/platform-api/api.odin`; rules in the Task 15 log. Adopted by the assistant on the evidence in the log. |
| D17 | Touch layout | **REVISED 2026-10-03 (decision review 3): Mobile is landscape-only and keeps the original border: the whole 416 x 240 frame (paper plus its border, shown 2:1) is scaled to fit, so the game looks exactly like the original and like the desktop version, only smaller.** Controls: a D-pad column on the left and Confirm/Cancel on the right, each button at least 41 px; if the side columns would leave the frame under 1.3x scale (for example iPhone SE sized screens) the controls are hidden and the frame fills the screen. **Fingers are imprecise: a touch tap selects, a second tap on the selected item confirms; mouse and pen taps confirm at once.** One start tap unlocks audio, goes fullscreen and locks landscape where allowed; portrait on touch shows a rotate prompt. Desktop: whole frame at an integer scale, no on-screen controls. Rules in the Task 16 log; `layout.js` is the code. |
| D18 | Events | **Rules are called directly; there is no event bus.** Content names behaviors with two generated enums, `Check` (18) and `Action` (27), each implemented by one `switch`; handlers take a by-value `Event_Context{world, character, item, location}` instead of a `Long()`. Item types carry `purify`/`can_use`/`use`/`decay` fields, spells and quests carry theirs. Results reach the player through bounded queues in the world: a ring of 16 fixed-size messages (player only; the message's sound is played when it is first shown) and an immediate sound list. Handlers never crash on missing things. Details in the Task 18 log. Adopted by the assistant on the evidence in the log. |
| D19 | Memory and strings | **Four lifetimes, one allocator for each.** (1) Long-lived state (`Game`: world, core, UI data, the 208 x 240 frame) is allocated at start with `new` and is never on the stack; **amended by the revised D9:** the entity maps grow on the heap as characters and items are created (the heap reuses freed memory). (2) Per-frame scratch uses `context.temp_allocator`, freed with `free_all` at the end of every `core_step`; nothing from it survives a step. (3) A load or import parses into a temporary arena that is dropped as soon as the world is filled. (4) Strings crossing from JS go into a buffer the Odin side allocates and frees after the step. **No struct that outlives a step owns a string**: content strings are literals, messages are fixed buffers. The wasm stack is 1 MiB: no local larger than 64 KiB. Details in the Task 19 log. Adopted by the assistant on the evidence in the log. |
| D20 | Audio delivery | **Sounds and music are fetched at run time from `assets/` next to the page, never embedded in the wasm.** The 8 effects are decoded into Web Audio buffers on the first user gesture; the theme streams through an `<audio>` element routed through a gain node (so volume works on iOS); the first format in a fallback list that the browser can play is used (m4a, mp3, then the shipped ogg), moving to the next on any error. The core only names a `Sfx` and sets two volumes; the audio is paused when the page is hidden. Details and an open blocker in the Task 20 log. Adopted by the assistant on the evidence in the log. |
| D21 | Randomness | **The game has its own generator: xoshiro256** seeded by splitmix64, state `[4]u64` stored in the save.** Every roll goes through the world's generator (`rng_range`, `roll(Dice)`, `pick_weighted`, `pick_index`), all unbiased. New games seed from the platform's entropy service and keep the seed; `?seed=N` (browser) and `--seed N` (native) fix it for tests and bug reports. Decoration (the orb's color) never touches the game generator. Details in the Task 21 log. Adopted by the assistant on the evidence in the log. **AMENDED 2026-10-03 (decision review 6): the seed is visible and can be typed.** A seed is a number from 0 to 999,999,999 (at most 9 digits, so it fits the 22-column screen); new games draw one from the platform's entropy in that range. The Status screen shows a line `SEED nnnnnnnnn`. The title menu gets an extra item "Start with seed..." (right after "Start") opening a seed screen that edits the number one digit at a time with the same six commands as everything else (Left/Right choose the digit, Up/Down change it, Confirm starts the game, Cancel goes back), so it works with a D-pad or taps and needs no keyboard. The same seed always generates the same world with the same ids (D9); play then diverges only if the person's actions differ. The seed is saved with the game. Consequence: the title screen is no longer identical to the VB title (one extra menu line); the golden test against `01-title` will compare the original six lines only, or be updated. |
| D22 | Tests | **No VB test is ported.** New tests are written against a small portable kit and run on both targets: natively through `odin test`, and the same compiled-for-wasm suite headless under node (`run_wasm_node.js`, no browser needed). Every case runs under a tracking allocator, so a leak is a failure. Layers: rules (state in, state out), world invariants after generation, load and random-action soaks, save round trips and fuzz, scripted UI sessions with fake services, exporter freshness, node tests for the JS code, and later differential checks against the running VB build. `tools/test.sh` runs everything and fails on any failure. Details in the Task 22 log. Adopted by the assistant on the evidence in the log. |
| D23 | Reference corpus | **The running VB game is the specification for behavior, recorded by `tools/vb-oracle`** (a headless driver, C#, outside the shipped code). `docs/reference/vb/` holds 54 recorded screens (text, exact cells, PNG, frame hash) covering 30 of the 32 UI states; the Odin rasterizer is tested bit for bit against them. New behavior questions are answered by extending `scenes.txt`, not by guessing. Adopted by the assistant on the evidence in the log. |
| D24 | Repo layout | **The Odin code lives in `odin/` (`game` core package, `platform/web`, `platform/native`, `tests`), tooling in `tools/`, recorded references in `docs/reference/`.** The VB game in `src/` stays untouched as the reference until parity; `spikes/` stays as history. Imports use a collection (`import "kc:game"`). One command builds (`tools/build.sh`), one tests everything on both targets (`tools/test.sh`). Details in the Task 24 log. Adopted by the assistant. |

### Consequences of the decisions

- **Static vs mutable data (from D1).** Content that never changes at runtime (item types, character types, spells, stat ranges, route/shoppe types, etc.) is separated from mutable game state. Static content is either Odin constants or a JSON file loaded at startup. Mutable state lives in one `World_State` that is what gets serialized into a save slot.
- **Retired tooling (from D1).** `boilerplate.db`, `KordanorsCabal.Scaffolder` and `KordanorsCabal.Descaffolder` have no equivalent in the port. `KordanorsCabal.Schema` populators are the source for the seed data and have to be translated.
- **Platform interface (from D2/D3).** The platform layer exposes one small, identical API on both targets: a pixel/cell-buffer present call, an input event stream (keys, pointer/touch), `storage_get/set/delete(key)`, `play_sfx`, `play_music`/`set_volume`, a time source and an entropy source. SDL2 and the JS shim are two implementations of it. Native storage writes the same JSON to files in a user directory.
- **Main loop (from D2).** Browsers call into exported procs, so the game is structured as `init()` plus `step(dt)` driven by `requestAnimationFrame` in wasm and by an ordinary loop in SDL2.
- **Touch (from D4).** The current game is driven by a keyboard `Command` set (see `src/KordanorsCabal/CommandUtility.vb`). The core must consume abstract commands, with keyboard and touch both mapping into them.

## Prerequisite tasks (before any porting)

Nothing here ports game code. These tasks produce the information and scaffolding the port depends on.

### A. Toolchain feasibility
1. Install Odin and confirm a hello-world builds for `js_wasm32` and runs in a browser via `odin.js`.
2. Confirm `vendor:sdl2` builds and opens a window natively on Linux (and note the Windows story).
3. Confirm the same trivial Odin package compiles for both targets with a platform-specific file split.
4. Confirm `core:encoding/json` marshal/unmarshal works on `js_wasm32` with a nested struct, including maps/arrays/enums.
5. Confirm a minimal `localStorage` round trip through the JS shim, and note the size limit (about 5 MB) against an estimated save size.

### B. Inventory of the existing game (reading only)
6. List every table in `Schema/Scaffolder.vb` (49) and classify it as static content or mutable state.
7. Inventory the seed data per Populator (`Schema/*/…Populator.vb`) and decide how to translate it (hand port vs. a one-off exporter that dumps `boilerplate.db` to JSON).
8. Document how `Patterns.vb` and the PETSCII bitmaps are defined, and how `CharacterSprites.vb`/`RouteSprites.vb` are built (patterns vs. image assets).
9. Document the maze and level generation: what is seed data, what runs at game time, and what must be saved.
10. Catalogue the polymorphic parts (`ItemType` subclasses and descriptors, character types, shoppe and townsfolk modes) and note which map to tagged unions vs. data tables.
11. List every event raised in `Events.vb` / `ItemEvents` / `SfxPlayer`, and who consumes it.
12. List all UI states/screens (`UIState`, `MainProcessor`) and the commands each one accepts.
13. List every place state is read or written outside `IWorldData` (for example `UIConfig` for screen size and volumes).

### C. Design work
14. Define the **JSON save format**: top-level shape, version field, slot keys, what is in `World_State`, and how entity IDs are represented.
15. Define the **platform interface** (see above) as an Odin spec: exact procs, types and event structs.
16. Define the **touch input model**: how the keyboard `Command` set maps to touch (on-screen buttons, tap targets on cells, swipes), how the layout adapts to portrait vs. landscape, and how the 22×23 cell grid scales and positions on small screens. Produce a rough layout sketch per screen type.
17. Decide the **ID and handle scheme** for entities (distinct integer types vs. pointers) and the approach to replacing the interface/mocking layers.
18. Decide the **event strategy**: direct calls vs. a per-frame event queue drained by UI and audio.
19. Decide **allocator strategy** for wasm32 (persistent arena, per-frame temp allocator, string formatting).
20. Decide **audio delivery**: embed vs. fetch the wav/ogg assets, and the "click to start" gesture needed to unlock browser audio.
21. Decide the **RNG** approach: seedable generator, weighted-table helper, seed source on each platform.
22. Decide the **test strategy**: which of the ~5.8k lines of VB tests are worth porting, and how `odin test` runs on the native target.

### D. Baseline and repo setup
23. Capture reference behavior from the VB build (`pub-linux`) for each screen: screenshots plus a short script of expected interactions, to compare against during the port.
24. Decide the repo layout for the Odin code (for example `odin/core`, `odin/platform_sdl2`, `odin/platform_js`, `odin/web`) and where `PORT.md`-style docs live.
25. Decide how the old VB code is handled during the port (kept in `src/` as the reference until the port reaches parity).
26. Adapt `shippit.sh` for the new build (an HTML5 zip for itch.io) once a build exists.

27. **(Added in task 7.)** Build the one-off `boilerplate.db` to Odin source exporter (D8). **Done** (see the Task 27 log): `tools/gen/gen_content.py`.

28. **(Added with D12.)** Spike: load a `js_wasm32` build as an itch.io HTML5 upload (private/draft page) and verify `localStorage` read/write, audio unlock, fullscreen and landscape lock inside itch's iframe, desktop and phone. Needs the user's itch.io page.
29. **(Added with D11.)** Design and spike the save export/import: download a JSON file from the page and load one back through a file picker (JS shim additions: save-as-file and pick-a-file; native build: files). Include validation of imported data (version, shape, `world_validate`) before it replaces a slot.
30. **(Added with D13.)** Dead-code audit rule: before porting each system, list what is unreachable or unused in the VB code (content rows never referenced, commands no screen handles, hooks no data uses) and record what is dropped. Already identified: the add-to-inventory item event (id 5), the commands `Yellow`/`Start`/`Back`/`NextItem`/`PreviousItem`, the unused `DateTimeOffset` timing lines in `World.CreateDungeonLevel`, the Scaffolder and Descaffolder tools, `_boilerplate.db`.

## Process rule

**After every step or task, log its findings in this file before starting the next one.** Each log entry records: what was done, what was verified (and how), what was *not* verified, findings that affect the design, and open issues blocking the next step. Update the task status in the table below at the same time.

## Task status

| Task | Status |
|------|--------|
| 1 | Done (see log) |
| 2 | Done (see log) |
| 3 | Done (see log); **D5 amended** |
| 4 | Done (see log); **tagged unions not usable as-is in save data** |
| 5 | Done (see log); real-browser quota untested |
| 6 | Done (see log) |
| 7 | Done (see log); exporter itself is new task 27 |
| 8 | Done (see log) |
| 9 | Done (see log) |
| 10 | Done (see log) |
| 11 | Done (see log) |
| 12 | Done (see log) |
| 13 | Done (see log); **completes section B** |
| 14 | Done (see log); **D15** |
| 15 | Done (see log); **D16** |
| 16 | Done (see log); **D17**; real-phone testing still needed |
| 17 | Done (see log); **D9** |
| 18 | Done (see log); **D18** |
| 19 | Done (see log); **D19** |
| 20 | Done (see log); **D20**; needs an mp3/m4a of the theme for iOS safety |
| 21 | Done (see log); **D21** |
| 22 | Done (see log); **D22** |
| 23 | Done (see log); **D23** |
| 24 | Done (see log); **D24**; skeleton builds, tests pass |
| 25 | Done (see log) |
| 26 | Done (see log) |
| 27 | Done (see log) |
| 28 | **Blocked on the user** (needs an itch.io page and a phone) |
| 29 | Done (see log) |
| 30 | Done (see log) |

## Spikes

Throwaway experiments live in `spikes/<name>/` (not the final layout; see task 24). Each spike's findings are logged below. Build outputs are git-ignored. `spikes/wasm-hello` expects `odin.js` copied next to its `index.html` from `$(odin root)/core/sys/wasm/js/odin.js`.

## Findings log

### Task 1: Odin builds and runs on `js_wasm32` (2026-10-03)

**Done:** Built a hello-world (`main` + exported `step`) with `odin build . -target:js_wasm32 -out:hello.wasm` using Odin `dev-2026-07-nightly:ab0131c`. Served it with `python3 -m http.server` and loaded it in the Claude browser pane using `odin.js` from `$(odin root)/core/sys/wasm/js/`. The spike lives in the session scratchpad, not in the repo.

**Verified:** Clean build, no warnings, wasm about 294 KB (inflated by `core:fmt`). `main` ran and `fmt.println` reached `console.log` through `odin.js` with no custom glue. `step` was called once per frame with a positive `dt`, and returning `false` ended the loop.

**Not verified:** Firefox/Safari; `core:encoding/json` on wasm (task 4); `localStorage` (task 5); SDL2 (task 2).

**Findings affecting design:**
1. `odin.js` requires `@(export) step :: proc(dt: f64) -> (keep_going: bool)` in the default Odin calling convention (not `proc "c"`). `dt` is in seconds; returning `false` stops the loop and calls `_end` if exported.
2. `odin.js` calls `_start` (which runs `main`) and then starts the `requestAnimationFrame` loop only if `step` is exported. So `main` is the `init()` of our `init()` + `step(dt)` structure.
3. `requestAnimationFrame` does not fire in a hidden page, and the browser pane reports `document.hidden === true`. Testing in the pane requires a shim (`window.requestAnimationFrame = f => setTimeout(() => f(performance.now()), 16)` when hidden). This is a testing aid only, not part of the game.
4. `odin.js` sets up the default context, so `context` and allocators work normally (relevant to task 19).

**Open issues before task 2 (SDL2 native build):**
- Confirm `vendor:sdl2` actually links and opens a window. The Odin vendor package has the bindings, and the system has `libSDL2`, `libSDL2_mixer`, `libSDL2_image` and `libSDL2_ttf` installed, but nothing has been built yet.
- Decide where spike code lives. The task 1 spike is in a temp scratchpad and will be lost. Task 24 (repo layout) is needed at least in a minimal form, or spikes go in a `spikes/` directory.
- Windows story for SDL2 (DLL shipping) is untested and can wait until after Linux works.
- Decide whether browser-pane wasm testing is enough, or whether a visible real browser is needed for rendering and touch testing later (the pane may not emulate touch).

### Task 2: `vendor:sdl2` builds and runs natively (2026-10-03)

**Done:** Moved the task 1 spike into `spikes/wasm-hello`. Wrote `spikes/sdl2-window` (SDL2 window + renderer + streaming RGBA texture fed from a CPU pixel buffer, 120 frames, event polling) and `spikes/sdl2-mixer` (SDL2_mixer init, load a game wav and the music ogg). Built with plain `odin build . -out:...` (default linux target).

**Verified (Linux, X11 session):**
- Builds clean, about 395 KB binary. SDL video driver `x11`, renderer `opengl`.
- 120 frames rendered with the window shown, `RenderSetLogicalSize` and `UpdateTexture`/`RenderCopy` path works, exit code 0. This is the same shape as the planned platform layer: CPU framebuffer to texture to screen.
- SDL2_mixer opened audio and loaded `LevelUp.wav` and `MinorTheme.ogg` from `src/KordanorsCabal/Content/`. Run with `SDL_AUDIODRIVER=dummy`, so no sound was heard.
- The `wasm-hello` spike still builds after the move.

**Not verified:**
- Actual window contents were not looked at (no screenshot taken), and no input events were generated, so keyboard/mouse/touch event handling is untested.
- Audible playback, volume control and music looping.
- Windows (SDL2.dll shipping); macOS is out of scope (D7).
- Linking uses the system SDL2 libraries; a fresh machine without them would fail. Odin's vendor package does not bundle Linux libs.

**Findings affecting design:**
1. The CPU-framebuffer-to-streaming-texture path works natively, so the existing `Renderer.Update` design (rasterize PETSCII cells into an RGBA buffer) maps directly onto both targets. The only per-platform step is "present this buffer".
2. SDL2 gives the native build `FINGERDOWN`/`MOUSEBUTTONDOWN` events, so the abstract input events (keys, pointer/touch) from the platform interface can be produced natively too, although touch itself would only be testable on a touch device or by emulating pointer events.
3. Mixer separates music (`LoadMUS`, one stream) from sound effects (`LoadWAV`, chunks), matching the existing game's split of one looping theme plus 8 effects. The platform audio API can stay at `play_sfx(id)` / `play_music` / `set_*_volume`.
4. Native builds depend on system `libSDL2` and `libSDL2_mixer` packages. That is fine for development but should be documented for contributors.

**Open issues before task 3 (same package builds for both targets):**
- Decide the file split convention for platform code (for example `_wasm.odin` / `_sdl2.odin` suffixes, or `when ODIN_ARCH == .wasm32` blocks, or separate packages with a shared `core` package).
- Decide whether `core:fmt`/`core:os` calls in core code are allowed, since `core:os` does not exist on `js_wasm32` and the SDL2 build needs it only for `os.exit`.
- Run the SDL2 spike once more with a visible check (screenshot or read-back of pixels) before relying on it for rendering.

#### Task 2 addendum: pixel readback (2026-10-03)

**Done:** Added `readback_check` to `spikes/sdl2-window`. At frame 10 it calls `RenderReadPixels` on the renderer, computes the logical-size scale and letterbox offset, and compares 25 sampled points (5 x 5 grid, including all four corners) against the CPU pixel buffer.

**Verified:** Output size 576 x 756 (window 3x the 192 x 252 logical size), scale 3, and 25/25 sampled pixels match exactly. So the CPU buffer to streaming texture to screen path draws precisely what was written, with nearest-neighbor scaling, with no color-channel swap (RGBA32 on both sides).

**Not verified:** Only the renderer's output was read back, not what the compositor shows on screen. Non-integer scales (for example fractional window sizes or letterboxing) and the sRGB/color handling of the real PETSCII palette are not tested.

**Findings:** `RenderReadPixels` takes a `u32` format (`u32(SDL.PixelFormatEnum.RGBA32)`), not the enum directly. This readback approach can serve as an automated rendering test in the native build, since it needs no screenshot tooling.

**Updated open issues before task 3:** Platform code split (resolved: D5, `when`). `core:os` policy (resolved: D6, native only). The SDL2 pixel readback is done. Remaining for task 3: confirm that one package with `when`-selected platform code builds for both `js_wasm32` and the native target, and that `core:fmt` (allowed in shared code) works on both.

### Task 3: one package, two targets (2026-10-03)

**Done:** Wrote `spikes/dual-target`, a single Odin package (`package main`) with: `game.odin` (shared logic using only `core:fmt` and `core:math`), `platform_js.odin` (`#+build js`: `main` as init plus exported `step`), `platform_native.odin` (`#+build !js`: imports `core:os` and `core:time`, runs a loop, `os.exit`), and `game_test.odin` (`#+build !js`, `core:testing`).

**Verified:**
- Native: `odin build .` succeeds and runs (init, step, clean exit).
- Browser: `odin build . -target:js_wasm32` succeeds (about 309 KB) and, loaded in the browser pane with the `requestAnimationFrame` shim, printed `game_init` and the frame-3 line from the same shared `game_step`.
- `odin test .` runs the shared logic natively: 1 test passed.
- `odin check` passes for `linux_amd64` and `windows_amd64`.
- `core:fmt` and `core:math` work in shared code on both targets.

**Not verified:** Real-browser (non-pane) behavior; a shared file that uses SDL2 on native only (the real structure, since SDL2 is imported from native-only files too).

**Findings affecting design:**
1. **Odin does not allow `import` inside a `when` block** (syntax error). The compiler recommends file suffixes or `#+build` tags. **D5 is amended:** use `when` for code selection and `#+build` tags for any file that has an import that is target-specific.
2. **Importing `core:os` on `js_wasm32` is a compile-time panic** ("core:os is unsupported on js/wasm"). An unused import is still fatal, so D6 can only be enforced by file separation: `core:os` may appear only in `#+build !js` files.
3. `#+build` tags work per file within one package, so game logic plus a platform pair of files in one package is a viable structure, and `main` is simply defined differently per target.
4. Test files can be `#+build !js` so `odin test` runs natively without affecting the wasm build.
5. The shared `game_step(dt) -> bool` is the seam: the browser calls it from `step`, and native calls it from its own loop.
6. `-target:freestanding_wasm32` is not supported with `core:os` or time-using native files. This is irrelevant, since we ship `js_wasm32`.

**Open issues before task 4 (`core:encoding/json` on wasm):**
- Decide whether shared code uses `#+build` tags only, or also a `-define` or Odin collection split once the real layout (task 24) exists; this spike used one flat package.
- Task 4 needs a save-like nested struct (arrays, maps, enums, nested structs, optional or sentinel IDs) to test marshal and unmarshal on both targets, and it should measure the wasm size increase from `core:encoding/json`.
- Whether allocation behavior (marshal returns allocated bytes) is acceptable under the planned allocator strategy (task 19); marshal needs a defined allocator on wasm.

### Task 4: `core:encoding/json` on `js_wasm32` (2026-10-03)

**Done:** Wrote `spikes/json-roundtrip`: a save-like `World_State` (nested structs, `[dynamic]`, fixed and enumerated arrays, `bit_set`, `distinct int` IDs, `Maybe(int)`, `map[string]struct`, `map[int]bool`, tagged `union`, `f32`, a near-max `u64`, a string with quotes/newline) marshalled with `{pretty = true, use_enum_names = true}`, unmarshalled, and compared field by field. It also tests malformed input and marshal/unmarshal inside a `mem.Arena`. Same shared code and `#+build` platform files as task 3, run natively and in the browser pane.

**Verified (identical results on native and `js_wasm32`):**
- PASS: string escapes, enumerated array, fixed array, `bit_set`, `distinct int` IDs, `Maybe(int)` (some and none), dynamic array, `map[string]struct`, `map[int]bool`, `f32`, `u64` near max (exact), malformed JSON returns `Invalid_Data` (no crash), and marshal plus unmarshal using an arena allocator (about 2.3 to 2.9 KB for the test state; 32-bit pointers use less than 64-bit).
- **FAIL: tagged unions.** Marshal writes only the variant's fields with no tag, so unmarshal cannot tell variants apart and silently picks the first one (`Combat_Mode{enemy=2, hp=17}` came back as `Shoppe_Mode{shoppe_type=0}`). No error is reported.
- wasm size: 532 KB, versus 294 KB for the fmt-only hello-world, so `core:encoding/json` costs about 240 KB of unoptimized wasm (default build; not size-optimized or gzipped).

**Not verified:** Release-size optimization (`-o:size`) and gzip/brotli transfer size; large saves (a realistic full game state, performance and allocation at that scale); round trip of other enums-as-map-keys; a real visible browser; `localStorage` itself (task 5).

**Findings affecting design:**
1. **Tagged unions must not appear in save data.** Use an explicit discriminator: an enum `kind` field plus flat fields (verified to round trip), or a hand-written tagged encoding. This affects the polymorphic parts of the game (task 10: modes, item types), which should be data-driven (kind enum plus table) rather than unions when persisted.
2. Enumerated arrays (`[Stat]int`) serialize as objects keyed by enum name with `use_enum_names`, so saves are readable and robust to enum reordering. Plain enum fields without that option serialize as integers (the `kind` field came out as `2`), and `bit_set` always serializes as an integer. Renumbering or inserting enum values would break old saves, so a `version` field and append-only enums are needed.
3. `Maybe(T)` round trips as a value or `null`, which suits the game's nullable columns (for example durability).
4. Maps with integer keys round trip through string keys (`"42": true`), so IDs can key maps.
5. Failures on bad input are reported as errors, except for the union case above which fails silently. Save loading should validate after unmarshal.
6. The allocator model is simple: marshal and unmarshal use `context.allocator`, so a save/load can run inside a temporary arena that is dropped afterward. Data that must outlive the arena (like `World_State`) needs to be loaded with the persistent allocator, and strings are allocated by unmarshal (not slices of the input).
7. Floats are written with 8 decimals (`12.50000000`), which is fine for the game's values.

**Open issues before task 5 (`localStorage` round trip):**
- Estimate a realistic save size (the VB `boilerplate.db` and the per-slot state) to compare with the roughly 5 MB `localStorage` limit (strings are UTF-16, so the effective capacity in characters is lower).
- Decide the JS-shim API shape for storage: functions callable from Odin with pointer and length into wasm memory (get/set/remove by key).
- Decide whether saves will be compressed before storing, based on the size estimate.

### Task 5: `localStorage` round trip through the JS shim (2026-10-03)

**Done:** Wrote `spikes/storage`: `storage.js` (the JS shim, 4 functions) plus Odin bindings and a test program. `storage_js.odin` (`#+build js`) declares the foreign procs; `storage_native.odin` (`#+build !js`) implements the same Odin API with one file per key under `./saves/` using `core:os` (D6). `game.odin` is shared and calls only `storage_set/get/remove`.

**Shim API (the entire storage surface of the JS glue):**
- `js_storage_set(key, value) -> bool`: false if the write throws (quota, storage disabled).
- `js_storage_len(key) -> int`: UTF-8 byte length, `-1` if missing.
- `js_storage_get(key, buf) -> int`: bytes written into the caller's buffer, `-1` if missing or buffer too small.
- `js_storage_remove(key)`.
Odin wraps these as `storage_set(key, value) -> bool`, `storage_get(key, allocator) -> (string, bool)` (length query, allocate, fill), and `storage_remove(key)`. Strings cross as (ptr, len) pairs; the shim is wired in with `odin.runWasm(wasm, null, storageImports(mem), mem)` where `mem` is an `odin.WasmMemoryInterface`.

**Verified (native and browser pane, same results):**
- Set then get returns the identical bytes for JSON containing quotes, an escaped newline, accented letters, em dash, CJK and an emoji (85 bytes, so UTF-8 byte lengths and the UTF-16 conversion in `localStorage` agree).
- Overwrite, remove, and missing-key behavior are correct.
- Persistence: a counter stored by the page survived a reload (`previous runs: 1` then `2`); the native build also persisted across process runs.
- Values up to 8 MB (ASCII) stored and read back in the pane. A direct JS probe in the pane stored up to 32M characters before throwing `QuotaExceededError`.

**Not verified:**
- The pane's quota is far larger than a normal browser's. Mobile and desktop browsers typically allow only about 5 MB (and `localStorage` strings are UTF-16), so the real limit was not measured. A real visible browser, and in particular mobile Safari/Chrome, still need to be tested.
- The Odin-side `false` return when `setItem` throws was not exercised (the JS-level `QuotaExceededError` was confirmed, but the shim's catch path from Odin was not).
- Behavior in private browsing or with storage disabled; saves from multiple tabs; `storage` events.
- A realistic save size has not been estimated (carried over from task 4); it is still needed to compare with the roughly 5 MB limit.
- Windows native path handling in `storage_native.odin`.

**Findings affecting design:**
1. The whole storage glue is four tiny JS functions with no handles or callbacks; the Odin API is synchronous, which matches `localStorage`. If we ever move to IndexedDB (async), the Odin API would have to change, so the 5 MB limit is the trigger for that decision.
2. Passing `string` and `[]byte` to foreign procs works directly on `js_wasm32` with `@(default_calling_convention = "contextless")`; the JS side receives (ptr, len) pairs.
3. The shim needs a `WasmMemoryInterface` instance passed to both `runWasm` and the imports; memory is attached automatically after instantiation. This is the same wiring the audio, input and render shims will need.
4. Reading via length-then-get requires a buffer allocation per read; that is fine for save slots but not for per-frame use.
5. Keys use a prefix (`spike:`); real saves should use a game prefix (for example `kc:slot1`, `kc:config`). The native stand-in maps `:` to `_` in filenames.
6. Newer Odin `core:os` returns `Error` values instead of `bool` (needed `== nil` checks), which matters when writing the native file code.

**Open issues before the prerequisite tasks that follow (6 onward are inventory/design; no technical blockers remain from A):**
- Estimate a realistic full-game save size from the VB data model (task 6/14) to decide whether 5 MB is enough and whether compression is needed.
- Test in a real visible browser (desktop and a phone) for quota, private browsing, and rendering/touch before relying on the pane for these.
- Section A (toolchain feasibility, tasks 1 to 5) is complete.

### Task 6: classify the 49 tables as static or mutable (2026-10-03)

**Done:** Read every table from `src/KordanorsCabal/boilerplate.db` (columns, row counts) and cross-checked against the code: each table has exactly one `*Data.vb` class, and I grepped which `*Data.vb` files call `Store.Replace` / `Store.Create` / `Store.Clear`. Then I measured a real new game: a throwaway C# console project (kept in the session scratchpad, not the repo) referenced `KordanorsCabal.Game`, `Data` and `SPLORR.Data`, copied `boilerplate.db`, ran `World.Start()` and saved, then counted rows and estimated JSON sizes from the result.

**Verified:**
- **Exactly 19 tables are mutable and 30 are static.** The 19 tables with zero rows in `boilerplate.db` are exactly the 19 tables whose Data class has write calls. No static table is ever written at runtime.
- The VB game builds and runs under the installed .NET 10 SDK via a net10.0 host project, and generates a full world in a second or so.

**Static (30), the content that becomes constants or a content file:**

| Group | Tables |
|-------|--------|
| Enumerations | `Directions` (8), `DungeonLevels` (6), `EquipSlots` (8), `LocationTypes` (8), `ShoppeTypes` (5), `FeatureTypes` (9), `RouteTypes` (11), `StatisticTypes` (37), `SpellTypes` (2), `QuestTypes` (1) |
| Character types | `CharacterTypes` (16), `CharacterTypeInitialStatistics` (182), `CharacterTypeAttackTypes` (17), `CharacterTypeBribes` (6), `CharacterTypeEnemies` (30), `CharacterTypeLoots` (31), `CharacterTypePartingShots` (14), `CharacterTypeSpawnCounts` (43), `CharacterTypeSpawnLocations` (80) |
| Item types | `ItemTypes` (53), `ItemTypeStatistics` (133), `ItemTypeEquipSlots` (18), `ItemTypeEvents` (41), `ItemTypeShopTypes` (43), `ItemTypeSpawnCounts` (29), `ItemTypeSpawnLocationTypes` (87), `ItemTypeCharacterStatisticBuffs` (5) |
| Other | `Lores` (25), `RouteTypeLocks` (6), `SpellTypeRequiredPowers` (4) |

About 1,000 static rows in total.

**Mutable (19), the `World_State`:**

| Table | Meaning | Rows after a new game |
|-------|---------|-----------------------|
| `Characters` | every character, with `CharacterTypeId` and `LocationId` (player and monsters) | 1,087 |
| `CharacterStatistics` | per-character stat values | 11,071 |
| `CharacterEquipSlots` | what each character has equipped | 0 |
| `CharacterLocations` | the set of locations a character has **visited** (not the current location) | 1 |
| `CharacterQuests`, `CharacterQuestCompletions` | active and completed quests | 0, 0 |
| `CharacterSpells` | known spells and levels | 0 |
| `Players` | single row: character, facing direction, player mode, current shoppe type (nullable) | 1 |
| `Locations` | every location with its type | 737 |
| `LocationDungeonLevels` | dungeon level of a location | 726 |
| `LocationStatistics` | per-location stats (only Dungeon Column and Dungeon Row, one pair per dungeon location) | 1,452 |
| `Routes` | directed edges (location, direction, route type, destination) | 1,722 |
| `Features` | one instance of each feature type placed in a location | 9 |
| `Items` | every item with its type and a name | 362 |
| `ItemStatistics` | per-item stat values (durability etc.) | 911 |
| `ItemEvents` | per-item copies of event names | 158 |
| `ItemLores` | item to lore text assignment | 0 |
| `Inventories` | an inventory owned by a character **or** a location (ground) | 267 |
| `InventoryItems` | which items are in which inventory | 362 |

**Save-size estimate (answers the open question from tasks 4 and 5).** About 18,900 mutable rows after a new game. Serialized as JSON:
- naive row-as-object, compact: **about 1.1 MB** (pretty-printed about 1.4 MB);
- rows as compact arrays (no repeated column names): **about 215 KB**;
- for comparison the SQLite save file is 831 KB.
Even the naive form is well under the 5 MB `localStorage` limit, but five slots plus config would be about 5.5 MB in the naive form, which is over the limit. The compact-array form is about 1.1 MB for five slots. `CharacterStatistics` is 60% of the volume (11,071 rows) and `Routes` is next.

**Not verified:** Save size late in a long game (more items, quests, spells, equipment; monsters removed); whether `Items` and `ItemEvents` rows are really independent of their types (they are per-instance copies); the VB player-mode values.

**Findings affecting design:**
1. **The static/mutable split is clean and enforced by the code**, so D1 (static tables as content, one `World_State` struct) is a straightforward cut.
2. **Most of the save volume is `CharacterStatistics`.** Instead of a row per (character, stat), the Odin `Character` can hold a stat array (`[Stat]int`, which serializes as a small named object, or a fixed array). That shrinks the save a lot and also removes the join logic. Same idea for `ItemStatistics` and `LocationStatistics`.
3. **`LocationStatistics` only stores a location's grid coordinates (column, row) for dungeon locations.** In Odin these are plain fields on `Location`.
4. **`Routes`, `Locations` and `LocationDungeonLevels` describe a graph**, which maps naturally to location structs holding per-direction route entries (8 directions) instead of a separate edges table.
5. **`Inventories` mixes two owners** (a character or a location) and `InventoryItems` is the link table. In Odin an inventory can be a plain list of item IDs embedded in the owner. Also, `Characters.LocationId` is the current location while `CharacterLocations` is a visited-set; keep them as two separate concepts.
6. **IDs are `Long` autoincrement integers.** A new-game world has about 1,100 characters, 740 locations and 360 items, so `distinct int` IDs indexing dynamic arrays (or IDs that stay stable for removal, since characters and items are deleted when killed or used) both work; deletion handling must be decided in task 17.
7. **`ItemEvents`** are per-item rows that duplicate `ItemTypeEvents` (158 versus 41); this looks like a per-item override or copy made at creation. Whether it can be derived from the item type needs checking in task 11.
8. The generator is the producer of nearly all mutable rows. Maze size is 11 x 11 per level over 6 levels (the moon level included), which also yields the 726 dungeon locations.

**Open issues before task 7:**
- Task 7 needs the seed data translation approach: decide hand-port vs. an exporter. The static data is only about 1,000 rows, which makes an exporter that dumps `boilerplate.db` to Odin source or JSON easy and less error-prone than hand-porting; this should be the recommendation.
- Check `Schema/*Populator.vb` against `boilerplate.db` to be sure they agree (the db is generated from them, but `_boilerplate.db` and `boilerplate.db` both exist and differ in size).

### Task 7: seed data inventory and translation approach (2026-10-03)

**Decision:** D8, a one-off exporter dumps `boilerplate.db` to Odin source.

**Done:** Regenerated a database from the VB source of truth by running the existing `KordanorsCabal.Scaffolder` (under the .NET 10 SDK with `DOTNET_ROLL_FORWARD=Major`) into the scratchpad, then compared it with the committed `src/KordanorsCabal/boilerplate.db`. Inspected the shape of the static data and how the VB code consumes it.

**Verified:**
- **`boilerplate.db` is exactly what the `Schema` populators produce.** All 49 tables have identical schemas (whitespace-insensitive) and identical rows (no differences in any table). So the db is a faithful, current dump of the 17 populator files (about 1,700 lines of VB), and either may be used as the source; the exporter reads the db.
- **`src/KordanorsCabal/_boilerplate.db` is stale** (older schema: different table names such as `CharacterStatisticTypes`, missing `Lores`/`ItemLores`/`ItemEvents`, 52 item types instead of 53). It can be ignored.
- The Scaffolder runs under the installed SDK and builds a full db in about a second.

**Not verified:** Whether each populator's data matches every use in the code (only db-versus-populator equality was checked); the hand-edited parts of `Schema/Populator.vb` versus the generated `Populate.vb` from the Descaffolder.

**Findings affecting the exporter and the port:**
1. **The static tables contain code-dispatch keys as strings.** `ItemTypeEvents.EventName` has 36 distinct names (`DrinkPotion`, `CanUseEarthShard`, `UseTownPortal`, `FoodDecay`, `ReadNote`, ...), `SpellTypes` has `CastCheck`/`Cast` names (`CharacterCanCastHolyBolt`), and `QuestTypes` has four event names. In VB, `Events.vb` (45 entries: 18 checkers and 27 actions) looks them up in dictionaries keyed by those strings. In Odin the exporter should turn each distinct name into an enum value, and the game code needs a `switch` from that enum to the implementing procedure. Their implementations are the real porting work (task 11/18).
2. **Numeric IDs are used as magic numbers in game and UI code** (71 literal `FromId(worldData, N)` calls, `Constants.CharacterTypes.N00b = 11`, `PlayerModes` 0 to 12, world generation referencing location type 6, direction 5/6, route types 3 to 9, etc.). The exporter must therefore preserve the original IDs and also emit named enum values from the name columns (`Location_Type.Dungeon_Boss`), and the ported code should use the names. IDs must not be renumbered unless the mapping is regenerated.
3. **Some data are really enums**: `Directions` (8, with previous/opposite/next links), `EquipSlots`, `LocationTypes` (with flags `IsDungeon`, `CanMap`, `RequiresMP`), `DungeonLevels`, `ShoppeTypes`, `StatisticTypes` (37, with min/default/max), `RouteTypes` (the abbreviation is a two-character glyph; some are blank), `FeatureTypes`. These become Odin `enum`s plus a `#partial` array of definition structs indexed by the enum.
4. **Some data are tables of relations** that become definition-struct fields instead of separate tables: e.g. `CharacterType` gets its initial stats, attack-type weights, loot weights, parting shots, bribes, enemies, spawn counts and spawn locations as arrays/maps; `ItemType` gets its stats, equip slots, events, shop types, spawn dice and spawn location types.
5. **Free text**: `Lores` (25 paragraphs of prose), `PartingShots` (14 taunt strings), names with embedded quotes (`"Honest" Dan`). The exporter must escape strings correctly (Odin raw strings or `\"` escapes). `MoneyDropDice` and `SpawnDice` are dice strings like `2d8`, `3d6`, `0d1`; they could stay strings (parsed at use) or become a `Dice{count, sides}` struct in the export (decision for task 10/22).
6. **Nullable columns** (`StatisticTypes.DefaultValue`, `Directions.PreviousDirectionId` for non-cardinals) become `Maybe` or a sentinel such as `0`, which has to follow the convention chosen for IDs in task 17.
7. The static data is only about 1,000 rows (about 1,100 lines of Odin once formatted), so the generated file set is small and reviewable in a diff.

**Proposed exporter design (to be built as task 27, not yet written):**
- Input: `src/KordanorsCabal/boilerplate.db`. Language: Odin or Python; Python with `sqlite3` is the least effort for a throwaway tool (Python is available, and the tool is not shipped).
- Output: one generated `content_*.odin` file per group (enums, character types, item types, other) with a header comment naming the exporter and the source db, checked into the repo.
- Emits enums from `*Name` columns (identifier-sanitized, duplicates disambiguated), definition arrays indexed by enum, and relation data folded into the definition structs.
- Re-runnable and deterministic so the result can be diffed; if the VB db changes it can be regenerated.

**Open issues before task 8:**
- None blocking. Task 8 (document how patterns and sprites are defined) is pure reading.
- Task 27 needs the decisions from tasks 10, 14, 17 and 18 before the exporter output shape is final.

### Task 8: how patterns, sprites and rendering are defined (2026-10-03)

**Done:** Read all of `SPLORR.UI` (581 lines), `KordanorsCabal.UI/CharacterSprites.vb`, `RouteSprites.vb`, `RouteTypeExtensions.vb`, `ItemType/Descriptor/*`, `HueUtility.vb`, and the draw path in `Root.vb`. Counted the pattern, bitmap and sprite entries with scripts.

**Verified (by reading and counting; nothing was run):**
- **No image assets are used at all.** Everything on screen is built from 128 hard-coded 8 x 8 PETSCII glyph bitmaps, a 16-colour palette, and a cell grid. `sourcematerial/*.png` is only reference art. There is nothing to load or decode in the port.
- **Glyphs:** `Pattern` is an enum of 128 values; `Patterns.PETSCII` is a list of exactly 128 `PatternBitmap(line0..line7)` entries, indexed by the enum's ordinal (Space is also a real glyph, `Blank` is a separate one). Each line is a byte; **bit 0 is the leftmost pixel** (`ColumnMasks = 1, 2, 4 ... 128` indexed by column), which is mirrored relative to the usual C64 byte order. The `B` glyph is `62, 68, 68, 60, 68, 68, 62, 0`: the C64 `B` is `0x7C = 01111100` per row, and `62 = 00111110` is its mirror, which is consistent with bit 0 being the left edge.
- **Cells:** a `PatternCell` is (pattern, hue, inverted). Its byte form is `pattern` plus 128 if inverted, i.e. the VIC-20 screen-code convention. Drawing a cell sets a pixel to the cell hue where `bitmap XOR inverted` is true, else to the screen hue (white).
- **Screen:** 22 columns x 23 rows of 8 x 8 cells = 176 x 184 pixels, plus a border of 16 px left/right and 28 px top/bottom, for a total frame of **208 x 240**. The window is drawn twice as wide as tall (`ViewWidth * ScreenSize * 2`), i.e. a **2:1 pixel aspect** (comment in `Patterns.vb`: VIC-20 ASPECT_RATIO 2:1). The screen size option is an integer scale (default 4).
- **Palette:** 16 named hues (`Hue` enum), mapped by `HueUtility.HueColors` to RGB (VIC-20 palette, exact hex values are in `HueUtility.vb`). One border hue (default Cyan) and one screen hue (default White).
- **Renderer:** `Renderer.Update` loops over every framebuffer pixel (208 x 240 = about 50k per frame), with a per-pixel dictionary lookup of the hue colour. This is the CPU rasterizer the platform layer will keep; only the final upload (`texture.SetData`) is MonoGame-specific.
- **Text:** `WriteText` maps characters through `CharacterPattern` (a `Char` to `Pattern` dictionary with 100+ entries). Lowercase letters map to the same glyphs as uppercase (the font is uppercase-only), and a few box/suit characters map to special glyphs (`│`, `♠`, `♥`, `┼`, `£`, `↑`, `←`...). Some table entries are commented out, e.g. entries for patterns without a Unicode equivalent that can only be placed by `Pattern` value.
- **Sprites:** there are 15 character-type sprites and 1 route sprite (keyed by type ID, route type 10), each **12 x 12 cells** defined as ASCII art strings: `.` is empty, space is a blank cell, `#` is an *inverted* space, i.e. a solid block of the sprite's hue. So sprites are pure one-bit-per-cell block art with a single hue each. `ShowSprite` copies the cells into the pattern buffer at an XY offset.
- **Item display:** items are shown as a single glyph (`ItemTypeUIDescriptor`: pattern, cell position, hue; 52 item types have one, keyed by item type ID), plus a special subclass for elemental orbs.
- **Other colour tables** are in the UI: `RouteTypeExtensions.TextHue` (route type ID to hue; default Blue); most hue use is `Hue.Black` (149 occurrences) then `Blue`, `Purple`, `Red`, `Orange`, `Green`, `Cyan`, `Yellow`.

**Not verified:** That `PETSCII[i]` and `Pattern` ordinals really line up glyph for glyph (only the counts match, 128 and 128); the full `CharacterPattern` coverage (every character the UI writes has a mapping; `CharacterPattern(character)` throws `KeyNotFoundException` for unmapped ones); the exact RGB values in the palette (they are in `HueUtility.vb` but were not copied here); pixel-for-pixel visual equivalence.

**Findings affecting design:**
1. **The framebuffer is a small CPU-rasterized 208 x 240 RGBA image**, which is the exact thing task 2 and the pixel-readback test proved works on the SDL2 path. The port can keep the design: a 22 x 23 `Cell` buffer, a glyph table, a palette, and a rasterize procedure.
2. **Rendering could instead be done on the GPU** (a texture with all 128 glyphs and a cell-grid shader), but there is no performance reason to: about 50k pixels per frame is trivial in wasm. Keeping the CPU rasterizer also gives byte-exact output that can be compared against the VB build and tested by pixel readback.
3. **Data to carry over as constants:** 128 glyph bitmaps (1,024 bytes), 16 hue RGB values, the `Char` to `Pattern` map, 15+1 sprite grids, 52 item descriptors, and the route/character hue tables. All tiny. Like the seed data (D8), these could be extracted by a script from the VB source rather than retyped; the glyph list and sprite art are plain text patterns that regex-extract cleanly.
4. **Aspect and scaling:** the 2:1 pixel aspect must be reproduced: draw the 208 x 240 frame stretched 2x horizontally, then scale by an integer factor. On a phone this is a presentation choice tied to the touch layout (task 16); a 416 x 240 display area is very wide and short compared with a portrait phone screen, so the layout will use the screen mostly in landscape or letterbox the game in portrait. This needs a decision in task 16.
5. **IDs again:** sprites, item glyphs, hues and route hues are all keyed by raw numeric IDs in UI code (as noted in task 7). In Odin these should be fields on the type definitions (or arrays indexed by the generated enums) rather than separate ID-keyed maps.
6. **Unmapped characters throw** in the VB code; the port should either draw a placeholder glyph or assert in debug builds, since player-typed text and item names go through it.
7. There is no animation system, no per-frame state in the renderer, and no transparency/alpha: the whole frame is redrawn from the cell buffer each update. This makes presenting trivial for both targets and makes a unit test of "cells in, pixels out" easy.

**Open issues before task 9:**
- None blocking. Task 9 (maze and level generation) is pure reading. Resolve the 2:1 aspect question in task 16, and decide in task 27/the content exporter that glyphs, palette and sprites also get a one-off extractor.

### Task 9: maze and level generation (2026-10-03)

**Done:** Read `SPLORR.Game/Maze/*` (the maze generator), `SPLORR.Game/RNG.vb`, and all of the world generation in `KordanorsCabal.Game/World.vb` (`Start`, town, dungeon, moon, features, player). Searched for every place entities are created outside world generation. Cross-checked route counts against the measured new game from task 6.

**Verified (by reading, plus a route-count check against the measured data):**
- **Everything procedural is generated once, at New Game, and then stored.** `World.Start()` calls `worldData.Reset()`, then town, dungeon, moon, features and the player. Load copies a saved database; nothing is regenerated on load. So the generated map has to be part of `World_State` (it is already part of the 19 mutable tables).
- **Maze algorithm:** randomized Prim's algorithm on an 11 x 11 grid (121 cells): pick a random start cell, keep a frontier list, repeatedly take a random frontier cell and open a door to a random neighbour already inside. The result is a **spanning tree**: 120 open doors, so no loops and every cell reachable. Four directions only (ids 1 to 4: N, E, S, W with deltas (0,-1), (1,0), (0,1), (-1,0)); the table `MazeDirections` is hard-coded in `World.vb`. The `Maze` class is generic over a direction type and only 58 lines. It uses `List.Contains` (linear) which is fine at 121 cells.
- **Route arithmetic matches the measured new game exactly:** 5 dungeon levels x 120 doors x 2 directions = 1,200; the moon is an 11 x 11 torus with 4 routes per cell = 484; the town is 12 stitched pairs plus the church entrance pair = 26; the cellar pair = 2; 5 level-to-level connections x 2 = 10. Total 1,722, which is exactly the `Routes` row count measured in task 6. Locations: 5 x 121 + 121 moon + 9 town + church entrance + cellar = 737, also exact.
- **World layout:** town (9 locations in a ring around the town square, plus a church entrance attached at a random free cardinal direction of a random town location); a cellar location under the innkeeper's feature location; five dungeon levels, each a fresh maze; the "up/down" routes connect each level's boss room to the next level's random entry cell, the first level to the church entrance; and the moon, a separate wrap-around 11 x 11 grid of route type 11 with no maze (reached by the Moon Portal item, item type 29; its event destroys any route in that direction at the current location and links it to a random moon location).
- **Location typing:** after the maze is built, a cell with exactly one open door becomes a dead end (type 5). A random dead end becomes the boss room (type 6) and its inbound route gets the level's boss route type (5 to 9). Every other dead end's inbound route is locked with route type 4 (the FE door) and a key item is spawned for it; the first key slot is replaced by the level's boss key item type. Item type 1 is `FE Key`, which fits the 208 `FE Key` items counted in the measured new game of task 6 (not checked item by item).
- **Population:** `PopulateCharacters` spawns `SpawnCount` characters per character type per level at random locations whose type allows it (`CanSpawn`); `PopulateItems` rolls each item type's `SpawnDice` per level and `SpawnItem` drops the item into a random allowed location's ground inventory. The player starts in the town square and rolls stats with two weighted-dice tables (`FirstRollTable`, `SecondRollTable`). Features are each placed at a random free location of the feature type's location type.
- **Entities are also created and destroyed at runtime**, so the world is not static after generation: `Events.vb` creates rats (cellar-rats quest, repeatable with growing counts), creates and destroys **portal routes** at runtime (`UseTownPortal`, moon portal: it destroys any existing route in that direction at the current location and at the destination and creates a pair of route-type-10 routes), and creates items (loot drops, shop purchases, drunk and chicken events). Combat deletes characters; items are consumed.
- **Randomness:** one global unseeded `System.Random` (`RNG`): `FromRange`, `FromList`/`FromEnumerable`, `FromGenerator` (weighted table of value to weight; the sum of the weights is the range), `RollXDY` and `RollDice`/`MaximumRoll`/`ValidateDice`. The dice parser supports strings like `2d8`, sets joined by `+` (`1d6+2d4`), a `*N` multiplier, a `/N` integer divisor, a negative die count (subtracts), and any of `d`/`D`. `RNG.` is used in 11 files and 37 call sites.

**Not verified:**
- Nothing was run to confirm the algorithm properties empirically (spanning-tree property, dead-end counts per level, key counts); they follow from the code and agree with the totals measured in task 6.
- Whether some quest or event code depends on generation order (for example `Location.FromLocationType(...).Single` assumes exactly one cellar, one town square, one church entrance).
- Edge cases in `PopulateCharacters` (an empty `candidates` list would throw in `FromList`), and what happens if `Single`/`First` assumptions fail.
- Behaviour with 0 dice sides or unusual dice strings in `RollDice`.

**Findings affecting design:**
1. **Procedural generation is small and self-contained** (about 60 lines of Prim's maze plus about 150 lines of world setup). It ports directly. It only needs: the RNG, a grid, and the content tables from task 7 (spawn counts, spawn location types, spawn dice).
2. **A seedable RNG is needed.** The VB build is unseeded and cannot be matched number-for-number (Odin's generator differs), so equivalence tests (task 22/23) have to be **structural invariants**, not exact outputs: 121 cells per level, 120 doors, a spanning tree, reachable boss room, one boss per level, one key per locked dead end, character counts equal to `SpawnCount`, 737 locations and 1,722 routes at new-game, and so on. These are cheap to test natively with a fixed seed.
3. **Entity creation and deletion happen at runtime**, in combat, shops, loot and events, so `World_State` needs stable entity IDs and a deletion policy (free list vs. tombstone vs. generation counter). This is the main input to task 17. A pooled array with generation counters would also protect against stale references from events.
4. **Portals mutate the route graph during play**, so routes cannot be assumed immutable; per-location route slots (one per direction, up to 8) are a natural fit: replace or clear the slot.
5. **Dice strings are parsed at use** and appear in the content data (`MoneyDropDice`, `SpawnDice`) and in code. A small `Dice` value type plus a parser/roller is enough; parsing can be done once at export (task 7/27) so the runtime never parses strings. The `*N` and `/N` forms need to be checked against the data (checked: every dice string in `boilerplate.db`, i.e. `MoneyDropDice` and `SpawnDice`, matches the simple form `XdY`: 0d1, 1d1, 1d6, 2d3, 2d4, 2d6, 2d8, 3d6, 3d8, 4d6, 4d8, 5d1, 5d8), so a `Dice{count, sides}` struct covers all of the shipped data; the `+`, `*`, `/` and negative forms are not used by the content and would only matter if code calls `RollDice` with literals.
6. The world uses **magic IDs for directions, route types and location types** throughout generation, as noted in task 7; the ported generator should use the enum names.
7. **Generation runs once per new game** and takes well under a second in VB (the 11 x 11 mazes are tiny), so there is no need for an async or incremental generator in the wasm build.

**Open issues before task 10:**
- None blocking. Task 10 (polymorphic parts: item types, character types, modes) is pure reading.
- Decisions this task feeds: entity ID/deletion policy (task 17), seedable RNG (task 21), structural invariant tests for generation (task 22/23).

### Task 10: catalogue of the polymorphic parts (2026-10-03)

**Done:** Listed every `Inherits`/`MustInherit`/`Overridable`/`Overrides` in the VB projects, read the base classes (`BaseThingie`, `BaseItemType`, `SubcharacterBase`, `ModeProcessor`, `MenuProcessor`, `BaseProcessor`, `ShoppeProcessor`), `IProcessor`, `MainProcessor`, `UIState`, `Command`, `CommandUtility`, `Button`, `InPlayProcessor`, the item and character type implementations, and the UI descriptors. Queried which item stats and events exist in the content data. Counted special-casing of IDs in code.

**Verified (by reading and queries; nothing was run):**

**1. Domain layer: there is almost no real inheritance.**
- `BaseThingie` (an ID plus the `IWorldData` handle) is the base of about 45 wrapper classes (`Character`, `Location`, `Item`, `Route`, `ItemType`, `CharacterType`, ...); `SubcharacterBase` is the base of the 14 per-character facets (`CharacterHealth`, `CharacterMana`, `CharacterEquipment`, ...). They are **views over an ID**, not objects with their own state, and each has a mirror interface (`ICharacter`...) that exists mainly for mocking.
- The only override in the domain is `PlayerCharacter.EnqueueMessage` (the player queues messages for the UI; other characters ignore them). `BaseItemType` is `MustInherit` but has one subclass.
- **Item behaviour is data-driven, not polymorphic.** Weapon, armor, equipment, durability and repair are sub-objects *computed from item type statistics* (the README TODO "IItem subobjectification" is about this). The content has only 9 item stat kinds (`SingleUse` 53 rows, `Price`, `Offer`, `Encumbrance`, `RepairPrice`, `MaximumDurability`, `MaximumDamage`, `DefendDice`, `AttackDice`), plus equip slots (17 item types are equippable), events (19 item types have events), shop types, and 5 stat buffs. So an Odin `Item_Type` is one struct of optional fields; no unions needed.
- **Character types are also data-driven** (stats, attack weights, loot, enemies, spawn tables, `IsUndead`). Code special-cases only two character types by ID: the player (`N00b`, 2 uses) and `Rat` (4 uses, for the cellar quest).
- Code refers to about a dozen item types by constant (`ItemType26` 8 uses, `ItemType37`, `ItemType30`, `ItemType24`, ...), e.g. keys, the note, the drunk's item.

**2. Event dispatch by string name (45 entries: 18 checkers and 27 actions, in a 449-line `Events.vb`; corrected in task 11, an earlier count of 92 was a count of `Function`/`Sub` keywords).** Two dictionaries keyed by name: checkers (`Func(IWorldData, Long()) As Boolean`, for `Can...`, `Is...`, `Has...` names, `AlwaysTrue`) and actions (`Sub(IWorldData, Long())`). Parameters are a `Long()`, where `[0]` is a character ID. Names come from the data (item type events: 36 distinct names; spells: cast checks and casts; quests: four events). They are the only place where game *rules* beyond stats live (portals, learning spells, eating, food decay, reading notes, beer, etc.).

**3. Player modes (persisted): 13 values, `0..12`.** `Constants.PlayerModes`: None, Neutral, Turn, Move, then 9 townsfolk modes (Elder, InnKeeper, TownDrunk, Chicken, BlackMarket, BlackMage, Blacksmith, Constable, Healer). The mode ID is stored in the `Players` row. `InPlayProcessor` keeps a dictionary mode ID to `ModeProcessor`; a `ModeProcessor` implements four members: `UpdateBuffer`, `UpdateButtons`, `HandleButton`, `HandleRed`. The 12 mode processor classes are about 1,000 lines total. Current shoppe type is also saved in `Players`.

**4. UI state machine (not persisted):** `UIState` has 33 values (`None` plus 32 screens); `MainProcessor` holds a dictionary of 32 processors, one per screen (`FinalizeCharacterProcessor` is one class registered twice, with different parameters, for Finalize and LevelUp; corrected in task 12, the earlier text had these two counts swapped). `IProcessor` has three members: `ProcessCommand(world, command) -> next UIState`, `UpdateBuffer(world, buffer)`, `Initialize()`. Base classes: `BaseProcessor`; `MenuProcessor` (a list of (label, `Func(Of UIState)` closure), a prompt, `HandleRed` override, up/down/select behaviour); `ShoppeProcessor(Of T)` (generic list screen). Processors hold their own mutable state in instance fields (selected row, etc.); `ModeProcessor.Buttons` and `CurrentButtonIndex` are **shared static mutable state**. `Root` provides `PushUIState`/`PopUIState` hooks (a stack for sub-screens like Message).

**5. Input is already abstract and gamepad-like.** `Command` has 12 values: Up, Right, Down, Left, Green, Blue, Yellow, Red, Start, Back, NextItem, PreviousItem. `CommandUtility.KeyCommands` maps keys (arrows and numpad to directions; Space/NumPad5 = Green, Enter = Blue, Escape = Red, Tab = Yellow, Backspace = Back, F10 = Start, comma/period = Previous/Next item). Green and Blue both mean confirm; Red means cancel/back.

**6. On-screen buttons are cell-based.** In-play `Button`s have a title, a cell position, a width in cells and an index; they are drawn into the pattern buffer (orange, inverted when selected). Menus are rows of centered text. All interactive elements are therefore already **known cell rectangles at draw time**.

**7. Other:** `ItemTypeUIDescriptor` (item glyph, position, hue) with one subclass `ElementalOrbUIDescriptor` whose `DisplayHue` calls `RNG.FromEnumerable` *on every draw* (so the orb's colour flickers randomly each frame, unlike everything else, which is deterministic); `Sfx` enum plus `SfxPlayer` event (`PlaySfx`) for sound effects; `Events` raised through `IEventData` in the Data layer.

**Not verified:** Whether `HandleButton` implementations contain much shared or duplicated logic that would be better factored; the exact rules in the 92 event bodies (that is task 11); whether any UI processor stores state that must survive save/load (apparently not: only the world data is saved).

**Findings affecting design (answers the "tagged union vs. data table" question from task 4):**
1. **Persisted polymorphism is only enums.** The only polymorphic value in the save is the player mode (a number) and, indirectly, event names (which come from static content). So the union-in-saved-data problem from task 4 mostly does not occur: save a `Player_Mode` enum (append-only, versioned) and rebuild everything else from static content.
2. **Polymorphism in the UI and rules is code dispatch**, which in Odin is a `switch` on an enum, or an array of structs of procedure pointers indexed by the enum (a hand-rolled vtable). Suggested shapes:
   - `UI_State` enum with a table of `Processor{ process_command, update_buffer, initialize }`; processor-local state in tagged unions or a per-screen struct (not persisted, so tagged unions are fine here).
   - `Player_Mode` enum with a `Mode_Processor` table of four procs.
   - `Event_Name` enum (generated by the exporter, D8) with `switch`-based checker and action procs taking `(world, params)`.
3. **Closures:** menus capture lambdas (`Function() SaveSlot(1)`). Odin procedures cannot capture, so menu items become (label, proc, int argument) or an enum of actions with a `switch`.
4. **Wrapper classes are not needed.** Since they are views over IDs, Odin code should use plain procedures taking the world and an ID (or pointer). The 14 character facets collapse into procedures grouped in files, and the mirror interfaces disappear.
5. **Shared static UI state** (`Buttons`, `CurrentButtonIndex`) should become fields of an explicit `UI_State_Data` struct in the platform-independent core, so the core is pure `step(input) -> frame` and testable natively.
6. **Touch input mapping is straightforward**, which feeds task 16: the 12 `Command`s can be produced by an on-screen D-pad plus Green/Blue/Red/Yellow buttons drawn outside the game frame, and/or by tapping cell rectangles (menu rows and in-play buttons) which the UI already knows when it draws them. Because buttons are recorded with position and size, a tap-to-button hit test is trivial to add.
7. **The elemental orb's random per-draw colour** needs a decision: drive it from a frame counter (deterministic, testable) rather than the game RNG, so rendering never consumes game randomness (otherwise screenshots and seeded tests would change with the number of redraws).

**Open issues before task 11:**
- None blocking. Task 11 (every event raised and who consumes it) is the detailed reading of `Events.vb` and the event plumbing in the Data layer and `ItemEvents`.
- Decisions this feeds: event dispatch (task 18), UI structure and touch input (task 16), entity handles (task 17).

### Task 11: events, who raises them and who consumes them (2026-10-03)

**Correction to earlier logs:** `Events.vb` has **45 entries (18 checkers, 27 actions)**, not 92. The 92 in tasks 7 and 10 was a count of `Function`/`Sub` keywords. The earlier entries are fixed in place.

**Done:** Read `Events.vb` (all 449 lines), `IEventData`, `ItemEvents`, `Inventory.Add`, `Location.DecayItems`, `Route`, `QuestType`, `SpellType`, `CharacterItems`, the `Sfx` enum and `SfxPlayer`, `PlayerCharacter`/`Message`, `MessageProcessor`, and the sound effect table in `Root.vb`. Cross-checked every event name in the content data against the two dispatch tables with a script, and searched all call sites.

**There are four different mechanisms called "events" in the VB code:**

**A. Named rule events (`IEventData.Test` / `Perform`) — the real rules engine.** A name from content (or hard-coded) is looked up in `checkerTable` (returns Boolean) or `actionTable` (does something). The parameter list is a `Long()` whose meaning depends on the caller:

| Raised by | Event kind | Name comes from | Parameters |
|-----------|-----------|-----------------|-----------|
| `ItemEvents.CanUse` | checker | `ItemEvents` row, event id 2 | `[characterId]` |
| `ItemEvents.Use` | action | event id 3 | `[characterId, itemId]` |
| `ItemEvents.Decay` | action | event id 4 | `[itemId]` |
| `ItemEvents.Purify` | action | event id 1 | `[itemId]` |
| `Inventory.Add` | action | `ItemTypeEvents`, event id 5 | `[inventoryId, itemId]` |
| `Location.DecayItems` | action | hard-coded `"LocationDecayItems"` | `[locationId]` |
| `SpellType.CanCast` / `Cast` | checker / action | `SpellTypes` columns | `[characterId]` |
| `QuestType.CanAccept` / `Accept` / `CanComplete` / `Complete` | checker / action | `QuestTypes` columns | `[characterId]` |

Consumers of those entry points: `CharacterItems` (use item: `CanUse`, then `Use`, then destroy the item if the `SingleUse` stat is set; purify calls `Purify`), `InteractItemProcessor` (UI asks `CanUse` to decide whether to enable Use), `Route.vb` (moving the player into a new location calls `Location.DecayItems`, which runs the decay event of every item lying on the ground there), spellbook and quest code, and the UI's spell and quest screens.

**B. The item event ids** are code constants in `ItemEvents`: 1 Purify, 2 CanUse, 3 Use, 4 Decay, 5 AddToInventory. In the content data, `ItemTypeEvents` has **no rows with event id 5**, so the add-to-inventory hook is **dead code** that never fires. Counts per event id: Purify 1 name (`PurifyFood`), CanUse 19 rows / 14 names, Use 19 rows / 19 names, Decay 2 names (`FoodDecay`, `RottenFoodDecay`). `ItemEvents` (the per-item table, 158 rows in a new game) is filled when an item is created, copying that item type's rows.

**C. Domain notifications through `SfxPlayer`.** `Sfx` has 8 values (CharacterCreation, EnemyDeath, EnemyHit, LevelUp, Miss, PlayerDeath, PlayerHit, UnlockDoor). `SfxPlayer.Play(sfx)` raises a `PlaySfx` .NET event; the only subscriber is `Root` (adds a handler and plays a `SoundEffect` at the saved SFX volume; `CharacterCreation` maps to `RollDice.wav`, the others to wavs of the same name). Callers: `World.RollUpPlayerCharacter` (`CharacterCreation`), `Route` when a locked door is opened (`UnlockDoor`), `MessageProcessor`, and `Root`'s volume slider (plays `CharacterCreation` as a preview). Combat sounds go through messages, not directly.

**D. The player message queue.** `PlayerCharacter.EnqueueMessage(sfx?, lines...)` pushes a `Message` (lines plus optional `Sfx`) onto a **shared static** `Queue(Of Message)` (27 calls in `Events.vb`; 20 more in combat, equipment, spellbook, quests and interaction code). Other characters' `EnqueueMessage` is a no-op. Consumers: UI processors that did something return `UIState.Message`; `MessageProcessor` shows the first message, and **plays its sfx during `UpdateBuffer`** (a draw-time side effect), then Green/Blue/Red dequeues it; when empty it goes to `Dead` if the player died, else pops the UI stack. The queue is **not saved**; messages pending at save time are lost, and nothing clears it on load or new game (the queue is static, so abandoning a game with pending messages could show them in the next).

**Verified:**
- **Every event name used in content resolves.** All names in `ItemTypeEvents`, `SpellTypes` (cast check and cast) and `QuestTypes` exist in the tables: no missing handler.
- No name is in both tables. Item-event ids line up with the table kind: all CanUse (id 2) names are in the checker table and all others are in the action table.
- The only table entry not referenced by content is `LocationDecayItems`, which is called by name from code.
- **Distinct names:** 36 in item events (14 check + 19 use + 2 decay + 1 purify; `AlwaysTrue` among the check names), plus spell checks/casts (2 + 2) and quest events (4). 45 handlers in total.
- There is no general pub/sub bus: nothing subscribes to or unsubscribes from events except the single `PlaySfx` handler.

**Not verified:** The correctness of individual handler bodies (read for structure only; nothing was executed); whether `FoodDecay` and `RottenFoodDecay` run only on ground items (they appear to, via `LocationDecayItems`, so food in the player's inventory never decays); whether pending messages leak across new games (inferred from the static queue).

**Findings affecting design:**
1. **A handler is a pure function of (world, params).** It needs only the world state, a parameter list and a way to add messages and play sounds. In Odin: `Event_Name :: enum` (generated by the exporter, D8) with two `switch`-based procedures, `event_test(world, name, params) -> bool` and `event_perform(world, name, params)`. Using a fixed small parameter struct (for example `Event_Params{character, item, inventory, location: Maybe(ID)}`) is cleaner and safer than an unlabeled `Long()` whose meaning depends on the caller.
2. **Split checker names from action names in the enum** (two enums or a flag), so a content error can be caught at export time rather than becoming a runtime `KeyNotFoundException`. The cross-check above shows the current data is consistent.
3. **Output events become a queue in the core**, drained by the platform layer each frame: `Sfx` events (an enum list) and `Message`s (lines plus optional sfx). This replaces both the .NET event and the static queue. Putting the message queue in an explicit core struct (not in `World_State`) also fixes the leaking-queue problem; whether to save it is a design decision (saving it would let you save mid-message, which the VB game cannot do).
4. **Playing sound during `UpdateBuffer` must move out of the draw path**: the core's `step` should emit "play this sfx" outputs when the message is *first shown* (a state change), not when it is drawn, so redraws and tests do not repeat it.
5. **The dead add-to-inventory hook can be dropped** (no content uses id 5), unless task 27's exporter or the author wants to keep it. Drop it and say so.
6. The exporter (task 27) should generate the `Event_Name` enum from the union of all event names in the content, plus `LocationDecayItems`, and flag any name in content without a handler.
7. **Several handlers create or destroy entities and routes** (rats, portals, item type changes like food rotting by writing `Item.ItemType`), so they depend on the entity-handle decision (task 17). Food rotting (`FoodDecay`) changes an item's type in place to item type 35, which means an item's type is mutable state, not fixed at creation.
8. Handlers call RNG (`RollDice("1d3")`, `FromGenerator`), so event handling must go through the seedable RNG (task 21), which helps structural tests.

**Open issues before task 12:**
- None blocking. Task 12 (list all UI states/screens and the commands each accepts) is mostly reading, starting from the `UIState` list in task 10.
- Decisions this feeds: event dispatch shape and parameter struct (task 18), message/sfx output queue (task 18), entity handles (task 17), seedable RNG (task 21).

### Task 12: UI screens and the commands each one accepts (2026-10-03)

**Correction to task 10:** `UIState` has **33 values (`None` plus 32 screens)** and `MainProcessor` registers **32 processors**; task 10 had these swapped. Fixed in place.

**Done:** Extracted, with a script, for every `*Processor.vb` (49 files): base class, line count, the `Command` values it references, the `UIState` values it can return, and the members it overrides. Then read `Root.ProcessInput`/`Update`, `MainProcessor`, `MenuProcessor`, `ModeProcessor` (buttons), `NeutralModeProcessor`, `MapProcessor`, `MessageProcessor` and the push/pop call sites.

**How input reaches a screen (verified in `Root.vb`):** every `Update`, the keyboard's currently pressed keys are mapped to `Command`s; only **newly pressed** commands (not in last frame's set) are sent to the current screen, one at a time. So there is **no key repeat**: holding a key does nothing more. The order for several new commands in one frame comes from a `HashSet`, i.e. is not defined. After the commands, **`UpdateBuffer` of the current screen runs every frame** and the whole 22 x 23 buffer is rasterized every frame. When `ProcessCommand` returns a new state, `MainProcessor` calls that state's `Initialize()`. Returning `UIState.None` exits the program (only `ConfirmQuit` does this).

**Commands actually used:** `Up`, `Down`, `Green`, `Blue`, `Red` everywhere; `Left`/`Right` **only** on the in-play screen (they jump between the two button columns). **`Yellow`, `Start`, `Back`, `NextItem` and `PreviousItem` are mapped to keys but handled by no screen at all.** `Green` and `Blue` always mean *confirm*; `Red` means *cancel/back* (and on some screens the only way out). The touch UI therefore needs just: up, down, left, right, confirm, cancel (6 inputs, or 7 if both colours are kept distinct), plus direct tapping.

**The 32 screens, by group** (base class in parentheses; "Menu" = a vertical menu of centered labels with Up/Down/Green+Blue/Red; "Page" = a screen where Green/Blue/Red just dismiss or advance; "List" = a scrolling item list with Up/Down/Green+Blue/Red):

| Group | Screen | Kind | Leads to |
|-------|--------|------|----------|
| Boilerplate | `TitleScreen` | Menu | FinalizeCharacter (new game), LoadGameScreen, OptionsScreen, InstructionsScreen, AboutScreen, ConfirmQuit |
| | `AboutScreen`, `InstructionsScreen` | Page | TitleScreen |
| | `ConfirmQuit` | Menu | `None` (exit) or TitleScreen |
| | `OptionsScreen` | Menu | ScreenSizer, SfxVolumizer, MuxVolumizer, TitleScreen |
| | `ScreenSizer`, `SfxVolumizer`, `MuxVolumizer` | Menu (adjust a value) | OptionsScreen |
| | `GameMenuScreen` | Menu | SaveGameScreen, ConfirmAbandonGame, InPlay |
| | `SaveGameScreen` (5 slots), `LoadGameScreen` (5 slots) | Menu | InPlay / GameMenuScreen / TitleScreen |
| | `ConfirmAbandonGame` | Menu | TitleScreen or InPlay |
| Character | `FinalizeCharacter` and `LevelUp` (the same class twice) | Menu (assign stat points) | Prolog / InPlay |
| | `Prolog` | Page | InPlay |
| | `Status` | Page | InPlay |
| | `Dead` | Page | TitleScreen |
| Play | `InPlay` | **Hub**: draws the current mode and 10 buttons | everything below |
| | `Message` | Page (queue of messages) | pops the UI stack, or Dead |
| | `Map` | Page (any command returns) | InPlay |
| | `Enemies`, `GroundInventory`, `Inventory`, `Equipment`, `SpellList` | List | InPlay / sub-screens / Message |
| | `InteractItem`, `EquipmentDetail` | Menu (Use, Drop, Equip...) | Inventory, Equipment, Message, InPlay |
| Shoppe | `ShoppeOffers` and `ShoppePrices` | List (read only; Red leaves) | InPlay |
| | `ShoppeBuy`, `ShoppeSell`, `ShoppeRepair` | List (act on an item) | InPlay |

**The `InPlay` hub in detail.** It is one screen whose content depends on the **player mode** (task 10: 12 mode processors): `Neutral` (exploring), `Turn`, `Move`, and nine townsfolk. All of them draw into the same layout: the dungeon "tunnel" artwork and sprites in the upper 18 rows, then a fixed bank of **10 buttons** (`Button(index, title, cell position, width 11)`: two columns of five, at cell positions `(0,18..22)` and `(11,18..22)`). Button titles are recomputed every frame from game state (for example "FIGHT!" vs "Turn...", "Level up!" vs "Status", "Enemies(3)" vs "Ground..."); an empty title means the button is inactive. Up/Down cycle through the 10 buttons, Left/Right jump to the other column, Green/Blue run `HandleButton(button)` for the current mode, Red runs `HandleRed` (in Neutral it just resets the mode). `CurrentButtonIndex` is shared static state with its own small stack (`PushButtonIndex`/`PopButtonIndex`) so a sub-mode can return the cursor to the button that opened it.

**The UI is also the game controller.** `HandleButton` and similar handlers call game actions directly (for example `PhysicalCombat.Fight()`, `Interaction.Interact()`, buying and selling), set the player mode, enqueue messages and then choose the next `UIState`. A recurring pattern is `PushUIState(UIState.InPlay)` followed by `Return UIState.Message`: it shows the queued messages and, when they are dismissed, `MessageProcessor` pops the stack to return to InPlay (11 call sites: combat, movement, townsfolk, spells, shops). There is **no separate game-logic layer between UI and Game** for these actions; the Game layer exposes the actions (`Fight`, `Run`, `Interact`...) and the UI sequences them.

**State that lives in the UI layer and is not saved:** the UI state stack (in `Root`), the current screen, button cursor, menu cursors, list scroll positions, `MapProcessor.redrawBuffer`, and the message queue (task 11). Save is only offered from `GameMenuScreen` reached via the `Game Menu` button in Neutral mode, so there is nothing mid-action to save. Loading goes to `InPlay`.

**Verified:** The screen list, transitions and commands above come directly from the script output and the files named (not executed). Counts: 33 `UIState` values, 32 registered processors, 49 processor files, 12 `Mode` processors.

**Not verified:** The exact destination for every command on every screen (the table is the union of possible return states per screen, not a full transition table); the layout and text of each screen; what `Back`/`Start`/`Yellow` were meant to do (they appear to be unfinished); whether `HashSet` order ever matters in practice (only if two keys are newly pressed in the same frame).

**Findings affecting design:**
1. **A single `ui_step(input_command)` and a single `ui_draw(buffer)` fit the structure.** Odin's core can keep the same shape: a `UI_State` enum, a `Processor` table of procedures, and one explicit `UI_Data` struct that holds the current state, the push/pop stack (a small fixed array), cursors and scroll positions. Redraw every frame as the VB does (it is cheap), but keep `ui_draw` free of side effects (task 11, finding 4) and compute anything stateful (e.g. the map's redraw flag) as part of `ui_step`.
2. **Because input is edge-triggered commands, touch maps cleanly onto it.** A tap on a menu row or a button rectangle is "select that item, then confirm"; a D-pad and Confirm/Cancel on screen produce the same `Command`s; there is no continuous or analogue input. The 6 or 7 used commands make a small on-screen control pad feasible (task 16).
3. **Every interactive element is a cell rectangle.** Menus: one row per item, centered text; in-play: the 10 fixed 11 x 1 rectangles; lists: one row per entry. The core can expose "hit regions" (a list of rectangles with an action) produced by `ui_draw`, so the platform layer can turn a tap into either a `Command` sequence or a direct "activate item N" event. The hit regions must be derived from the same data that draws them.
4. **`Yellow`, `Start`, `Back`, `NextItem`, `PreviousItem` can be dropped or kept as unused enum values** (they have no behaviour). Dropping them simplifies the touch controls; decide when defining the command set (task 16/18).
5. **Game logic is called from UI handlers**, so the first ported version can keep that structure (UI procs call game procs) as long as both live in the platform-independent core and the platform layer only passes commands in and reads the frame, sounds and saves out. That keeps `step` testable natively by scripting commands.
6. **The push/pop stack is used for exactly one idea** ("after the messages, return to where I was", plus the spell list return). It can be a fixed array of 4 or so `UI_State`s in `UI_Data`; the pop-with-nothing case needs a defined behaviour (VB would throw on an empty stack).
7. **Load/Save use 5 fixed slot names** and Save is only reachable from the in-play menu, so slot handling in the shim (task 5) is simple: keys `kc:slot1..5` and a config key; the slot list screen needs to know which slots are occupied (`storage_get` for existence).

**Open issues before task 13:**
- None blocking. Task 13 (everything that reads or writes state outside `IWorldData`, e.g. `UIConfig` for screen size and volumes) is a short reading task.
- Decisions this feeds: the touch input model and layout (task 16), the platform interface's input event type (task 15), the UI core structure (tasks 17, 18).

### Task 13: state and I/O outside `IWorldData` (2026-10-03)

**Done:** Searched all non-test, non-generated VB source (excluding `obj/`, the Scaffolder tools and the Schema populators) for file, time, environment, thread and console use, and for every `Shared` (static) mutable member, module-level variable and long-lived field. Read `UIConfig`, `Program`, `Root` (fields, init, input, sfx, volume), `StaticWorldData`, `Store`/`StoreMeta`/`Backer`, and the save/load screens.

**Verified (by reading and grep; nothing executed):**

**1. Everything that touches the outside world.**
- **Only one file is read or written by game code directly: `config.json`** (`UIConfig`, relative to the working directory). It holds `ScreenSize` (int, default 2), `SfxVolume` and `MuxVolume` (floats, default 0.5), serialized with `System.Text.Json`. A missing or unreadable file silently yields the defaults. It is **saved immediately on every change** and **re-read from disk on every use** (each volume read calls `UIConfig.Load`).
- **SQLite files**, reached through `IWorldData` and `Store`: `boilerplate.db` (the read-only new-game template, opened from the working directory) and `SaveSlot1.db` to `SaveSlot5.db` (slot names come from `UI/Utility/SaveSlotName`).
- Content files read by `Root`: `Content/*.wav` and `Content/MinorTheme.ogg`.
- **There is no network, console, threading, timer, environment-variable, registry or process use at all, and no wall-clock gameplay.** The only time call is two `DateTimeOffset.Now` lines in `World.CreateDungeonLevel` whose values are never used (dead code). The game is entirely turn-based and driven by commands, so apart from the RNG it is deterministic.

**2. How the live world is held (important, not obvious from `IWorldData`):**
- The working database is an **in-memory SQLite database**. `New Game` and `Load` do `Reset`: close, open `:memory:`, and copy a template file into it (`boilerplate.db` for a new game, `SaveSlotN.db` for a load). `Save` copies the in-memory database out to the slot file. So there is a full copy on every save and load, and no incremental persistence.
- There is **one global world**: `StaticWorldData.WorldData` is a module-level singleton used by the UI everywhere (processors reach the world through it, as well as through arguments). It is constructed at startup with `New Game.Events`.
- **Slot occupancy is detected by loading each slot into the live world.** The Load screen calls `WorldData.Load(slot)` for slots 1 to 5 and checks `World.IsValid` (a `Players` row exists). The Save screen does the same but swaps the live connection out and back (`WorldData.Renew()` / `Restore(old)`) to avoid destroying the current game. A slot's label is "Slot N" or "(empty)". Both screens cache the result in a `Validated` static flag; the Load screen never resets it, so its labels can be stale for the rest of the session (slots saved after the first visit still read "(empty)" until restart; loading still works because `ContinueSlot` re-checks validity).

**3. Static/shared mutable state (everything that lives outside the world data):**

| Where | What | Saved? |
|-------|------|--------|
| `RNG` | one unseeded `System.Random` | no |
| `PlayerCharacter.Messages` | queue of pending messages (task 11) | no |
| `ModeProcessor` | `Buttons` (10 buttons with changing titles), `CurrentButtonIndex`, a button-index stack | no |
| `Root` | `uiState`, `uiStack` (push/pop), `pressedCommands` (edge detection), `ScreenSize`, music `Song`, loaded `SoundEffect`s, the framebuffer texture | config only (screen size) |
| `InteractItemProcessor.InteractItem`, `EquipmentDetailProcessor.EquipSlot` | **screen "arguments" passed as static properties** (which item/slot the next screen is about) | no |
| `Load/SaveGameScreenProcessor.Validated` | slot label caches | no |
| Per-processor fields | list contents and cursors: `EquipmentProcessor`, `GroundInventoryProcessor`, `InventoryProcessor`, `SpellListProcessor`, `MapProcessor.redrawBuffer`, `FinalizeCharacterProcessor.prompt`, `MenuProcessor.currentItem` | no |
| `*Volumizer/ScreenSizer Processor.Get/Set...` | **function-pointer hooks** assigned by `Root` so the options screens can read and write the window/volume | via config |
| `MainProcessor.PushUIState/PopUIState` | function-pointer hooks assigned by `Root` (stack in `Root`) | no |
| `SfxPlayer.PlaySfx` | .NET event with `Root` as the only subscriber | no |
| `StaticWorldData.WorldData` | the global world | the world, via save slots |

**Not verified:** What exactly happens when `Load` opens a slot file that does not exist (`Microsoft.Data.Sqlite` creates an empty file; whether reading `Players` then throws or is handled by `NoInitializer` was not traced); whether the `ScreenSize` default conflict (the field defaults to 4, the config default is 2, the config wins at startup) matters; the stale Load labels were inferred from the code, not observed.

**Findings affecting design:**
1. **The "outside state" is small and well-bounded**: one config of 3 numbers, five save slots, and a short list of UI statics. Nothing hidden in time, environment or threads. The port's platform interface (task 15) really does only need: storage get/set/remove, time source for dt and entropy, input events, framebuffer present, sound and music.
2. **Replace the global/static state with one explicit `Game` struct** passed to `step`: `World_State` (saved), `UI_Data` (screen, stack, cursors, button titles, screen arguments, cached lists), `Config` (volumes, scale), `RNG_State`, and the output queues (sfx, messages). Screen "arguments" (`InteractItem`, `EquipSlot`) become fields of `UI_Data`; the function-pointer hooks and the .NET event disappear because the core can read the config directly and the platform reads the output queues.
3. **Config persistence**: a tiny JSON object under one storage key (`kc:config`), loaded once at start and saved when changed (not re-read per use). Defaults on missing or corrupt data, as today. Volume and scale changes should also be pushed to the platform layer explicitly.
4. **Save slots become independent JSON blobs** under `kc:slot1..5`; there is no need to load a slot into the live world to see if it is occupied. Check existence (and, ideally, store a small summary such as character level and location type for the slot label) with a cheap `storage_len`/get. This removes the `Renew`/`Restore` dance and the stale-label bug. Saving writes `World_State` JSON only; the in-memory copy of a whole database goes away.
5. **No time source needed in the core except `dt`**, and nothing in the game depends on it; the core can be driven by a fixed step. Frame timing therefore only matters for presentation (input repeat if we add it, animation if we add it).
6. **Window-size state** (`ScreenSize`, an integer scale 1 to N) is a presentation setting. On the web it becomes responsive scaling of the 208 x 240 frame (stretched 2:1) to the available space, which makes the "Screen Size" options screen mostly obsolete on the web (keep or hide it, decide in task 16).
7. **Resetting state on new game and load** should reset *all* of it (message queue, UI stack, cursors), unlike the VB build, which leaves statics such as the message queue untouched (task 11).

**Section B is complete (tasks 6 to 13).** Open issues before task 14 (the first design task):
- None blocking. Task 14 (JSON save format) can now be done with the full inventory: the 19 mutable tables (task 6) folded into entity-owned fields, plus config and slot metadata (this task).
- Decisions that task 14 depends on: entity handle and deletion policy (17), enum/versioning scheme (4), `Maybe` or sentinel convention for nullable fields (7).

### Task 17: ID and handle scheme, deletion policy, replacing the interface layers (2026-10-03)

> **Superseded in part (decision review 1):** the owner replaced the pool-and-generation design below with UUID-keyed maps (see D9). What survives: items know where they are, no wrappers or interfaces, deletion destroys what an entity holds, handlers re-resolve ids and treat a failed lookup as "gone", and the invariant checker. What is dropped: fixed capacities, generations, free lists, the saved-pool form. `odin/game/pool.odin` and its tests are to be replaced when the world is ported.

**Decision:** D9 (see the decision table). This task was a design decision, so the evidence for it and the rules it implies are recorded here. It was adopted by the assistant, not chosen from options given by the user; it is cheap to change now and expensive to change after tasks 14 and 27 build on it.

**Done:** Read how the VB code creates, destroys and cleans up entities (`Character.Destroy`, `CharacterData.Clear`, `Item.Create/Destroy/ItemType setter`, `Routes.DestroyRoute`, `DropLoot`, and the `Events.vb` create/destroy sites); used the measured entity counts from tasks 6 and 9; wrote `spikes/entity-pool`, an Odin generational pool with save and restore, and ran it natively and as `js_wasm32` in the browser pane.

**Evidence from the VB code:**
- **Only characters and items are really created and destroyed during play.** Characters: 1,087 at new game; rats are created at runtime by the cellar quest and by rotting food (`RottenFoodDecay`); combat and several events destroy them. Items: 362 at new game; created by loot drops, shop purchases, drunk and chicken events, lore notes; destroyed by use, decay, selling, repair and equip rules. Locations and features are created once at generation and never destroyed. Routes are destroyed and recreated by portals (task 9) but nothing else refers to a route by ID.
- **Cleanup is manual and wide.** `CharacterData.Clear` hand-calls eight `ClearForCharacter` procedures (quests, quest completions, equipment, inventories, visited locations, statistics, player row, spells); `Item.Destroy` clears equip slot, inventory link, statistics and events. Forgetting one leaves orphan rows. Embedding those things in the owner struct removes the whole class of bug.
- **An item is in exactly one place** at a time: a character's inventory, a location's ground inventory, or equipped (equipping removes it from the inventory); nothing in the VB schema enforces this, the code does.
- **An item's type is mutable** (`FoodDecay` changes it), and `Item.ItemType`'s setter **copies the type's statistics and events into per-item rows** (about 911 stat rows and 158 event rows for 362 items). Most of the copied values never change; only a few (such as current durability) do. Task 14 decides which item stats are really per-instance.
- Several UI screens hold item references across frames (`InteractItem`, `groundItems`, `items`, `table` of equip slots) and the VB code has no protection if the item is gone.

**Design (what D9 means in practice):**
1. **Static content**: enums for every content type (`Item_Type`, `Character_Type`, `Direction`, `Location_Type`, `Route_Type`, `Statistic_Type`, ...) with `None = 0` as the first value, and each enum value numerically equal to the VB ID (so `Direction.North = 1`). Content definition tables are arrays indexed by the enum (task 7, D8). "Null" for a content reference is just `.None`.
2. **Locations**: `World.locations: [MAX_LOCATIONS]Location`, `Location_ID :: distinct u32` is the index, index 0 unused (= none). Never destroyed, so no generation. A location holds its own route slots (`routes: [Direction]Route_Slot{to: Location_ID, type: Route_Type}`, the empty slot is `to = 0`), its dungeon level, its column/row, its feature type (the 9 features are not entities), a `visited` flag, and nothing about who is inside it.
3. **Characters and items**: one fixed-capacity generational pool each (`Pool(T, N)`): `slots: [N]Slot(T)` with `gen`, `alive`, `next_free`; slot 0 reserved; a free list threaded through dead slots (last destroyed is reused first); handles are `distinct u64` = `(generation << 32) | index`; zero is "none". Lookup returns `(^T, ok)` and fails for zero, out-of-range, dead or old-generation handles. Iteration is in slot order (deterministic).
4. **Not entities**: routes (fields of the source location), inventories (derived, see below), quests, quest completions, spells, equipment, statistics and the visited set (fields of the character or location). That removes `Routes`, `Inventories`, `InventoryItems`, `CharacterQuests`, `CharacterQuestCompletions`, `CharacterSpells`, `CharacterEquipSlots`, `CharacterStatistics`, `ItemStatistics`, `LocationStatistics`, `CharacterLocations` and `LocationDungeonLevels` as tables (they become fields), and `Features` becomes a field of `Location`.
5. **Where is an item?** Items store their own position: `holder: Item_Holder` (an enum `{None, Carried, On_Ground, Equipped}`), `holder_character: Character_Handle`, `holder_location: Location_ID`, `equip_slot: Equip_Slot` and `seq: u32` (a world counter taken whenever the item is placed, to keep a stable insertion order). "Exactly one place" is then true by construction, moving an item is one write, and no dynamic arrays are needed. Listing a container is a scan of the item pool in slot order, sorted by `seq` (the scan is about 4k slots; the per-frame UI queries are a handful of these, which is trivial). No union is used (task 4 limitation).
6. **Where is a character?** `character.location: Location_ID`. "Who is at this location" is a scan of the character pool (about 1.1k). Add a cache only if profiling ever shows a need.
7. **The player** is `World.player: Character_Handle`. Player-only state (mode, facing direction, current shoppe type) is a `Player_State` struct next to it in `World_State`, not a separate entity table.
8. **Deletion policy**: `destroy_character(world, h)` and `destroy_item(world, h)` are the only ways to remove them. Destroying a character first destroys every item it holds or has equipped (this matches the effective VB behaviour where loot is dropped first, `DropLoot`, and any rest is orphaned and unreachable). Destroying an item just frees its slot. Everything owned by value (stats, quests, spells) disappears with the slot. Destroying the player is not allowed (assert).
9. **Stale handles**: every cross-frame reference (UI caches, event parameters held across a call) is a handle and is re-resolved when used; if it fails to resolve, the code treats the thing as gone (for example leave the screen). Pointers returned by lookup are used only within a procedure and never held across a call that can destroy things.
10. **Capacity and exhaustion**: provisional capacities are 2,048 characters (1,087 at start; peak live count stays near that because kills free slots) and 4,096 items (362 at start). `create_*` returns `(handle, ok)`; when a pool is full the caller skips the spawn (no loot dropped, no new rat) and, in debug builds, asserts. The numbers are to be confirmed by simulating long games (task 22/23) and by task 19 (memory).
11. **Save form**: a pool is saved as every slot below `used_top` (generation, alive flag, free-list link, value) plus `used_top` and `free_head`. This keeps stale handles stale and gives the **identical reuse order after load** (verified), so a loaded game and the original behave identically under the same RNG seed. The default JSON marshal of a fixed array would write all N slots, hence the dedicated form. Handles are JSON numbers (a packed u64).
12. **Replacing the interface/mocking layers**: no `I*` interfaces, no wrapper classes (`BaseThingie`, `SubcharacterBase`, the 45 view classes), no `IWorldData`. Game procedures take `^World_State` (and handles); content definitions are global read-only tables. Tests build a `World_State` directly and assert on its fields; Moq-style tests (the ~3,200 lines in `Game.Tests`) are not ported mechanically (task 22). A `world_validate` procedure checks the invariants (every item's holder resolves, no two items in one equip slot, every character's location is valid, the player is alive, pool counts match) and runs after generation, after load, and in tests.

**Verified (`spikes/entity-pool`, native and `js_wasm32` in the browser pane, identical results):**
- A new handle resolves; the zero handle never does; destroy succeeds once and a second destroy is rejected; an old handle is rejected after destroy and after the slot is reused; the reused slot has generation + 1 at the same index.
- A full pool reports failure instead of overflowing (7 usable of 8 slots, slot 0 reserved).
- Iteration is deterministic in slot order.
- A handle round-trips through `core:encoding/json` as one number, including a generation above 2^31 (`12884901888000000005`).
- Saving the pool (all slots below `used_top`), unmarshalling and restoring it keeps live and stale results and produces the **same next handle** (`0x100000004`) as the original pool.
- Sizes: on wasm32 a pool slot for a small test character is 44 bytes and for a small item 16 bytes (80 and 24 on 64-bit native). Real struct sizes will be larger.

**Not verified:**
- Sizes with real `Character` and `Item` structs. A rough upper bound: 2,048 slots of a few hundred bytes is about 1 MB and 4,096 item slots about 0.3 MB; a `World_State` of that size must not live on the stack of a test or on the wasm stack (allocate it with `new`/global).
- That scanning is fast enough with the real per-frame query pattern (it should be: a few thousand slot reads).
- That VB inventory display order equals insertion order (the schema has no order column; SQLite returns by row order, which `seq` approximates). Compare screens against the VB build (task 23).
- Odin generic ergonomics for several handle types: the spike used one `Handle`; the real code should have `Character_Handle` and `Item_Handle` as distinct types so they cannot be mixed up (this needs the pool to be parameterized by handle type or conversions at the boundary).
- Whether per-character visited sets are needed; only the player moves, so `Location.visited` is assumed.

**Findings affecting later tasks:**
1. Task 14 (save format) gets a concrete shape: `World_State` = version, seed/RNG state, `locations` array, `characters` and `items` as saved pools, player handle and `Player_State`, plus counters (the item `seq` counter, dungeon-level data). No relational tables.
2. Task 27 (exporter) only exports **static** content, so it is not affected by entity layout, only by "enums with `None = 0` equal to the VB IDs".
3. Task 18 (events): handler parameters become a small struct of handles (`character`, `item`, `location`), which can be validated at the top of each handler.
4. Task 19 (allocators): fixed-capacity pools mean no allocation during play for characters and items; strings (item names, character names) are the remaining dynamic data and need a decision (short fixed buffers vs. allocated strings).
5. Task 22: `world_validate` becomes the central invariant test, run after generation and after random action sequences.

**Open issues before the next design task:**
- Task 14 now depends only on tasks 15/18 loosely; it can proceed. The remaining open choice inside it is which item statistics are per-instance.
- Strings in pooled structs (names): to settle in task 14/19.

### Decisions D10 to D14 (2026-10-03, from the user)

Recorded after the "important decisions" review (see the decision table for the wording). Effects on the plan:

1. **D10 (stretch and landscape).** Task 16's remaining question is only the control scheme and the layout of controls around the stretched frame. The frame is 208 x 240 stretched 2:1, so 416 x 240 effective; in landscape on a phone it fits wide screens well, and controls can sit left and right of the frame. Browsers generally only allow `screen.orientation.lock("landscape")` while in fullscreen (and not at all in some, such as iOS Safari), so the real behaviour is: request fullscreen plus landscape lock where supported, otherwise show a rotate prompt in portrait. Both need the JS shim (small additions). The options screen "Screen Size" (an integer window scale) is no longer meaningful; scale will be automatic, and the screen can be dropped (D13).
2. **D11 (export/import).** Adds JS shim functions for "download this text as a file" and "let the user pick a file and hand me its text", and native equivalents. Import must validate before replacing a slot. This is also the answer to the lost-`localStorage` risk. New task 29.
3. **D12 (itch.io only).** `shippit.sh` becomes an HTML5 zip upload with `butler` (task 26); the Windows and Linux native uploads are no longer shipping targets (native builds stay a development tool, D2, D7). Itch.io serves HTML5 games in an iframe, so storage, audio unlock, fullscreen and orientation lock must be tested there. New task 28.
4. **D13 (dead code).** New rule and task 30; the Options screens for volumes remain (audio is live), the screen size option is dropped.
5. **D14 (no VB saves).** The task 5 shim and task 14 format are free of any compatibility constraint, and the task 27 exporter does not need to produce anything for old saves.

**Still open:** the touch control scheme (task 16); item stat per-instance question (task 14); string storage in pooled structs (tasks 14/19); real-device testing arrangements (the browser pane cannot emulate touch or real mobile limits).

### Task 14: JSON save format (2026-10-03)

> **Amended (decision review 1):** with UUID-keyed maps (D9) the saved pools become sorted arrays of records with `"id"` strings, and every reference to a character, item or location is an id string. Size estimate for a fresh game: about 480 KB (up from 298 KB) with canonical 36-character ids; five slots are then about 2.4 million characters, still under the roughly 5 million of `localStorage`, but closer. If size becomes a problem the ids can be written as 22-character base64url strings (about 360 KB) without changing anything else.

**Decision:** D15. Built and tested as `spikes/save-format` (native and `js_wasm32` in the browser pane): draft `World`/`Location`/`Character`/`Item`/`Player_State` types, a generated enum stub for content (`gen_stub.py` writes `content_stub.odin` from `boilerplate.db`, a prototype of the enum part of task 27), converters between the world and the file shape, a loader that validates everything, `world_validate`, a fake world of the same size and shape as a fresh game (737 locations, 1,087 characters with churn, 362 items), and a fuzz test.

**Which item and character data is really per-instance (the question you asked me to evaluate):**
- **Items.** In a measured new game all 911 `ItemStatistics` rows, all 158 `ItemEvents` rows and all 362 item names are identical to the item type's values. In the code the **only item statistic ever written after creation is `Durability` (stat 28)**, a wear counter: `Reduce` adds to it and `Repair.Perform` sets it to 0; current durability is `MaximumDurability - wear`, and `MaximumDurability` is already read from the item type. Every other item stat is read from the per-item copy but never written. So per instance an item has: `type`, `wear`, `lore` (which of the 25 lore texts a note shows; the item name becomes the lore's item name), plus its holder and `seq` (task 17). Stats, events and the name are **derived from the type** (and lore), not saved.
- **Characters.** At new game every monster's statistics equal its type's initial statistics (the only differences are the player's rolled values), but statistics are written in play: Wounds, Stress, Immobilization (combat), and for the player XP, XP Goal, Money, Fatigue, MP, Mana, Hunger, HP and others, plus attribute points at level up. So characters keep a full `stats` array in memory and **save only the non-zero entries** as `[stat id, value]` pairs. Monsters have about 10 non-zero values each; this is lossless because the in-memory array already holds effective values, defaults included. A character stores `type`, `location` and `stats` only; it has no name (the type gives it one).
- **Quests and spells** are saved only in `Player_State` (assumption, see below). **Locations** keep `type`, dungeon `level`, `column`, `row`, `feature`, `visited` and their routes (generation output).

**Behaviour change this causes (a defect fix under D13):** in VB, when food rots, `FoodDecay` changes only the item's type column to type 35 (the rotten "kottbulle"); the item keeps the *old type's* per-item copies of events and statistics, so the rotten food keeps firing `FoodDecay`, never `RottenFoodDecay` (which makes rats appear and destroys the item) and eats with the fresh food's `EatFood` instead of `UseRottenFood`. With data derived from the current type, rotten food will behave as the content defines for type 35. This changes gameplay slightly (rotten food will now be dangerous as designed). Say if you want it kept as it was.

**The format (v1):**
- **Envelope:** `{"format":"kordanors-cabal-save","version":1,"summary":{"place":<Dungeon_Level>,"hp":N,"xp":N},"world":{...}}`. The summary is for the slot labels (for example "Level II  HP 3  XP 0") so the Load/Save screens only read the text, never build a world just to see if a slot is used.
- **World:** `item_seq`, `rng_state` (placeholder until task 21), `player` (`character` handle, `mode`, `facing`, `shoppe`, `quests_active`, `quest_completions`, `spells`), `locations` (array; element *i* is location id *i*+1; each has `routes` as `[direction id, destination id, route type id]` triples), `characters` and `items` as saved pools (`used_top`, `free_head`, and every slot below `used_top` as `{gen, alive, next_free, value}`).
- **Encoding rules:** compact JSON (298 KB); scalar enum fields written by **name** (`"mode":"Neutral"`), stats and routes written as numeric tuples using the ids, which are stable because enum values equal the VB ids and enums are append-only. Handles are packed u64 numbers (task 17).
- **Storage:** `kc:slot1` to `kc:slot5` hold the whole text; `kc:config` holds the config (task 13). **Export** downloads the slot text as `kordanors-cabal-slotN.json`; **import** reads a file, rejects anything over a size limit (proposed 2 MB), parses into a temporary world, runs the loader and `world_validate`, and only then replaces the chosen slot, so a bad file never damages a slot.
- **Not saved:** UI state, screen stack, button cursor, pending messages, config values, and anything derivable from content.
- **Versioning:** `version` is an integer; the loader accepts only versions it knows and reports "unsupported version" otherwise; migrations are written only when a version 2 exists. Enums are append-only and never renumbered.

**Verified (native and browser pane, identical results unless noted):**
- The generated fresh-game-sized world passes `world_validate`; marshal, load and compare field by field is **exact**; `save(load(save(w)))` is **byte-identical**; the next handle created after a load equals the next handle created in the original world.
- Sizes: **298 KB compact** (locations 93 KB, characters 140 KB, items 64 KB, player 0.2 KB), 753 to 810 KB pretty-printed. Compare: the VB SQLite save is 831 KB, the naive row-per-object estimate of task 6 was 1.1 MB, the compact-array estimate 215 KB. Five slots are about 1.5 million characters, comfortably under the roughly 5 million characters of `localStorage`.
- Rejections with a specific error and no crash: empty input, non-JSON, wrong format string, a future version, and a file without a live player.
- **Fuzz:** 150 mutated copies of the save (truncation, random bytes, changed digits) were loaded; 0 crashes natively and in the browser; every accepted mutation still passed `world_validate` and could be saved again. The outcomes were mostly rejection at parse time, some at shape check, and about a third accepted (changed digits that still form a valid world).
- Odin facts relevant to the format (found while building it, all checked with small tests):
  1. `json.marshal` **cannot write enum-keyed maps** (`Unsupported_Type`), and map iteration order is not deterministic, so no maps are used in the save shape (the first attempt broke byte-identical re-saves).
  2. **`omitempty` struct tags are ignored**; zero fields are always written.
  3. Unmarshal **accepts an unknown enum name silently** (leaves the value at its previous or zero value) and **accepts out-of-range enum numbers** (the enum then holds an invalid value). The loader must therefore validate every enum (`reflect.enum_value_has_name`) and every id, which it does. Scalar enums by name still round-trip by number or name.
  4. Enumerated arrays are written with every name including `None` (so `quest_completions` and `spells` carry a `"None":0` entry); `bit_set` is written as an integer.
  5. `where` is an Odin keyword, so a field with that name cannot exist; `place` is used.
  6. In recent Odin `core:os` file procedures return `Error`, not `bool` (task 5).

**Not verified:**
- The real game structs and rules: sizes were measured with a fake world of the right shape, not generated by the real generator or from late-game play (more items, more wear entries, churn). Item growth is the main risk to size; even 4,096 items would add roughly 0.6 MB per save.
- Browser storage of a 300 KB value was only tested up to 8 MB in task 5, in the pane (real quotas and Safari/Firefox are untested).
- Fuzz coverage is modest (150 mutations per target); import of a very large or deeply nested file (the 2 MB cap is a proposal, not tested).
- That only the player ever has quests or spells (they are saved only in `Player_State`). All VB code paths that grant them are player-driven, but monsters casting or learning spells was not exhaustively checked.
- That the Load/Save screen labels from the summary give enough information; the summary fields are a proposal.

**Findings affecting later tasks:**
1. **No strings in pooled structs.** Characters have no names, items get names from their type or lore, so entities contain only numbers and enums. This settles the open "strings in pooled structs" question from task 17: names are looked up from content tables at draw time, nothing to allocate or free per entity.
2. Task 15 (platform interface): storage is `get/set/remove` by key plus a **size check**; export needs `download_text(filename, text)` and import needs `pick_file_text() -> (text, ok)` (asynchronous in the browser; the core should receive the text as an event in a later frame). The native build uses files for both.
3. Task 21 (RNG): `rng_state` must be serializable (a fixed array of integers).
4. Task 22: `world_validate` plus the save/load round-trip and fuzz tests in this spike are the seeds of the save tests.
5. Task 27 (exporter): the enum generator prototype works (ids 1..N are contiguous for all content tables; duplicate names such as the two "kottbulle" types need an id suffix, e.g. `kottbulle_35`, and names with quotes are sanitized).

**Open issues before the next design task:**
- Task 15 (platform interface) and task 16 (touch controls) are next in the design list; task 18 (event dispatch), 19 (allocators), 20 (audio delivery), 21 (RNG) and 22 (tests) follow.
- Decisions this task made on its own authority that you may want to review: D15 itself, the rotten food fix, the 2 MB import cap proposal, and the label summary.

### Task 15: platform interface (2026-10-03)

**Decision:** D16. The interface is written as compilable Odin in `spikes/platform-api/api.odin` (79 lines, the authoritative text) and implemented twice plus a fake: `platform_js.odin` with `platform.js`, `platform_sdl2.odin`, and the fakes in `core_test.odin`. The spike also has a toy core (`core.odin`) that exercises every part of the interface and renders with the **real PETSCII glyphs, palette and rasterizer** extracted from the VB source (`gen_font.py` writes `font_data.odin` from `SPLORR.UI/Patterns.vb` and `Pattern.vb`: 128 glyphs, and a character map covering 88 printable ASCII characters; lowercase letters share the uppercase glyphs; `\ _ ` { | } ~` have no glyph, as in the VB game).

**The interface:**
- **Core entry points:** `core_init(core, services)` and `core_step(core, input, out)`, called once per presented frame. `core_step` must not block, must not keep pointers into its input after returning, and fills the whole output every call.
- **Input** (`Step_Input`: `dt` plus a list of `Input_Event`): events are `Command` (Up, Down, Left, Right, Confirm, Cancel; Green and Blue are merged into Confirm because every VB screen treats them identically, per task 12 and D13), `Tap` with **cell coordinates** (the platform converts a pointer or touch position on the frame to column/row; positions outside the grid are passed through and ignored by the core), `File_Text` (the contents of the file the user picked, valid for this step only), and `File_Cancelled`.
- **Output** (`Step_Output`): a pointer to the 208 x 240 frame (`[]u32`, bytes in memory order R, G, B, A, i.e. `0xAABBGGRR`) and a changed flag, up to 16 sound effects to start (`Sfx`: the 8 VB values), current `sfx_volume` and `music_volume` (always valid, 0 to 1), and `quit_requested`.
- **Services** (supplied once at init, so the core never imports a platform and tests pass fakes): `storage_get(key, allocator)`, `storage_set(key, value) -> ok`, `storage_remove(key)`, `download_text(filename, text)`, `request_file_pick()` (the result comes back later as an event), `entropy() -> u64`, and `log(message)`.
- **Constants** (the single source for platform code): the 22 x 23 cell grid, 8-pixel cells, borders 16 and 28, frame 208 x 240, and `FRAME_STRETCH_X = 2`.

**Who owns what:**
| Concern | Owner |
|---------|-------|
| Pixel rasterization of the 22 x 23 cell grid, hues, glyphs | **core** |
| Scaling, 2:1 stretch, letterbox, orientation prompt, fullscreen | platform |
| Keyboard map, on-screen control buttons (the "both" scheme of task 16), pointer to cell | platform |
| Which cell rectangles are tappable and what a tap does | **core** (it receives cell taps and decides) |
| Sound files, decoding, unlocking audio after a gesture, playing | platform (the core only names a `Sfx` and sets volumes) |
| Config (volumes) and saves: content and persistence timing | **core** (through `storage_*`); the platform only stores strings |
| File dialogs and downloads | platform; the core builds and validates the text |

**Browser implementation** (the whole JS surface for these jobs, 136 lines in `platform.js` and `storage.js`): Odin imports 8 JS functions (4 storage from task 5; `download_text`, `request_file_pick`, `entropy_u32`, `log`); JS calls 12 exported `proc "c"` functions (`platform_command`, `platform_tap`, `platform_file_text`, `platform_file_cancelled`, `platform_alloc`, `platform_frame`, and getters for the frame pointer, sound list and volumes, and quit). The page runs its **own `requestAnimationFrame` loop** and calls `platform_frame(dt)`; it does not use `odin.js`'s built-in `step` loop, so it can present and play sounds after every step. Pixels are drawn with `putImageData` onto a 208 x 240 canvas styled to an integer scale and twice as wide as tall with `image-rendering: pixelated`. Events are queued in a fixed array in wasm memory between frames. A file picked by the user is copied by JS into a buffer the Odin side allocates (`platform_alloc`), handed over as an event, and freed by Odin after the step.

**Native implementation** (119 lines): SDL2 window with `RenderSetLogicalSize(416, 240)` (the 2:1 stretch), a streaming `ABGR8888` texture fed straight from the core's frame, keyboard to commands, mouse and finger presses to cell taps (window to output pixels to logical to de-stretched frame to cell), SDL2_mixer for the 8 effects and the looping theme, files under `saves/` and `exports/` for storage and download, and `./import.json` as a stand-in file picker.

**Verified:**
- **Core, with fake services (`odin test`, 6 tests, all pass):** confirm runs the selected item and reports a sound; a tap on a menu row selects and activates it while taps in the border or outside the grid are ignored; save, export, import and load round-trip through the fake storage and recorded downloads; bad imports (not JSON, wrong format, cancelled picker) leave the slot untouched; the frame has the right border and background colors and glyph pixels; volumes are reported in the output.
- **Browser (`js_wasm32`, 471 KB; checked in the Claude browser pane):** the screen renders with the real PETSCII font and palette at the 2:1 stretch (screenshot); keyboard keys drive the menu and counter; the on-screen buttons (◀ ▲ ▼ ▶ OK ESC) work; clicking the canvas converted to the correct cell (a click on the NEW SEED row reported `TAP 5,12`); save wrote `kc:spike` into `localStorage`; the entropy service returned a fresh 64-bit value; export produced a download named `kordanors-cabal-slot1.json` with the slot text; importing a valid file stored it and loading showed the imported value (99); importing garbage left the slot unchanged; cancelling the picker left it unchanged. No console errors or warnings (this includes sound decoding and music start, though nothing audible was heard in the pane).
- **Native:** builds; the SDL2 window with the toy core and audio opened and ran 3 seconds without error (killed by timeout; input and screen contents were not inspected).

**Not verified:**
- **Real audio output**: only that decoding and `music.play()` did not report errors. iOS/Safari Ogg support and autoplay unlocking on a phone.
- **File pick from the frame loop on Safari/iOS**: the picker is opened from `requestAnimationFrame` code a frame after the tap. Chrome allows this inside the transient user-activation window; Safari may require the call to be made directly inside the event handler. This is the main risk for D11 and goes into task 28/29 (test on an iPhone). A fallback is to make Import a platform-level DOM button that opens the picker itself.
- **Real touch events** (everything was mouse clicks; the code uses pointer events, which cover touch), multi-touch, HiDPI, window resizing beyond the initial size, fullscreen and landscape lock (D10), the rotate prompt.
- The native click-to-cell mapping (written by hand because the Odin SDL binding has no `RenderWindowToLogical`), native storage on Windows, quit flow, gamepads.
- Performance of `putImageData` of a 208 x 240 image every frame on a phone (it is 200 KB per frame; expected fine).

**Findings and rules this produced:**
1. **Strings passed to services are borrowed for the call only.** The core builds them with the temporary allocator and frees it at the end of the step, so a platform that keeps a string (a fake store, a queued download) must copy it. The first run of the fake storage test failed exactly this way. It is written into the contract.
2. **Procedure-pointer services force globals in fakes**, so core tests must run single-threaded (`-define:ODIN_TEST_THREADS=1`), or `Services` needs a user-data pointer. Single-threaded is enough for now.
3. **Exports for the browser must be `proc "c"` and set up a context** (`runtime.default_context()`); anything they call that has no context must be `contextless` (the event queue push). `odin.js` only calls a default-convention `step` itself, which is why the page uses its own loop.
4. **The frame memory view must be recreated every frame** in JS (`new Uint8ClampedArray(memory.buffer, ptr, n)`), because wasm memory growth detaches old views. Done in the spike.
5. The hidden-pane `requestAnimationFrame` shim from task 1 is now part of `platform.js` as a testing aid.
6. **The font/palette extraction by script works and is exact enough to look right**; task 27's exporter can use the same approach for item/character content, and the sprites (task 8) can use it for the 16 sprite grids.
7. The core stays free of `when` and platform imports: only `api.odin`, `core.odin`, `font_data.odin` and `core_test.odin` (no `#+build` except the test file), plus one platform file per target, as D5 and D6 require.
8. The interface leaves the **layout of the touch controls** entirely to the platform. The toy page has six buttons below the frame as a minimal proof of "both" input schemes; the real layout (landscape, controls beside the frame) is task 16.

**Open issues before task 16 and later design tasks:**
- Task 16 (touch layout): the interface supports it; what remains is the control design (D-pad and button placement around a 416 x 240 effective frame in landscape, the portrait prompt, fullscreen/orientation handling) and verifying on real devices with Claude in Chrome (the pane cannot emulate touch).
- The Safari/iOS file-picker question (task 28/29).
- Task 18 (events), 19 (allocators; the core uses `context.temp_allocator` per step, freed at the end of `core_step`), 20 (audio delivery; the spike fetches wavs and the ogg from `assets/` at first gesture), 21 (RNG), 22 (tests) are independent of this interface.

### Task 16: touch layout and controls (2026-10-03)

> **Amended (decision review 3):** the owner chose to keep the original border on phones, so rule 1 below (cropping the border away to gain about 20% more scale) no longer applies. `layout.js` now scales the whole 416 x 240 frame; the cyan page background is the same color as the border, so the picture is the original frame. Consequences: at 844 x 390 the scale is 1.33 (cells 21 x 11 px, was 1.57), at 915 x 412 it is 1.63, and screens narrower than about 720 px (iPhone SE class: 667 x 375, 640 x 360) no longer have room for the control columns and use the full-screen layout without them (scale 1.56 and 1.50, cells 12 px tall). The finger two-step tap matters even more with cells this small. The table below is the pre-amendment version; `node layout_test.js` prints the current one.

**Decision:** D17, on top of D4 (touch first class), D10 (2:1 stretch kept, landscape on mobile) and your choice to support both control schemes. Built into `spikes/platform-api`: `layout.js` (a pure function `computeLayout({width, height, touch, insets})`, 60 lines), `layout_test.js` (node, 13 device viewports), the new `platform.js` and `index.html` (start overlay, rotate overlay, positioned control buttons, safe-area insets), and a `precise` flag on tap events in `api.odin` with the matching core logic and a new core test (7 tests now).

**The problem the layout has to solve (computed, not guessed):** the paper is 22 x 23 cells of 8 pixels shown 2:1, so on a phone in landscape a cell is only **about 11 to 15 CSS pixels tall** (and 22 to 31 wide). Fingers need 40 px or more. Menu rows and the 10 in-play buttons are single, adjacent cell rows, so no finger can hit one reliably on the first try. That is why the design below has two parts: make the paper as large as the controls allow, and change what a tap means.

**Rules (D17):**
1. **Border becomes the page.** The frame's cyan border (16 pixels at the sides, 28 above and below) is not interactive, so mobile crops it away: only the 176 x 184 paper is drawn (352 x 184 layout units with the stretch) and the page background takes the border color (read from frame pixel (0,0), so it follows the core with no extra API). It looks like the original screen and gives about 20 percent more scale than showing the border. The border hue is never changed anywhere in the VB code (`BorderHue` and `ScreenHue` are constants), so this is safe.
2. **Landscape only on touch.** In portrait on a touch device the game shows an opaque "please rotate your device" screen and nothing else. A tap on it counts as the start gesture (see 6), so on Android it may rotate the device by itself.
3. **Side controls.** A column on each side, width 13 percent of the usable width clamped to 96 to 128 px: **left** Up (full width), Left and Right (half width each), Down (full width); **right** Confirm (double height) above Cancel. Button height is 15 percent of the usable height clamped to 44 to 60 px; the narrowest button is 41 px. The paper takes the remaining space, centered, at a fractional scale.
4. **Small screens drop the controls.** If the side columns would leave the paper under 1.3x, the controls are hidden and the paper uses the full width (the 640 x 360 case). Tapping still works with the two-step rule.
5. **Desktop (no touch):** the whole frame including its original border at the largest integer scale, no on-screen controls, keyboard as before. A window smaller than the frame scales it down.
6. **One start gesture.** A "tap or press a key to start" screen waits for the first gesture and uses it to create and resume audio, request fullscreen with navigation UI hidden and lock the orientation to landscape (both only on touch, both optional: failures are caught and recorded in `__platform.startInfo`). That gesture is consumed and is not sent to the game.
7. **Two-step taps for fingers.** The platform sends taps with `precise = false` for `pointerType == "touch"` and `true` for mouse and pen. For an imprecise tap the core **selects** the tapped item, and **confirms** only if that item was already selected; a precise tap selects and confirms at once (`core.odin`, tested). The highlight is the feedback, the D-pad fixes a near miss, and OK confirms whatever is selected. (The real game's screens have to implement the same rule, in `ui_step`: one function that maps a tap to "select item N" or "confirm".)
8. **Rendering for fractional scales.** The frame is drawn at an integer multiple of the paper size (`ceil(scale)`, at most 4) with nearest-neighbor, then the browser scales it down smoothly. That keeps pixels even and edges clean; at an exact integer scale it switches to `image-rendering: pixelated`.
9. **Touch hygiene in the page:** `touch-action: none`, `overscroll-behavior: none`, no text selection or long-press callout or tap highlight, no context menu, `viewport-fit=cover` and safe-area insets so the notch does not cover controls or paper, layout recomputed on `resize`, `orientationchange` and `visualViewport` changes.
10. **Dropped, per D13:** the Screen Size options screen (scale is automatic); no hold-to-repeat on the D-pad (the VB game has no key repeat).

**Computed layouts (`node layout_test.js`; CSS pixels, safe-area insets for notched phones):**

| Device | Viewport | Result |
|---|---|---|
| iPhone SE landscape | 667x375 | touch-sides scale 1.35 paper 475x248 cell 21.6x10.8 upscale 2 buttons 41px+ |
| iPhone 12/13/14 landscape | 844x390 | touch-sides scale 1.57 paper 554x290 cell 25.2x12.6 upscale 2 buttons 42px+ |
| iPhone 14 Pro Max landscape | 932x430 | touch-sides scale 1.71 paper 602x315 cell 27.4x13.7 upscale 2 buttons 46px+ |
| Pixel 7 landscape | 915x412 | touch-sides scale 1.92 paper 677x354 cell 30.8x15.4 upscale 2 buttons 53px+ |
| Galaxy S8 landscape | 740x360 | touch-sides scale 1.56 paper 548x286 cell 24.9x12.5 upscale 2 buttons 41px+ |
| small Android landscape | 640x360 | touch-full  scale 1.82 paper 640x335 cell 29.1x14.5 upscale 2 no controls |
| iPad landscape | 1024x768 | touch-sides scale 2.18 paper 768x401 cell 34.9x17.5 upscale 3 buttons 57px+ |
| iPad Pro 12.9 landscape | 1366x1024 | touch-sides scale 3.15 paper 1110x580 cell 50.5x25.2 upscale 4 buttons 57px+ |
| desktop 1280x720 | 1280x720 | desktop     scale 3.00 paper 1056x552 cell 48.0x24.0 upscale 3 no controls |
| desktop 1920x1080 | 1920x1080 | desktop     scale 4.00 paper 1408x736 cell 64.0x32.0 upscale 4 no controls |
| desktop small 600x400 | 600x400 | desktop     scale 1.00 paper 352x184 cell 16.0x8.0 upscale 1 no controls |
| iPhone 14 portrait | 390x844 | rotate prompt |
| Pixel 7 portrait | 412x915 | rotate prompt |

All 13 viewports pass the layout checks: the paper stays inside the safe area, controls never overlap each other or the paper, every button is at least 40 px, desktop scales are integers, only touch portrait shows the rotate prompt, and touch cells are at least 10 px tall.

**Verified (browser pane with mobile emulation, which reports 5 touch points and a coarse pointer under 768 px; and the real Chrome through Claude in Chrome):**
- Viewports 667 x 375, 844 x 390, 915 x 412 and 1024 x 768 (touch), 1280 x 720 (desktop) and 390 x 844 (portrait) were loaded and screenshotted: full-bleed cyan background, centered paper, D-pad and OK/ESC columns, the rotate screen in portrait (opaque after a fix), and the desktop frame at 3x with its border. Resizing from portrait to landscape re-laid out without a reload. Pane layouts equal the node results.
- Auto-detection of touch (`pointer: coarse`) works in the emulated mobile viewport; `?touch=1/0` overrides it for testing.
- Two-step tapping, tested with synthetic touch pointer events: the first tap on NEW SEED only highlighted it, the second ran it. D-pad Up moved the highlight. Mouse clicks activate immediately.
- The start overlay hides, audio setup does not error, and the failure paths are safe: in both browsers fullscreen and orientation lock failures are caught and the game continues.
- Core tests: 7 pass (new: finger taps select first, then confirm).

**Not verified (needs a real phone or tablet):**
- **Real touch input and fullscreen.** Both browsers refused the programmatic requests ("API can only be initiated by a user gesture" for synthetic events, and "not granted" for the automated click in real Chrome), and the desktop Chrome has no orientation lock (`NotSupportedError`). So whether the start tap really enters fullscreen and locks landscape on Android is untested, and iOS Safari does not offer the fullscreen API for pages on iPhone (expected: it will only show the rotate prompt and run in the normal browser UI). In the code the start gesture is `pointerdown` for a mouse and `pointerup` for touch, because per the HTML spec only those count as user activation for each pointer type.
- Fat-finger accuracy: with 11 to 15 px rows even the two-step rule may feel fiddly; this needs a hands-on trial. Knobs if it does: a slightly larger paper by hiding the controls, a long-press or drag-to-select, or tap targets that grow after selection.
- HiDPI sharpness of the smooth downscale, the notch/safe-area behavior on real devices (insets were tested only as numbers in the node test), the itch.io iframe (fullscreen needs its "Fullscreen button" setting; task 28), and tablets in portrait.
- Haptics and a pointer-capture/drag design were not tried.

**Findings affecting later tasks:**
1. The core API gained one field (`precise` on `Input_Event`); everything else in D16 is unchanged. The real UI code needs a single `tap_to_item(...)`-style rule shared by all menus, lists and the in-play button bank, so two-step behavior is uniform.
2. `computeLayout` is plain JS with no DOM, so it is easy to keep tested; the same numbers drive the page and the node test. A native build does not need it (desktop only).
3. The in-play screen's bottom 5 rows hold the 10 buttons (two columns of 11 cells, 5 rows); at touch scale they are about 150 x 14 px each, which is the hardest screen to hit. It will be the first real test of the two-step rule.
4. Task 28 (itch.io) should check fullscreen permission, the iframe's size on phones, and the rotate prompt inside an iframe (`screen.orientation.lock` is not allowed in cross-origin iframes unless the embedding allows it).

**Open issues:**
- Needs a real device pass (Android Chrome, iOS Safari, a tablet) once an itch.io draft page exists (task 28); I can drive the desktop Chrome side, but a phone needs you or a hosted URL.
- Next design tasks: 18 (event dispatch), 19 (allocators), 20 (audio delivery), 21 (RNG), 22 (tests), then repo layout (24). 23 (reference captures) and 25 to 30 follow.

### Task 18: event dispatch (2026-10-03)

**Decision:** D18. Built as `spikes/event-dispatch` and run natively and as `js_wasm32` (28 scenario checks, all pass on both): generated content (`gen_events.py` reads `boilerplate.db` and writes `content_events.odin`; it is the next slice of the task 27 exporter), a minimal world, a small game API (`rules.odin`), the dispatch (`events.odin`) and 12 + 5 real handlers translated from `Events.vb`.

**What the 45 VB handlers actually are** (read in full, `Events.vb` lines 1 to 449):
- **18 checkers** are pure reads of the world: the character's location type (dungeon or not), enemies at the location, mana, whether a particular item is carried, whether an enemy can be bribed with an item (`CharacterTypeBribes`), whether a spell can be learned, quest state.
- **27 actions** do a small number of things: change a statistic (healing is a negative Wounds change), set a status (hunger, drunkenness, chafing, food poisoning, highness), create or destroy items, characters and routes, move the character, learn a spell, change quest state, change an item's type, assign a lore, and roll dice or pick from a weighted table. Three of them (Holy Bolt, Holy Water, Fire Shard) share one *strike* sequence (damage, death, loot/XP lines, message, counter attacks); Earth Shard shares the tail.
- **All output to the player is messages** (27 calls) with an optional sound; nothing else is "raised". Parameters in the VB `Long()` were: `[character]`, `[character, item]`, `[item]`, `[location]`, `[inventory, item]`.

**The design (D18):**
1. **Direct calls.** `check(name, ctx) -> bool` and `perform(name, ctx)` are plain procedures, each one `switch` over a generated enum. No subscribers, no queue of events to process later. A per-frame event queue was considered and rejected: nothing in the game reacts to "something happened" asynchronously, rules run to completion inside one player action, and order of effects (item destroyed after its event, counter attacks after damage) matters and is easiest to keep when the code is straight-line.
2. **Two enums, not one.** `Check` and `Action` are separate, so a content row that points a "can use" slot at an action is a compile error, and the VB runtime failure "no such event name" cannot happen. The generator folds item events into `Item_Type` fields (`purify`, `can_use`, `use`, `decay`; the unused add-to-inventory slot is dropped, D13) and spell and quest events into their content structs. For 19 item types this is 19 lines of data; the 36 distinct names plus the spell and quest names and the hard-coded `LocationDecayItems` give exactly 18 checks and 27 actions, matching the VB tables.
3. **`Event_Context` replaces `Long()`.** It carries `world`, `character`, `item` and `location`; unused handles are zero and each handler validates the ones it uses (a stale handle is simply a no-op or a "cannot use that now" message). Mapping from the old callers: item use `{character, item}`, can-use `{character, item}`, decay and purify `{item, location}`, location decay `{location}`, spells and quests `{character}`.
4. **Item use rule kept in one place** (`item_use`): ask `can_use`, run `use`, then destroy the item if its type is in `SINGLE_USE` (47 of 53 types; the orb, the four shards and the note are reusable). The destroy comes after the event because the event may change or destroy the item itself (rotten food, notes).
5. **Messages and sounds are bounded queues inside the world.** `Message{sfx, size, text[768]}` in a ring of 16; lines are separated by `\n` and written with `strings.builder_from_bytes`, so there is no allocation and no dangling temporary string. 768 bytes covers the longest lore text (417 characters). Only the player speaks (as in VB, other characters' messages are ignored). If the ring is full the newest message is dropped (a debug build should assert). Sounds raised by rules go to an immediate list (16) that the core copies into `Step_Output`; a message's own sound is played when the UI first shows it (task 11, finding 4) rather than while drawing. This replaces the static queue and the .NET event, and it is reset on new game and load because it lives in the world (task 13).
6. **Handlers never crash.** The VB code dereferences a missing enemy in `UseFireShard`, `UseEarthShard`, `UseWaterShard`, `UseBottle` and `UseHolyWater` (the `Can...` checks normally prevent it) and throws in `ReadNote` when every lore text is taken. The Odin versions look the enemy up with `(handle, ok)`, say "You cannot use that now!" and return, and `unassigned_lore` falls back to a random lore when none is free (verified with 26 notes).
7. **Derived names.** An item's displayed name is its lore's item name if it has a lore, else its type's name (`item_name`), so the notes need no stored name (task 14); `Purify_Food` and `Food_Decay` only change `item.type`, and every property after that comes from the new type (this is the rotten food fix of task 14, shown working: the rotten type has `Use_Rotten_Food`, `Purify_Food` and `Rotten_Food_Decay`).
8. **Randomness and ids** go through the world's generator (`roll`, `rng_range`, `pick_weighted`), so seeded runs reproduce (the generator itself is chosen in task 21; the spike uses splitmix64).
9. **Port progress is measurable.** Unported handlers call `todo`, which counts hits; a test probes every enum value against an empty context and reports how many are still stubs (now: checks 5 of 18 and actions 12 of 27 ported). The `#partial switch` is the only place unported cases hide; when all are done it can become a full switch so the compiler enforces exhaustiveness for any future content.

**Verified (native and browser pane, identical):**
- Generation: the item event table matches `boilerplate.db` for potion, kottbulle and rotten kottbulle; the enum sizes are exactly 18 and 27.
- Potion: can-use, use, consumed, wounds reduced by 2d4, empty bottle given, and the message text equals the VB text ("Potion heals up to N HP!" and "You now have N HP!"). Using the same potion handle again does nothing (stale handle).
- Fire Shard: not usable without an enemy, usable with one in a dungeon, the shard survives use (SingleUse = 0), the strike sequence produces the VB text ("You use Fire Shard on Rat!", "You do N damage!", "Rat dies!") and an `Enemy_Death` sound when it kills.
- Town Portal: usable in a dungeon, creates the out/in routes both ways, not usable in town.
- Beer: usable exactly when the data says the enemy can be bribed with beer.
- Decay chain through `location_decay_items`: food rots into the rotten type, rotten food eventually spawns a rat and vanishes.
- 25 notes read give 25 different lore texts; the 26th does not crash.
- Quest accept creates the right number of rats; the Magic Egg creates and gives a weighted item with the VB message.
- Message queue: bounded (16), ordered, drops the newest when full, ignores non-player speakers.

**Not verified:**
- The stand-ins for combat and character systems (`damage_and_maybe_kill`, `first_enemy` with real factions/enemy tables, `health_current` as HP minus Wounds, `mana_current` as Mana minus Fatigue, counter attacks, XP and loot) are simplifications; the real semantics are ported with those systems, and the handlers must be re-checked against them. Beer's effect on `MP` (I set Stress to 0) is a guess at `MentalCombat.CurrentMP`.
- 28 of 45 handlers are not translated (13 checks and 15 actions): the other shards, spells, quests, learning books, holy water, air/earth/water shards, bottle, rotten egg, pr0n, herb, rotten food, moon portal, purify, quest completion. They are mechanical translations of the same patterns, but each needs the system it touches.
- Messages longer than 768 bytes or 16 queued at once never occur in the shipped content, but a long chain of events in one action was not stress tested. Message text wrapping on the 22-column screen belongs to the UI.
- Real-device behaviour is irrelevant here (no platform code).

**Findings affecting later tasks:**
1. Task 27: the exporter generates, per content type, structs with event fields and the `Check`/`Action` enums; the quest and spell tables also need `can_accept`/`accept`/`can_complete`/`complete` and `can_cast`/`do_cast` (the generator emits spells already; quests are in the enum but not yet in a struct). `cast` is an Odin keyword (field renamed `do_cast`); more keyword clashes are possible in content names.
2. Task 19 (allocators): handlers format text with `fmt.tprintf` into the temporary allocator; `core_step` frees it at the end (task 15). No handler allocates persistent memory; entity creation uses the fixed pools.
3. Task 21 (RNG): the world holds one generator state; every roll is `roll(count, sides)` or `pick_weighted(weights)`; dice strings are never parsed at run time (the content has `Dice` values; the handlers here write `roll(w, 2, 4)` directly).
4. Task 22 (tests): handler tests need only a `World` and no mocks; the spike's scenarios are the model. A cheap, valuable check: run every `Action` on a small random world with a fixed seed and call `world_validate` (task 14) afterwards, so no handler can leave a dangling item, character or route.
5. The UI layer (task 12) calls `item_can_use` and `item_use`; the Message screen (task 11) pops from `world.messages`.

**Open issues:** none blocking. Next design tasks: 19 (allocators and strings), 20 (audio delivery), 21 (RNG), 22 (test strategy); then 24 (repo layout) is the gate for real code.

### Task 19: allocators and strings (2026-10-03)

**Decision:** D19. Measured with `spikes/memory-model` (a wasm module with probes called from JS) and with a tracking allocator added to `spikes/save-format` (native and `js_wasm32`).

**Facts about the wasm target (verified in the browser pane):**
- **Default allocator:** on `js_wasm32` it is `default_wasm_allocator`, a port of emmalloc. Memory it obtains is never returned to the browser, but `free` recycles it: allocating and freeing 8 x 1 MiB grew the module from 21 to 158 pages (64 KiB each, 10 MiB) and doing it again did not grow it at all.
- **Start size:** 21 pages = 1.3 MiB, which includes a 256 KiB static array I declared, the stack, and the data segment.
- **Stack:** 1 MiB (default). Recursion with 1 KiB frames worked to 800 KiB deep and trapped with "memory access out of bounds" at 1.6 MiB. After a trap the instance cannot be trusted. A trap is a hard crash with no recovery, so large locals are forbidden.
- **Temporary allocator:** 2,000 frames of 200 `fmt.tprintf` calls with `free_all(context.temp_allocator)` each frame: memory stayed flat. 500 frames **without** `free_all`: +65 pages (4 MiB). So `core_step` must free the temp allocator every call (it does, task 15), and anything that runs outside `core_step` (tests, load code) must do the same.
- **Browser limit:** a tab could grow a test memory to at least 1 GiB, far above anything planned.
- **Arena:** a `mem.Arena` laid over a 512 KiB heap block served exactly 128 allocations of 4 KiB and then failed cleanly (returned an error, no crash).

**Measured costs of saving and loading (a fresh-game-sized world, task 14; native 64-bit and wasm give nearly the same numbers):**
- The `World` struct of the spike (fixed pools of 2,048 characters and 4,096 items, 800 locations) is 646 KB. The real structs are larger, so budget up to 2 MB.
- **Save:** output 298 KB; peak heap while saving 740 KB (the intermediate shape plus the output); 1,785 allocations.
- **Load:** peak heap about 800 KB. Loading into the default heap **leaked** the parsed shape: 158 KB stayed allocated after every load. Loading into a dedicated arena fixed that: the arena used 314 KB (of a 2 MB buffer), the heap outside it grew by 0 bytes, the loaded world was identical, and an arena that was too small (64 KiB) made the load fail with a normal error instead of crashing.

**The rules (D19):**
1. **Long-lived state** is one `Game` struct allocated with `new` at startup (world, core, UI data, frame buffer 208 x 240 x 4 = 200 KB), never on the stack, never freed. `core_init` takes the pointer. Tests create and free their own with `new`/`free`.
2. **Per-step scratch** is `context.temp_allocator` only (formatting, short lists). `core_step` calls `free_all(context.temp_allocator)` as its last act. A step may not store a temp string anywhere that outlives it: the message queue copies text into its fixed buffers (task 18); services copy what they keep (task 15).
3. **Load and import** allocate a `mem.Arena` buffer of `max(1 MiB, 4 x file size)` with `make`, pass its allocator to `json.unmarshal` (`load_from_json` has a `parse_allocator` parameter), copy into the world, then `delete` the buffer. The 2 MiB import cap (task 14) therefore bounds this at 8 MiB; on a too-small arena the load returns an error. This is the only place that allocates in bulk, and the heap reuses the block afterwards.
4. **Save** formats the output with the default allocator (about 740 KB peak) and frees it as soon as it has been handed to `storage_set`.
5. **JS to Odin buffers** (the picked file) come from `platform_alloc` and are freed by the Odin side after the step (task 15). Audio files never enter wasm memory (task 20).
6. **No owned strings in long-lived data.** Entities and the world contain only numbers, enums and fixed arrays (tasks 14 and 17); content text (item names, lore, messages' static parts) is `string` literals in the read-only data; message bodies and any text that must persist across steps live in fixed byte buffers. Therefore there is no string ownership, `delete`, or `clone` anywhere in the core's steady state.
7. **Stack hygiene:** no local object over 64 KiB (the frame buffer, `World`, pools and the arena buffers are all on the heap or static); recursion depth is bounded (the only recursion in the game is none: the maze is iterative). The 1 MiB default is enough; it can be raised by a linker flag if ever needed.
8. **Leaks are test failures.** The test runner's tracking allocator reports unfreed memory per test; the loader and the handlers must be leak-free (the spike's `load_from_json` was not until it got the arena parameter).
9. **Budget for the wasm module** (all in the 21-page start plus growth): static and stack about 1.3 MiB, `Game` 2 to 3 MiB, load or save spike 1 to 2 MiB transient, audio and images not in wasm memory. Expected total well under 16 MiB, far from the browser limit.

**Verified:** the numbers above (browser pane for wasm; tracking allocator natively and in the wasm run of the save spike, which printed save peak 722 KB and load peak 784 KB, live after load 139 KB before the arena fix); the arena load is exact (`worlds_equal`) and graceful on exhaustion.

**Not verified:** mobile browser memory pressure (iOS Safari kills tabs at a few hundred MB; we are far below, but untested); real struct sizes; the memory behaviour over a long play session (fragmentation in emmalloc is not expected to matter with fixed pools and an arena, but there is no soak test yet: task 22/23 should include one); the cost of growing memory in the middle of a frame (a `memory.grow` can take a few milliseconds).

**Findings affecting later tasks:**
1. Task 22 (tests): the tracking allocator must be on for core tests; a long random-actions soak test (thousands of steps, `world_validate` after each, memory flat) is cheap and valuable.
2. Task 24 (layout): `Game` is created by the platform's `main`/init, not by the core.
3. Task 27: exporter output is read-only data; no allocation.

**Open issues:** none blocking.

### Task 20: audio delivery (2026-10-03)

**Decision:** D20. Implemented in `spikes/platform-api/platform.js` (about 50 lines of audio code) and checked in the browser pane; `audio_manifest_test.js` (node) checks the sound list against the `Sfx` enum.

**Assets (from `src/KordanorsCabal/Content`):** 8 stereo 44.1 kHz 16-bit WAV effects (0.12 to 1.63 seconds; 22 to 287 KB each, 1.14 MB together) and `MinorTheme.ogg` (1.89 MB, 119.9 seconds, looping). Total about 3.0 MB. The theme is a generated piece (README: Abundant Music, seed 2645320710). There is no `ffmpeg`, `sox` or other encoder on this machine, so no other format could be produced here.

**Design (D20):**
1. **Fetch, don't embed.** Embedding 3 MB in the wasm with `#load` would bloat the module, put audio in wasm memory (task 19) and delay start. Files sit next to the page in `assets/` and are fetched after the first gesture (itch.io serves them from the zip; the browser caches them).
2. **Effects are Web Audio buffers**: `decodeAudioData` on all 8 at the first gesture, in parallel; playing is `createBufferSource().start()` into a sound gain node. Latency is low and it works on iOS. Decoded PCM is held by the browser (about 4 to 5 MB of float samples), not in wasm memory. A sound requested before its buffer is decoded, or while the context is not running, is silently skipped.
3. **The theme streams** through an `<audio>` element with `loop = true`, connected to a music gain node with `createMediaElementSource`. This is deliberate: iOS Safari ignores `HTMLMediaElement.volume`, but a gain node works everywhere.
4. **Format fallback.** The platform keeps an ordered list of shipped formats (`m4a`, `mp3`, `ogg`), keeps those where `canPlayType` is not empty, ranks "probably" before "maybe", and tries them in turn, moving on at the first `error` event or `NotSupportedError`. This was needed: in the pane `canPlayType('audio/mp4')` answers "maybe" even though no such file exists, so a naive "first playable type" pick failed with a 404. With the chain, `.mp3` failed, and `.ogg` played (verified).
5. **Lifecycle.** Context and music start in the start gesture (task 16); any later pointer or key resumes a suspended context (iOS "interrupted" state after a call); the theme pauses when the page is hidden and resumes when it is shown again; failures are recorded in `__platform.audio.errors` and never stop the game.
6. **Core interface unchanged** (task 15): `Step_Output` lists up to 16 `Sfx` and gives `sfx_volume` and `music_volume`; the platform applies both volumes to the gain nodes every frame. Muting is volume 0.
7. **Native build** keeps SDL2_mixer with the same files (verified in task 2 and 15).
8. **Manifest check.** The JS sound list is ordered like the `Sfx` enum; `node audio_manifest_test.js` fails if the names, the count or a file are out of step (passes: 8 sounds).

**Verified (browser pane, desktop Chrome engine):** the audio context runs after the start gesture; all 8 effects decode with no errors; the theme starts, `currentTime` advances (3.28 s to 4.78 s over 1.5 s), `duration` is 119.9 s and `loop` is true; the fallback chain skips a missing mp3 and uses the ogg; changing the core's music volume through the menu changed the music gain node 0.5 to 0.75 to 1 to 0, and the sound gain follows `sfx_volume`.

**Not verified:**
- **Actually hearing anything**: the pane has no audio I can listen to. Decode and playback state were checked, not sound.
- **iOS and Safari**: whether Safari plays Ogg Vorbis is something I could not establish here, and I do not know the Safari version threshold, so Ogg must be treated as unsupported on iPhone until tested. The `m4a`/`mp3` slots in the fallback list are waiting for files. **This is the blocker for audio on iPhones (see "Needs your attention").**
- **Loop seam**: whether the ogg loops without an audible gap in `<audio loop>` (compressed formats can click or gap at the seam; MP3/AAC usually do, Ogg usually does not). If the seam is audible, the fix is to decode the theme into a Web Audio buffer and loop it with `loop = true` on a buffer source (uses about 40 MB of decoded float PCM at stereo 44.1 kHz for 2 minutes, which is too much for phones; a mono or 22 kHz decode would be 10 MB), so prefer to listen first.
- Autoplay policies on iOS inside an itch.io iframe (needs the real page).
- Sound effect overlap and mixing levels (not tuned), battery use, and Bluetooth latency.

**Findings:**
1. `canPlayType` is not proof that a file exists or plays ("maybe"); the fallback chain on errors is the real mechanism.
2. The effects total 1.1 MB and could be shrunk (mono, lower rate, or converted) if download size ever matters on mobile; not needed now.
3. Nothing in wasm memory is audio, so task 19's budget is unaffected.
4. The task 28 itch.io test must include: audio starts after the start tap inside the iframe, the theme loops, and the Ogg/mp3 choice on an iPhone.

**Open issues (also in "Needs your attention"):** an `.mp3` or `.m4a` of `MinorTheme` (or permission to install an encoder such as `ffmpeg`) is needed to cover iPhones. Listening to the loop seam needs a person.

### Task 21: random numbers (2026-10-03)

**Decision:** D21. Built and tested as `spikes/rng` (generator, helpers, a port of the maze algorithm, 12 checks) on native and `js_wasm32`.

**Why not `core:math/rand`:** the game needs the same seed to give the same world on the browser and on the native test build, a generator state small enough to save, no dependence on what a given Odin release does inside its default generator (a saved game must keep working after an Odin upgrade), and no modulo bias. A 15-line xoshiro256** is easier to guarantee than any of that.

**Design (D21):**
1. **Generator:** xoshiro256** (state four `u64`), seeded from one `u64` through splitmix64, as the algorithm's authors recommend. The state is exactly the `rng_state: [4]u64` that the save format (task 14) already reserves, and it is part of `World_State`.
2. **API** (the replacement for the VB `RNG` module): `rng_range(lo, hi)` (inclusive, VB `FromRange`), `roll(Dice{count, sides})` (VB `RollXDY` and `RollDice`; every dice string in the content is plain `XdY`, task 9, so the exporter turns them into `Dice` values and nothing is parsed at run time), `pick_weighted(weights)` (VB `FromGenerator`), `pick_index(count)` (VB `FromList`/`FromEnumerable`). Bounded values use Lemire's rejection method: no modulo bias.
3. **Seeding:** a new game takes `services.entropy()` (task 15) as its seed and stores it in the world (`seed`) so a bug report can say "seed 123". `?seed=N` on the page and `--seed N` on the native build force a seed for tests. Loading a game restores the saved state exactly (verified: the continued sequence equals the original).
4. **One generator for the game, none for decoration.** All game randomness comes from the world's generator, called in a deterministic order. Cosmetic randomness (the elemental orb that flickers because the VB UI called `RNG` while drawing, task 10) uses a separate, unsaved function of the frame counter, so drawing never advances the game's sequence and a redraw can never change what happens next.
5. **Determinism is a contract with consequences.** For a given seed the world generator and every rule produce the same results on every platform, so tests can compare exact outcomes. Changing the order or number of random calls in a procedure changes the worlds a seed produces; that is acceptable (saves store the finished world, not the seed), but golden-value tests will need regenerating when generation code changes on purpose.

**Verified (native and browser pane, identical output):**
- **Known answers:** splitmix64 from seed 0 reproduces the published outputs (`e220a8397b1dcdaf`, `6e789e6aa1b965f4`, `06c45d188009454f`); xoshiro256** from state {1, 2, 3, 4} reproduces the published vector (11520, 0, 1509978240, 1215971899390074240).
- **Cross-platform identity:** seed 12345 gives `be6a36374160d49b 214aaa0637a688c6 f69d16de9954d388` on both targets; a checksum over 1,000,000 draws (seed 777) is `118c3d0b5dcf42cf` on both; the maze hash for seed 1 is `e0e232178e19fd5a` on both.
- **Distribution:** 3,000,000 draws of `rng_range(0, 2)` were within 0.2 percent of uniform (limit 0.5); `3d6` spanned exactly 3 to 18 with mean 10.513 over 200,000 rolls; the real Magic Egg table (14 weights, total 2,168) over 2,000,000 picks had a worst error of 0.67 percent on the frequent entries and produced the 1/2168 entries about 925 and 944 times (expected 922).
- **Save and restore:** the state marshals to a JSON array of four numbers and the restored generator continues the identical sequence for 1,000 draws.
- **Maze (port of `Maze.Generate`):** 200 different seeds each produced exactly 120 open doors and a fully connected 11 x 11 grid (a spanning tree); the same seed gives the same maze; the average number of dead ends is 42.1 per maze, which matches the VB game's numbers (208 FE keys over 5 levels is about 42 per level), a good sign that the port reproduces the VB algorithm's statistics.

**Not verified:**
- **Quality beyond these tests:** no statistical test battery (xoshiro256** is well established; uniformity at this scale was checked).
- **The rest of generation** (location typing, boss/key placement, item and character population) is not ported yet; only the maze is; the invariants listed in task 9 are for the real generator.
- **A seed shown to players** (not decided: see "Needs your attention").
- Performance: 1,000,000 draws take a negligible fraction of a second natively; wasm speed was not timed separately (the spike finished well within the pane's wait).

**Findings:**
1. Task 22 can use fixed seeds for exact-outcome tests as well as invariant tests, on both targets.
2. The save format needs no change (`rng_state: [4]u64`); `world.seed` is a new number to add to the envelope.
3. The maze port is 60 lines and uses only arrays and the generator; the full world generator (task 9) is the same style.

**Open issues:** none blocking.

### Task 22: test strategy (2026-10-03)

**Decision:** D22. Built as `spikes/test-runner` (kit, example cases, native wrapper, wasm runner, `run_tests.sh`) and run on both targets.

**The VB tests, assessed:** `KordanorsCabal.Game.Tests` has 245 facts in 3,181 lines and `KordanorsCabal.Data.Tests` 155 facts in 2,682 lines. I read samples of both. They are almost all **interaction tests**: set up a Moq `IWorldData`, call a property, then `Verify` that particular data calls were made with particular ids ("have_current_hp" checks that `Character.ReadLocation(id)` and `Player.Read()` were called). They check how the code is wired to the data layer, not what the game does, and they mostly assert default or zero values. The data layer no longer exists (D1), and the interfaces they mock are gone (D9), so **none of them is worth porting** (D13); the Data tests exercise SQL that disappears entirely. The knowledge about game behaviour is in the VB source (formulas, rules) and in the running VB build, not in these tests.

**Test kit (verified on both targets):**
- `core:testing` needs an OS and threads, so it does not exist on `js_wasm32`. The kit is 50 lines: `T{name, failures, checks}`, `expect`, `expect_eq` and `run_all(cases)`; a case is a plain `proc(t: ^T)`; a suite is an array of `Test_Case{name, run}`.
- **Native:** one `@(test)` wrapper calls `run_all`, so `odin test` works and the output is the kit's per-case lines; the native `main` also works as a plain runner.
- **Wasm under node:** `run_wasm_node.js` (25 lines) loads Odin's own `odin.js` in node with `window` aliased to `global` and a timer-based `requestAnimationFrame`, runs the module, and exits with status 1 unless the output contains `TESTS PASSED` and not `TESTS FAILED`. No browser is needed, so the wasm suite can run in any CI or script. (The same module also runs in a browser page if a human wants to watch it.)
- **Leaks fail tests:** `run_all` wraps every case in a `mem.Tracking_Allocator` and fails the case if anything it allocated is still live. A deliberately leaking and failing case (enabled with `-define:SPIKE_FAIL=true`) was reported correctly and the script exited 1; the clean run exits 0.
- **Environment facts:** `odin test` must run with `-define:ODIN_TEST_THREADS=1` while the services are global fakes (task 15). `size_of(int)` is 4 on wasm and 8 on native, and one example case asserts that, as a reminder that code must not depend on it (the task 14 fuzz run already gave different outcomes on the two targets for a few mutated files).

**Test layers (D22):**
1. **Rule tests**, one file per ported system: build a small world, call the procedure, assert on the world and on the messages and sounds produced. The expected behaviour is taken from the VB source (formulas, texts) and, for exact values, from fixtures recorded from the running VB build (layer 8). Task 18 gave the pattern.
2. **World invariants:** `world_validate` (task 14) after generation, after load, and after every step of a seeded **random-action soak** (for example 20,000 random commands against `core_step`), with the tracking allocator asserting flat memory at the end.
3. **Generation invariants** (task 9), over many seeds: 121 cells per level, 120 doors, connected, exactly one boss room per level, one key per locked dead end, character counts equal the spawn counts, 737 locations and 1,722 routes. Plus golden values for fixed seeds (task 21), identical on both targets.
4. **Save:** exact round trip, byte-identical re-save, rejection of every kind of bad file, and the mutation fuzz from task 14 at larger counts. Import limits (2 MB) and the arena-exhaustion path of task 19.
5. **UI sessions:** feed scripted `Command` and `Tap` sequences to `core_step` with fake services, assert on screens reached, messages shown, storage written, sounds requested; and a handful of **golden frame hashes** for key screens (title, in-play, inventory) to catch accidental rendering changes. Few and cheap: a changed hash is reviewed, then re-recorded.
6. **Content and exporter freshness:** a check that regenerating the content files from `boilerplate.db` reproduces the committed files (`git diff --exit-code` after running the generators), and row-count checks against the database.
7. **JS and platform contracts:** the node tests already written (`layout_test.js`, `audio_manifest_test.js`) plus the fake-services tests of task 15.
8. **Differential tests against the VB build (planned with task 23):** the VB game runs headless (task 6 proved it from a small C# console project). A harness can record, for fixed states and inputs, the exact output of deterministic VB procedures (damage and defence math using maximum rolls, encumbrance, prices, repair costs, XP thresholds, message texts) as JSON fixtures; the Odin tests then check the same numbers. This is how the VB behaviour that the old tests never captured gets pinned down. Because the VB RNG cannot be seeded, random behaviour is compared statistically or through maximum-roll variants (`RNG.MaximumRoll`).
9. **One command:** `tools/test.sh` (placed in task 24) runs the native suite, the wasm suite under node, the node tests, and the freshness check; it exits non-zero on any failure. Soak and fuzz sizes are parameters (small for every run, large for release checks).

**Verified:** the kit, native wrapper and node runner pass the same 8 cases on both targets (10,209 checks; xoshiro256** vector, seeds, dice, 50 mazes, the golden maze hash `e0e232178e19fd5a`, no leaks, `int` size); failure and leak reporting work with exit status 1.

**Not verified:** the harness against the VB build (layer 8) has not been written; soak and golden-frame tests need the real core; `odin test` with parallel threads (needs the fakes to stop being globals); the test kit on Windows (not needed).

**Findings:**
1. The wasm-under-node approach also gives a place to run **performance and soak checks on the real target** cheaply; and it lets the final build be smoke-tested without a browser.
2. The "recorded golden value" style (maze hash) works across targets because the integer-only generator is identical (task 21); floating-point is not used in game rules, which should stay true.
3. Test code lives next to the code it tests and compiles into the same package only under `#+build !js` where it needs `core:testing`; the kit itself is portable and can be part of a `tests` package that the wasm test build includes.

**Open issues:** the VB differential harness depends on being allowed to put a small C# project in the repo (`tools/vb-oracle/`) or keep it outside; see "Needs your attention".

### Task 23: reference captures from the VB build (2026-10-03)

**Decision:** D23. Built `tools/vb-oracle` and recorded `docs/reference/vb/` (218 files, 1.5 MB).

**What was built:** a small C# console project (`tools/vb-oracle`, about 330 lines) that references the VB projects and runs the original UI state machine headlessly: it sets the hooks that `Root` normally sets (UI stack, options callbacks), feeds `Command`s to `MainProcessor.ProcessCommand`, draws with `MainProcessor.UpdateBuffer` into the real `SPLORR.UI` renderer, and writes each screen as text, exact cells, a PNG (208 x 240, 2:1, 3x) and a frame hash. A script language (`scenes.txt`) drives it: `! meta` commands set up game state directly (start a game, move the player to a location type or a feature, give items, add money, XP or wounds, set the mode, reset the UI), `: D G ...` send commands, `pick N` and `button N` address menu items and in-play buttons by index (the VB screens remember their cursors), `finalize` spends leftover character points, `snap NAME` records. Everything the harness does is something a player could do, except the meta setup shortcuts, which are listed in `Program.cs`.

**What was recorded:** 54 screens in `docs/reference/vb/` (the title, instructions, about, options and the three option screens, quit confirmation, load, save, abandon, game menu, character finalization, prolog, status, level up, the in-play screen in town and in the dungeon, turn and move modes, all nine townsfolk (elder, innkeeper, drunk, chicken, black market, black mage, blacksmith, healer, constable), the four shoppe screens, enemies, inventory, item interaction, equip and use messages, equipment, map, ground items, spell list, fight and counter-attack messages, death). **30 of the 32 UI states** are covered; `EquipmentDetail` and `ShoppeRepair` are not (they need an equipped worn item and a repair scenario). The script was run three times with different random worlds: no errors, because `finalize` and `snap-if` absorb the one source of divergence (a random roll that may or may not leave unassigned points).

**Verified:**
- **The Odin rasterizer is bit-exact:** a test in `spikes/platform-api` loads each recorded `.cells` file into the Odin core, rasterizes, and compares an FNV hash of the 208 x 240 frame with the VB renderer's hash: all 54 screens match. This proves, on real screens, that the extracted glyph table, the character map, the palette, the border, and the `bit 0 = leftmost pixel` rule all reproduce the original output exactly (task 8 had not been able to claim that).
- **The VB texts are on record**, for example the potion message (`POTION HEALS UP TO 6 HP!`, `YOU NOW HAVE 3 HP!`, hard-wrapped at 22 columns inside words), the combat transcript (`YOU ROLL AN ATTACK OF 0.`, `SNAKE ROLLS A DEFEND OF 1.`, `YOU MISS!`, `COUNTER-ATTACK 1/1:`, `SNAKE DOES 1 DAMAGE!`, `YOU HAVE 2 HP LEFT.`), the shop and townsfolk menus and the spell, level-up and death screens. The potion text matches the Odin handler from task 18.
- The UI layout facts from task 12 (10-button bank at rows 18 to 22, two columns of 11 cells, button indices by position) are confirmed by driving the real screens.

**Defects of the VB game found while recording (for the fix list under D13):**
1. **Continue (Load Game) crashes when a save slot file does not exist or is not a valid game database** (`SQLite Error 1: no such table: Players`, from `LoadGameScreenProcessor.ValidateSlot`). The oracle hit this on an empty working directory: opening a missing slot file makes SQLite create an empty database without tables, and `World.IsValid` queries a table that is not there. The same code runs in the Save screen's slot check. This is a harness observation of the original code path; I did not confirm it in the shipped binary on a fresh machine. The port is immune by design (slot existence is a storage lookup, task 12/13).
2. **Randomly rolled characters often start with unassigned points**, so the Finalize Character screen appears in a large share of new games (it appeared in about half of the harness runs); not a defect, but it means that screen is part of the common path, not an edge case.

**Not verified:**
- Not recorded: `EquipmentDetail`, `ShoppeRepair`, quest flows (cellar rats), most spell casting results, the moon, and the long tail of item effects; the oracle can reach them by extending `scenes.txt` when each system is ported.
- The reference frames come from the .NET 10 runtime running the netstandard2.1/net6 VB assemblies; the output is deterministic text and integer pixels, so a different runtime should not change it, but this was not cross-checked.
- Game rules involving the VB random generator cannot be compared exactly (it cannot be seeded); the oracle gives exact values only for deterministic paths and screens.
- `tools/vb-oracle` builds only with a .NET SDK available; it is a development tool, not part of any build.

**Findings:**
1. The oracle is also the tool for the differential tests in D22 (layer 8): it already instantiates the real Game layer, so recording fixtures for damage, defence, encumbrance, prices and XP rules is a matter of adding meta commands that print values.
2. The 54 `.hash` files are an immediate regression test for the Odin renderer and a template for per-screen golden tests of the ported UI: once a screen is ported, feed it the same game state (via a shared description of the scene) and compare cells exactly.
3. To compare a ported screen with its reference, the scene's game state has to be identical in both games; the recorded scenes are random-world dependent, so screen-level differential tests will use *static* screens (menus, texts) and constructed states, not the random world.

**Open issues:** none blocking.

### Task 24: repository layout and the first real slice (2026-10-03)

**Decision:** D24, and the repository skeleton was created and made to work end to end (this is the start of the port proper: infrastructure and one screen, no game rules).

**Layout (created):**
```
odin/game/            package game      core: api.odin (platform interface), core.odin (core_init/core_step), render.odin (cells, hues,
                                        rasterizer), rng.odin, pool.odin, font_data.odin (generated)
odin/platform/web/    package main      main.odin (wasm entry and exports), storage_web.odin; page/ = index.html, platform.js, layout.js,
                                        storage.js, layout_test.js, audio_manifest_test.js
odin/platform/native/ package main      main.odin (SDL2 window, keyboard/mouse, SDL2_mixer), storage_native.odin (files)
odin/tests/           package main      testkit.odin, tests_core.odin, reference_data.odin (generated), main_native.odin, main_js.odin
tools/                build.sh test.sh serve.sh run_wasm_node.js; gen/ (gen_font.py, gen_reference.py); vb-oracle/
docs/reference/vb/    54 recorded VB screens
build/                outputs (git-ignored)
```
Packages: one `game` package holds all portable code (Odin has no `private` by default, so tests in a separate package can reach what they need); each platform is its own `main` package that imports `kc:game` (collection `kc` = `odin/`, passed as `-collection:kc=odin`). `#+build js` / `#+build !js` tags stay on the platform files as a guard (D5/D6). Generated files carry a header naming their generator, and `tools/test.sh` fails if regenerating changes them.

**Handling of the VB code (task 25):** `src/` stays exactly as it is, as the behavioural reference, until the port reaches parity; then it can be removed in one commit. `pub-linux/` and `pub-windows/` (published VB binaries) and the old `shippit.sh` are left alone. `spikes/` is kept as history (each spike is documented in this file); anything proven in a spike has been promoted into `odin/` (RNG, pools, rasterizer, platform interface, storage shim, test kit, node runner), and spikes are not referenced by the build.

**What runs now:**
- **Core:** the title screen, ported from `TitleScreenProcessor` and `MenuProcessor` (menu with six items, selection moved by Up/Down or by tapping a row, wrapping), drawn through the ported renderer.
- **Web build:** `tools/build.sh web` produces `build/web` (platform.wasm is 48 KB; plus page, `odin.js`, the sounds and the theme). Checked in the browser pane: the screen shows the title exactly as the VB game does, with the selection moving.
- **Native build:** `tools/build.sh native` produces an SDL2 binary (opened a window and ran until stopped).
- **Tests:** `tools/test.sh` runs five groups and passes: generated files up to date, the native suite, the same suite as wasm under node (10 cases, 183 checks on each target), the node tests for the page logic (layout for 13 viewports, sound manifest), and a type check of both shipping targets. Notable cases: the ported rasterizer reproduces all 54 recorded VB screens bit for bit, and **the Odin title screen equals the VB title screen exactly** (cells and frame hash): the first end-to-end golden test.

**Verified:** all of the above, run just now; `tools/build.sh all` and `tools/test.sh` exit 0.

**Not verified:** builds on Windows; a clean checkout (the build copies assets from `src/KordanorsCabal/Content`, so `src/` must be present until the assets move); `tools/test.sh` in a CI service (none is configured); the web build on a phone.

**Findings / notes:**
1. The `game` package currently has no dependencies outside `base` and `core`; keep it that way (D6).
2. The collection flag has to be passed by every tool (`odin build`, `check`, `test`); the scripts do. An IDE needs `-collection:kc=odin` too.
3. `tools/build.sh` copies the audio from `src/KordanorsCabal/Content`. When the VB code is removed, move those files to `assets/` first.
4. The web page's music list now only names the `.ogg`; add an `.mp3`/`.m4a` entry when such a file exists (blocker in "Needs your attention").
5. Task 27 (content exporter): `tools/gen/` is where it goes; `gen_font.py` and `gen_reference.py` are the pattern, and the prototypes for enums and events are in `spikes/save-format/gen_stub.py` and `spikes/event-dispatch/gen_events.py`.

**Open issues:** none blocking.

### Task 25: what happens to the VB code (2026-10-03)

Decided together with task 24 (see above): `src/` is kept untouched as the reference until parity, then removed in one commit; the published VB binaries and the old `shippit.sh` are left in place; `docs/reference/vb/` and `tools/vb-oracle` keep working from `src/` until then. The VB projects remain buildable and are used by the oracle. Nothing else to do.

### Task 26: packaging for itch.io (2026-10-03)

**Done:** `tools/ship.sh` replaces the VB `shippit.sh` for the web target (the old script is untouched and still ships the VB builds). It runs `tools/test.sh` first, builds the web target with `-o:size`, and writes `build/kordanors-cabal-html5.zip` with `index.html` at the zip root, as itch.io requires. Pushing is opt-in: `tools/ship.sh --push` runs `butler push build/kordanors-cabal-html5.zip thegrumpygamedev/kordanors-cabal:html5`. Without the flag nothing leaves the machine. The old script's trailing `git add -A` and `git commit "shipped it!"` was not carried over (publishing a build and committing should be separate acts).

**Verified:** the script ran: tests passed, the web build was made with `-o:size`, and the zip is 2.7 MB with 15 files (`platform.wasm` is 26 KB with `-o:size`; the audio is 2.4 MB of it). I did not run `butler` (it needs a login on your account and uploads publicly visible content).

**Not verified:** the upload itself, the itch.io page settings (kind of project = HTML, "SharedArrayBuffer" is not needed, embed size, fullscreen button, mobile friendly), and whether the channel name `html5` matches what you want. See "Needs your attention".

### Task 29: save export and import (2026-10-03)

> **Amended (decision review 4):** placement is on the existing screens, not the Options screen. The **Save Game screen** (reached from the in-game menu) gets an extra item "Export a slot..." that opens a slot chooser listing the 5 slots; choosing a used slot calls `download_text`. The **Load Game screen** (reached from the title menu) gets an extra item "Import a file..." that calls `request_file_pick`; when the file arrives and passes validation, a slot chooser asks where to put it, with a confirmation before overwriting a used slot. Consequences: exporting is only possible from inside a game and importing only from the title screen (the two screens exist in those places in the VB game); a person with saves on another device can import before starting a game, and a person who wants a backup must load or start a game first. The "Options screen" proposal in the section below is superseded.

**Done:** the web platform (`odin/platform/web/page/platform.js` and `index.html`) now implements the two services of the platform interface (`download_text`, `request_file_pick`) with a small modal, and the browser risk found in task 15 is designed out.

**The design (D11 made concrete):**
- **Export:** the core builds the slot's JSON (task 14) and calls `download_text("kordanors-cabal-slotN.json", text)`. The platform shows a modal "SAVE FILE READY" with a real `<a download>` link; the person taps it, the browser downloads the file, and the modal closes.
- **Import:** the core calls `request_file_pick()`; the platform shows a modal "CHOOSE A SAVE FILE" containing a real `<input type="file">` styled as a large button that covers the label, plus a Close button. The person taps the button, picks a file, and the platform reads it (rejecting files over 2 MiB), copies the bytes into a buffer allocated on the Odin side and delivers them as a `File_Text` event; Close, a rejected file or a read failure delivers `File_Cancelled`.
- **Why a modal instead of opening the picker or the download from the frame loop:** Safari on iPhone (and sometimes other browsers) only allows `input.click()` and programmatic downloads from a direct tap, and the core asks from inside `requestAnimationFrame`. A real control that the person taps is allowed everywhere. The cost is one extra tap. The core interface did not change.
- **Before anything replaces a slot** (core side, unchanged from task 14): parse into a temporary world through an arena (task 19), run the loader's checks and `world_validate`, and only then write the slot; a failure leaves the slot untouched and shows a message. A file that is not JSON, from another game, from a newer version, or tampered with is rejected with a specific message.
- **Where the person finds it (proposal for the UI port):** the VB screens have no place for this. Add two items to the Options screen, "Export save..." and "Import save...", each opening one new menu screen that lists the 5 slots (a copy of the Save screen's slot list); choosing a used slot exports it, choosing a slot after a successful pick imports into it (asking before overwriting a used slot, using the same confirmation style as the abandon-game screen). This adds one `UI_State` and keeps Save and Load unchanged.
- **Native build:** `download_text` writes `exports/<filename>`; `request_file_pick` reads `./import.json` (a stand-in for a dialog), enough to test the flow.

**Verified (browser pane, with the real wasm of the port):**
- The export modal appears with a download link whose `download` attribute is the requested file name and whose content is exactly the text the core passed; tapping the link closes the modal.
- The import modal appears with a file input; choosing a file delivers its bytes (20 bytes in the test) to the Odin side; Close delivers a cancel; a file larger than 2 MiB is refused as a cancel.
- The type check and the full `tools/test.sh` still pass.

**Not verified:**
- **Real taps on iOS Safari and Android Chrome.** The design satisfies the user-gesture rules by construction, but no phone has run it. This remains in task 28's checklist.
- The actual download in the pane (the test prevented the browser's default so no file was written) and the file picker dialog itself (synthetic file selection was used).
- Behaviour in an itch.io iframe (downloads inside sandboxed iframes need `allow-downloads`; itch.io sets its sandbox attributes, see task 28).
- The Odin side of the flow (a menu that exports and imports) does not exist yet; the title-only core does not call the services. The toy core of task 15 exercised them end to end earlier (export text, import valid/invalid/cancelled, slot untouched on bad files).

**Findings:** the whole transfer feature is about 45 lines of JS and nothing in wasm; the 2 MiB cap is enforced in two places (JS, and again in the core before parsing).

**Open issues:** the iframe download permission (task 28) and the UI placement proposal above.

### Task 30: dead code audit (2026-10-03)

**Done:** `docs/dead-code-audit.md` records everything that is dropped or fixed, with evidence. Methods: (1) the reading done in tasks 6 to 13 (hooks, commands, unused columns); (2) a script over all non-test VB source that lists members declared but never referenced elsewhere; (3) reachability queries over `boilerplate.db` for item types (spawn tables, loot, shops, bribes, locks, code references), character types, statistics, route types and lore; (4) defects observed while recording the reference screens (task 23).

**Results:** 638 declared members were scanned and **17 are never used** (listed in the audit). Content is lean: every statistic and route type is used, all character types are reachable, and only **three item types are unobtainable** (Membership Card, Ring of HP, Amulet of DEX). Twelve defects and quirks of the VB game are listed with the port's chosen behaviour (the rotten food bug, the Continue crash on missing slots, stale labels, the message queue leak, the sound in the draw routine, the orb flicker, null references in fight items, the empty-lore crash, unmapped characters, generation assumptions, popping an empty UI stack).

**Not verified:** the unused-member scan is a text heuristic (a member used only through reflection or only named in a string would be misreported; none was found, but interface members and their implementations were counted together); the three unobtainable item types could be reachable through a path I did not recognize (a quest reward in unported UI code; I searched all code for their ids); the author's intent for those three is unknown.

**Rule going forward:** every system port starts with a reachability check of what it is about to port, and adds any new finding to this file.

**Open issues:** the three item types (keep rows, no way to obtain them, unless told otherwise).

### Task 27: content exporter (2026-10-03)

**Decision:** D8 carried out. `tools/gen/gen_content.py` reads `src/KordanorsCabal/boilerplate.db` and writes `odin/game/content_enums.odin` (an enum for every content table with values equal to the database ids and 0 = `None`, plus the `Check` and `Action` event enums) and `odin/game/content_data.odin` (the definition tables). The hand-written shapes are in `odin/game/content.odin`. The output is deterministic, and `tools/test.sh` fails if regenerating changes it.

**What is exported:** 53 item types (name, nine statistics, equip slots, the four event slots, which shoppes buy, sell or repair them, per-level spawn rule with dice and allowed location types, stat buffs when worn), 16 character types (name, XP, money dice, undead flag, initial statistics with the statistic defaults applied, attack-type weights, bribe items, enemies, loot table, parting shot, per-level spawn counts and location types), 37 statistics, 8 directions (with previous, opposite and next), 8 location types (flags), 9 feature types (location type and interaction mode), 11 route types with their locks, 2 spell types (events and required power), the quest type (events), shoppe, equip-slot and dungeon-level names, and the 25 lore texts. Dice strings become `Dice{count, sides}`; the unused add-to-inventory event is dropped. Compile output is clean (about 490 lines of generated Odin).

**Problems the checks found (and the fixes):**
1. **Folding silently dropped data.** A test that recounts the folded relations against the generator's own counts failed: 87 item spawn location rows existed but only 37 were kept, because I had attached locations only to levels that also have spawn dice. Items such as the keys have places to spawn but no dice (the world generator places them, task 9). The spawn rule now exists for every level that has dice or locations. Without the test this would have shipped as missing keys.
2. **Statistic defaults.** The VB game reads a missing character statistic as the statistic type's default value, and two statistics default to 1 (Unarmed Maximum Damage and Base Maximum Defend) while 60 of the source rows are explicit zeros. The export applies the defaults and drops zeros, so a character's `initial_stats` array is the effective value (absent in the source = default, as in VB). Without this a type missing those rows would have started with 0 instead of 1.
3. Fixed-size arrays were needed for loot (at most 4 entries per character type) and parting shots (1): slices in a global table make the compiler warn that the initialization is non-constant and large; fixed arrays with an unused-slot convention (weight 0) compile as data.

**Verified:** `tools/test.sh` passes (12 cases, 224 checks on native and wasm), including `content: sizes, names and a few facts` (enum values equal the database ids, potion price, rotten food events, undead flags, direction opposites, lore text, level names) and `content: nothing was lost when the relation tables were folded` (equip slots, shop pairs, spawn levels and location bits, buffs, initial statistics, attacks, bribes, enemies, loot, parting shots, spawn counts and locations all recounted from the Odin tables and matched to the generator's counts and to the source row counts).

**Not verified:**
- The semantics of a few columns were taken from the code without running it: transaction ids (1 offer, 2 price, 3 repair) and attack types (1 physical, 2 mental) are from constants in `ItemType.vb` and `AttackType.vb`; feature `interaction_mode` is kept as the raw VB player mode id.
- Keyword clashes: none occurred in the current content; a future content change with a name like `cast` or `map` would fail to compile and need an exception list in the generator.
- Which statistics the Odin game uses as an array index (`[Stat]i32` includes `None` at 0 and 37 more).

**Findings:** the whole static game definition is 490 lines and a few kilobytes of read-only data (the wasm module is still about 48 KB). The generator's pattern (fold in Python, test the fold in Odin) caught two real bugs in its own first version, so it is worth keeping the same pattern for any later generated data.


### Port step 1: world state, generation, save and load (2026-10-03)

**Done:** the first real game systems, in `odin/game/`: `uuid.odin` (ids), `world.odin` (the mutable world and its operations), `worldgen.odin` (a port of `World.Start`: town, five mazes with locks, keys and bosses, the moon, features, the player and her two stat rolls), `save.odin` (JSON writer, validating loader, `world_validate`). `pool.odin` and its two tests are deleted (superseded by D9's UUID maps). New tests in `odin/tests/tests_world.odin`; the suite is now 19 cases, 1,203 checks, passing natively and as wasm under node.

**The shape (decision D9 as amended):**
- `Character_ID`, `Item_ID`, `Location_ID` are distinct `[16]u8`, version 4 UUIDs drawn from the world's seeded generator (so a seed always yields the same ids), never reused. Text form is the canonical 36 lower-case characters; the parser is strict (a hand-edited id with capitals or missing dashes is rejected).
- `World` holds three maps (id to value) and three order lists (creation order). The order lists exist because map iteration order is not preserved by a save and load, and the game must behave the same after loading. Deleting from an order list is a linear search; deletions happen a few times per fight, not per frame.
- Items know their holder (`None/Carried/On_Ground/Equipped`), the owner id, the slot and a `placed` counter that orders item lists (the order the original showed). Characters know their location. Locations hold their own eight direction slots (`routes: [Direction]Route`), the feature type and a `visited` flag; no one else records any of it.
- Item stats, events and names are derived from the type; the only saved per-item values are `type`, `wear`, `lore` and the holder fields (task 14's evaluation). Characters save their non-zero statistics as `[stat id, value]` pairs.
- Player state: character id, mode, facing, shoppe, `quests_active` (bit set), `quest_completions`, `spells` (level, 0 = unknown).
- Allocation: **this amends D19 rule 1.** The world owns heap storage (the maps and lists), allocated from the allocator given to `world_init` and released by `world_destroy`. Everything else about D19 holds: per-step scratch is the temporary allocator, loads parse into a disposable allocator (the tests use the temporary allocator, the game will use an arena), and the leak checker covers all of it.

**Generation, compared with the VB code:** same structure and the same counts (737 locations, 1,722 routes, 1,086 monsters plus the player, 25 notes, the right number of keys); the random draws are in a different order and use a different generator, so worlds cannot match the original number for number (decided in task 9). Two deliberate differences: (1) the player's roll tables are arrays of (stat, weight) pairs; (2) if the data asks for monsters where no location allows them, the spawn is skipped instead of throwing (the shape test would catch it: the monster count would be short).

**Verified:**
- `worldgen: a new game has the shape the original builds` (5 seeds): 737 locations, 1,722 routes, monster count equals the sum of the data's spawn counts plus one, location type counts (1 square, 8 town, 1 church entrance, 1 cellar, 5 bosses, 121 moon), every feature on exactly one location of its type, the cellar below the inn, the player alone in the town square with 13 stat points in total.
- `worldgen: every level is a spanning tree...` (5 seeds): everything except the moon is reachable from the square through routes; per level: 121 cells, one boss, one boss door, FE doors = dead ends - 1; one FE key on the ground per FE door; the four boss keys exist; 25 notes; 5 x 240 corridor routes.
- Same seed gives byte-identical saves (ids included); a different seed differs.
- Save: a fresh game is **471,985 bytes** (the estimate was about 480 KB); load then re-save is byte-identical; creation order and wear, worn slot, spell, quest and shoppe all survive; the loaded game creates the same next id and draws the same random numbers as the original for 50 steps.
- Rejections: empty, non-JSON, wrong format, future version, no player, bad seed, a character on a nonexistent location; each returns an error and leaves the target world empty (nothing leaks; the leak checker is on).
- Fuzz: 60 mutated saves (digit flips, random bytes, truncation), 5 accepted and all of those still valid and re-savable, 55 rejected, no crash natively or in wasm.

**Odin facts found:** `&map[key]` returns nil for a missing key (the code relies on it for "gone"); a `proc` taking `$T` with `where size_of(T) == 16` lets one text function serve the three distinct id types; constants cannot be indexed by a variable (use a variable for tables); `for x in 0 ..< roll(...)` evaluates the roll once.

**Not verified:**
- Whether every spawn count in the data is satisfiable on every seed (the shape test would show a wrong monster count if not, on five seeds; there is no test over the data alone yet).
- Behaviour beyond fresh worlds (no play exists yet), so churn-heavy saves are untested.
- Timing: generation plus save took well under the test's seconds natively; wasm timing in a browser has not been measured. The save is 472 KB of JSON and the browser has to write it to `localStorage` synchronously.
- u64 seeds and generator words are written as hex text (not numbers) because Odin's JSON reads integers as signed 64-bit; this is not tested with a seed above 2^63 (the tests use small seeds, but the generator words are full-range and round-trip, which covers the same parsing).

**Findings affecting later tasks:** the slot summary (`place`, `hp`, `xp`) is written and `save_peek_summary` reads it, but it still parses the whole file; if slot labels are drawn every frame this must be cached by the Load/Save screens (step 2). The "who is here" queries scan all characters (about 1,100) and the item lists scan all items; fine for a few calls per frame, to be re-checked when the in-play screen exists.


### Port step 2: the UI shell (2026-10-03)

**Done:** `core.odin` is now the screen state machine of the original (`UI_State`, one cursor per menu that survives leaving and returning, as in `MenuProcessor`), with `ui_screens.odin` (all screens, input, drawing) and `config.odin` (volumes). Screens: title (7 items), seed entry, instructions, about, credits, options, SFX and MUX volume, confirm quit, Continue (load), Save, Export, Import (wait for the file, choose a slot), game menu, confirm abandon, Finalize Character, prolog, notices, and a placeholder "In Play" (to be replaced in step 3; Confirm opens the game menu so the save/abandon flows work). Taps follow D17: a mouse tap selects and confirms, a finger tap selects first and confirms on the second tap; on pages any tap is Confirm.
- New game: Start draws the seed from the platform's entropy (0 to 999,999,999) and generates the world; "Start with seed..." edits nine digits (Left/Right choose, Up/Down change, Confirm starts, Cancel goes back; the same cells can be tapped); Finalize Character is shown only when points are left unassigned (the two stat rolls often use them all) and Cancel there throws the new world away.
- Save writes the world text to `kc:slotN` (failure shows a notice and keeps the game); Continue reads it, loads into a scratch arena (D19) and replaces the live world only if it loads; Export hands the stored text to `download_text` as `kordanors-cabal-slotN.json`; Import asks for a file, checks the size (2 MiB) and the whole save (parse, shape, invariants) before offering the slots, and stores the text only when a slot is chosen, so a bad file never touches a slot. Volumes are stored as `kc:config` and read at start.
- Slot labels are read from the first 512 bytes of each slot (`save_peek_summary`, no full parse), refreshed whenever a slot screen opens (this also fixes the VB stale-label defect).

**Differences from the original, all deliberate:** the title has the extra "Start with seed..." line; Options has no "Screen Size" (D13); the Load and Save screens have one more item (Import / Export); About's last lines point at Credits; Continue on an empty slot shows a notice instead of silently starting a new game (the VB `ContinueSlot` did that, which looked like a bug; say if you disagree).

**Verified (25 cases, 1,279 checks, native and wasm under node):** screens compared cell for cell with the recorded VB frames: title (all but the menu rows), instructions, about (all but the changed last lines), options (all but the dropped item), SFX and MUX volume, confirm quit, load (all but the extra item), finalize character (with the recorded stat values), prolog, game menu and confirm abandon. Behaviour tests: wrapping cursor, finger and mouse taps, border taps ignored, quit needs Yes, volume stored and restored (and bad config falls back to the defaults), new game to prolog to in play, typed seed reproduces the same world (same player id), save, abandon, continue, empty and damaged slots, export (the download equals the slot text), import (cancel, non-save file, good file into a slot, then continue from it), credits scrolling clamp. The wasm build was started in the browser pane: Start generated a world and showed the prolog.

**Not verified:**
- The Save screen was not compared with `16-save-game` (it is the Load layout plus one item; `09-load-game` covers the layout).
- Credits and notice layouts have no reference (they are new); the credits text still lacks the sound effect sources, and the music line says only "generated with Abundant Music" (owner to confirm the wording).
- Real taps on touch devices; the file picker modal in a browser with the new core (the platform side was tested in task 29, the core side here with fakes).
- Time: a full native test run grew from 7 s to 20 s because each UI test generates and loads a 470 KB world under the leak-checking allocator; the game itself does one such load per Continue.

**Findings affecting later tasks:** (1) `Core` still has no message queue or UI stack; the in-play step needs both (a stack of four is enough, per task 12). (2) The tests' `pick` helper bounds its search because a stuck cursor otherwise loops forever; keep that. (3) `build/t_native [name]`-style runs: the native test binary now runs only the cases whose names contain its first argument (the `odin test` run is unchanged). (4) World generation plus parsing dominates test time: prefer one generated world per test.


### Port step 3: the in-play screen, movement, map and status (2026-10-03)

**Done:** `ui_play.odin` (the hub: ten buttons, the Neutral, Turn and Move modes, the dungeon view, messages, map, status, death), `rules.odin` (messages and sounds, health, encumbrance, who is where, `can_move` and `move_character`) and `tools/gen/gen_ui.py` -> `ui_data.odin` (the 128 `Glyph` names, 15 character sprites, the portal sprite, and where each of the 53 item types is drawn on the floor; checked for freshness by `tools/test.sh`). The core gained the UI stack (depth 4, "where to go after the messages"; an empty pop falls back to the in-play screen instead of throwing), the button cursor and its stack, and a step counter. The messages queue (16 messages of 768 bytes, D18) and a small queue for sounds the rules raise live in the `World` as runtime-only fields, cleared with the world and never saved.
- Movement is a faithful port of `CharacterMovement.Move`: hunger rises by 1 (more when high or food poisoned) and drunkenness, highness, food poisoning and chafing fall by 1 with each step; a locked door needs the key in the pack, spends it, opens the door for good and plays the unlock sound; single-use routes (portals) vanish; at 100 hunger the hunger halves and the walker loses a hit point ("You take damage from starvation!" as a message, or the death page if that was the last one). Encumbrance (items carried and worn against 50 + 10 x strength, from the data) stops all movement. The place is marked visited.
- The buttons show the original titles for every state that exists today, so FIGHT!, RUN!, Enemies(n), Ground..., Inventory, Equipment, Spells, Interact... and Intimidate! already appear when they should, but only Turn, Move, Map, Status and Game Menu do anything. The rest are marked `TODO(step N)` in `handle_button`: items (4), combat and level-up (5), townsfolk and spells (6).
- Status shows the seed on its last line as decided in review 6 (`Seed nnnnnnnnn`).

**Differences from the original:** (1) the Elemental Orb's colour follows the step counter (it changes about ten times a second) instead of a random colour every frame; (2) Red in Turn or Move now restores the previous button position like the Cancel button does (the original left it on the stack); (3) the recorded dungeon spaces have a black hue and ours blue; they look identical (the tests compare what is visible); (4) route types 7 and 8 show the portal picture for any route, the original threw for other types (none exist).

**Verified (30 cases, 1,358 checks, native and wasm under node):** compared with the recorded VB frames cell for cell: town square in play (`11`), status (`12`, all but the seed line), turn mode (`13`), move mode (`14`) and the dungeon with two goblins, items on the floor, a door ahead and a passage to the right (`40`), which exercises the walls, doors, sprite, item glyphs, the facing letter and all ten button titles. Behaviour: turning and moving update the facing, the location, visited, hunger and the button cursor; walls do nothing; locked door without and with the key; key spent; door opened; unlock sound collected by the core; encumbrance blocks every direction; starvation message, hunger halving, hit point lost, the pop back to the in-play screen; death page and return to the title; the map draws the visited place inverted with its legend; the message queue is first in first out and bounded; popping an empty UI stack is safe. The web build was run in the browser pane: new game, prolog, the Move screen renders like the recording.

**Not verified:**
- The map against the original (no reference frame was recorded; the code was ported by reading `MapProcessor`). Only one visited place was drawn in the test, so the elbow selection for corridors is untested.
- `location_decay_items` is a stub (food does not rot yet; step 4).
- The portal route pictures and the town portal text (no test world has portals).
- Hunger and the other statuses over a long walk (only single steps were tested).
- Real-device input for the button bank (taps use the same two-step rule as menus).

**Findings affecting later tasks:** (1) Handlers must not destroy while iterating the world's order lists (my own test did and skipped characters): collect ids first. (2) Interact with `Intimidate!` shows only when the enemy has willpower and is not backed by a crowd, as in the original; with two goblins the button is blank, as the recording shows. (3) The next screens (lists, item interaction) need the same two-step tap rule: factor `tap_to_row` out when the first list screen is written.


### Port step 4: items, equipment, the ground and the item events (2026-10-03)

**Done:** `items.odin` (names, durability and wear, pick up, drop, equip and unequip, the grouped inventory list), `events.odin` (the event dispatch of D18: `check` for all 18 checks, `perform` for the actions that need neither combat nor quests, `item_use` with the single-use rule, `item_decay`, `location_decay_items` which replaces the step 3 stub, so walking now rots food), `ui_items.odin` (Inventory, Interact Item, On the Ground, Equipment, Equipment Detail) and the statistic rules in `rules.odin` rewritten to take `(world, id)`: **a statistic is now read as the character's own value plus the buffs of everything worn** (`stat_of`), which is what `CharacterStatistics.GetStatistic` did; health, mana, MP, encumbrance, intimidation and the Status page all use it. The in-play buttons Inventory, Equipment and Ground... now work. The list screens redirect to the in-play screen when there is nothing to list (the original divided by zero), and "use" and "equip" go through the message page and come back to the inventory. 
- **Ported event actions:** Drink Potion, Eat Food, Use Rotten Food, Purify Food, Food Decay, Rotten Food Decay, Location Decay Items, Read Note, both Learn books, Town and Moon portals, Air and Water shards, Herb, Magic Egg, Beer, Rotten Egg, Bottle, Pr0n Scroll. **Not yet (7), each says "That does not work yet in this version."**: Holy Water, Fire Shard, Earth Shard and Cast Holy Bolt (need the strike sequence of step 5: damage, kill, loot and XP, counter attacks), and Cast Purify, Accept and Complete Cellar Rats (step 6). `action_ported` lists them and a test pins the count at 7.
- Fidelity: the Magic Egg table is the original's (the design spike had mismatched weights and kinds; fixed), notes pick a lore no other item shows, with a random fallback after 25.

**Verified (35 cases, 1,495 checks, native and wasm under node):** compared with the recorded VB frames cell for cell: inventory (`42`, grouped, sorted, encumbrance 45/120), the item menu (`43`), the equip message (`44`), the in-play screen with equipment (`45`), the equipment page (`46`), the book's message (`48`), the **map** (`49`: a place with two exits and enemies, drawn inverted in pink, so the elbow selection and the legend are now confirmed) and the ground list (`61`, and the town screen `60` with its changed rows excluded). Behaviour: grouping and ordering, encumbrance and maximum, durability counts down and an item breaks at zero, equipping chooses the first free slot or replaces and returns the old item, two rings fill both hands, buffs add to the statistic and unequipping removes them, unequippable items say so; a potion heals 2d4 and returns a bottle, food empties the stomach, food rots and purifies, rotten food on the floor rots away, the Magic Egg swaps itself for a prize, the Town Portal makes a pair of routes to the square, 26 notes show 25 different texts and then fall back, the book teaches Holy Bolt once and then refuses, the water shard heals and costs mana, the air shard stays on the level, the herb refills mana, beer sends one goblin away; every action runs safely on a world with and without a named actor and the world still validates.

**Differences from the original:** statistic changes no longer fold buffs into the stored value (defect, in the audit); the unported actions show a message instead of acting; `Use_Beer` sets stress to zero for "current MP = maximum" (same effect).

**Questions for you (also in `docs/dead-code-audit.md`):** Amulet of STR has no buff in the data; Lotion never runs out. Neither was changed.

**Not verified:** repair and the shoppes (step 6; the durability arithmetic is ready); the Moon Portal; the Elemental Orb as an item (it has no use event); the position of the cursor after use-then-return (it resets to the top as the original's Initialize does); long inventories (more than the screen shows scroll as in the original, tested only with four groups); touch taps on lists (written, the two-step rule applied, not exercised by a test).

**Findings for later steps:** (1) The strike sequence is the next shared piece: `perform` already has its slots. (2) `Item.lore` stays 0 until read; two notes never share a lore while one is free. (3) `item_groups` is recomputed on every draw and every key press; it is a few dozen items, but if the inventory ever grows to hundreds it needs caching.


### Port step 5: combat, death, experience and level up (2026-10-03)

**Done:** `combat.odin` (a port of `CharacterPhysicalCombat`, `CharacterMentalCombat`, the wear rules of `CharacterEquipment` and `CharacterAdvancement.AddXP`), the Enemies and Level Up screens, and the in-play buttons FIGHT!, RUN!, Intimidate!, Enemies(n) and Level up! (the last uses the Finalize Character menu with its own title; Cancel and the last point return to the game). The strike family of item and spell actions is now real: Holy Water, Fire Shard, Earth Shard and Cast Holy Bolt (the shared sequence is `strike`: damage, kill, loot, XP, counter attacks). **Only three actions are still unported**: Cast Purify and the two Cellar Rats actions (step 6).
- Rules as in the original: a die succeeds on a six; attack dice are strength plus worn weapons' attack dice, defence dice dexterity plus worn armour's, one die fewer when drunk, high or chafing; the defence roll is capped by the maximum defend; damage is the attack minus defence, capped by the sum of the worn items' damage limits (or the unarmed limit); weapons wear by the damage dealt and armour by the attack rolled against it, a random worn piece per point, and broken pieces vanish with "Yer X breaks!". Enemies attack physically or mentally by their weights; a demoralizing mental hit makes the player drop everything, lose half the money and wake in the town square. A kill pays money and XP, drops the victim's items and one loot item by weight, and a level doubles the XP goal, adds a point to assign and clears wounds, stress and fatigue. Running turns the player to a random compass direction and goes if that way is open. The parting shot of the killer is quoted when the player dies.
- Death: after the queued messages the death page shows; Confirm discards the world and returns to the title.

**Verified (39 cases, 1,894 checks, native and wasm under node):**
- **The recorded fights are reproduced exactly.** The dice cannot be set, so the test searches for a world seed whose first rolls equal the recording and then compares whole screens: the attack/defend/kill/money/XP message (`50`) and the counter-attack message with its hard 22-column breaks (`51`) matched at seed 685; the fight while dying (`70`), the death page (`71`) and the return to the title (`72`) also match. The enemies list (`41`) and the level-up page (`63`) match the recordings.
- Rules: experience and levels, damage caps with a dagger and chainmail, dice counts, the drunk penalty, dice succeed about one time in six (6,000 dice), a dagger breaks after ten wear points, a kill pays and drops (a potion the goblin carried lies on the floor), panic flight, 60 seeds x up to 40 fights each with the world validating after every fight, running both succeeds and fails, intimidation needs a lone enemy, an immobilized enemy skips its attack and counts down.

**Defects fixed (in the audit):** an immobilized enemy now really skips its turn (the original tested the player's immobilization, so the Earth Shard did nothing).

**Not verified:** the mental counter attack against the player by a real Malcontent (it is covered only by the rules test and the 60-seed fights, where Malcontents do not appear), the death page's "dignity" line (legs slot), the weapon wear of enemies (monsters carry nothing), the level-up sound (the original plays none; the `Level_Up` sound still has no use), and balance over a whole game.

**Findings for step 6:** (1) Townsfolk need `Interact...` (button 1) and the modes; the button currently does nothing unless intimidation applies. (2) The quest's rats are created with `create_character(.Rat, cellar)` using the data's initial statistics. (3) The test seed search takes about 20 seconds of the native run; if it grows, cap the search or fix the seed in a constant.


### Port step 6: the townspeople, shoppes, repair, spells and the cellar rats quest (2026-10-03)

**Done:** `shoppes.odin` (what a shoppe offers, sells and repairs, selling, buying, repair costs, the spell list and casting, quest queries), `ui_town.odin` (the nine townspeople as player modes with their dialogue and button banks; the Offers, Prices, Buy, Sell, Repair and Spell List screens) and the last event actions: Cast Purify, Accept and Complete Cellar Rats (`action_ported` is gone: **every one of the 18 checks and 27 actions is ported**). The Interact button works (feature -> mode), Spells opens the list, and Red leaves a conversation restoring the button cursor.
- People: the Elder (pep talk restores 1 MP), Graham the Innkeeper (the rat quest: Do Quest!/Quest Done!; one more rat each time; up to ten rat tails are paid 1 each; prices and buy), Yermom the Drunk (beer for an empty bottle), Sander the Chicken (feed fresh or rotten food; one time in six an egg: a Magic Egg or a Rotten Egg), "Honest" Dan (two-up gambling for 5, win 15 on two heads; prices and buy), Marcus the Black Mage (offers, sell, restore mana, prices, buy), Samuli the Blacksmith (offers, sell, repair when something needs it, prices, buy), Nihilist Healer Marten (heal when wounded, prices, buy), David the Constable (10 money per Membership Card). What each says depends on the selected button, as in the original.
- Lists: Buy shows only what you can afford; Sell shows carried items the shoppe buys; Repair costs `wear x full price / max durability` rounded up, carried items first, then worn; Offers and Prices are read-only. A list that empties returns to the game; a spell that cannot be cast says "You cannot cast X now." and comes back to the list.

**Verified (43 cases, 1,951 checks, native and wasm under node):** compared cell for cell with the recorded VB frames: all nine townspeople (`21` to `29`), the black mage's Offers, Prices, Sell (empty) and Buy with 50 money (`30` to `33`), and the Spell List (`67`). Behaviour: buying (affordability list, money, item), selling, repair arithmetic (a half-worn dagger costs 1, a shield worn 7 of 10 costs 5), too poor to repair, healing, restoring mana, the pep talk, bounties, beer to the drunk, the chicken's eggs (60 feedings), two-up odds, the rat quest end to end (rats appear in the cellar, tails are paid, completion counts, the next quest brings two more rats), Purify (rotten food fresh again, one fatigue, refused without mana); every action still runs safely with and without a named actor.

**Not verified:** the Healer's and Blacksmith's repair in a full game with wear from combat; casting Holy Bolt in a fight from the list (the strike itself is tested); Moon travel; touch taps on the shoppe lists (the shared list code applies the two-step rule but no test taps them).

**What is left of the game itself:** nothing known. Every system of the original is now ported. Open content questions for the owner are in `docs/dead-code-audit.md` (Amulet of STR, Lotion; the three unobtainable items from review 5).
