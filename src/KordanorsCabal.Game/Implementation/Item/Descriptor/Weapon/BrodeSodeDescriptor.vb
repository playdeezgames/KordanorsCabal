Friend Class BrodeSodeDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.BrodeSode,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
