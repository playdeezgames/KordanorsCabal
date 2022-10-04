Public Class ItemType
    Inherits BaseThingie
    Implements IItemType
    Public Shared Function FromId(worlddata As IWorldData, id As Long?) As IItemType
        Return If(id.HasValue, New ItemType(worlddata, id.Value), Nothing)
    End Function
    ReadOnly Property Name As String Implements IItemType.Name
        Get
            Return WorldData.ItemType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property IsConsumed As Boolean Implements IItemType.IsConsumed
        Get
            Return If(WorldData.ItemType.ReadIsConsumed(Id), 0) > 0
        End Get
    End Property
    ReadOnly Property SpawnLocationTypes(dungeonLevel As IDungeonLevel) As HashSet(Of ILocationType) Implements IItemType.SpawnLocationTypes
        Get
            Dim results = WorldData.ItemTypeSpawnLocationType.ReadAll(Id, dungeonLevel.Id)
            If results Is Nothing Then
                Return New HashSet(Of ILocationType)
            End If
            Return New HashSet(Of ILocationType)(results.Select(Function(x) LocationType.FromId(WorldData, x)))
        End Get
    End Property
    Private ReadOnly Property SpawnCounts(dungeonLevel As IDungeonLevel) As String
        Get
            Return WorldData.ItemTypeSpawnCount.Read(Id, dungeonLevel.Id)
        End Get
    End Property
    Private ReadOnly Property ItemTypeStatistic(itemTypeStatisticType As ItemTypeStatisticType) As Long?
        Get
            Return WorldData.ItemTypeStatistic.Read(Id, itemTypeStatisticType.Id)
        End Get
    End Property
    ReadOnly Property Encumbrance As Long Implements IItemType.Encumbrance
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 1)), 0)
        End Get
    End Property
    ReadOnly Property AttackDice As Long Implements IItemType.AttackDice
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 2)), 0)
        End Get
    End Property
    ReadOnly Property MaximumDamage As Long? Implements IItemType.MaximumDamage
        Get
            Return ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 3))
        End Get
    End Property
    ReadOnly Property DefendDice As Long Implements IItemType.DefendDice
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 4)), 0)
        End Get
    End Property
    ReadOnly Property MaximumDurability As Long? Implements IItemType.MaximumDurability
        Get
            Return ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 5))
        End Get
    End Property
    ReadOnly Property Offer As Long Implements IItemType.Offer
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 6)), 0)
        End Get
    End Property
    ReadOnly Property Price As Long Implements IItemType.Price
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 7)), 0)
        End Get
    End Property
    ReadOnly Property RepairPrice As Long Implements IItemType.RepairPrice
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 8)), 0)
        End Get
    End Property
    ReadOnly Property EquipSlots As IEnumerable(Of IEquipSlot) Implements IItemType.EquipSlots
        Get
            Return WorldData.ItemTypeEquipSlot.ReadForItemType(Id).Select(Function(x) EquipSlot.FromId(WorldData, x))
        End Get
    End Property
    Private Const OfferTransactionTypeId = 1L
    Private Const PriceTransactionTypeId = 2L
    Private Const RepairTransactionTypeId = 3L
    Private ReadOnly Property boughtAt As IEnumerable(Of ShoppeType)
        Get
            Return WorldData.ItemTypeShopType.
                ReadForTransactionType(Id, OfferTransactionTypeId).
                Select(Function(x) CType(x, ShoppeType))
        End Get
    End Property
    Private ReadOnly Property soldAt As IEnumerable(Of ShoppeType)
        Get
            Return WorldData.ItemTypeShopType.
                ReadForTransactionType(Id, PriceTransactionTypeId).
                Select(Function(x) CType(x, ShoppeType))
        End Get
    End Property
    Private ReadOnly Property repairedAt As IEnumerable(Of ShoppeType)
        Get
            Return WorldData.ItemTypeShopType.
                ReadForTransactionType(Id, RepairTransactionTypeId).
                Select(Function(x) CType(x, ShoppeType))
        End Get
    End Property
    Const UseEventId = 3L
    Const CanUseEventId = 2L
    Const PurifyEventId = 1L
    Private ReadOnly Property UseActionName As String
        Get
            Return WorldData.ItemTypeEvent.Read(Id, UseEventId)
        End Get
    End Property
    Private ReadOnly Property CanUseFunctionName As String
        Get
            Return WorldData.ItemTypeEvent.Read(Id, CanUseEventId)
        End Get
    End Property
    Private ReadOnly Property PurifyActionName As String
        Get
            Return WorldData.ItemTypeEvent.Read(Id, PurifyEventId)
        End Get
    End Property
    ReadOnly Property Purify As Action(Of IItem) Implements IItemType.Purify
        Get
            Dim result As Action(Of IItem) = Nothing
            Dim eventName = PurifyActionName
            If eventName IsNot Nothing AndAlso PurifyActions.TryGetValue(eventName, result) Then
                Return result
            End If
            Return Sub(i)
                   End Sub
        End Get
    End Property
    ReadOnly Property Use As Action(Of ICharacter) Implements IItemType.Use
        Get
            Dim result As Action(Of ICharacter) = Nothing
            Dim eventName = UseActionName
            If eventName IsNot Nothing AndAlso UseActions.TryGetValue(eventName, result) Then
                Return result
            End If
            Return Sub(c)
                   End Sub
        End Get
    End Property

    ReadOnly Property CanUse As Func(Of ICharacter, Boolean) Implements IItemType.CanUse
        Get
            Dim result As Func(Of ICharacter, Boolean) = Nothing
            Dim eventName = CanUseFunctionName
            If eventName IsNot Nothing AndAlso CanUseFunctions.TryGetValue(eventName, result) Then
                Return result
            End If
            Return Function(c) False
        End Get
    End Property

    Public ReadOnly Property IsWeapon As Boolean Implements IItemType.IsWeapon
        Get
            Return AttackDice > 0
        End Get
    End Property

    Public ReadOnly Property IsArmor As Boolean Implements IItemType.IsArmor
        Get
            Return DefendDice > 0
        End Get
    End Property
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long? Implements IItemType.EquippedBuff
        Return WorldData.ItemTypeCharacterStatisticBuff.Read(Id, statisticType.Id)
    End Function
    Function RollSpawnCount(dungeonLevel As IDungeonLevel) As Long Implements IItemType.RollSpawnCount
        Return RNG.RollDice(SpawnCounts(dungeonLevel))
    End Function
    ReadOnly Property HasOffer(shoppeType As ShoppeType) As Boolean Implements IItemType.HasOffer
        Get
            Return boughtAt.Contains(shoppeType)
        End Get
    End Property
    ReadOnly Property HasPrice(shoppeType As ShoppeType) As Boolean Implements IItemType.HasPrice
        Get
            Return soldAt.Contains(shoppeType)
        End Get
    End Property
    ReadOnly Property CanRepair(shoppeType As ShoppeType) As Boolean Implements IItemType.CanRepair
        Get
            Return repairedAt.Contains(shoppeType)
        End Get
    End Property
    Sub New(
           worldData As IWorldData,
           itemTypeId As Long)
        MyBase.New(worldData, itemTypeId)
    End Sub
