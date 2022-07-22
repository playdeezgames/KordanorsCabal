Imports System.Runtime.CompilerServices

Public Enum ItemType
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
End Enum
Public Module ItemTypeExtensions
    <Extension>
    Function SpawnLocationTypes(itemType As ItemType, level As Long) As HashSet(Of LocationType)
        Return ItemTypeDescriptors(itemType).SpawnLocationTypes(level)
    End Function
    <Extension>
    Function Name(itemType As ItemType) As String
        Return ItemTypeDescriptors(itemType).Name
    End Function
    <Extension>
    Function CanUse(itemType As ItemType, character As Character) As Boolean
        Return ItemTypeDescriptors(itemType).CanUse(character)
    End Function
    <Extension>
    Function Encumbrance(itemType As ItemType) As Single
        Return ItemTypeDescriptors(itemType).Encumbrance
    End Function
    <Extension>
    Public Function PurchasePrice(itemType As ItemType) As Long?
        Return ItemTypeDescriptors(itemType).PurchasePrice
    End Function
    <Extension>
    Public Sub Use(itemType As ItemType, character As Character)
        ItemTypeDescriptors(itemType).Use(character)
    End Sub
    <Extension>
    Public Function RollSpawnCount(itemType As ItemType, level As Long) As Long
        Return ItemTypeDescriptors(itemType).RollSpawnCount(level)
    End Function
    <Extension>
    Public Function EquipSlot(itemType As ItemType) As EquipSlot?
        Return ItemTypeDescriptors(itemType).EquipSlot
    End Function
    <Extension>
    Public Function AttackDice(itemType As ItemType) As Long
        Return ItemTypeDescriptors(itemType).AttackDice
    End Function
    <Extension>
    Public Function IsWeapon(itemType As ItemType) As Boolean
        Return ItemTypeDescriptors(itemType).AttackDice > 0
    End Function
    <Extension>
    Public Function DefendDice(itemType As ItemType) As Long
        Return ItemTypeDescriptors(itemType).DefendDice
    End Function
    <Extension>
    Public Function IsArmor(itemType As ItemType) As Boolean
        Return ItemTypeDescriptors(itemType).DefendDice > 0
    End Function
    <Extension>
    Public Function IsConsumed(itemType As ItemType) As Boolean
        Return ItemTypeDescriptors(itemType).IsConsumed
    End Function
    <Extension>
    Public Function MaximumDamage(itemType As ItemType) As Long?
        Return ItemTypeDescriptors(itemType).MaximumDamage
    End Function
    <Extension>
    Public Function MaximumDurability(itemType As ItemType) As Long?
        Return ItemTypeDescriptors(itemType).MaximumDurability
    End Function
End Module
