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
    Friend ReadOnly Property Offer As Long
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 6)), 0)
        End Get
    End Property
    Friend ReadOnly Property Price As Long
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 7)), 0)
        End Get
    End Property
    Friend ReadOnly Property RepairPrice As Long
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
            Return WorldData.ItemTypeShopType.ReadForTransactionType(OfferTransactionTypeId).Select(Function(x) CType(x, ShoppeType))
        End Get
    End Property
    '[ItemTypeShopTypes]([ItemTypeId],[ShoppeTypeId],[TransactionType])--TransactionType = offer, price, repair
    Private ReadOnly Property soldAt As IEnumerable(Of ShoppeType)
    Private ReadOnly Property repairedAt As IEnumerable(Of ShoppeType)
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
           Optional boughtAt As IReadOnlyList(Of ShoppeType) = Nothing, 'TODO: yank
           Optional soldAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional repairedAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional purifyActionName As String = Nothing,
           Optional canUseFunctionName As String = Nothing,
           Optional useActionName As String = Nothing)
        MyBase.New(worldData, itemTypeId)
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
                OldItemType.AmuletOfYendor,,
                MakeList(ShoppeType.BlackMarket))},
            {OldItemType.BatWing, New TrophyDescriptor(OldItemType.BatWing, MakeList(ShoppeType.BlackMage))},
            {OldItemType.Beer, New BeerDescriptor},
            {OldItemType.Bong, New TrophyDescriptor(OldItemType.Bong, , MakeList(ShoppeType.BlackMage))},
            {OldItemType.BookOfHolyBolt, New ItemType(
                    StaticWorldData.World,
                    OldItemType.BookOfHolyBolt,,
                    MakeList(ShoppeType.BlackMage),,,
                    "CanLearnHolyBolt",
                    "LearnHolyBolt")},
            {OldItemType.BookOfPurify, New ItemType(
                    StaticWorldData.World,
                    OldItemType.BookOfPurify,,
                    MakeList(ShoppeType.BlackMage),,,
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
            {OldItemType.GoblinEar, New TrophyDescriptor(OldItemType.GoblinEar, MakeList(ShoppeType.BlackMage))},
            {OldItemType.GoldKey, New GoldKeyDescriptor},
            {OldItemType.Helmet, New HelmetDescriptor},
            {OldItemType.Herb, New HerbDescriptor},
            {OldItemType.HornsOfKordanor, New HornsOfKordanorDescriptor},
            {OldItemType.HolyWater, New HolyWaterDescriptor},
            {OldItemType.IronKey, New IronKeyDescriptor},
            {OldItemType.Lotion, New LotionDescriptor},
            {OldItemType.MagicEgg, New MagicEggDescriptor},
            {OldItemType.MembershipCard, New TrophyDescriptor(OldItemType.MembershipCard)},
            {OldItemType.MoonPortal, New MoonPortalDescriptor},
            {OldItemType.Mushroom, New TrophyDescriptor(OldItemType.Mushroom, MakeList(ShoppeType.BlackMage))},
            {OldItemType.PlateMail, New PlateMailDescriptor},
            {OldItemType.PlatinumKey, New PlatinumKeyDescriptor},
            {OldItemType.Potion, New PotionDescriptor},
            {OldItemType.Pr0n, New Pr0nDescriptor},
            {OldItemType.RatTail, New TrophyDescriptor(OldItemType.RatTail, MakeList(ShoppeType.BlackMage))},
            {OldItemType.RingOfHP, New RingOfHPDescriptor},
            {OldItemType.RottenEgg, New RottenEggDescriptor},
            {OldItemType.RottenFood, New RottenFoodDescriptor},
            {OldItemType.SilverKey, New SilverKeyDescriptor},
            {OldItemType.Shield, New ShieldDescriptor},
            {OldItemType.ShoeLaces, New TrophyDescriptor(OldItemType.ShoeLaces)},
            {OldItemType.Shortsword, New ShortswordDescriptor},
            {OldItemType.SkullFragment, New TrophyDescriptor(OldItemType.SkullFragment, MakeList(ShoppeType.BlackMage))},
            {OldItemType.SnakeFang, New TrophyDescriptor(OldItemType.SnakeFang, MakeList(ShoppeType.BlackMage))},
            {OldItemType.SpaceSord, New SpaceSordDescriptor},
            {OldItemType.TownPortal, New TownPortalDescriptor},
            {OldItemType.Trousers, New TrousersDescriptor},
            {OldItemType.WaterShard, New WaterShardDescriptor},
            {OldItemType.ZombieTaint, New TrophyDescriptor(OldItemType.ZombieTaint, MakeList(ShoppeType.BlackMage))}
        }
    Public ReadOnly Property AllItemTypes As IEnumerable(Of OldItemType)
        Get
            Return ItemTypeDescriptors.Keys
        End Get
    End Property
End Module
