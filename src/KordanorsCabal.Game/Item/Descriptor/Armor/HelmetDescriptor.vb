Friend Class HelmetDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Helmet,
            "Helmet",
            2,
            MakeDictionary(
                (DungeonLevel.Level1, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (DungeonLevel.Level2, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (DungeonLevel.Level3, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (DungeonLevel.Level4, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (DungeonLevel.Level5, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd))),
                MakeDictionary((DungeonLevel.Level1, "3d6")),
                MakeList(EquipSlot.Head.ToDescriptor),,,,
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
