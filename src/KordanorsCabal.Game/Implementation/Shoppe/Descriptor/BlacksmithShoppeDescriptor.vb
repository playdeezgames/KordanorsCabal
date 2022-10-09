Friend Class BlacksmithShoppeDescriptor
    Inherits ShoppeType

    Sub New(worldData As IWorldData)
        MyBase.New(
            worldData,
            OldShoppeType.Blacksmith)
    End Sub
End Class
