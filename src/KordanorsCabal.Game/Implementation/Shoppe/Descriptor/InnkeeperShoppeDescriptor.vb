Friend Class InnkeeperShoppeDescriptor
    Inherits ShoppeType

    Sub New(worldData As IWorldData)
        MyBase.New(worldData, OldShoppeType.InnKeeper)
    End Sub
End Class
