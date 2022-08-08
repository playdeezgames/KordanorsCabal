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
    ShoeLaces
    SpaceSord
    HornsOfKordanor
    AmuletOfHP
    RingOfHP
    BookOfPurify
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
    Public Sub Use(itemType As ItemType, character As Character)
        ItemTypeDescriptors(itemType).Use(character)
    End Sub
    <Extension>
    Public Function RollSpawnCount(itemType As ItemType, level As DungeonLevel) As Long
        Return ItemTypeDescriptors(itemType).RollSpawnCount(level)
    End Function
    <Extension>
    Public Function EquipSlotS(itemType As ItemType) As IEnumerable(Of EquipSlot)
        Return ItemTypeDescriptors(itemType).EquipSlots
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
    <Extension>
    Public Function EquippedBuff(itemType As ItemType, statisticType As CharacterStatisticType) As Long?
        Return ItemTypeDescriptors(itemType).EquippedBuff(statisticType)
    End Function
    <Extension>
    Public Sub Purify(itemType As ItemType, item As Item)
        ItemTypeDescriptors(itemType).Purify(item)
    End Sub
    <Extension>
    Public Function Offer(itemType As ItemType) As Long
        Return ItemTypeDescriptors(itemType).Offer
    End Function
    <Extension>
    Public Function HasOffer(itemType As ItemType, shoppeType As ShoppeType) As Boolean
        Return ItemTypeDescriptors(itemType).HasOffer(shoppeType)
    End Function
    <Extension>
    Public Function Price(itemType As ItemType) As Long
        Return ItemTypeDescriptors(itemType).Price
    End Function
    <Extension>
    Public Function HasPrice(itemType As ItemType, shoppeType As ShoppeType) As Boolean
        Return ItemTypeDescriptors(itemType).HasPrice(shoppeType)
    End Function
    <Extension>
    Public Function RepairPrice(itemType As ItemType) As Long
        Return ItemTypeDescriptors(itemType).RepairPrice
    End Function
    <Extension>
    Public Function CanRepair(itemType As ItemType, shoppeType As ShoppeType) As Boolean
        Return ItemTypeDescriptors(itemType).CanRepair(shoppeType)
    End Function
End Module
