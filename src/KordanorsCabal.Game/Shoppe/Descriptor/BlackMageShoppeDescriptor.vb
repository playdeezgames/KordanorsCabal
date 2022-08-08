Friend Class BlackMageShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New()
        MyBase.New("Magic")
    End Sub

    Public Overrides ReadOnly Property Offers As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return AllItemTypes.Where(
                Function(x) x.HasOffer(ShoppeType.BlackMage)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Offer)
        End Get
    End Property

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.Bong, 25},
                    {ItemType.BookOfPurify, 50},
                    {ItemType.BookOfHolyBolt, 100},
                    {ItemType.Herb, 5},
                    {ItemType.MoonPortal, 5000},
                    {ItemType.TownPortal, 50}
                }
        End Get
    End Property
End Class
