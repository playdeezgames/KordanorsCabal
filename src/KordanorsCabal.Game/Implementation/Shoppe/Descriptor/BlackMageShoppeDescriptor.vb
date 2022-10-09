Friend Class BlackMageShoppeDescriptor
    Inherits ShoppeType

    Sub New(worldData As IWorldData)
        MyBase.New(
            worldData,
            OldShoppeType.BlackMage)
    End Sub
End Class
