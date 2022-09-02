Public Class ItemTypeDescriptor
    Inherits BaseThingie
    ReadOnly Property Name As String
        Get
            Return WorldData.ItemType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property IsConsumed As Boolean
        Get
            Return WorldData.ItemType.ReadIsConsumed(Id).Value > 0
        End Get
    End Property
    ReadOnly Property SpawnLocationTypes(dungeonLevel As DungeonLevel) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)(WorldData.ItemTypeSpawnLocationType.ReadAll(Id, dungeonLevel.Id).Select(Function(x) LocationType.FromId(WorldData, x)))
        End Get
    End Property
    Private ReadOnly Property SpawnCounts(dungeonLevel As DungeonLevel) As String
        Get
            Return WorldData.ItemTypeSpawnCount.Read(Id, dungeonLevel.Id)
        End Get
    End Property

    '[ItemTypeStatistics]([ItemTypeId],[ItemTypeStatisticType],[StatisticValue])
    ReadOnly Property Encumbrance As Long 'ItemTypeStatisticType
    ReadOnly Property AttackDice As Long 'ItemTypeStatisticType
    ReadOnly Property MaximumDamage As Long? 'ItemTypeStatisticType
    ReadOnly Property DefendDice As Long 'ItemTypeStatisticType
    ReadOnly Property MaximumDurability As Long? 'ItemTypeStatisticType
    Friend ReadOnly Property Offer As Long 'ItemTypeStatisticType
    Friend ReadOnly Property Price As Long 'ItemTypeStatisticType
    Friend ReadOnly Property RepairPrice As Long 'ItemTypeStatisticType

    '[ItemTypeEquipSlots]([ItemTypeId],[EquipSlotId])
    ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)

    '[ItemTypeOfferShopTypes]([ItemTypeId],[ShoppeTypeId],[TransactionType])--TransactionType = offer, price, repair
    Private ReadOnly Property boughtAt As IReadOnlyList(Of ShoppeType)
    Private ReadOnly Property soldAt As IReadOnlyList(Of ShoppeType)
    Private ReadOnly Property repairedAt As IReadOnlyList(Of ShoppeType)

    '[ItemTypeCharacterStatisticBuffs]([ItemTypeId],[CharacterStatisticTypeId],[StatisticValue])
    Private ReadOnly Property buffs As IReadOnlyDictionary(Of Long, Long)

    '[ItemTypeActions]([ItemTypeId],[ItemActionId],[ItemActionName],[ItemActionFilterName])
    ReadOnly Property Purify As Action(Of Item)
    ReadOnly Property Use As Action(Of Character)
    ReadOnly Property CanUse As Func(Of Character, Boolean)

    Function EquippedBuff(statisticType As CharacterStatisticType) As Long?
        If buffs Is Nothing Then
            Return Nothing
        End If
        Dim result As Long = 0
        If buffs.TryGetValue(statisticType.Id, result) Then
            Return result
        End If
        Return Nothing
    End Function
    Function RollSpawnCount(dungeonLevel As DungeonLevel) As Long
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
           worldData As WorldData,
           itemTypeId As Long,
           Optional encumbrance As Long = 0,
           Optional equipSlots As IEnumerable(Of EquipSlot) = Nothing,
           Optional buffs As IReadOnlyDictionary(Of Long, Long) = Nothing,
           Optional attackDice As Long = 0,
           Optional maximumDamage As Long? = Nothing,
           Optional defendDice As Long = 0,
           Optional maximumDurability As Long? = Nothing,
           Optional offer As Long = 0,
           Optional boughtAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional price As Long = 0,
           Optional soldAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional repairPrice As Long = 0,
           Optional repairedAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional purify As Action(Of Item) = Nothing,
           Optional canUse As Func(Of Character, Boolean) = Nothing,
           Optional use As Action(Of Character) = Nothing)
        MyBase.New(worldData, itemTypeId)
        Me.Encumbrance = encumbrance
        Me.EquipSlots = If(equipSlots, Array.Empty(Of EquipSlot))
        Me.Offer = offer
        Me.Price = price
        Me.RepairPrice = repairPrice
        Me.boughtAt = If(boughtAt, New List(Of ShoppeType))
        Me.soldAt = If(soldAt, New List(Of ShoppeType))
        Me.repairedAt = If(repairedAt, New List(Of ShoppeType))
        Me.AttackDice = attackDice
        Me.MaximumDamage = maximumDamage
        Me.DefendDice = defendDice
        Me.MaximumDurability = maximumDurability
        Me.buffs = buffs
        Me.Purify = If(purify, Sub(item)
                               End Sub)
        Me.CanUse = If(canUse, Function(character) False)
        Me.Use = If(use, Sub(character)
                         End Sub)
    End Sub
