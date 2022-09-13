Friend Class BlackMageShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New()
        MyBase.New(
            "Magic",
            AllItemTypes.Where(
                Function(x) x.HasOffer(ShoppeType.BlackMage)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Offer))
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of OldItemType, Long)
        Get
            Return AllItemTypes.Where(
                Function(x) x.HasPrice(ShoppeType.BlackMage)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Price)
        End Get
    End Property
End Class
