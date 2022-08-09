Friend Class PlateMailDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            "Platemail",
            40.0!,
            MakeDictionary(
                (DungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd)),
                (DungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (DungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (DungeonLevel.Level5, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon))),
                MakeDictionary((DungeonLevel.Level2, "1d6")),
                MakeList(EquipSlot.Torso),,,,
                4,
                50,,
                50,
                MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
