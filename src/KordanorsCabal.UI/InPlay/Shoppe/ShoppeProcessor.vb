﻿MustInherit Class ShoppeProcessor(Of TListItem)
    Inherits BaseProcessor

    Public Shared Property ShoppeType As IShoppeType

    Protected items As List(Of TListItem)
    Protected currentItemIndex As Integer = 0
    Protected Const ListStartRow = 1
    Protected Const ListHiliteRow = 10
    Protected Const ListEndRow = 21
End Class
