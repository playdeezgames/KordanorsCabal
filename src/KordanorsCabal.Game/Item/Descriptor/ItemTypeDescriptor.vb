Public Class ItemTypeDescriptor
    '[ItemTypes]([ItemTypeId],[ItemTypeName],[Encumbrance])
    ReadOnly Property Id As Long
    ReadOnly Property Name As String
    ReadOnly Property Encumbrance As Long
    ReadOnly Property IsConsumed As Boolean

    '[ItemTypeSpawnLocationTypes]([ItemTypeId],[DungeonLevelId],[LocationTypeId])
    ReadOnly Property SpawnLocationTypes As IReadOnlyDictionary(Of DungeonLevel, HashSet(Of LocationType))
    Private ReadOnly spawnCounts As IReadOnlyDictionary(Of DungeonLevel, String)

    '[ItemTypeStatistics]([ItemTypeId],[ItemTypeStatisticType],[StatisticValue])
    ReadOnly Property AttackDice As Long
    ReadOnly Property MaximumDamage As Long?
    ReadOnly Property DefendDice As Long
    ReadOnly Property MaximumDurability As Long?
    Friend ReadOnly Property Offer As Long
    Friend ReadOnly Property Price As Long
    Friend ReadOnly Property RepairPrice As Long

    '[ItemTypeEquipSlots]([ItemTypeId],[EquipSlotId])
    ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)

    '[ItemTypeOfferShopTypes]([ItemTypeId],[ShoppeTypeId],[TransactionType])--TransactionType = offer, price, repair
    Private ReadOnly Property boughtAt As IReadOnlyList(Of ShoppeType)
    Private ReadOnly Property soldAt As IReadOnlyList(Of ShoppeType)
    Private ReadOnly Property repairedAt As IReadOnlyList(Of ShoppeType)

    '[ItemTypeCharacterStatisticBuffs]([ItemTypeId],[CharacterStatisticTypeId],[StatisticValue])
    Private ReadOnly Property buffs As IReadOnlyDictionary(Of CharacterStatisticType, Long)

    '[ItemTypeActions]([ItemTypeId],[ItemActionId],[ItemActionName],[ItemActionFilterName])
    ReadOnly Property Purify As Action(Of Item)
    ReadOnly Property Use As Action(Of Character)
    ReadOnly Property CanUse As Func(Of Character, Boolean)

    Function EquippedBuff(statisticType As CharacterStatisticType) As Long?
        If buffs Is Nothing Then
            Return Nothing
        End If
        Dim result As Long = 0
        If buffs.TryGetValue(statisticType, result) Then
            Return result
        End If
        Return Nothing
    End Function
    Function RollSpawnCount(level As DungeonLevel) As Long
        If spawnCounts Is Nothing Then
            Return 0
        End If
        Dim dice As String = ""
        If spawnCounts.TryGetValue(level, dice) Then
            Return RNG.RollDice(dice)
        End If
        Return 0
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
           itemTypeId As Long,
           name As String,
           Optional encumbrance As Long = 0,
           Optional spawnLocationTypes As IReadOnlyDictionary(Of DungeonLevel, HashSet(Of LocationType)) = Nothing,
           Optional spawnCounts As IReadOnlyDictionary(Of DungeonLevel, String) = Nothing,
           Optional equipSlots As IEnumerable(Of EquipSlot) = Nothing,
           Optional buffs As IReadOnlyDictionary(Of CharacterStatisticType, Long) = Nothing,
           Optional attackDice As Long = 0,
           Optional maximumDamage As Long? = Nothing,
           Optional defendDice As Long = 0,
           Optional maximumDurability As Long? = Nothing,
           Optional isConsumed As Boolean = True,
           Optional offer As Long = 0,
           Optional boughtAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional price As Long = 0,
           Optional soldAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional repairPrice As Long = 0,
           Optional repairedAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional purify As Action(Of Item) = Nothing,
           Optional canUse As Func(Of Character, Boolean) = Nothing,
           Optional use As Action(Of Character) = Nothing)
        Me.Id = itemTypeId
        Me.Name = name
        Me.Encumbrance = encumbrance
        If spawnLocationTypes Is Nothing Then
            Me.SpawnLocationTypes =
                AllDungeonLevels.ToDictionary(
                Function(x) x,
                Function(x) New HashSet(Of LocationType))
        Else
            Me.SpawnLocationTypes =
                AllDungeonLevels.ToDictionary(
                Function(x) x,
                Function(x) If(spawnLocationTypes.ContainsKey(x), spawnLocationTypes(x), New HashSet(Of LocationType)))
        End If
        Me.EquipSlots = If(equipSlots, Array.Empty(Of EquipSlot))
        Me.spawnCounts = spawnCounts
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
        Me.IsConsumed = isConsumed
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
            {ItemType.AmuletOfDEX, New AmuletDescriptor(ItemType.AmuletOfDEX, CharacterStatisticType.Dexterity)},
            {ItemType.AmuletOfHP, New AmuletDescriptor(ItemType.AmuletOfHP,
                CharacterStatisticType.HP,
                MakeDictionary(
                    (DungeonLevel.Level1, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.DungeonBoss))),
                MakeDictionary((DungeonLevel.Level1, "1d1")))},
            {ItemType.AmuletOfMana, New AmuletDescriptor(ItemType.AmuletOfMana,
                CharacterStatisticType.Mana,
                MakeDictionary(
                    (DungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.DungeonBoss))),
                MakeDictionary((DungeonLevel.Level2, "1d1")))},
            {ItemType.AmuletOfPOW, New AmuletDescriptor(ItemType.AmuletOfPOW, CharacterStatisticType.Power,
                MakeDictionary(
                    (DungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.DungeonBoss))),
                MakeDictionary((DungeonLevel.Level3, "1d1")))},
            {ItemType.AmuletOfSTR, New AmuletDescriptor(ItemType.AmuletOfSTR, CharacterStatisticType.Strength,
                MakeDictionary(
                    (DungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.DungeonBoss))),
                MakeDictionary((DungeonLevel.Level4, "1d1")))},
            {ItemType.AmuletOfYendor, New ItemTypeDescriptor(
                ItemType.AmuletOfYendor,
                "Amulet of Yendor",,,,
                MakeList(EquipSlot.Neck),,,,,,,,,
                1000,
                MakeList(ShoppeType.BlackMarket))},
            {ItemType.BatWing, New TrophyDescriptor(ItemType.BatWing, "Bat Wing", 3, MakeList(ShoppeType.BlackMage))},
            {ItemType.Beer, New BeerDescriptor},
            {ItemType.Bong, New TrophyDescriptor(ItemType.Bong, "Bong", , , 25, MakeList(ShoppeType.BlackMage))},
            {ItemType.BookOfHolyBolt, New ItemTypeDescriptor(
                    ItemType.BookOfHolyBolt,
                    $"Book of {SpellType.HolyBolt.Name}",,,,,,,,,,,,,
                    100,
                    MakeList(ShoppeType.BlackMage),,,,
                    Function(character) character.CanLearn(SpellType.HolyBolt),
                    Sub(character) character.Learn(SpellType.HolyBolt))},
            {ItemType.BookOfPurify, New ItemTypeDescriptor(ItemType.BookOfPurify,
                    $"Book of {SpellType.Purify.Name}",,,,,,,,,,,,,
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
            {ItemType.GoblinEar, New TrophyDescriptor(ItemType.GoblinEar, "Goblin Ear", 5, MakeList(ShoppeType.BlackMage))},
            {ItemType.GoldKey, New GoldKeyDescriptor},
            {ItemType.Helmet, New HelmetDescriptor},
            {ItemType.Herb, New HerbDescriptor},
            {ItemType.HornsOfKordanor, New HornsOfKordanorDescriptor},
            {ItemType.HolyWater, New HolyWaterDescriptor},
            {ItemType.IronKey, New IronKeyDescriptor},
            {ItemType.Lotion, New LotionDescriptor},
            {ItemType.MagicEgg, New MagicEggDescriptor},
            {ItemType.MembershipCard, New TrophyDescriptor(ItemType.MembershipCard, "Membership Card", 10)},
            {ItemType.MoonPortal, New MoonPortalDescriptor},
            {ItemType.Mushroom, New TrophyDescriptor(ItemType.Mushroom, "Mushroom", 25, MakeList(ShoppeType.BlackMage))},
            {ItemType.PlateMail, New PlateMailDescriptor},
            {ItemType.PlatinumKey, New PlatinumKeyDescriptor},
            {ItemType.Potion, New PotionDescriptor},
            {ItemType.Pr0n, New Pr0nDescriptor},
            {ItemType.RatTail, New TrophyDescriptor(ItemType.RatTail, "Rat Tail", 1, MakeList(ShoppeType.BlackMage))},
            {ItemType.RingOfHP, New RingOfHPDescriptor},
            {ItemType.RottenEgg, New RottenEggDescriptor},
            {ItemType.RottenFood, New RottenFoodDescriptor},
            {ItemType.SilverKey, New SilverKeyDescriptor},
            {ItemType.Shield, New ShieldDescriptor},
            {ItemType.ShoeLaces, New TrophyDescriptor(ItemType.ShoeLaces, "Shoe Laces", 0)},
            {ItemType.Shortsword, New ShortswordDescriptor},
            {ItemType.SkullFragment, New TrophyDescriptor(ItemType.SkullFragment, "Skull Fragment", 5, MakeList(ShoppeType.BlackMage))},
            {ItemType.SnakeFang, New TrophyDescriptor(ItemType.SnakeFang, "Snake Fang", 3, MakeList(ShoppeType.BlackMage))},
            {ItemType.SpaceSord, New SpaceSordDescriptor},
            {ItemType.TownPortal, New TownPortalDescriptor},
            {ItemType.Trousers, New TrousersDescriptor},
            {ItemType.WaterShard, New WaterShardDescriptor},
            {ItemType.ZombieTaint, New TrophyDescriptor(ItemType.ZombieTaint, "Zombie Taint", 5, MakeList(ShoppeType.BlackMage))}
        }
    Public ReadOnly Property AllItemTypes As IEnumerable(Of ItemType)
        Get
            Return ItemTypeDescriptors.Keys
        End Get
    End Property
End Module
