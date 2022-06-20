Imports System.Runtime.CompilerServices

Public Enum StatisticType
    None
    Strength
    Dexterity
    Influence
    Willpower
    Power
    HP
    MP
    Mana
    Unassigned
End Enum
Public Module StatisticTypeExtensions
    <Extension>
    Friend Function DefaultValue(statisticType As StatisticType) As Long
        Return StatisticTypeDescriptors(statisticType).DefaultValue
    End Function
    <Extension>
    Function Name(statisticType As StatisticType) As String
        Return StatisticTypeDescriptors(statisticType).Name
    End Function
End Module
