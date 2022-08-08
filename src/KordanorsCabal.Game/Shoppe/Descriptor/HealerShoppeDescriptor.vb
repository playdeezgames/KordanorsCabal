Friend Class HealerShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New()
        MyBase.New("Healer")
    End Sub

    Public Overrides ReadOnly Property Offers As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long)
        End Get
    End Property

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.HolyWater, 10},
                    {ItemType.Potion, 15}
                }
        End Get
    End Property
End Class
