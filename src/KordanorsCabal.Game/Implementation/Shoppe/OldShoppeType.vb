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
