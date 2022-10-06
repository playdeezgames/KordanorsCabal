Imports System.Runtime.CompilerServices

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
End Enum
Public Module ItemTypeExtensions
    <Extension>
    Function ToNew(itemType As OldItemType, worldData As IWorldData) As IItemType
        Return Game.ItemType.FromId(worldData, itemType)
    End Function
End Module
