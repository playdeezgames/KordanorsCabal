Friend Class BrodeSodeDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.BrodeSode,
            "BrodeSode",
            10,
            MakeDictionary(
                (DungeonLevel.Level1, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level2, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level3, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level4, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level5, MakeHashSet(LocationType.Dungeon))),
            MakeDictionary(
                (DungeonLevel.Level2, "2d6"),
                (DungeonLevel.Level3, "1d6")),
                MakeList(EquipSlotDescriptor.FromName(Weapon)),,
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
