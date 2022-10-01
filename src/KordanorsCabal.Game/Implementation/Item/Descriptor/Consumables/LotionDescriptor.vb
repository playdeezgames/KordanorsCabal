Friend Class LotionDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Lotion,,,
            5,,
            25,
            MakeList(ShoppeType.BlackMarket))
    End Sub
End Class
