Friend Class HelmetDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Helmet,
            MakeList(ShoppeType.Blacksmith),
            MakeList(ShoppeType.Blacksmith),
            4,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
