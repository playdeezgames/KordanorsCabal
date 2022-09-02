Friend Class PlateMailDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.PlateMail,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 4L)),,,,
            4,
            50,
            50,
            MakeList(ShoppeType.Blacksmith),
            250,
            MakeList(ShoppeType.Blacksmith),
            100,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
