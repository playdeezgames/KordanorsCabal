﻿Friend Class InnkeeperShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New()
        MyBase.New("The Inn")
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return AllItemTypes.Where(
                Function(x) x.HasPrice(ShoppeType.BlackMage)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Price)
            'Return New Dictionary(Of ItemType, Long) From
            '    {
            '        {ItemType.Beer, 5},
            '        {ItemType.Food, 2}
            '    }
        End Get
    End Property
End Class
