Public MustInherit Class ItemTypeDescriptor
    ReadOnly Property SpawnLocationTypes As IReadOnlyDictionary(Of DungeonLevel, HashSet(Of LocationType))
    ReadOnly Property Name As String
    ReadOnly Property Encumbrance As Single
    Private ReadOnly spawnCounts As IReadOnlyDictionary(Of DungeonLevel, String)
    Overridable ReadOnly Property PurchasePrice() As Long?
        Get
            Return Nothing
        End Get
    End Property

    Overridable ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return False
        End Get
    End Property

    Overridable Sub Use(character As Character)
        'nothing, by default
    End Sub

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

    Overridable ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return Array.Empty(Of EquipSlot)
        End Get
    End Property

    Overridable ReadOnly Property AttackDice As Long
        Get
            Return 0
        End Get
    End Property

    Overridable ReadOnly Property MaximumDamage As Long?
        Get
            Return Nothing
        End Get
    End Property

    Overridable ReadOnly Property MaximumDurability As Long?
        Get
            Return Nothing
        End Get
    End Property

    Overridable ReadOnly Property DefendDice As Long
        Get
            Return 0
        End Get
    End Property

    Overridable ReadOnly Property IsConsumed As Boolean
        Get
            Return True
        End Get
    End Property

    Overridable Function EquippedBuff(statisticType As CharacterStatisticType) As Long?
        Return Nothing
    End Function

    Overridable Sub Purify(item As Item)
        'by default, do nothing
    End Sub

    Friend ReadOnly Property Offer() As Long
    Private ReadOnly Property boughtAt As IReadOnlyList(Of ShoppeType)

    Friend Overridable Function HasOffer(shoppeType As ShoppeType) As Boolean
        Return boughtAt.Contains(shoppeType)
    End Function

    Friend ReadOnly Property Price() As Long
    Private ReadOnly Property soldAt As IReadOnlyList(Of ShoppeType)

    Friend Overridable Function HasPrice(shoppeType As ShoppeType) As Boolean
        Return soldAt.Contains(shoppeType)
    End Function

    Friend ReadOnly Property RepairPrice() As Long
    Private ReadOnly Property repairedAt As IReadOnlyList(Of ShoppeType)

    Friend Function CanRepair(shoppeType As ShoppeType) As Boolean
        Return repairedAt.Contains(shoppeType)
    End Function

    Sub New(
           name As String,
           Optional encumbrance As Single = 0!,
           Optional spawnLocationTypes As IReadOnlyDictionary(Of DungeonLevel, HashSet(Of LocationType)) = Nothing,
           Optional spawnCounts As IReadOnlyDictionary(Of DungeonLevel, String) = Nothing,
           Optional offer As Long = 0,
           Optional boughtAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional price As Long = 0,
           Optional soldAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional repairPrice As Long = 0,
           Optional repairedAt As IReadOnlyList(Of ShoppeType) = Nothing)
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
        Me.spawnCounts = spawnCounts
        Me.Offer = offer
        Me.Price = price
        Me.RepairPrice = repairPrice
        Me.boughtAt = If(boughtAt, New List(Of ShoppeType))
        Me.soldAt = If(soldAt, New List(Of ShoppeType))
        Me.repairedAt = If(repairedAt, New List(Of ShoppeType))
    End Sub
