Friend Class PlateMailDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            ItemType.PlateMail,
            "Platemail",
            40,
            MakeDictionary(
                (OldDungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd)),
                (OldDungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level5, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon))),
                MakeDictionary((OldDungeonLevel.Level2, "1d6")),
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
