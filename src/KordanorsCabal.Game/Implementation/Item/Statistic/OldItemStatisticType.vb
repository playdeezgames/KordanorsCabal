Imports System.Runtime.CompilerServices

Public Enum OldItemStatisticType
    None
    Wear
End Enum
Module ItemStatisticTypeExtensions
    <Extension>
    Function DefaultValue(statisticType As OldItemStatisticType) As Long
        Return ItemStatisticTypeDescriptors(statisticType).DefaultValue
    End Function
End Module