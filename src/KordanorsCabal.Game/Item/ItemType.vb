Imports System.Runtime.CompilerServices

Public Enum ItemType
    None
    IronKey
    CopperKey
    SilverKey
    GoldKey
    PlatinumKey
    ElementalOrb
    Potion
End Enum
Public Module ItemTypeExtensions
    <Extension>
    Function SpawnLocationTypes(itemType As ItemType) As HashSet(Of LocationType)
        Return ItemTypeDescriptors(itemType).SpawnLocationTypes
    End Function

    <Extension>
    Function Name(itemType As ItemType) As String
        Return ItemTypeDescriptors(itemType).Name
    End Function
    <Extension>
    Function Encumbrance(itemType As ItemType) As Single
        Return ItemTypeDescriptors(itemType).Encumbrance
    End Function
    <Extension>
    Public Function PurchasePrice(itemType As ItemType) As Long?
        Return ItemTypeDescriptors(itemType).PurchasePrice
    End Function
End Module
