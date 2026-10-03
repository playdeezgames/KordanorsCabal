#!/usr/bin/env python3
"""Monte Carlo estimate of how hard each dungeon level is, using the game's combat rules (a die counts on a six; damage = attack hits
- defence hits, the player's defence capped at 1, damage capped by the worn items' limits; enemies counter-attack together).
Reads the content from boilerplate.db. The player kits are assumptions (see docs/difficulty-analysis.md). Run: python3 tools/analysis/difficulty.py"""
import sqlite3, random, statistics as st, sys
random.seed(7)
import os
ROOT=os.path.join(os.path.dirname(os.path.abspath(__file__)),'..','..')
db=sqlite3.connect(os.path.join(ROOT,'src','KordanorsCabal','boilerplate.db')); q=lambda s,*a: db.execute(s,a).fetchall()
char={i:n for i,n in q("select CharacterTypeId,CharacterTypeName from CharacterTypes")}
xpv=dict(q("select CharacterTypeId,XPValue from CharacterTypes"))
init={}
for c,s,v in q("select * from CharacterTypeInitialStatistics"): init.setdefault(c,{})[s]=v
ST={'STR':1,'DEX':2,'INF':3,'WIL':4,'POW':5,'HP':6,'MP':7,'MAXDMG':10,'MAXDEF':11}
def mon(c):
    d=init[c]; return {k:d.get(v,0) for k,v in ST.items()}
atkw={}
for c,a,w in q("select * from CharacterTypeAttackTypes"): atkw.setdefault(c,{})[a]=w
cnt={}; locs={}
for c,l,n in q("select * from CharacterTypeSpawnCounts"): cnt.setdefault(l,{})[c]=n
for c,l,t in q("select * from CharacterTypeSpawnLocations"): locs.setdefault((c,l),set()).add(t)
def hits(n):
    return sum(1 for _ in range(max(n,0)) if random.random()<1/6)

def prim():
    N=11; inside=set(); front=[]; edges=set()
    s=(random.randrange(N),random.randrange(N)); inside.add(s)
    def nb(c): return [(c[0]+dx,c[1]+dy) for dx,dy in((0,-1),(1,0),(0,1),(-1,0)) if 0<=c[0]+dx<N and 0<=c[1]+dy<N]
    for n in nb(s): front.append(n)
    while front:
        c=front.pop(random.randrange(len(front)))
        if c in inside: continue
        to=random.choice([n for n in nb(c) if n in inside]); edges.add((c,to)); inside.add(c)
        for n in nb(c):
            if n not in inside and n not in front: front.append(n)
    deg={}
    for a,b in edges: deg[a]=deg.get(a,0)+1; deg[b]=deg.get(b,0)+1
    return sum(1 for v in deg.values() if v==1)
dead=[prim() for _ in range(300)]
D=round(st.mean(dead)); print("dead ends per level (mean of 300 mazes): %.1f (min %d max %d); corridor rooms %d"%(st.mean(dead),min(dead),max(dead),121-D))

def rooms_for_level(l):
    """one random level: dict room -> list of monster type ids. rooms: ('c',i) corridor, ('d',i) dead end, 'b' boss"""
    Dn=prim()
    R={('c',i):[] for i in range(121-Dn)}; R.update({('d',i):[] for i in range(Dn-1)}); R[('b',0)]=[]
    kind={4:[k for k in R if k[0]=='c'],5:[k for k in R if k[0]=='d'],6:[k for k in R if k[0]=='b']}
    for c,n in cnt[l].items():
        cand=[r for t in locs[(c,l)] for r in kind.get(t,[])] if l<6 else [('c',i) for i in range(121)]
        if l==6: R={('c',i):[] for i in range(121)}
        for _ in range(n): R[random.choice(cand)].append(c)
    return R

