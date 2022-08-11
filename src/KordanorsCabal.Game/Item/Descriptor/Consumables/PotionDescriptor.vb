Friend Class PotionDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Potion,
            "Potion",
            2,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon))),
                (2L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon))),
                (3L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon))),
                (4L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon))),
                (5L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon)))),
            MakeDictionary(
                (1L, "3d6"),
                (2L, "3d6"),
                (3L, "3d6"),
                (4L, "3d6"),
                (5L, "3d6")),,,,,,,,,,
            15,
            MakeList(ShoppeType.Healer),,,,
            Function(character) True,
            Sub(character)
                Dim healRoll = RNG.RollDice("2d4")
                character.ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Wounds), -healRoll)
                character.Inventory.Add(Item.Create(ItemType.Bottle))
                character.EnqueueMessage(
                $"Potion heals up to {healRoll} HP!",
                $"You now have {character.CurrentHP} HP!")
            End Sub)
    End Sub
End Class
