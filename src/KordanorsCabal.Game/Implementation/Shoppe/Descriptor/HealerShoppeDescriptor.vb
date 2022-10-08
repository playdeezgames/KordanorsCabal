Friend Class HealerShoppeDescriptor
    Inherits ShoppeType

    Sub New(worldData As IWorldData)
        MyBase.New(worldData, OldShoppeType.Healer, "Healer")
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of IItemType, Long)
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.HasPrice(OldShoppeType.Healer)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Price)
        End Get
    End Property
End Class
