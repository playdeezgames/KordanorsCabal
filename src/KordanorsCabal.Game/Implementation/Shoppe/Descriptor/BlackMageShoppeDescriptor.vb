Friend Class BlackMageShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New(worldData As IWorldData)
        MyBase.New(
            worldData,
            "Magic",
            AllItemTypes(worldData).Where(
                Function(x) x.HasOffer(ShoppeType.BlackMage)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Offer))
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of IItemType, Long)
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.HasPrice(ShoppeType.BlackMage)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Price)
        End Get
    End Property
End Class
