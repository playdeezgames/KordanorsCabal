Imports System.Runtime.CompilerServices
Imports SQLitePCL

Public Enum ShoppeType
    None
    BlackMage
    Blacksmith
    InnKeeper
    Healer
    BlackMarket
End Enum
Public Module ShoppeTypeExtensions
    <Extension>
    Function Name(shoppeType As ShoppeType) As String
        Return ShoppeTypeDescriptors(shoppeType).Name
    End Function
    <Extension>
    Function Offers(shoppeType As ShoppeType) As IReadOnlyDictionary(Of IItemType, Long)
        Return ShoppeTypeDescriptors(shoppeType).Offers
    End Function
    <Extension>
    Function BuyPrice(shoppeType As ShoppeType, itemType As IItemType) As Long?
        Dim value As Long
        If ShoppeTypeDescriptors(shoppeType).Offers.ToDictionary(Function(x) x.Key.Id, Function(x) x.Value).TryGetValue(itemType.Id, value) Then
            Return value
        End If
        Return Nothing
    End Function
    <Extension>
    Function WillBuy(shoppeType As ShoppeType, itemType As IItemType) As Boolean
        Return shoppeType.BuyPrice(itemType).HasValue
    End Function
    <Extension>
    Function Prices(shoppeType As ShoppeType) As IReadOnlyDictionary(Of IItemType, Long)
        Return ShoppeTypeDescriptors(shoppeType).Prices
    End Function
    <Extension>
    Function Repairs(shoppeType As ShoppeType) As IReadOnlyDictionary(Of IItemType, Long)
        Return ShoppeTypeDescriptors(shoppeType).Repairs
    End Function
    <Extension>
    Function RepairPrice(shoppeType As ShoppeType, itemType As IItemType) As Long?
        Dim value As Long
        If ShoppeTypeDescriptors(shoppeType).Repairs.ToDictionary(Function(x) x.Key.Id, Function(x) x.Value).TryGetValue(itemType.Id, value) Then
            Return value
        End If
        Return Nothing
    End Function
    <Extension>
    Function WillRepair(shoppeType As ShoppeType, itemType As IItemType) As Boolean
        Return shoppeType.RepairPrice(itemType).HasValue
    End Function
End Module
