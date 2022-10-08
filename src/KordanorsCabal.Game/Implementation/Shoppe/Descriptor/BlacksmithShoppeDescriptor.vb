Friend Class BlacksmithShoppeDescriptor
    Inherits ShoppeType

    Sub New(worldData As IWorldData)
        MyBase.New(
            worldData,
            OldShoppeType.Blacksmith,
            "Blacksmith",
            AllItemTypes(worldData).Where(
                Function(x) x.HasOffer(OldShoppeType.Blacksmith)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Offer))
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of IItemType, Long)
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.HasPrice(OldShoppeType.Blacksmith)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Price)
        End Get
    End Property

    Public Overrides ReadOnly Property Repairs As IReadOnlyDictionary(Of IItemType, Long)
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.CanRepair(OldShoppeType.Blacksmith)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.RepairPrice)
        End Get
    End Property
End Class