End Class
Public Module ItemTypeDescriptorUtility
    Friend ReadOnly ItemTypeDescriptors As IReadOnlyDictionary(Of ItemType, ItemTypeDescriptor) =
        New Dictionary(Of ItemType, ItemTypeDescriptor) From
        {
            {ItemType.AirShard, New AirShardDescriptor},
            {ItemType.AmuletOfDEX, New AmuletDescriptor(CharacterStatisticType.Dexterity)},
            {ItemType.AmuletOfHP, New AmuletDescriptor(
                CharacterStatisticType.HP,
                MakeDictionary(
                    (DungeonLevel.Level1, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.DungeonBoss))),
                MakeDictionary((DungeonLevel.Level1, "1d1")))},
            {ItemType.AmuletOfMana, New AmuletDescriptor(CharacterStatisticType.Mana)},
            {ItemType.AmuletOfPOW, New AmuletDescriptor(CharacterStatisticType.Power)},
            {ItemType.AmuletOfSTR, New AmuletDescriptor(CharacterStatisticType.Strength)},
            {ItemType.BatWing, New TrophyDescriptor("Bat Wing", 3, MakeList(ShoppeType.BlackMage))},
            {ItemType.Beer, New BeerDescriptor},
            {ItemType.Bong, New TrophyDescriptor("Bong", , , 25, MakeList(ShoppeType.BlackMage))},
            {ItemType.BookOfHolyBolt, New BookDescriptor(SpellType.HolyBolt)},
            {ItemType.BookOfPurify, New BookDescriptor(SpellType.Purify)},
            {ItemType.Bottle, New BottleDescriptor},
            {ItemType.BrodeSode, New BrodeSodeDescriptor},
            {ItemType.ChainMail, New ChainMailDescriptor},
            {ItemType.CopperKey, New CopperKeyDescriptor},
            {ItemType.Dagger, New DaggerDescriptor},
            {ItemType.EarthShard, New EarthShardDescriptor},
            {ItemType.ElementalOrb, New ElementalOrbDescriptor},
            {ItemType.FireShard, New FireShardDescriptor},
            {ItemType.Food, New FoodDescriptor},
            {ItemType.GoblinEar, New TrophyDescriptor("Goblin Ear", 5, MakeList(ShoppeType.BlackMage))},
            {ItemType.GoldKey, New GoldKeyDescriptor},
            {ItemType.Helmet, New HelmetDescriptor},
            {ItemType.Herb, New HerbDescriptor},
            {ItemType.HornsOfKordanor, New HornsOfKordanorDescriptor},
            {ItemType.HolyWater, New HolyWaterDescriptor},
            {ItemType.IronKey, New IronKeyDescriptor},
            {ItemType.Lotion, New LotionDescriptor},
            {ItemType.MagicEgg, New MagicEggDescriptor},
            {ItemType.MembershipCard, New TrophyDescriptor("Membership Card", 10)},
            {ItemType.MoonPortal, New MoonPortalDescriptor},
            {ItemType.Mushroom, New TrophyDescriptor("Mushroom", 25, MakeList(ShoppeType.BlackMage))},
            {ItemType.PlateMail, New PlateMailDescriptor},
            {ItemType.PlatinumKey, New PlatinumKeyDescriptor},
            {ItemType.Potion, New PotionDescriptor},
            {ItemType.Pr0n, New Pr0nDescriptor},
            {ItemType.RatTail, New TrophyDescriptor("Rat Tail", 1, MakeList(ShoppeType.BlackMage))},
            {ItemType.RingOfHP, New RingOfHPDescriptor},
            {ItemType.RottenEgg, New RottenEggDescriptor},
            {ItemType.RottenFood, New RottenFoodDescriptor},
            {ItemType.SilverKey, New SilverKeyDescriptor},
            {ItemType.Shield, New ShieldDescriptor},
            {ItemType.ShoeLaces, New TrophyDescriptor("Shoe Laces", 0)},
            {ItemType.Shortsword, New ShortswordDescriptor},
            {ItemType.SkullFragment, New TrophyDescriptor("Skull Fragment", 5, MakeList(ShoppeType.BlackMage))},
            {ItemType.SnakeFang, New TrophyDescriptor("Snake Fang", 3, MakeList(ShoppeType.BlackMage))},
            {ItemType.SpaceSord, New SpaceSordDescriptor},
            {ItemType.TownPortal, New TownPortalDescriptor},
            {ItemType.Trousers, New TrousersDescriptor},
            {ItemType.WaterShard, New WaterShardDescriptor},
            {ItemType.ZombieTaint, New TrophyDescriptor("Zombie Taint", 5, MakeList(ShoppeType.BlackMage))}
        }
    Public ReadOnly Property AllItemTypes As IEnumerable(Of ItemType)
        Get
            Return ItemTypeDescriptors.Keys
        End Get
    End Property
End Module
