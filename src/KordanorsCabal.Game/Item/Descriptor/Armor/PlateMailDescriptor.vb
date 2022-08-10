Friend Class PlateMailDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            ItemType.PlateMail,
            "Platemail",
            40,
            MakeDictionary(
                (2L, MakeHashSet(OldLocationType.DungeonDeadEnd)),
                (3L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon)),
                (4L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon)),
                (5L, MakeHashSet(OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon))),
                MakeDictionary((2L, "1d6")),
                MakeList(EquipSlot.FromName(Torso)),,,,
                4,
                50,,
                50,
                MakeList(ShoppeType.Blacksmith),
                250,
                MakeList(ShoppeType.Blacksmith),
                100,
                MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
