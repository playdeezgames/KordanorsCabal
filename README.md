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
* 029 - 20220717
* 030 - 20220718
* 031 - 20220719
* 032 - 20220720
* 033 - 20220721
* 034 - 20220722
* 035 - 20220801
* 036 - 20220802
* 037 - 20220803
* 038 - 20220804
* 039 - 20220805
* 040 - 20220806
* 041 - 20220807
* 042 - 20220808
* 043 - 20220809
* 044 - 20220810
* 045 - 20220811
* 046 - 20220812
* 047 - 20220813
* 048 - 20220814
* 049 - 20220831
* 050 - 20220901 

## Credit Due

* Kordanor
* Zooperdan
* Urizen

## Shards

* Air   = Teleport  , Level 1
* Earth = Immobilize, Level 2
* Fire  = Damage    , Level 3
* Water = Heal      , Level 4


| **KC Table** | **Purpose** | **Note** |
|:---|:---|:---|
| CharacterEquipSlots | Items equipped on a character in an equip slot. |  |
| CharacterLocations | Locations known to a character. | Perhaps a visit count should also be tracked? |
| CharacterSpells | Tracks knowledge levels for spells for a character. | Perhaps a little specific? Can spells be considered skills or some other sort of statistic? |
| CharacterStatisticTypes | Metadata about Character Statistic Types. | Should I combine different statistic types into a single table? |
| CharacterStatistics | Associates a statistic value with a character for a particular statistic. |  |
| CharacterTypeEnemies | This is a very "us or them" table. A record present means a one sided enmity exists. | Perhaps a faction mechanism and some way to store a non-binary relationship level. |
| CharacterTypeInitialStaticis | Gives the initial values for a given character type with the statistics in question. |  |
| CharacterTypeSpawnCounts | Tells me how many of what thing shows up on a given dungeon level. | Move to more of a biome mechanism. |


# Notes

https://www.aardwolf.com/
