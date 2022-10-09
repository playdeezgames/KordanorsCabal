Imports System.Runtime.CompilerServices

Public Interface IShoppeType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property Offers As IReadOnlyDictionary(Of IItemType, Long)
    ReadOnly Property Prices As IReadOnlyDictionary(Of IItemType, Long)
    ReadOnly Property Repairs As IReadOnlyDictionary(Of IItemType, Long)
    Function BuyPrice(itemType As IItemType) As Long?
    'Function WillBuy(shoppeType As OldShoppeType, itemType As IItemType) As Boolean
    'Function RepairPrice(shoppeType As OldShoppeType, itemType As IItemType) As Long?
    'Function WillRepair(shoppeType As OldShoppeType, itemType As IItemType) As Boolean
End Interface
