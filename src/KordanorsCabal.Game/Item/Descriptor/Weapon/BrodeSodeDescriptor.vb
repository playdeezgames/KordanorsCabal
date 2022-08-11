Friend Class BrodeSodeDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.BrodeSode,
            "BrodeSode",
            10,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromName(Dungeon))),
                (2L, MakeHashSet(LocationType.FromName(Dungeon))),
                (3L, MakeHashSet(LocationType.FromName(Dungeon))),
                (4L, MakeHashSet(LocationType.FromName(Dungeon))),
                (5L, MakeHashSet(LocationType.FromName(Dungeon)))),
            MakeDictionary(
                (2L, "2d6"),
                (3L, "1d6")),
                MakeList(EquipSlot.FromName(Weapon)),,
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
