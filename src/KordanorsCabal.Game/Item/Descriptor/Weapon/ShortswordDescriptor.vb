Friend Class ShortswordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Shortsword,
            "Shortsword",
                   5,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(Dungeon))),
                (2L, MakeHashSet(LocationType.FromId(Dungeon))),
                (3L, MakeHashSet(LocationType.FromId(Dungeon))),
                (4L, MakeHashSet(LocationType.FromId(Dungeon))),
                (5L, MakeHashSet(LocationType.FromId(Dungeon)))),
            MakeDictionary(
                (1L, "2d6"),
                (2L, "1d6")),
            MakeList(EquipSlot.FromId(1L)),,
            4,
            2,,
            20,,
            5,
            MakeList(ShoppeType.Blacksmith),
            25,
            MakeList(ShoppeType.Blacksmith),
            10,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
