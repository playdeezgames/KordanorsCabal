Friend Class ChainMailDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.ChainMail,
            "Chainmail",
            20,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromName(DungeonDeadEnd))),
                (2L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon))),
                (3L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon))),
                (4L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon))),
                (5L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon)))),
            MakeDictionary((1L, "1d6")),
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
