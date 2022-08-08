Friend Class PotionDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Potion",
            2,
            MakeDictionary(
                (DungeonLevel.Level1, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (DungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (DungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (DungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (DungeonLevel.Level5, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon))))
    End Sub

    Public Overrides ReadOnly Property PurchasePrice As Long?
        Get
            Return 25
        End Get
    End Property

    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        Dim healRoll = RNG.RollDice("2d4")
        character.ChangeStatistic(CharacterStatisticType.Wounds, -healRoll)
        character.Inventory.Add(Item.Create(ItemType.Bottle))
        character.EnqueueMessage(
                $"Potion heals up to {healRoll} HP!",
                $"You now have {character.CurrentHP} HP!")
    End Sub

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Return If(level = DungeonLevel.Moon, 0, RNG.RollDice("3d6"))
    End Function
End Class
