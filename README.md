# Kordanor's Cabal

TODO: IItem subobjectification

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
* 051 - 20220902
* 052 - 20220903
* 053 - 20220904
* 054 - 20220905
* 055 - 20220906
* 056 - 20220907
* 057 - 20220908
* 058 - 20220909
* 059 - 20220910
* 060 - 20220911
* 061 - 20220912
* 062 - 20220913
* 063 - 20220914
* 064 - 20220915
* 065 - 20221001
* 066 - 20221002
* 067 - 20221003
* 068 - 20221004
* 069 - 20221005
* 070 - 20221006
* 071 - 20221007
* 072 - 20221008
* 073 - 20221009
* 074 - 20221010
* 075 - 20221011
* 076 - 20221012
* 077 - 20221013
* 078 - 20221016

## Credit Due

* Kordanor
* Zooperdan
* Urizen

## Shards

* Air   = Teleport  , Level 1
* Earth = Immobilize, Level 2
* Fire  = Damage    , Level 3
* Water = Heal      , Level 4


# Notes

https://www.aardwolf.com/
^ its a mud

## Enum or not Enum?

If the enum needs metadata, then dont enum.

## When yer interface has too many methods

1. Sort methods into roughly related groups of methods
1. With each method group
    1. Stub an interface to move the methods into
    1. Add and stub a property to the original interface that returns an instance of the new interface
    1. Add test for the new property
    1. Make that test pass
    1. Make the class that implements the new interface
    1. Make a test class for the new class
    1. Move method signatures from original interface into new interface
    1. Move implementations from original interface to new interface
    1. Move tests from original test class to new test class
    1. Once it builds, it should be good, and tests should still pass

# DB Creation Order

* CharacterTypes
* CharacterStatisticTypes
* CharacterTypeInitialStatistics
* CharacterTypeAttackTypes