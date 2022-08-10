Friend Class ChainMailDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.ChainMail,
            "Chainmail",
            20,
            MakeDictionary(
                (1L, MakeHashSet(OldLocationType.DungeonDeadEnd)),
                (2L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon)),
                (3L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon)),
                (4L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon)),
                (5L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon))),
            MakeDictionary((1L, "1d6")),
            MakeList(EquipSlot.FromName(Torso)),,,,
            2,
            25,,
            10,
            MakeList(ShoppeType.Blacksmith),
            50,
            MakeList(ShoppeType.Blacksmith),
            20,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
