Friend Class BlacksmithShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Blacksmith"
        End Get
    End Property

    Private Shared ReadOnly blacksmithOffers As IReadOnlyDictionary(Of ItemType, Long) =
        New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.BrodeSode, 20},
                    {ItemType.ChainMail, 10},
                    {ItemType.Dagger, 1},
                    {ItemType.Helmet, 2},
                    {ItemType.PlateMail, 50},
                    {ItemType.Shield, 3},
                    {ItemType.Shortsword, 5}
                }

    Public Overrides ReadOnly Property Offers As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return blacksmithOffers
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

    Public Overrides ReadOnly Property Repairs As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.BrodeSode, 40},
                    {ItemType.ChainMail, 20},
                    {ItemType.Dagger, 2},
                    {ItemType.Helmet, 4},
                    {ItemType.PlateMail, 100},
                    {ItemType.Shield, 6},
                    {ItemType.Shortsword, 10}
                }
        End Get
    End Property
End Class
