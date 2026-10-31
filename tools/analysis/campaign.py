#!/usr/bin/env python3
"""Whole-campaign Monte Carlo: one player walks levels I to V with everything the single-fight model of difficulty.py leaves out.
Carried between rooms: hit points, stress (MP), gear and its wear, money, potions, XP and level-ups (which heal). Loot is placed by the
spawn rules of boilerplate.db, found gear is worn if better and sold if not, the smith's prices pay for upgrades and repairs, the healer
may charge, and a retreat to town happens when the player is hurt and has no potion.
Not modelled: hunger and food (see KNOBS), traveling distance, the Moon, spells, shards, intimidation, keys, bosses' loot.
Run: python3 tools/analysis/campaign.py [runs]   (compares RULES against the targets in docs/FINISH-PLAN.md)"""
import copy, os, random, re, statistics as st, sys, io, contextlib
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
with contextlib.redirect_stdout(io.StringIO()):
    import difficulty as D
q = D.q

# ---- data from the database -------------------------------------------------------------------------------------------
STAT = {'atk': 30, 'maxdmg': 31, 'def': 32, 'dur': 33, 'offer': 34, 'price': 35, 'repair': 36}
item_id = {n: i for i, n in q("select ItemTypeId,ItemTypeName from ItemTypes")}
GEAR = {}
for name, slot in [('Dagger', 'W'), ('Shortsword', 'W'), ('Brodesode', 'W'), ('Shield', 'S'), ('Helmet', 'H'), ('Chainmail', 'T'), ('Platemail', 'T')]:
    d = {k: 0 for k in STAT}
    for sid, v in q("select StatisticTypeId,StatisticValue from ItemTypeStatistics where ItemTypeId=?", item_id[name]):
        for k, s in STAT.items():
            if s == sid: d[k] = v
    d['slot'] = slot; d['name'] = name; d['id'] = item_id[name]
    GEAR[name] = d
POTION = item_id['Potion']
money_dice = dict(q("select CharacterTypeId,MoneyDropDice from CharacterTypes"))
spawn_item = {}   # (item id, level) -> (dice, location types)
for i, l, dc in q("select ItemTypeId,DungeonLevelId,SpawnDice from ItemTypeSpawnCounts"): spawn_item.setdefault((i, l), [dc, set()])
for i, l, t in q("select ItemTypeId,DungeonLevelId,LocationTypeId from ItemTypeSpawnLocationTypes"): spawn_item.setdefault((i, l), [None, set()])[1].add(t)

def roll_dice(s):
    n, m = (int(x) for x in s.split('d'))
    return sum(random.randint(1, m) for _ in range(n))

# ---- rules under test; change these (or the database) and compare ------------------------------------------------------------
RULES = dict(
    maxdef=1,                       # the player's defence cap (Base Maximum Defend)
    heal_cost=lambda level, wounds: 0,   # what the healer charges for a full heal
    heal_cooldown=40,               # steps the player must walk between two free heals (the game: HEAL_COOLDOWN in ui_town.odin)
    steps_per_room=6,               # steps walked to reach and clear one room
    steps_per_trip=24,              # steps for the walk to town and back
    potions_found=0.6,              # share of the spawned potions the player finds and keeps
    potion_dice='2d4',
    retreat_below=0.5,              # go home when hurt below this share of HP with no potion
    start_points=dict(STR=3, DEX=1, HP=3, WIL=1),
    level_points=['HP', 'HP', 'DEX', 'STR', 'HP', 'WIL'],   # where the level-up points go, in turn
    items_found=0.8,                # share of the placed gear the player finds
    hunger_hp_per_level=0.0,        # HP lost to hunger per level (the game: about 100 steps per HP; a level takes a few hundred steps)
    wear=True,
    avoid_above=0.02,
    push_above=0.04,                # when only risky rooms are left the player takes the least bad up to this risk, then goes to the boss
    heal_before=0.8,                # a careful player heals (a trip to town) below this share of HP before the next room               # a careful player leaves a room alone while its risk of death or panic is above this
    max_panics=4,                   # panics on one level after which the run counts as dead
    risk_trials=100,
    flee_below=0.34,                # drink a potion (or try to run) when hurt below this share of HP during a fight
)
# Targets from docs/FINISH-PLAN.md: the chance that a level kills the model player (low, high), given the player entered it. A level has
# 60 to 100 fights, so a per-room chance of even 1% is a big number per level. The player policy is calibrated (2026-10-29) to the owner's
# report: a skilled player who almost never dies after level I; the targets are what the finished game should do to that player.
TARGET = {1: (0.10, 0.20), 2: (0.08, 0.15), 3: (0.10, 0.18), 4: (0.12, 0.22), 5: (0.20, 0.30)}

