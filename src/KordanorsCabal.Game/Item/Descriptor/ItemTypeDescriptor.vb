Public MustInherit Class ItemTypeDescriptor
    MustOverride ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
    MustOverride ReadOnly Property Name As String
    Overridable ReadOnly Property Encumbrance As Single
        Get
            Return 0!
        End Get
    End Property
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

    Overridable Function RollSpawnCount(level As DungeonLevel) As Long
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
End Class
Public Module ItemTypeDescriptorUtility
    Friend ReadOnly ItemTypeDescriptors As IReadOnlyDictionary(Of ItemType, ItemTypeDescriptor) =
        New Dictionary(Of ItemType, ItemTypeDescriptor) From
        {
            {ItemType.AirShard, New AirShardDescriptor},
            {ItemType.AmuletOfHP, New AmuletOfHPDescriptor},
            {ItemType.BatWing, New TrophyDescriptor("Bat Wing")},
            {ItemType.Beer, New BeerDescriptor},
            {ItemType.Bong, New BongDescriptor},
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
            {ItemType.GoblinEar, New TrophyDescriptor("Goblin Ear")},
            {ItemType.GoldKey, New GoldKeyDescriptor},
            {ItemType.Helmet, New HelmetDescriptor},
            {ItemType.Herb, New HerbDescriptor},
            {ItemType.HornsOfKordanor, New HornsOfKordanorDescriptor},
            {ItemType.HolyWater, New HolyWaterDescriptor},
            {ItemType.IronKey, New IronKeyDescriptor},
            {ItemType.Lotion, New LotionDescriptor},
            {ItemType.MagicEgg, New MagicEggDescriptor},
            {ItemType.MembershipCard, New TrophyDescriptor("Membership Card")},
            {ItemType.MoonPortal, New MoonPortalDescriptor},
            {ItemType.Mushroom, New TrophyDescriptor("Mushroom")},
            {ItemType.PlateMail, New PlateMailDescriptor},
            {ItemType.PlatinumKey, New PlatinumKeyDescriptor},
            {ItemType.Potion, New PotionDescriptor},
            {ItemType.Pr0n, New Pr0nDescriptor},
            {ItemType.RatTail, New TrophyDescriptor("Rat Tail")},
            {ItemType.RingOfHP, New RingOfHPDescriptor},
            {ItemType.RottenEgg, New RottenEggDescriptor},
            {ItemType.RottenFood, New RottenFoodDescriptor},
            {ItemType.SilverKey, New SilverKeyDescriptor},
            {ItemType.Shield, New ShieldDescriptor},
            {ItemType.ShoeLaces, New TrophyDescriptor("Shoe Laces")},
            {ItemType.Shortsword, New ShortswordDescriptor},
            {ItemType.SkullFragment, New TrophyDescriptor("Skull Fragment")},
            {ItemType.SnakeFang, New TrophyDescriptor("Snake Fang")},
            {ItemType.SpaceSord, New SpaceSordDescriptor},
            {ItemType.TownPortal, New TownPortalDescriptor},
            {ItemType.Trousers, New TrousersDescriptor},
            {ItemType.WaterShard, New WaterShardDescriptor},
            {ItemType.ZombieTaint, New TrophyDescriptor("Zombie Taint")}
        }
    Public ReadOnly Property AllItemTypes As IEnumerable(Of ItemType)
        Get
            Return ItemTypeDescriptors.Keys
        End Get
    End Property
End Module
