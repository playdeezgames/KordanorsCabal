Friend Class ShieldDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Shield,
            "Shield",
            5,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (2L, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (3L, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (4L, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (5L, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon))),
                MakeDictionary((1L, "3d6")),
                MakeList(EquipSlot.FromName(Shield)),,,,
                2,
                10,,
                3,
                MakeList(ShoppeType.Blacksmith),
                15,
                MakeList(ShoppeType.Blacksmith),
                6,
                MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
