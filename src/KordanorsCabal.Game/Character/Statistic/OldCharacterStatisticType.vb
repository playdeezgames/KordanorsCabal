Imports System.Runtime.CompilerServices

Public Enum OldCharacterStatisticType
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
    FoodPoisoning
    Chafing
    Immobilization
End Enum
Public Module CharacterStatisticTypeExtensions
    <Extension>
    Friend Function DefaultValue(statisticType As OldCharacterStatisticType) As Long?
        Return CharacterStatisticTypeDescriptors(statisticType).DefaultValue
    End Function
    <Extension>
    Function Name(statisticType As OldCharacterStatisticType) As String
        Return CharacterStatisticTypeDescriptors(statisticType).Name
    End Function
    <Extension>
    Function Abbreviation(statisticType As OldCharacterStatisticType) As String
        Return CharacterStatisticTypeDescriptors(statisticType).Abbreviation
    End Function
    <Extension>
    Friend Function MinimumValue(statisticType As OldCharacterStatisticType) As Long
        Return CharacterStatisticTypeDescriptors(statisticType).MinimumValue
    End Function
    <Extension>
    Friend Function MaximumValue(statisticType As OldCharacterStatisticType) As Long
        Return CharacterStatisticTypeDescriptors(statisticType).MaximumValue
    End Function
End Module
