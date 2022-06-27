Imports System.Runtime.CompilerServices

Public Enum ItemType
    None
    IronKey
    CopperKey
    SilverKey
    GoldKey
    PlatinumKey
    ElementalOrb
End Enum
Module ItemTypeExtensions
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
End Module
