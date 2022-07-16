# Kordanor's Cabal


## Music
https://pernyblom.github.io/abundant-music/index.html

Seed: 2645320710

## Stream Log

XYZ Kordanor's Cabal (A Game in VB.NET About Looking Like a Dungeon Crawler Written for the VIC-20) #Niche4LIFE

* 001 - 20220617
* 002 - 20220618
* 003 - 20220620
* 004 - 20220621
* 005 - 20220622
* 006 - 20220623
* 007 - 20220624
* 008 - 20220625
* 009 - 20220626
* 010 - 20220627
* 011 - 20220628
* 012 - 20220629
* 013 - 20220630
* 014 - 20220701
* 015 - 20220702
* 016 - 20220703
* 017 - 20220705
* 018 - 20220706
* 019 - 20220707
* 020 - 20220708
* 021 - 20220709
* 022 - 20220710
* 023 - 20220711
* 024 - 20220712
* 025 - 20220713
* 026 - 20220714
* 027 - 20220715
* 028 - 20220716
    * Drunkenness
    * Can replenish mana at BM shoppe
    * Herb is an item used to replenish Mana
    * Bong is an item



## Credit Due

* Kordanor
* Zooperdan
* Urizen

## Shards

* Air   = Teleport  , Level 1
* Earth = Immobilize, Level 2
* Fire  = Damage    , Level 3
* Water = Heal      , Level 4

# Universal Tables for all SPLORR!! SQLite based games

## CharacterEquipSlots

| Column Name | Type | Purpose |
| --- | --- | --- |
| CharacterId | INT | FK Characters |
| EquipSlot | INT | Implementation Specific |
| ItemId | INT | FK Items |

## CharacterLocations

| Column Name | Type | Purpose |
| --- | --- | --- |
| CharacterId | INT | FK Characters |
| LocationId | INT | FK Locations |

## CharacterStatistics

| Column Name | Type | Purpose |
| --- | --- | --- |
| CharacterId | INT | FK Characters |
| StatisticType | INT | Implementation Specific |
| StatisticValue | INT | Statistic Value |

## Characters

| Column Name | Type | Purpose |
| --- | --- | --- |
| CharacterId | INT | PK |
| LocationId | INT | FK to Locations |
| CharacterType | INT | Implementation Specific |

## Inventories

| Column Name | Type | Purpose |
| --- | --- | --- |
| InventoryId | INT | PK |
| CharacterId | INT? | FK Characters |
| LocationId | INT? | FK Locations |

CK CharacterId IS NULL OR LocationId IS NULL, but not both!

## InventoryItems

| Column Name | Type | Purpose |
| --- | --- | --- |
| InventoryId | INT | FK Inventories |
| ItemId | INT | FK Items |

## ItemStatistics

| Column Name | Type | Purpose |
| --- | --- | --- |
| ItemId | INT | FK Items |
| StatisticType | INT | Implementation Specific |
| StatisticValue | INT | Statististic Value |

## Items

| Column Name | Type | Purpose |
| --- | --- | --- |
| ItemId | INT | PK |
| ItemType | INT | Implementation Specific |

## LocationStatistics

| Column Name | Type | Purpose |
| --- | --- | --- |
| LocationId | INT | FK Locations |
| StatisticType | INT | Implementation Specific |
| StatisticValue | INT | Statistic Value |

## Locations

| Column Name | Type | Purpose |
| --- | --- | --- |
| LocationId | INT | PK |
| LocationType | INT | Implementation Specific |

## Players

| Column Name | Type | Purpose |
| --- | --- | --- |
| PlayerId | INT | CK=1 |
| CharacterId | INT | FK Characters |

## Routes

| Column Name | Type | Purpose |
| --- | --- | --- |
| RouteId | INT | PK |
| FromLocationId | INT | FK Locations |
| ToLocationId | INT | FK Locations |
| RouteType | INT | Implementation Specific |
| Direction | INT | Implementation Specific |

