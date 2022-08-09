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
                (DungeonLevel.Level5, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon))),
            MakeDictionary(
                (DungeonLevel.Level1, "3d6"),
                (DungeonLevel.Level2, "3d6"),
                (DungeonLevel.Level3, "3d6"),
                (DungeonLevel.Level4, "3d6"),
                (DungeonLevel.Level5, "3d6")))
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
End Class
