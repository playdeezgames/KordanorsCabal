Imports System.Runtime.CompilerServices

Public Enum OldItemType
    None
    IronKey
    CopperKey
End Enum
Public Module ItemTypeExtensions
    <Extension>
    Function ToNew(itemType As OldItemType, worldData As IWorldData) As IItemType
        Return Game.ItemType.FromId(worldData, itemType)
    End Function
End Module
