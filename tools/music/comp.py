# Somber theme for Kordanor's Cabal as PLAY text: three 16-bar sections, melody and bass voices of equal length.
CODE = {4: "1", 3: "2.", 2: "2", 1.5: "4.", 1: "4", 0.5: "8"}
def play(bars, tempo=52):
    out = ["T%d" % tempo]; cur = None
    for bar in bars:
        assert abs(sum(b for _, b in bar) - 4) < 1e-9, bar
        for n, b in bar:
            o = n[-1]; name = n[:-1]
            if o != cur: out.append("O" + o); cur = o
            out.append(name + CODE[b])
    return " ".join(out)
def rep(*bars): return list(bars)
def N(s):   # "A3 2, G3 1, E3 1" -> [("A3",2),...]
    return [(t.split()[0], float(t.split()[1])) for t in s.split(",")]
def passes(chords, *ps):
    out = []
    for p in ps:
        assert len(p) == chords
        out += [N(b) for b in p]
    return out
A_mel = passes(4, ["A3 2, G3 1, E3 1", "F3 2, E3 1, C3 1", "D3 2, E3 1, F3 1", "E3 2, G#3 2"],
                  ["C4 2, B3 1, A3 1", "A3 2, G3 1, F3 1", "F3 1.5, E3 .5, D3 2", "B3 2, G#3 1, E3 1"],
                  ["E3 4", "C3 2, F3 2", "D3 2, A3 2", "G#3 2, B3 2"],
                  ["A3 1, C4 1, E4 2", "F4 2, E4 1, C4 1", "D4 2, C4 1, A3 1", "B3 1, G#3 1, E3 2"])
B_mel = passes(4, ["E3 2, G3 1, E3 1", "D3 2, G3 1, B3 1", "C4 2, B3 1, A3 1", "A3 2, G3 1, F3 1"],
                  ["G3 2, E3 2", "B3 2, D4 2", "C4 3, B3 1", "A3 2, F3 2"],
                  ["E4 2, D4 1, C4 1", "B3 2, A3 1, G3 1", "A3 2, C4 2", "A3 1, G3 1, F3 2"],
                  ["C4 4", "B3 2, D4 2", "C4 2, E4 2", "F4 2, E4 1, C4 1"])
C_mel = passes(4, ["D3 2, F3 2", "E3 2, A3 2", "F3 2, A3 2", "G#3 2, E3 2"],
                  ["A3 2, D4 2", "C4 2, E4 2", "F4 3, E4 1", "D4 2, B3 2"],
                  ["F4 2, E4 1, D4 1", "C4 1, E4 1, A4 2", "A4 2, G4 1, F4 1", "G#4 2, E4 2"],
                  ["D4 2, A3 2", "E3 2, C3 2", "A3 4", "G#3 2, E3 2"])
def bass(roots4, whole_pass=None):
    out = []
    for p in range(4):
        for (r, f) in roots4:
            out.append([(r, 4)] if p == 2 else [(r, 2), (f, 2)])
    return out
A_bass = bass([("A2", "E2"), ("F2", "C3"), ("D2", "A2"), ("E2", "B2")])
B_bass = bass([("C2", "G2"), ("G2", "D3"), ("A2", "E3"), ("F2", "C3")])
C_bass = bass([("D2", "A2"), ("A2", "E2"), ("F2", "C3"), ("E2", "B2")])
S = {"theme_A_mel": A_mel, "theme_A_bass": A_bass, "theme_B_mel": B_mel, "theme_B_bass": B_bass, "theme_C_mel": C_mel, "theme_C_bass": C_bass}
if __name__ == "__main__":
    import json
    print(json.dumps({k: play(v) for k, v in S.items()}))
