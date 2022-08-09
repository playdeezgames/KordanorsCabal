Friend Class InnkeeperShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New()
        MyBase.New("The Inn")
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return AllItemTypes.Where(
                Function(x) x.HasPrice(ShoppeType.InnKeeper)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Price)
        End Get
    End Property
End Class
