Friend Class ChainMailDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.ChainMail,
            "Chainmail",
            20,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.DungeonDeadEnd)),
                (2L, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (3L, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (4L, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (5L, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon))),
            MakeDictionary((OldDungeonLevel.Level1, "1d6")),
            MakeList(EquipSlot.FromName(Torso)),,,,
            2,
            25,,
            10,
            MakeList(ShoppeType.Blacksmith),
            50,
            MakeList(ShoppeType.Blacksmith),
            20,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
