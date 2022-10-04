Friend Class BlackMageShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New()
        MyBase.New(
            "Magic",
            AllItemTypes.Where(
                Function(x) x.ToNew(StaticWorldData.World).HasOffer(ShoppeType.BlackMage)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(StaticWorldData.World).Offer))
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of OldItemType, Long)
        Get
            Return AllItemTypes.Where(
                Function(x) x.ToNew(StaticWorldData.World).HasPrice(ShoppeType.BlackMage)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(StaticWorldData.World).Price)
        End Get
    End Property
End Class
