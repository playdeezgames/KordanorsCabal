Friend Class BrodeSodeDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.BrodeSode,
            10,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)),,
            6,
            3,,
            40,
            20,
            MakeList(ShoppeType.Blacksmith),
            100,
            MakeList(ShoppeType.Blacksmith),
            40,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
