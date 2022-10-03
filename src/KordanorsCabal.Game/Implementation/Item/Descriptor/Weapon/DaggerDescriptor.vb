Friend Class DaggerDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Dagger,
            MakeList(ShoppeType.Blacksmith),
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
