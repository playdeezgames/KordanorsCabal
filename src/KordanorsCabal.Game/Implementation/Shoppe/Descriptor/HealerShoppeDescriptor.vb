Friend Class HealerShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New(worldData As IWorldData)
        MyBase.New(worldData, "Healer")
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of IItemType, Long)
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.HasPrice(ShoppeType.Healer)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Price)
        End Get
    End Property
End Class
