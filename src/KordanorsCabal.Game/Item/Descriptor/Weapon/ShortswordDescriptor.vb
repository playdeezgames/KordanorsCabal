Friend Class ShortswordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Shortsword,
            "Shortsword",
                   5,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(4L))),
                (2L, MakeHashSet(LocationType.FromId(4L))),
                (3L, MakeHashSet(LocationType.FromId(4L))),
                (4L, MakeHashSet(LocationType.FromId(4L))),
                (5L, MakeHashSet(LocationType.FromId(4L)))),
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
