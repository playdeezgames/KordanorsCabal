Friend Class ShieldDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Shield,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
