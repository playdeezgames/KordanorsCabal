Imports System.Runtime.CompilerServices

Public Enum OldItemStatisticType
    None
    Wear
End Enum
Module ItemStatisticTypeExtensions
    <Extension>
    Function DefaultValue(worldData As IWorldData, statisticType As OldItemStatisticType) As Long
        Return ItemStatisticTypeDescriptors(worldData)(statisticType).DefaultValue
    End Function
End Module