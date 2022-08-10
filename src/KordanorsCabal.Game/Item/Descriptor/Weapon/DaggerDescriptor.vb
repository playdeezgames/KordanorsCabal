Friend Class DaggerDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Dagger,
            "Dagger",
            1,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.Dungeon)),
                (2L, MakeHashSet(LocationType.Dungeon)),
                (3L, MakeHashSet(LocationType.Dungeon)),
                (4L, MakeHashSet(LocationType.Dungeon)),
                (5L, MakeHashSet(LocationType.Dungeon))),
            MakeDictionary(
                (OldDungeonLevel.Level1, "4d6"),
                (OldDungeonLevel.Level2, "2d6")),
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