End Class
Public Module ItemTypeDescriptorUtility
    Friend ReadOnly ItemTypeDescriptors As IReadOnlyDictionary(Of OldItemType, ItemType) =
        New Dictionary(Of OldItemType, ItemType) From
        {
            {OldItemType.AirShard, New AirShardDescriptor},
            {OldItemType.AmuletOfDEX, New AmuletDescriptor(OldItemType.AmuletOfDEX)},
            {OldItemType.AmuletOfHP, New AmuletDescriptor(OldItemType.AmuletOfHP)},
            {OldItemType.AmuletOfMana, New AmuletDescriptor(OldItemType.AmuletOfMana)},
            {OldItemType.AmuletOfPOW, New AmuletDescriptor(OldItemType.AmuletOfPOW)},
            {OldItemType.AmuletOfSTR, New AmuletDescriptor(OldItemType.AmuletOfSTR)},
            {OldItemType.AmuletOfYendor, New ItemType(
                StaticWorldData.World,
                OldItemType.AmuletOfYendor)},
            {OldItemType.BatWing, New TrophyDescriptor(OldItemType.BatWing)},
            {OldItemType.Beer, New ItemType(StaticWorldData.World, OldItemType.Beer)},
            {OldItemType.Bong, New TrophyDescriptor(OldItemType.Bong)},
            {OldItemType.BookOfHolyBolt, New ItemType(
                    StaticWorldData.World,
                    OldItemType.BookOfHolyBolt)},
            {OldItemType.BookOfPurify, New ItemType(
                    StaticWorldData.World,
                    OldItemType.BookOfPurify)},
            {OldItemType.Bottle, New BottleDescriptor},
            {OldItemType.BrodeSode, New BrodeSodeDescriptor},
            {OldItemType.ChainMail, New ChainMailDescriptor},
            {OldItemType.CopperKey, New CopperKeyDescriptor},
            {OldItemType.Dagger, New DaggerDescriptor},
            {OldItemType.EarthShard, New EarthShardDescriptor},
            {OldItemType.ElementalOrb, New ElementalOrbDescriptor},
            {OldItemType.FireShard, New FireShardDescriptor},
            {OldItemType.Food, New ItemType(StaticWorldData.World, OldItemType.Food)},
            {OldItemType.GoblinEar, New TrophyDescriptor(OldItemType.GoblinEar)},
            {OldItemType.GoldKey, New GoldKeyDescriptor},
            {OldItemType.Helmet, New HelmetDescriptor},
            {OldItemType.Herb, New ItemType(StaticWorldData.World, OldItemType.Herb)},
            {OldItemType.HornsOfKordanor, New HornsOfKordanorDescriptor},
            {OldItemType.HolyWater, New HolyWaterDescriptor},
            {OldItemType.IronKey, New IronKeyDescriptor},
            {OldItemType.Lotion, New ItemType(StaticWorldData.World, OldItemType.Lotion)},
            {OldItemType.MagicEgg, New MagicEggDescriptor},
            {OldItemType.MembershipCard, New TrophyDescriptor(OldItemType.MembershipCard)},
            {OldItemType.MoonPortal, New MoonPortalDescriptor},
            {OldItemType.Mushroom, New TrophyDescriptor(OldItemType.Mushroom)},
            {OldItemType.PlateMail, New PlateMailDescriptor},
            {OldItemType.PlatinumKey, New PlatinumKeyDescriptor},
            {OldItemType.Potion, New ItemType(StaticWorldData.World, OldItemType.Potion)},
            {OldItemType.Pr0n, New ItemType(StaticWorldData.World, OldItemType.Pr0n)},
            {OldItemType.RatTail, New TrophyDescriptor(OldItemType.RatTail)},
            {OldItemType.RingOfHP, New RingOfHPDescriptor},
            {OldItemType.RottenEgg, New ItemType(StaticWorldData.World, OldItemType.RottenEgg)},
            {OldItemType.RottenFood, New ItemType(StaticWorldData.World, OldItemType.RottenFood)},
            {OldItemType.SilverKey, New SilverKeyDescriptor},
            {OldItemType.Shield, New ShieldDescriptor},
            {OldItemType.ShoeLaces, New TrophyDescriptor(OldItemType.ShoeLaces)},
            {OldItemType.Shortsword, New ShortswordDescriptor},
            {OldItemType.SkullFragment, New TrophyDescriptor(OldItemType.SkullFragment)},
            {OldItemType.SnakeFang, New TrophyDescriptor(OldItemType.SnakeFang)},
            {OldItemType.SpaceSord, New SpaceSordDescriptor},
            {OldItemType.TownPortal, New TownPortalDescriptor},
            {OldItemType.Trousers, New TrousersDescriptor},
            {OldItemType.WaterShard, New WaterShardDescriptor},
            {OldItemType.ZombieTaint, New TrophyDescriptor(OldItemType.ZombieTaint)}
        }
    Public ReadOnly Property AllItemTypes As IEnumerable(Of OldItemType)
        Get
            Return ItemTypeDescriptors.Keys
        End Get
    End Property
End Module
