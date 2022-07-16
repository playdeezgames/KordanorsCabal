Imports System.Runtime.CompilerServices

Public Enum CharacterStatisticType
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
    UnarmedMaximumDamage
    BaseMaximumDefend
    Wounds
    Stress
    Money
    Fatigue
    XP
    XPGoal
    Drunkenness
    Highness
    Hunger
End Enum
Public Module CharacterStatisticTypeExtensions
    <Extension>
    Friend Function DefaultValue(statisticType As CharacterStatisticType) As Long?
        Return CharacterStatisticTypeDescriptors(statisticType).DefaultValue
    End Function
    <Extension>
    Function Name(statisticType As CharacterStatisticType) As String
        Return CharacterStatisticTypeDescriptors(statisticType).Name
    End Function
    <Extension>
    Friend Function MinimumValue(statisticType As CharacterStatisticType) As Long
        Return CharacterStatisticTypeDescriptors(statisticType).MinimumValue
    End Function
    <Extension>
    Friend Function MaximumValue(statisticType As CharacterStatisticType) As Long
        Return CharacterStatisticTypeDescriptors(statisticType).MaximumValue
    End Function
End Module
