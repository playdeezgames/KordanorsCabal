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
    ReadOnly Property Encumbrance As Long
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 1)), 0)
        End Get
    End Property
    ReadOnly Property AttackDice As Long
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 2)), 0)
        End Get
    End Property
    ReadOnly Property MaximumDamage As Long?
        Get
            Return ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 3))
        End Get
    End Property
    ReadOnly Property DefendDice As Long
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 4)), 0)
        End Get
    End Property
    ReadOnly Property MaximumDurability As Long?
        Get
            Return ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 5))
        End Get
    End Property
    '[ItemTypeStatistics]([ItemTypeId],[ItemTypeStatisticType],[StatisticValue])
    Friend ReadOnly Property Offer As Long 'ItemTypeStatisticType
    Friend ReadOnly Property Price As Long 'ItemTypeStatisticType
    Friend ReadOnly Property RepairPrice As Long 'ItemTypeStatisticType

    '[ItemTypeEquipSlots]([ItemTypeId],[EquipSlotId])
    ReadOnly Property EquipSlots As IEnumerable(Of IEquipSlot) Implements IItemType.EquipSlots
        Get
            Return WorldData.ItemTypeEquipSlot.ReadForItemType(Id).Select(Function(x) EquipSlot.FromId(WorldData, x))
        End Get
    End Property

    '[ItemTypeOfferShopTypes]([ItemTypeId],[ShoppeTypeId],[TransactionType])--TransactionType = offer, price, repair
    Private ReadOnly Property boughtAt As IReadOnlyList(Of ShoppeType)
    Private ReadOnly Property soldAt As IReadOnlyList(Of ShoppeType)
    Private ReadOnly Property repairedAt As IReadOnlyList(Of ShoppeType)
    '[ItemTypeActions]([ItemTypeId],[ItemActionId],[ItemActionName],[ItemActionFilterName])
    ReadOnly Property Purify As Action(Of Item)
        Get
            Dim result As Action(Of Item) = Nothing
            If PurifyActions.TryGetValue(PurifyActionName, result) Then
                Return result
            End If
            Return Sub(i)
                   End Sub
        End Get
    End Property
    Private ReadOnly Property PurifyActionName As String
    ReadOnly Property Use As Action(Of ICharacter)
        Get
            Dim result As Action(Of ICharacter) = Nothing
            If UseActions.TryGetValue(UseActionName, result) Then
                Return result
            End If
            Return Sub(c)
                   End Sub
        End Get
    End Property
    Private ReadOnly Property UseActionName As String

    ReadOnly Property CanUse As Func(Of ICharacter, Boolean)
        Get
            Dim result As Func(Of ICharacter, Boolean) = Nothing
            If CanUseFunctions.TryGetValue(CanUseFunctionName, result) Then
                Return result
            End If
            Return Function(c) False
        End Get
    End Property
    Private ReadOnly CanUseFunctionName As String


    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long?
        Return WorldData.ItemTypeCharacterStatisticBuff.Read(Id, statisticType.Id)
    End Function
    Function RollSpawnCount(dungeonLevel As IDungeonLevel) As Long
        Return RNG.RollDice(SpawnCounts(dungeonLevel))
    End Function
    Friend Function HasOffer(shoppeType As ShoppeType) As Boolean
        Return boughtAt.Contains(shoppeType)
    End Function
    Friend Function HasPrice(shoppeType As ShoppeType) As Boolean
        Return soldAt.Contains(shoppeType)
    End Function
    Friend Function CanRepair(shoppeType As ShoppeType) As Boolean
        Return repairedAt.Contains(shoppeType)
    End Function
    Sub New(
           worldData As IWorldData,
           itemTypeId As Long,
           Optional offer As Long = 0,
           Optional boughtAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional price As Long = 0,
           Optional soldAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional repairPrice As Long = 0,
           Optional repairedAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional purifyActionName As String = Nothing,
           Optional canUseFunctionName As String = Nothing,
           Optional useActionName As String = Nothing)
        MyBase.New(worldData, itemTypeId)
        Me.Offer = offer
        Me.Price = price
        Me.RepairPrice = repairPrice
        Me.boughtAt = If(boughtAt, New List(Of ShoppeType))
        Me.soldAt = If(soldAt, New List(Of ShoppeType))
        Me.repairedAt = If(repairedAt, New List(Of ShoppeType))
        Me.PurifyActionName = purifyActionName
        Me.CanUseFunctionName = canUseFunctionName
        Me.UseActionName = useActionName
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
                OldItemType.AmuletOfYendor,,,
                1000,
                MakeList(ShoppeType.BlackMarket))},
            {OldItemType.BatWing, New TrophyDescriptor(OldItemType.BatWing, 3, MakeList(ShoppeType.BlackMage))},
            {OldItemType.Beer, New BeerDescriptor},
            {OldItemType.Bong, New TrophyDescriptor(OldItemType.Bong, , , 25, MakeList(ShoppeType.BlackMage))},
            {OldItemType.BookOfHolyBolt, New ItemType(
                    StaticWorldData.World,
                    OldItemType.BookOfHolyBolt,,,
                    100,
                    MakeList(ShoppeType.BlackMage),,,,
                    "CanLearnHolyBolt",
                    "LearnHolyBolt")},
            {OldItemType.BookOfPurify, New ItemType(
                    StaticWorldData.World,
                    OldItemType.BookOfPurify,,,
                    50,
                    MakeList(ShoppeType.BlackMage),,,,
                    "CanLearnPurify",
                    "LearnPurify")},
            {OldItemType.Bottle, New BottleDescriptor},
            {OldItemType.BrodeSode, New BrodeSodeDescriptor},
            {OldItemType.ChainMail, New ChainMailDescriptor},
            {OldItemType.CopperKey, New CopperKeyDescriptor},
            {OldItemType.Dagger, New DaggerDescriptor},
            {OldItemType.EarthShard, New EarthShardDescriptor},
            {OldItemType.ElementalOrb, New ElementalOrbDescriptor},
            {OldItemType.FireShard, New FireShardDescriptor},
            {OldItemType.Food, New FoodDescriptor},
            {OldItemType.GoblinEar, New TrophyDescriptor(OldItemType.GoblinEar, 5, MakeList(ShoppeType.BlackMage))},
            {OldItemType.GoldKey, New GoldKeyDescriptor},
            {OldItemType.Helmet, New HelmetDescriptor},
            {OldItemType.Herb, New HerbDescriptor},
            {OldItemType.HornsOfKordanor, New HornsOfKordanorDescriptor},
            {OldItemType.HolyWater, New HolyWaterDescriptor},
            {OldItemType.IronKey, New IronKeyDescriptor},
            {OldItemType.Lotion, New LotionDescriptor},
            {OldItemType.MagicEgg, New MagicEggDescriptor},
            {OldItemType.MembershipCard, New TrophyDescriptor(OldItemType.MembershipCard, 10)},
            {OldItemType.MoonPortal, New MoonPortalDescriptor},
            {OldItemType.Mushroom, New TrophyDescriptor(OldItemType.Mushroom, 25, MakeList(ShoppeType.BlackMage))},
            {OldItemType.PlateMail, New PlateMailDescriptor},
            {OldItemType.PlatinumKey, New PlatinumKeyDescriptor},
            {OldItemType.Potion, New PotionDescriptor},
            {OldItemType.Pr0n, New Pr0nDescriptor},
            {OldItemType.RatTail, New TrophyDescriptor(OldItemType.RatTail, 1, MakeList(ShoppeType.BlackMage))},
            {OldItemType.RingOfHP, New RingOfHPDescriptor},
            {OldItemType.RottenEgg, New RottenEggDescriptor},
            {OldItemType.RottenFood, New RottenFoodDescriptor},
            {OldItemType.SilverKey, New SilverKeyDescriptor},
            {OldItemType.Shield, New ShieldDescriptor},
            {OldItemType.ShoeLaces, New TrophyDescriptor(OldItemType.ShoeLaces, 0)},
            {OldItemType.Shortsword, New ShortswordDescriptor},
            {OldItemType.SkullFragment, New TrophyDescriptor(OldItemType.SkullFragment, 5, MakeList(ShoppeType.BlackMage))},
            {OldItemType.SnakeFang, New TrophyDescriptor(OldItemType.SnakeFang, 3, MakeList(ShoppeType.BlackMage))},
            {OldItemType.SpaceSord, New SpaceSordDescriptor},
            {OldItemType.TownPortal, New TownPortalDescriptor},
            {OldItemType.Trousers, New TrousersDescriptor},
            {OldItemType.WaterShard, New WaterShardDescriptor},
            {OldItemType.ZombieTaint, New TrophyDescriptor(OldItemType.ZombieTaint, 5, MakeList(ShoppeType.BlackMage))}
        }
    Public ReadOnly Property AllItemTypes As IEnumerable(Of OldItemType)
        Get
            Return ItemTypeDescriptors.Keys
        End Get
    End Property
End Module
