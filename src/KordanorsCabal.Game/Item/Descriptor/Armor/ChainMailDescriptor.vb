Friend Class ChainMailDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.ChainMail,
            "Chainmail",
            20,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L))),
                (2L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L))),
                (3L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L))),
                (4L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L))),
                (5L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L)))),
            MakeDictionary((1L, "1d6")),
            MakeList(EquipSlot.FromId(StaticWorldData.World, 4L)),,,,
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
