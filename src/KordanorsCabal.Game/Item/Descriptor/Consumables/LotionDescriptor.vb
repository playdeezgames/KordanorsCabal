Friend Class LotionDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Lotion",
            2,,,,,,,,
            5,,,,
            25,
            MakeList(ShoppeType.BlackMarket))
    End Sub
End Class
