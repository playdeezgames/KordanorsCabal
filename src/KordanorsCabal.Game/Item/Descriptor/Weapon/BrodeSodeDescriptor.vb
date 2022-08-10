Friend Class BrodeSodeDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.BrodeSode,
            "BrodeSode",
            10,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.Dungeon)),
                (2L, MakeHashSet(LocationType.Dungeon)),
                (3L, MakeHashSet(LocationType.Dungeon)),
                (4L, MakeHashSet(LocationType.Dungeon)),
                (5L, MakeHashSet(LocationType.Dungeon))),
            MakeDictionary(
                (OldDungeonLevel.Level2, "2d6"),
                (OldDungeonLevel.Level3, "1d6")),
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
