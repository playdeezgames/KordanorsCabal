Friend Class TrousersDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Trousers,,
            MakeList(ShoppeType.BlackMarket))
    End Sub

End Class
