#!/usr/bin/env python3
"""The Zooperdan fight against assumed late-game players: no running, potions allowed (a round each), phase 2 at half HP adds mental
attacks and a demoralized player dies. Stats are the ones in the database (hand over) and in ending.odin (keep). Run: python3 tools/analysis/finale.py"""
import os, random, sys, io, contextlib, statistics as st
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
with contextlib.redirect_stdout(io.StringIO()):
    import difficulty as D
hits = D.hits
BOSS = dict(full=dict(STR=10, DEX=5, INF=6, HP=18, MAXDMG=4, MAXDEF=5), weak=dict(STR=8, DEX=4, INF=5, HP=12, MAXDMG=3, MAXDEF=4))
# late players (what the campaign model has at level V): atk dice, defence dice, damage limit, HP, willpower, MP, potions in the pack
PLAYERS = {
    'campaign end (brodesode+chainmail, HP 10)': dict(atk=5 + 6, dfn=4 + 2 + 2 + 2, maxdmg=5, hp=10, wil=3, mp=3, potions=4),
    'strong (STR 8, platemail, HP 12)':          dict(atk=8 + 6, dfn=4 + 4 + 2 + 2, maxdmg=3, hp=12, wil=3, mp=4, potions=6),
    'weak (shortsword, HP 7)':                   dict(atk=5 + 4, dfn=3 + 2 + 2, maxdmg=2, hp=7, wil=2, mp=3, potions=4),
}
def fight(b, p, potion_below=0.4):
    boss = dict(b); boss_w = 0; hp = p['hp']; stress = 0; potions = p['potions']; phase2 = False; rounds = 0
    while rounds < 300:
        rounds += 1
        if hp <= potion_below * p['hp'] and potions:
            potions -= 1; hp = min(p['hp'], hp + sum(random.randint(1, 4) for _ in range(2)))
        else:
            a = hits(p['atk']); d = min(hits(boss['DEX']), boss['MAXDEF'])
            if a - d > 0:
                boss_w += min(a - d, p['maxdmg'])
                if boss_w >= boss['HP']: return True, rounds, hp
            if not phase2 and boss_w * 2 >= boss['HP']: phase2 = True
        if phase2 and random.random() < 0.5:    # mental attack (weights 1:1)
            if hits(boss['INF']) - hits(p['wil']) > 0:
                stress += 1
                if stress >= p['mp']: return False, rounds, 0
        else:
            a = hits(boss['STR']); dd = min(hits(p['dfn']), 1)
            if a - dd > 0: hp -= min(a - dd, boss['MAXDMG'])
        if hp <= 0: return False, rounds, 0
    return False, rounds, hp
if __name__ == "__main__":
    random.seed(4)
    for bn, b in BOSS.items():
        print("\nZooperdan, %s (HP %d STR %d DEX %d INF %d)" % (bn, b['HP'], b['STR'], b['DEX'], b['INF']))
        for pn, p in PLAYERS.items():
            r = [fight(b, p) for _ in range(4000)]
            win = sum(1 for x in r if x[0]) / len(r)
            print("  %-45s win %3.0f%%  rounds %4.1f" % (pn, 100 * win, st.mean(x[1] for x in r)))
