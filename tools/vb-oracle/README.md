# vb-oracle

A headless driver for the original VB.NET game, used to record reference behaviour for the Odin port (see `PORT.md`, task 23).
It references the VB projects in `src/`, runs the real UI state machine (`MainProcessor`) and the real renderer without MonoGame,
and executes a script of commands. Not part of the shipped game.

```
dotnet run --project tools/vb-oracle -- <workdir> capture docs/reference/vb/scenes.txt <outdir>
dotnet run --project tools/vb-oracle -- <workdir> explore '!prep' '!start' '!play' ? "D D G" ?
```

`<workdir>` must contain a copy of `src/KordanorsCabal/boilerplate.db` (the game opens it relative to the working directory and
writes `SaveSlot*.db` there). Needs the .NET SDK (built and run with SDK 10; the VB projects target netstandard2.1). Script
directives are documented at the top of `docs/reference/vb/scenes.txt` and in the comments of `Program.cs`.
