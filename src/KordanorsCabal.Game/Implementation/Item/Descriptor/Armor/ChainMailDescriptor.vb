Friend Class ChainMailDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.ChainMail,,
            10,
            MakeList(ShoppeType.Blacksmith),
            50,
            MakeList(ShoppeType.Blacksmith),
            20,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
