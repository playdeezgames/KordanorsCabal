Friend Class BrodeSodeDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.BrodeSode,
            "BrodeSode",
            10,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(4L))),
                (2L, MakeHashSet(LocationType.FromId(4L))),
                (3L, MakeHashSet(LocationType.FromId(4L))),
                (4L, MakeHashSet(LocationType.FromId(4L))),
                (5L, MakeHashSet(LocationType.FromId(4L)))),
            MakeDictionary(
                (2L, "2d6"),
                (3L, "1d6")),
                MakeList(EquipSlot.FromId(1L)),,
                6,
                3,,
                40,,
                20,
                MakeList(ShoppeType.Blacksmith),
                100,
                MakeList(ShoppeType.Blacksmith),
                40,
                MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
