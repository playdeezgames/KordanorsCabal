Friend Class ChainMailDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.ChainMail,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
