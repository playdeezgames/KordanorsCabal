Friend Class DaggerDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Dagger,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)),,
            2,
            1,,
            10,
            1,
            MakeList(ShoppeType.Blacksmith),
            5,
            MakeList(ShoppeType.Blacksmith),
            2,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
