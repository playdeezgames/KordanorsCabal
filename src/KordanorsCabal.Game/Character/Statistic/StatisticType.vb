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
Friend Module StatisticTypeExtensions
    <Extension>
    Function DefaultValue(statisticType As StatisticType) As Long
        Return StatisticTypeDescriptors(statisticType).DefaultValue
    End Function
End Module