class Player:
    def __init__(s):
        s.stat = dict(STR=1, DEX=1, INF=1, WIL=1, HP=3, MP=3)
        for k, v in RULES['start_points'].items(): s.stat[k] += v
        s.wounds = 0; s.stress = 0; s.xp = 0; s.goal = 10; s.points = 0; s.lv = 0
        s.gear = {}      # slot -> [name, wear]
        s.money = 0; s.potions = 0
    def maxhp(s): return s.stat['HP']
    def hp(s): return s.maxhp() - s.wounds
    def atk(s): return s.stat['STR'] + sum(GEAR[g[0]]['atk'] for g in s.gear.values())
    def dfn(s): return s.stat['DEX'] + sum(GEAR[g[0]]['def'] for g in s.gear.values())
    def maxdmg(s):
        t = sum(GEAR[g[0]]['maxdmg'] for g in s.gear.values())
        return t if t > 0 else 1
    def level_up(s):
        k = RULES['level_points'][s.lv % len(RULES['level_points'])]; s.lv += 1
        s.stat[k] += 1; s.wounds = 0; s.stress = 0; s.goal *= 2
    def gain(s, xp):
        s.xp += xp
        while s.xp >= s.goal: s.xp -= s.goal; s.level_up()
    def worth(s, name):   # is this piece better than what is worn in its slot
        g = GEAR[name]; cur = s.gear.get(g['slot'])
        if not cur: return True
        c = GEAR[cur[0]]
        return (g['atk'], g['maxdmg'], g['def']) > (c['atk'], c['maxdmg'], c['def'])
    def equip(s, name, found):
        g = GEAR[name]
        if s.worth(name):
            old = s.gear.get(g['slot'])
            if old and found: s.money += GEAR[old[0]]['offer']
            s.gear[g['slot']] = [name, 0]; return True
        if found: s.money += g['offer']
        return False

def wear(p, slots, times):
    if not RULES['wear']: return
    for _ in range(times):
        c = [k for k in slots if k in p.gear and GEAR[p.gear[k][0]]['dur'] > 0]
        if not c: return
        k = random.choice(c); p.gear[k][1] += 1
        if p.gear[k][1] >= GEAR[p.gear[k][0]]['dur']: del p.gear[k]

