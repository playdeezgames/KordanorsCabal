Friend Class HelmetDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Helmet,
            2,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 3L)),,,,
            2,
            10,
            2,
            MakeList(ShoppeType.Blacksmith),
            10,
            MakeList(ShoppeType.Blacksmith),
            4,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
