﻿Imports System.Runtime.CompilerServices

Public Enum OldItemType
    None
    IronKey
    CopperKey
    SilverKey
    GoldKey
    PlatinumKey
    ElementalOrb
    Potion
    GoblinEar
    SkullFragment
    Dagger
    EarthShard
    WaterShard
    FireShard
    AirShard
    Shield
    Helmet
    ChainMail
    Shortsword
    BrodeSode
    PlateMail
    RatTail
    HolyWater
    TownPortal
    Food
    MagicEgg
    Beer
    Trousers
    Pr0n
    MoonPortal
    Bottle
    BookOfHolyBolt
    MembershipCard
    Bong
    Herb
    RottenFood
    Mushroom
    RottenEgg
    ZombieTaint
    Lotion
    BatWing
    SnakeFang
    ShoeLaces
    SpaceSord
    HornsOfKordanor
    AmuletOfHP
    RingOfHP
    BookOfPurify
    AmuletOfSTR
    AmuletOfDEX
    AmuletOfPOW
    AmuletOfMana
    AmuletOfYendor
End Enum
Public Module ItemTypeExtensions
    <Extension>
    Function ToNew(itemType As OldItemType, worldData As IWorldData) As IItemType
        Return Game.ItemType.FromId(worldData, itemType)
    End Function
    <Extension>
    Public Function HasOffer(itemType As OldItemType, shoppeType As ShoppeType) As Boolean
        Return ItemTypeDescriptors(itemType).HasOffer(shoppeType)
    End Function
    <Extension>
    Public Function HasPrice(itemType As OldItemType, shoppeType As ShoppeType) As Boolean
        Return ItemTypeDescriptors(itemType).HasPrice(shoppeType)
    End Function
    <Extension>
    Public Function CanRepair(itemType As OldItemType, shoppeType As ShoppeType) As Boolean
        Return ItemTypeDescriptors(itemType).CanRepair(shoppeType)
    End Function
End Module
