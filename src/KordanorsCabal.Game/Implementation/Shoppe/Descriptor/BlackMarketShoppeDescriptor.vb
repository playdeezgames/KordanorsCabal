Friend Class BlackMarketShoppeDescriptor
    Inherits ShoppeType

    Sub New(worldData As IWorldData)
        MyBase.New(worldData, OldShoppeType.BlackMarket)
    End Sub
End Class
