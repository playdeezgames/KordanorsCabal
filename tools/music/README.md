# The theme (MinorTheme.ogg)

Written 2026-10-31 in the Anvil sound editor as 12 short PLAY sounds (a melody and a bass for each half of three 16-bar sections A, B and C; Anvil allows 60 seconds a sound), saved in the Anvil world `kordanors-cabal-music.json`. The tune is in A minor at T52; form A B A C B A, 7 minutes 23 seconds, which loops from an E chord back to the first A minor chord.

* `comp.py` writes the PLAY text of the 12 sounds (`python3 comp.py` prints them as JSON); they were created in Anvil with `sound_new` and exported with `sound_export` to `wav/<name>.wav`.
* `mix.py` joins the sections, mixes melody (0.42) and bass (0.30), softens the square waves with a one-pole low-pass, adds two echoes (wrapped round the end so the loop is seamless) and writes `theme.raw` (22050 Hz mono, 16-bit).
* `enc.py ogg` scales it to half the loudness of the old theme (about 0.085 rms) and encodes `MinorTheme.new.ogg` through libsndfile (Vorbis); copy it to `src/KordanorsCabal/Content/MinorTheme.ogg`.

The previous theme came from Abundant Music (seed 2645320710); it is in git history.