def threat(p, en):
    """What a careful player fears this round: each physical attacker's hits at twice the expected, less the defence cap, within its limit."""
    t = 0
    for x in en:
        w = D.atkw[x['type']]; share = w.get(1, 0) / sum(w.values())
        t += share * min(max(-(-x['STR'] * 2 // 6) - RULES['maxdef'], 0), x['MAXDMG'])
    return int(t + 0.999) if t > 0.6 else 0

def enemy_turn(p, en):
    """Every enemy strikes back; returns 'dead', 'panic' or 'ok'."""
    for x in list(en):
        w = D.atkw[x['type']]; tot = sum(w.values())
        if random.random() * tot < w.get(1, 0):
            a = D.hits(x['STR']); wear(p, 'SHT', a)
            dd = min(D.hits(p.dfn()), RULES['maxdef'])
            if a - dd > 0: p.wounds += min(a - dd, x['MAXDMG'])
        else:
            i = D.hits(x['INF']); wv = D.hits(p.stat['WIL'])
            if i - wv > 0:
                p.stress += 1
                if p.stress >= p.stat['MP']: return 'panic'
        if p.hp() <= 0: return 'dead'
    return 'ok'

def room_fight(p, group, level, stats=None):
    """Fights a room. Returns (result, enemies left): 'ok', 'dead', 'panic' or 'fled'. State carries over. The player drinks a
    potion when hurt below flee_below, and with none left tries to run (half the directions are open)."""
    en = group if group and isinstance(group[0], dict) else [dict(D.MON[c], wounds=0, type=c) for c in group]
    rounds = 0
    while en and rounds < 80:
        rounds += 1
        if p.hp() <= RULES['flee_below'] * p.maxhp() or p.hp() <= threat(p, en):
            if p.potions > 0:
                p.potions -= 1; p.wounds = max(0, p.wounds - roll_dice(RULES['potion_dice']))
                if stats is not None: stats['potions'] += 1
                r = enemy_turn(p, en)
                if r != 'ok': return r, en
                continue
            if random.random() < 0.5: return 'fled', en
            r = enemy_turn(p, en)
            if r != 'ok': return r, en
            continue
        e = en[0]
        a = D.hits(p.atk()); d = min(D.hits(e['DEX']), e['MAXDEF'])
        if a - d > 0:
            dmg = min(a - d, p.maxdmg()); e['wounds'] += dmg; wear(p, 'W', dmg)
            if e['wounds'] >= e['HP']:
                en.pop(0)
                p.money += roll_dice(money_dice[e['type']]) if money_dice[e['type']].split('d')[0] != '0' else 0
                p.gain(D.xpv[e['type']])
        r = enemy_turn(p, en)
        if r != 'ok': return r, en
    return 'ok', en

_risk_cache = {}
def risk(p, group, level, full=False):
    """Share of trials in which this room kills or breaks the player from the present state (full HP if `full`). Cached by the
    room's group and the player's fighting numbers, since the kit changes rarely."""
    key = (tuple(sorted(c['type'] if isinstance(c, dict) else c for c in group)), p.atk(), p.dfn(), p.maxdmg(), p.maxhp(), p.stat['WIL'],
           p.stat['MP'] - p.stress, p.potions > 0, full, RULES['maxdef'])
    if key in _risk_cache: return _risk_cache[key]
    bad = 0
    for _ in range(RULES['risk_trials']):
        c = copy.copy(p); c.gear = {k: list(v) for k, v in p.gear.items()}
        if full: c.wounds = 0
        bad += room_fight(c, list(group), level)[0] in ('dead', 'panic')
    _risk_cache[key] = bad / RULES['risk_trials']
    return _risk_cache[key]

def needs_town(p):
    """No weapon but money for a dagger, or the weapon is nearly worn out and can be repaired."""
    w = p.gear.get('W')
    if not w: return p.money >= GEAR['Dagger']['price']
    d = GEAR[w[0]]
    return d['dur'] and w[1] > 0.7 * d['dur'] and p.money >= d['repair']

def town(p, level, st_):
    """Heal, repair, buy upgrades with what money allows (the smith's order of need)."""
    wounds = p.wounds
    cost = RULES['heal_cost'](level, wounds)
    p.cool = max(getattr(p, 'cool', 0) - RULES['steps_per_trip'], 0)   # the walk itself counts
    if wounds > 0 and p.cool > 0: st_['unhealed'] += 1
    elif wounds > 0 and p.money >= cost: p.money -= cost; p.wounds = 0; p.cool = RULES['heal_cooldown']
    elif wounds > 0 and cost > 0: st_['unhealed'] += 1
    for k, g in p.gear.items():   # repair
        d = GEAR[g[0]]
        if d['dur'] and g[1] > 0:
            c = -(-d['repair'] * g[1] // d['dur'])
            if p.money >= c: p.money -= c; g[1] = 0
    for name in ('Dagger', 'Shield', 'Helmet', 'Shortsword', 'Chainmail', 'Brodesode', 'Platemail'):
        if p.worth(name) and p.money >= GEAR[name]['price'] + 5:
            p.money -= GEAR[name]['price']; p.equip(name, False)

def place_items(level, rooms):
    """Puts gear and potions into rooms by the spawn rules; returns room -> list of item ids (only a share is ever found)."""
    kind = {4: [k for k in rooms if k[0] == 'c'], 5: [k for k in rooms if k[0] == 'd'], 6: [k for k in rooms if k[0] == 'b']}
    out = {k: [] for k in rooms}
    for (iid, l), (dice, locs) in spawn_item.items():
        if l != level or not dice: continue
        if iid not in [g['id'] for g in GEAR.values()] + [POTION]: continue
        cand = [r for t in locs for r in kind.get(t, [])]
        if not cand: continue
        for _ in range(roll_dice(dice)): out[random.choice(cand)].append(iid)
    return out

def campaign():
    p = Player(); res = {}
    for level in range(1, 6):
        R = D.rooms_for_level(level)
        items = place_items(level, R)
        s = dict(rooms=0, trips=0, potions=0, deaths=0, panics=0, lost=0.0, money_in=p.money, unhealed=0, kit=sorted(g[0] for g in p.gear.values()), points=p.lv)
        todo = [k for k in R if k[0] != 'b']; random.shuffle(todo)
        # a careful player takes the empty rooms (where the gear lies) first, then the weakest groups, leaving a room alone
        # while it looks too dangerous for the present kit, and the boss last
        todo.sort(key=lambda k: sum(D.MON[c]['STR'] * D.MON[c]['HP'] for c in R[k]))
        while todo or ('b', 0) in R:
            if needs_town(p): s['trips'] += 1; town(p, level, s)
            if todo and p.hp() < RULES['heal_before'] * p.maxhp():
                s['trips'] += 1; town(p, level, s)
            if todo:
                pick = None
                for k in todo[:10]:
                    rk = 0 if not R[k] else risk(p, R[k], level, full=True)
                    if rk <= RULES['avoid_above']: pick = k; break
                    if rk <= RULES['push_above'] and pick is None: pick = k
                if pick is None:      # nothing left worth the risk: on to the boss
                    todo = []; continue
                todo.remove(pick); k = pick
            else:
                k = ('b', 0)
            for iid in items[k]:
                if iid == POTION:
                    if random.random() < RULES['potions_found']: p.potions += 1
                elif random.random() < RULES['items_found']:
                    name = [g['name'] for g in GEAR.values() if g['id'] == iid][0]; p.equip(name, True)
            items[k] = []
            p.cool = max(getattr(p, 'cool', 0) - RULES['steps_per_room'], 0)
            if R[k]:
                s['rooms'] += 1; hp0 = p.hp()
                r, left = room_fight(p, R[k], level, s)
                s['lost'] += max(hp0 - p.hp(), 0) / p.maxhp()
                if r == 'dead': s['deaths'] = 1; s['boss_death'] = k == ('b', 0); s['room_size'] = len(left); res[level] = s; return res
                if r == 'panic':      # drop everything, lose half the money, wake in town
                    s['panics'] += 1
                    if s['panics'] > RULES['max_panics']:   # a player who keeps breaking down on one level is finished (also stops endless retries)
                        s['deaths'] = 1; s['boss_death'] = k == ('b', 0); s['room_size'] = len(R[k]); res[level] = s; return res
                    p.gear = {}; p.money //= 2; p.stress = 0; p.wounds = 0; town(p, level, s)
                    todo.append(k); R[k] = [c for c in R[k]]   # the room is still occupied; the enemies keep their wounds in the real game (ignored here)
                    continue
                if r == 'fled': R[k] = left; todo.insert(0, k) if k != ('b', 0) else None   # the survivors keep their wounds
                while p.hp() < RULES['retreat_below'] * p.maxhp() and p.potions > 0:
                    p.potions -= 1; s['potions'] += 1; p.wounds = max(0, p.wounds - roll_dice(RULES['potion_dice']))
                if p.hp() < RULES['retreat_below'] * p.maxhp():
                    s['trips'] += 1; town(p, level, s)
            if k == ('b', 0) and not R[k]: break
            if k == ('b', 0) and r != 'fled': break
        p.wounds += int(RULES['hunger_hp_per_level'])
        town(p, level, s); res[level] = s
    return res

def report(runs=300):
    random.seed(11)
    agg = {l: [] for l in range(1, 6)}; alive = {l: 0 for l in range(1, 6)}
    for _ in range(runs):
        r = campaign()
        for l, s in r.items(): agg[l].append(s); alive[l] += 1
    print("runs %d. Per level: reached | died on level (target) | deaths per occupied room | trips | potions | panics | HP lost per room (share of max) | money in | kit at entry" % runs)
    for l in range(1, 6):
        a = agg[l]
        if not a: continue
        rooms = sum(s['rooms'] for s in a); deaths = sum(s['deaths'] for s in a)
        pr = deaths / max(rooms, 1); lo, hi = TARGET[l]; dl = deaths / len(a)
        flag = 'LOW ' if dl < lo else ('HIGH' if dl > hi else 'ok  ')
        kit = {}
        for s in a:
            for g in s['kit']: kit[g] = kit.get(g, 0) + 1
        kits = ' '.join('%s%.0f%%' % (k[:4], 100 * v / len(a)) for k, v in sorted(kit.items(), key=lambda x: -x[1])[:5])
        print(" L%d | %3.0f%% | %4.1f%% (%2d-%2d%%) %s | %5.2f%% | %4.1f | %4.1f | %4.2f | %.2f | %5.0f | %s" % (
            l, 100 * len(a) / runs, 100 * dl, 100 * lo, 100 * hi, flag, 100 * pr,
            st.mean(s['trips'] for s in a), st.mean(s['potions'] for s in a), st.mean(s['panics'] for s in a),
            sum(s['lost'] for s in a) / max(rooms, 1), st.mean(s['money_in'] for s in a), kits))
    for l in range(1, 6):
        d = [s for s in agg[l] if s['deaths']]
        if d: print(" L%d deaths at the boss: %.0f%%, mean enemies left in the room %.1f" % (l, 100 * sum(1 for s in d if s['boss_death']) / len(d), st.mean(s['room_size'] for s in d)))
    print("finished level V alive: %.0f%%" % (100 * len(agg[5]) and 100 * sum(1 for s in agg[5] if not s['deaths']) / runs))

if __name__ == "__main__":
    # optional overrides: key=value pairs, e.g. avoid_above=0.02 push_above=0.06 risk_trials=40
    for a in sys.argv[2:]:
        k, v = a.split('=')
        if k.startswith('price_'): GEAR[k[6:]]['price'] = int(v); continue   # e.g. price_Chainmail=120 tries a shop price
        if k.startswith('cnt_'): _, t, l = k.split('_'); D.cnt.setdefault(int(l), {})[int(t)] = int(v); continue   # cnt_<type>_<level>=N: spawn count
        if k.startswith('atk_'): _, t, kind = k.split('_'); D.atkw.setdefault(int(t), {})[int(kind)] = int(v); continue   # atk_<type>_<kind>=weight
        if k.startswith('stat_'): _, t, name = k.split('_'); D.MON[int(t)][name] = int(v); continue   # stat_<type>_<STR|DEX|INF|WIL|HP|MP|MAXDMG|MAXDEF>=N
        RULES[k] = float(v) if '.' in v else int(v)
    report(int(sys.argv[1]) if len(sys.argv) > 1 else 300)
