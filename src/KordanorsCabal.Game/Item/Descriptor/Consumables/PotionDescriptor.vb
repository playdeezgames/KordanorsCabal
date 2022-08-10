Friend Class PotionDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Potion,
            "Potion",
            2,
            MakeDictionary(
                (OldDungeonLevel.Level1, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level5, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon))),
            MakeDictionary(
                (OldDungeonLevel.Level1, "3d6"),
                (OldDungeonLevel.Level2, "3d6"),
                (OldDungeonLevel.Level3, "3d6"),
                (OldDungeonLevel.Level4, "3d6"),
                (OldDungeonLevel.Level5, "3d6")),,,,,,,,,,
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
