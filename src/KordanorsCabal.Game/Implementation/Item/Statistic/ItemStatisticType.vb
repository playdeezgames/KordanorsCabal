Imports System.Runtime.CompilerServices

Public Enum ItemStatisticType
    None
    Wear
End Enum
Module ItemStatisticTypeExtensions
    <Extension>
    Function DefaultValue(statisticType As ItemStatisticType) As Long
        Return ItemStatisticTypeDescriptors(statisticType).DefaultValue
    End Function
End Module