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
    Function SpawnLocationTypes(itemType As OldItemType, level As DungeonLevel) As HashSet(Of ILocationType)
        Return ItemTypeDescriptors(itemType).SpawnLocationTypes(level)
    End Function
    <Extension>
    Function Name(itemType As OldItemType) As String
        Return ItemTypeDescriptors(itemType).Name
    End Function
    <Extension>
    Function CanUse(itemType As OldItemType, character As ICharacter) As Boolean
        Return ItemTypeDescriptors(itemType).CanUse(character)
    End Function
    <Extension>
    Function Encumbrance(itemType As OldItemType) As Long
        Return ItemTypeDescriptors(itemType).Encumbrance
    End Function
    <Extension>
    Public Sub Use(itemType As OldItemType, character As ICharacter)
        ItemTypeDescriptors(itemType).Use.Invoke(character)
    End Sub
    <Extension>
    Public Function RollSpawnCount(itemType As OldItemType, dungeonLevel As DungeonLevel) As Long
        Return ItemTypeDescriptors(itemType).RollSpawnCount(dungeonLevel)
    End Function
    <Extension>
    Public Function EquipSlots(itemType As OldItemType) As IEnumerable(Of EquipSlot)
        Return ItemTypeDescriptors(itemType).EquipSlots
    End Function
    <Extension>
    Public Function AttackDice(itemType As OldItemType) As Long
        Return ItemTypeDescriptors(itemType).AttackDice
    End Function
    <Extension>
    Public Function IsWeapon(itemType As OldItemType) As Boolean
        Return ItemTypeDescriptors(itemType).AttackDice > 0
    End Function
    <Extension>
    Public Function DefendDice(itemType As OldItemType) As Long
        Return ItemTypeDescriptors(itemType).DefendDice
    End Function
    <Extension>
    Public Function IsArmor(itemType As OldItemType) As Boolean
        Return ItemTypeDescriptors(itemType).DefendDice > 0
    End Function
    <Extension>
    Public Function IsConsumed(itemType As OldItemType) As Boolean
        Return ItemTypeDescriptors(itemType).IsConsumed
    End Function
    <Extension>
    Public Function MaximumDamage(itemType As OldItemType) As Long?
        Return ItemTypeDescriptors(itemType).MaximumDamage
    End Function
    <Extension>
    Public Function MaximumDurability(itemType As OldItemType) As Long?
        Return ItemTypeDescriptors(itemType).MaximumDurability
    End Function
    <Extension>
    Public Function EquippedBuff(itemType As OldItemType, statisticType As CharacterStatisticType) As Long?
        Return ItemTypeDescriptors(itemType).EquippedBuff(statisticType)
    End Function
    <Extension>
    Public Sub Purify(itemType As OldItemType, item As Item)
        ItemTypeDescriptors(itemType).Purify.Invoke(item)
    End Sub
    <Extension>
    Public Function Offer(itemType As OldItemType) As Long
        Return ItemTypeDescriptors(itemType).Offer
    End Function
    <Extension>
    Public Function HasOffer(itemType As OldItemType, shoppeType As ShoppeType) As Boolean
        Return ItemTypeDescriptors(itemType).HasOffer(shoppeType)
    End Function
    <Extension>
    Public Function Price(itemType As OldItemType) As Long
        Return ItemTypeDescriptors(itemType).Price
    End Function
    <Extension>
    Public Function HasPrice(itemType As OldItemType, shoppeType As ShoppeType) As Boolean
        Return ItemTypeDescriptors(itemType).HasPrice(shoppeType)
    End Function
    <Extension>
    Public Function RepairPrice(itemType As OldItemType) As Long
        Return ItemTypeDescriptors(itemType).RepairPrice
    End Function
    <Extension>
    Public Function CanRepair(itemType As OldItemType, shoppeType As ShoppeType) As Boolean
        Return ItemTypeDescriptors(itemType).CanRepair(shoppeType)
    End Function
End Module
