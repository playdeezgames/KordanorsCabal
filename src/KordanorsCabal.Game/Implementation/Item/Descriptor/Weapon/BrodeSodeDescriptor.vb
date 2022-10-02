Friend Class BrodeSodeDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.BrodeSode,
            MakeList(ShoppeType.Blacksmith),
            MakeList(ShoppeType.Blacksmith),
            40,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
