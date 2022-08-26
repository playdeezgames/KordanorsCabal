Friend Class PlateMailDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.PlateMail,
            40,
            MakeDictionary(
                (2L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L))),
                (3L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L))),
                (4L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L))),
                (5L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L)))),
                MakeDictionary((2L, "1d6")),
                MakeList(EquipSlot.FromId(StaticWorldData.World, 4L)),,,,
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
