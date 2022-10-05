Friend Class BlackMageShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New(worldData As IWorldData)
        MyBase.New(
            worldData,
            "Magic",
            AllItemTypes(worldData).Where(
                Function(x) x.ToNew(worldData).HasOffer(ShoppeType.BlackMage)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(worldData).Offer))
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of OldItemType, Long)
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.ToNew(WorldData).HasPrice(ShoppeType.BlackMage)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(WorldData).Price)
        End Get
    End Property
End Class
