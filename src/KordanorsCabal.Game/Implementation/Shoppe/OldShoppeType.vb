Imports System.Runtime.CompilerServices

Public Enum OldShoppeType
    None
    BlackMage
    Blacksmith
    InnKeeper
    Healer
    BlackMarket
End Enum
Public Module ShoppeTypeExtensions
    <Extension>
    Function Name(shoppeType As OldShoppeType) As String
        Return AllShoppeTypes(shoppeType).Name
    End Function
    <Extension>
    Function Offers(shoppeType As OldShoppeType) As IReadOnlyDictionary(Of IItemType, Long)
        Return AllShoppeTypes(shoppeType).Offers
    End Function
    <Extension>
    Function BuyPrice(shoppeType As OldShoppeType, itemType As IItemType) As Long?
        Dim value As Long
        If AllShoppeTypes(shoppeType).Offers.ToDictionary(Function(x) x.Key.Id, Function(x) x.Value).TryGetValue(itemType.Id, value) Then
            Return value
        End If
        Return Nothing
    End Function
    <Extension>
    Function WillBuy(shoppeType As OldShoppeType, itemType As IItemType) As Boolean
        Return shoppeType.BuyPrice(itemType).HasValue
    End Function
    <Extension>
    Function Prices(shoppeType As OldShoppeType) As IReadOnlyDictionary(Of IItemType, Long)
        Return AllShoppeTypes(shoppeType).Prices
    End Function
    <Extension>
    Function Repairs(shoppeType As OldShoppeType) As IReadOnlyDictionary(Of IItemType, Long)
        Return AllShoppeTypes(shoppeType).Repairs
    End Function
    <Extension>
    Function RepairPrice(shoppeType As OldShoppeType, itemType As IItemType) As Long?
        Dim value As Long
        If AllShoppeTypes(shoppeType).Repairs.ToDictionary(Function(x) x.Key.Id, Function(x) x.Value).TryGetValue(itemType.Id, value) Then
            Return value
        End If
        Return Nothing
    End Function
    <Extension>
    Function WillRepair(shoppeType As OldShoppeType, itemType As IItemType) As Boolean
        Return shoppeType.RepairPrice(itemType).HasValue
    End Function
End Module