End Class
Public Module ItemTypeDescriptorUtility
    Friend ReadOnly ItemTypeDescriptors As IReadOnlyDictionary(Of ItemType, ItemTypeDescriptor) =
        New Dictionary(Of ItemType, ItemTypeDescriptor) From
        {
            {ItemType.AirShard, New AirShardDescriptor},
            {ItemType.AmuletOfDEX, New AmuletDescriptor(ItemType.AmuletOfDEX, 2)},
            {ItemType.AmuletOfHP, New AmuletDescriptor(ItemType.AmuletOfHP,
                6)},
            {ItemType.AmuletOfMana, New AmuletDescriptor(ItemType.AmuletOfMana,
                8)},
            {ItemType.AmuletOfPOW, New AmuletDescriptor(ItemType.AmuletOfPOW, 5)},
            {ItemType.AmuletOfSTR, New AmuletDescriptor(ItemType.AmuletOfSTR, 1)},
            {ItemType.AmuletOfYendor, New ItemTypeDescriptor(
                StaticWorldData.World,
                ItemType.AmuletOfYendor,,
                MakeList(EquipSlot.FromId(StaticWorldData.World, 6L)),,,,,,,,
                1000,
                MakeList(ShoppeType.BlackMarket))},
            {ItemType.BatWing, New TrophyDescriptor(ItemType.BatWing, 3, MakeList(ShoppeType.BlackMage))},
            {ItemType.Beer, New BeerDescriptor},
            {ItemType.Bong, New TrophyDescriptor(ItemType.Bong, , , 25, MakeList(ShoppeType.BlackMage))},
            {ItemType.BookOfHolyBolt, New ItemTypeDescriptor(
                    StaticWorldData.World,
                    ItemType.BookOfHolyBolt,,,,,,,,,,
                    100,
                    MakeList(ShoppeType.BlackMage),,,,
                    Function(character) character.CanLearn(SpellType.HolyBolt),
                    Sub(character) character.Learn(SpellType.HolyBolt))},
            {ItemType.BookOfPurify, New ItemTypeDescriptor(
                    StaticWorldData.World,
                    ItemType.BookOfPurify,,,,,,,,,,
                    50,
                    MakeList(ShoppeType.BlackMage),,,,
                    Function(character) character.CanLearn(SpellType.Purify),
                    Sub(character) character.Learn(SpellType.Purify))},
            {ItemType.Bottle, New BottleDescriptor},
            {ItemType.BrodeSode, New BrodeSodeDescriptor},
            {ItemType.ChainMail, New ChainMailDescriptor},
            {ItemType.CopperKey, New CopperKeyDescriptor},
            {ItemType.Dagger, New DaggerDescriptor},
            {ItemType.EarthShard, New EarthShardDescriptor},
            {ItemType.ElementalOrb, New ElementalOrbDescriptor},
            {ItemType.FireShard, New FireShardDescriptor},
            {ItemType.Food, New FoodDescriptor},
            {ItemType.GoblinEar, New TrophyDescriptor(ItemType.GoblinEar, 5, MakeList(ShoppeType.BlackMage))},
            {ItemType.GoldKey, New GoldKeyDescriptor},
            {ItemType.Helmet, New HelmetDescriptor},
            {ItemType.Herb, New HerbDescriptor},
            {ItemType.HornsOfKordanor, New HornsOfKordanorDescriptor},
            {ItemType.HolyWater, New HolyWaterDescriptor},
            {ItemType.IronKey, New IronKeyDescriptor},
            {ItemType.Lotion, New LotionDescriptor},
            {ItemType.MagicEgg, New MagicEggDescriptor},
            {ItemType.MembershipCard, New TrophyDescriptor(ItemType.MembershipCard, 10)},
            {ItemType.MoonPortal, New MoonPortalDescriptor},
            {ItemType.Mushroom, New TrophyDescriptor(ItemType.Mushroom, 25, MakeList(ShoppeType.BlackMage))},
            {ItemType.PlateMail, New PlateMailDescriptor},
            {ItemType.PlatinumKey, New PlatinumKeyDescriptor},
            {ItemType.Potion, New PotionDescriptor},
            {ItemType.Pr0n, New Pr0nDescriptor},
            {ItemType.RatTail, New TrophyDescriptor(ItemType.RatTail, 1, MakeList(ShoppeType.BlackMage))},
            {ItemType.RingOfHP, New RingOfHPDescriptor},
            {ItemType.RottenEgg, New RottenEggDescriptor},
            {ItemType.RottenFood, New RottenFoodDescriptor},
            {ItemType.SilverKey, New SilverKeyDescriptor},
            {ItemType.Shield, New ShieldDescriptor},
            {ItemType.ShoeLaces, New TrophyDescriptor(ItemType.ShoeLaces, 0)},
            {ItemType.Shortsword, New ShortswordDescriptor},
            {ItemType.SkullFragment, New TrophyDescriptor(ItemType.SkullFragment, 5, MakeList(ShoppeType.BlackMage))},
            {ItemType.SnakeFang, New TrophyDescriptor(ItemType.SnakeFang, 3, MakeList(ShoppeType.BlackMage))},
            {ItemType.SpaceSord, New SpaceSordDescriptor},
            {ItemType.TownPortal, New TownPortalDescriptor},
            {ItemType.Trousers, New TrousersDescriptor},
            {ItemType.WaterShard, New WaterShardDescriptor},
            {ItemType.ZombieTaint, New TrophyDescriptor(ItemType.ZombieTaint, 5, MakeList(ShoppeType.BlackMage))}
        }
    Public ReadOnly Property AllItemTypes As IEnumerable(Of ItemType)
        Get
            Return ItemTypeDescriptors.Keys
        End Get
    End Property
End Module
