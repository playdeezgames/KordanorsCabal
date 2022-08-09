Friend Class LotionDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Lotion,
            "Lotion",
            2,,,,,,,,
            5,,,,
            25,
            MakeList(ShoppeType.BlackMarket))
    End Sub
End Class
