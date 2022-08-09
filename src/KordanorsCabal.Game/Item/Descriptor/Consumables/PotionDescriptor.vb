Friend Class PotionDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Potion,
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
                (DungeonLevel.Level5, "3d6")),,,,,,,,,,
            15,
            MakeList(ShoppeType.Healer),,,,
            Function(character) True,
            Sub(character)
                Dim healRoll = RNG.RollDice("2d4")
                character.ChangeStatistic(CharacterStatisticType.Wounds, -healRoll)
                character.Inventory.Add(Item.Create(ItemType.Bottle))
                character.EnqueueMessage(
                $"Potion heals up to {healRoll} HP!",
                $"You now have {character.CurrentHP} HP!")
            End Sub)
    End Sub
End Class
