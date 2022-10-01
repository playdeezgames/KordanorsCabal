Friend Class TrousersDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Trousers,,,,
            100,
            MakeList(ShoppeType.BlackMarket))
    End Sub

End Class
