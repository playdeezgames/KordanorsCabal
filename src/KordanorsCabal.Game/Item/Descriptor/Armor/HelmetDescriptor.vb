Friend Class HelmetDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Helmet,
            "Helmet",
            2,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(4L), LocationType.FromId(DungeonDeadEnd))),
                (2L, MakeHashSet(LocationType.FromId(4L), LocationType.FromId(DungeonDeadEnd))),
                (3L, MakeHashSet(LocationType.FromId(4L), LocationType.FromId(DungeonDeadEnd))),
                (4L, MakeHashSet(LocationType.FromId(4L), LocationType.FromId(DungeonDeadEnd))),
                (5L, MakeHashSet(LocationType.FromId(4L), LocationType.FromId(DungeonDeadEnd)))),
                MakeDictionary((1L, "3d6")),
                MakeList(EquipSlot.FromId(3L)),,,,
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
