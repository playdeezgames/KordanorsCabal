Friend Class ShieldDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Shield,
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
