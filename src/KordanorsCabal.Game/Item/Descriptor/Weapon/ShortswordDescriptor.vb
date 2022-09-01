Friend Class ShortswordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Shortsword,
            5,
            MakeDictionary(
                (1L, "2d6"),
                (2L, "1d6")),
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)),,
            4,
            2,,
            20,
            5,
            MakeList(ShoppeType.Blacksmith),
            25,
            MakeList(ShoppeType.Blacksmith),
            10,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
