Friend Class UsedMetalShoppeDescriptor
    Inherits ShoppeTypeDescriptor
    Public Overrides ReadOnly Property Name As String
        Get
            Return "Blacksmith"
        End Get
    End Property

    Public Overrides ReadOnly Property Offers As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.ChainMail, 10},
                    {ItemType.Dagger, 1},
                    {ItemType.Helmet, 2},
                    {ItemType.Shield, 3}
                }
        End Get
    End Property
End Class
