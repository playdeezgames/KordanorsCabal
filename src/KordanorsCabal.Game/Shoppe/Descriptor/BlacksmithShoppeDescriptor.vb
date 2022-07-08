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
                    {ItemType.BrodeSode, 20},
                    {ItemType.ChainMail, 10},
                    {ItemType.Dagger, 1},
                    {ItemType.Helmet, 2},
                    {ItemType.PlateMail, 50},
                    {ItemType.Shield, 3},
                    {ItemType.Shortsword, 5}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.BrodeSode, 100},
                    {ItemType.ChainMail, 50},
                    {ItemType.Dagger, 5},
                    {ItemType.Helmet, 10},
                    {ItemType.PlateMail, 250},
                    {ItemType.Shield, 15},
                    {ItemType.Shortsword, 25}
                }
        End Get
    End Property
End Class
