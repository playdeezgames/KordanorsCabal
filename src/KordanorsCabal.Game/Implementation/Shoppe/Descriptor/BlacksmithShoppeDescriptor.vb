Friend Class BlacksmithShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New()
        MyBase.New(
            "Blacksmith",
            AllItemTypes.Where(
                Function(x) x.HasOffer(ShoppeType.Blacksmith)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(StaticWorldData.World).Offer))
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of OldItemType, Long)
        Get
            Return AllItemTypes.Where(
                Function(x) x.HasPrice(ShoppeType.Blacksmith)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(StaticWorldData.World).Price)
        End Get
    End Property

    Public Overrides ReadOnly Property Repairs As IReadOnlyDictionary(Of OldItemType, Long)
        Get
            Return AllItemTypes.Where(
                Function(x) x.CanRepair(ShoppeType.Blacksmith)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(StaticWorldData.World).RepairPrice)
        End Get
    End Property
End Class
