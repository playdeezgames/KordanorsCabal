﻿Friend Class BlackMarketShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Black Market"
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
                    {ItemType.Lotion, 25},
                    {ItemType.Pr0n, 10},
                    {ItemType.Trousers, 100}
                }
        End Get
    End Property
End Class
