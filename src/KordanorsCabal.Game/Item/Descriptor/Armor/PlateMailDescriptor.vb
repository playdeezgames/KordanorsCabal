Friend Class PlateMailDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            ItemType.PlateMail,
            "Platemail",
            40,
            MakeDictionary(
                (2L, MakeHashSet(LocationType.FromName(DungeonDeadEnd))),
                (3L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon))),
                (4L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon))),
                (5L, MakeHashSet(LocationType.FromName(DungeonDeadEnd), LocationType.FromName(Dungeon)))),
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