def fight(player,group,maxrounds=60):
    """player: dict atk dice, defdice, maxdmg, hp, wil, mp ; group: list of monster dicts (mutable hp). returns (alive, hp_lost, panicked, rounds)"""
    hp=player['hp']; stress=0; en=[dict(m,wounds=0,type=t) for t,m in group]
    r=0; panicked=False
    while en and hp>0 and r<maxrounds:
        r+=1
        e=en[0]
        a=hits(player['atk']); d=min(hits(e['DEX']),e['MAXDEF'])
        if a-d>0:
            e['wounds']+=min(a-d,player['maxdmg'])
            if e['wounds']>=e['HP']: en.pop(0)
        for x in list(en):
            w=atkw[x['type']]; tot=sum(w.values()); 
            pick=random.random()*tot; kind=1 if pick<w.get(1,0) else 2
            if kind==1:
                a=hits(x['STR']); d=min(hits(player['def']),player['maxdef'])
                if a-d>0: hp-=min(a-d,x['MAXDMG'])
            else:
                i=hits(x['INF']); wv=hits(player['wil'])
                if i-wv>0:
                    stress+=1
                    if stress>=player['mp']: panicked=True; en=[]; break
            if hp<=0: break
    return hp>0 and not panicked, player['hp']-max(hp,0), panicked, r

PROF={
 'A starter (dagger, no armour) STR4 DEX2 HP5':       dict(atk=4+2,def_=2,maxdmg=1,hp=5,wil=2,mp=3),
 'B early kit (shortsword, shield, helmet) STR5 HP6':  dict(atk=5+4,def_=2+2+2,maxdmg=2,hp=6,wil=2,mp=3),
 'C mid kit (brodesode, chainmail, shield, helmet) STR6 DEX3 HP8': dict(atk=6+6,def_=3+2+2+2,maxdmg=3+2,hp=8,wil=2,mp=3),
 'D late kit (brodesode, platemail, shield, helmet) STR8 DEX3 HP10 WIL3': dict(atk=8+6,def_=3+4+2+2,maxdmg=3,hp=10,wil=3,mp=4),
}
def P(d): return {'atk':d['atk'],'def':d['def_'],'maxdef':1,'maxdmg':d['maxdmg'],'hp':d['hp'],'wil':d['wil'],'mp':d['mp']}
MON={c:mon(c) for c in char}
def level_stats(l,prof,levels_sample=12):
    deaths=0; fights=0; lost=[]; panics=0; xp=0; roomsum=[]; grp=[]; xpper=[]
    for _ in range(levels_sample):
        R=rooms_for_level(l)
        for k,g in R.items():
            if not g: continue
            fights+=1; grp.append(len(g))
            ok,loss,pan,r=fight(P(prof),[(c,MON[c]) for c in g])
            lost.append(loss); deaths+= (not ok and not pan); panics+=pan; xpx=sum(xpv[c] for c in g); xpper.append(xpx)
    n=fights
    return dict(rooms=n/levels_sample, mean_group=st.mean(grp), p_die=deaths/n, p_panic=panics/n, hp_lost=st.mean(lost), hp_per_xp=sum(lost)/sum(xpper))

K=list(PROF.items())
def room_report():
    for name,prof in PROF.items():
        print("\n"+name)
        print("level | occupied rooms | enemies/room | P(die) per room | HP lost/room | HP lost per XP")
        for l in range(1,7):
            r=level_stats(l,prof)
            print(" %d | %5.0f | %.2f | %5.1f%% | %.2f | %.2f"%(l,r['rooms'],r['mean_group'],100*r['p_die'],r['hp_lost'],r['hp_per_xp']))
def solo(c,prof,n=3000):
    loss=[];die=0
    for _ in range(n):
        ok,l,p,r=fight(P(prof),[(c,MON[c])]); loss.append(l); die+= (not ok)
    return st.mean(loss),die/n
def solo_report():
    print("\nSOLO FIGHTS per kit A B C D: mean HP lost / P(die)")
    for c in sorted(char):
        if c==11: continue
        print("%-13s xp%d |"%(char[c],xpv[c]),"  ".join("%.2f/%.0f%%"%(a,100*b) for a,b in (solo(c,p,1500) for _,p in K)))
    print("\nBOSSES")
    for l,c in ((1,1),(2,12),(3,4),(4,5),(5,8)):
        print("L%d %-12s HP%d |"%(l,char[c],MON[c]['HP']),"  ".join("%.2f/%.0f%%"%(a,100*b) for a,b in (solo(c,p) for _,p in K)))
if __name__=="__main__":
    room_report(); solo_report()
