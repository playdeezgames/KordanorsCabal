Friend Class DaggerDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Dagger,
            MakeList(ShoppeType.Blacksmith),
            5,
            MakeList(ShoppeType.Blacksmith),
            2,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
