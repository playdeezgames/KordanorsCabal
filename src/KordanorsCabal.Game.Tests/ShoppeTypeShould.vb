Public Class ShoppeTypeShould
    Inherits ThingieShould(Of IShoppeType)
    Sub New()
        MyBase.New(AddressOf ShoppeType.FromId)
    End Sub
End Class
