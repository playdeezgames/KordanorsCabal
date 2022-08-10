Friend Class ShieldDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Shield,
            "Shield",
            5,
            MakeDictionary(
                (1L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon)),
                (2L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon)),
                (3L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon)),
                (4L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon)),
                (5L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon))),
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
