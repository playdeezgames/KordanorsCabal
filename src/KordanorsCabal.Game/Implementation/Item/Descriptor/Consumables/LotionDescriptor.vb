Friend Class LotionDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Lotion,,
            MakeList(ShoppeType.BlackMarket))
    End Sub
End Class
