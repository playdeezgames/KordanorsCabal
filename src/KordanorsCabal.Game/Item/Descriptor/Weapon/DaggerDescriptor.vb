Friend Class DaggerDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Dagger,
            "Dagger",
            1,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromName(Dungeon))),
                (2L, MakeHashSet(LocationType.FromName(Dungeon))),
                (3L, MakeHashSet(LocationType.FromName(Dungeon))),
                (4L, MakeHashSet(LocationType.FromName(Dungeon))),
                (5L, MakeHashSet(LocationType.FromName(Dungeon)))),
            MakeDictionary(
                (1L, "4d6"),
                (2L, "2d6")),
            MakeList(EquipSlot.FromName(Weapon)),,
            2,
            1,,
            10,,
            1,
            MakeList(ShoppeType.Blacksmith),
            5,
            MakeList(ShoppeType.Blacksmith),
            2,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
