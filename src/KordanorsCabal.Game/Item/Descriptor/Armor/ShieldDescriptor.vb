Friend Class ShieldDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Shield,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 2L)),,,,
            2,
            10,
            3,
            MakeList(ShoppeType.Blacksmith),
            15,
            MakeList(ShoppeType.Blacksmith),
            6,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
