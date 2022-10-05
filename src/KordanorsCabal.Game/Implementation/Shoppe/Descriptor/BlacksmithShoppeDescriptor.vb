Friend Class BlacksmithShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New(worldData As IWorldData)
        MyBase.New(
            worldData,
            "Blacksmith",
            AllItemTypes(worldData).Where(
                Function(x) x.ToNew(worldData).HasOffer(ShoppeType.Blacksmith)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(worldData).Offer))
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of OldItemType, Long)
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.ToNew(WorldData).HasPrice(ShoppeType.Blacksmith)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(WorldData).Price)
        End Get
    End Property

    Public Overrides ReadOnly Property Repairs As IReadOnlyDictionary(Of OldItemType, Long)
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.ToNew(WorldData).CanRepair(ShoppeType.Blacksmith)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(WorldData).RepairPrice)
        End Get
    End Property
End Class
