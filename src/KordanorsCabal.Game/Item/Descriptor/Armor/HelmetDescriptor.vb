Friend Class HelmetDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Helmet,
            "Helmet",
            2,
            MakeDictionary(
                (1L, MakeHashSet(OldLocationType.Dungeon, OldLocationType.DungeonDeadEnd)),
                (2L, MakeHashSet(OldLocationType.Dungeon, OldLocationType.DungeonDeadEnd)),
                (3L, MakeHashSet(OldLocationType.Dungeon, OldLocationType.DungeonDeadEnd)),
                (4L, MakeHashSet(OldLocationType.Dungeon, OldLocationType.DungeonDeadEnd)),
                (5L, MakeHashSet(OldLocationType.Dungeon, OldLocationType.DungeonDeadEnd))),
                MakeDictionary((1L, "3d6")),
                MakeList(EquipSlot.FromName(Head)),,,,
                2,
                10,,
                2,
                MakeList(ShoppeType.Blacksmith),
                10,
                MakeList(ShoppeType.Blacksmith),
                4,
                MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
