Friend Class ShieldDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Shield,
            "Shield",
            5,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(DungeonDeadEnd), LocationType.FromId(Dungeon))),
                (2L, MakeHashSet(LocationType.FromId(DungeonDeadEnd), LocationType.FromId(Dungeon))),
                (3L, MakeHashSet(LocationType.FromId(DungeonDeadEnd), LocationType.FromId(Dungeon))),
                (4L, MakeHashSet(LocationType.FromId(DungeonDeadEnd), LocationType.FromId(Dungeon))),
                (5L, MakeHashSet(LocationType.FromId(DungeonDeadEnd), LocationType.FromId(Dungeon)))),
                MakeDictionary((1L, "3d6")),
                MakeList(EquipSlot.FromId(2L)),,,,
                2,
                10,,
                3,
                MakeList(ShoppeType.Blacksmith),
                15,
                MakeList(ShoppeType.Blacksmith),
                6,
                MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
