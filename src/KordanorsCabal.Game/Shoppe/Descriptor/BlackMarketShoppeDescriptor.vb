Friend Class BlackMarketShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New()
        MyBase.New("Black Market")
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.Lotion, 25},
                    {ItemType.Pr0n, 10},
                    {ItemType.Trousers, 100}
                }
        End Get
    End Property
End Class
