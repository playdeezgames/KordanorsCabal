Imports System.Runtime.CompilerServices

Public Enum ShoppeType
    None
    Trophy
End Enum
Public Module ShoppeTypeExtensions
    <Extension>
    Function Name(shoppeType As ShoppeType) As String
        Return ShoppeTypeDescriptors(shoppeType).Name
    End Function
End Module
