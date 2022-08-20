Friend Class HelmetDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Helmet,
            "Helmet",
            2,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 4L), LocationType.FromId(StaticWorldData.World, 5L))),
                (2L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 4L), LocationType.FromId(StaticWorldData.World, 5L))),
                (3L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 4L), LocationType.FromId(StaticWorldData.World, 5L))),
                (4L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 4L), LocationType.FromId(StaticWorldData.World, 5L))),
                (5L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 4L), LocationType.FromId(StaticWorldData.World, 5L)))),
                MakeDictionary((1L, "3d6")),
                MakeList(EquipSlot.FromId(3L)),,,,
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
