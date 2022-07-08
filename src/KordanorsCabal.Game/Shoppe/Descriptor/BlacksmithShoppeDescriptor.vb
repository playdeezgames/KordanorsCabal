Friend Class BlacksmithShoppeDescriptor
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

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.ChainMail, 50},
                    {ItemType.Dagger, 5},
                    {ItemType.Helmet, 10},
                    {ItemType.Shield, 15}
                }
        End Get
    End Property
End Class
