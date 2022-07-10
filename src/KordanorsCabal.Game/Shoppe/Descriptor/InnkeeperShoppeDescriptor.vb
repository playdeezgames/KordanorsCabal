Friend Class InnkeeperShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "The Inn"
        End Get
    End Property

    Public Overrides ReadOnly Property Offers As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long)
        End Get
    End Property

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.Beer, 5},
                    {ItemType.Food, 2}
                }
        End Get
    End Property
End Class
