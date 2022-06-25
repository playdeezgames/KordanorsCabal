Imports System.Runtime.CompilerServices

Public Enum ItemType
    None
    IronKey
    CopperKey
    SilverKey
    GoldKey
    PlatinumKey
End Enum
Module ItemTypeExtensions
    <Extension>
    Function SpawnLocationTypes(itemType As ItemType) As HashSet(Of LocationType)
        Return ItemTypeDescriptors(itemType).SpawnLocationTypes
    End Function
End Module
