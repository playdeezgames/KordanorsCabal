Friend Class PlateMailDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            ItemType.PlateMail,
            "Platemail",
            40,
            MakeDictionary(
                (2L, MakeHashSet(LocationType.FromId(DungeonDeadEnd))),
                (3L, MakeHashSet(LocationType.FromId(DungeonDeadEnd), LocationType.FromId(Dungeon))),
                (4L, MakeHashSet(LocationType.FromId(DungeonDeadEnd), LocationType.FromId(Dungeon))),
                (5L, MakeHashSet(LocationType.FromId(DungeonDeadEnd), LocationType.FromId(Dungeon)))),
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
