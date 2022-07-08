Imports System.Runtime.CompilerServices

Public Enum ShoppeType
    None
    Trophy
    UsedMetal
End Enum
Public Module ShoppeTypeExtensions
    <Extension>
    Function Name(shoppeType As ShoppeType) As String
        Return ShoppeTypeDescriptors(shoppeType).Name
    End Function
    <Extension>
    Function Offers(shoppeType As ShoppeType) As IReadOnlyDictionary(Of ItemType, Long)
        Return ShoppeTypeDescriptors(shoppeType).Offers
    End Function
End Module
