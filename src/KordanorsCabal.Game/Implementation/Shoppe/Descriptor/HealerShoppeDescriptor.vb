Friend Class HealerShoppeDescriptor
    Inherits ShoppeType

    Sub New(worldData As IWorldData)
        MyBase.New(worldData, OldShoppeType.Healer)
    End Sub
End Class
