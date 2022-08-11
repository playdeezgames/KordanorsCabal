﻿Imports System.Runtime.CompilerServices

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
    Public Const Strength = "Strength"
    Public Const Dexterity = "Dexterity"
    Public Const Influence = "Influence"
    Public Const Willpower = "Willpower"
    Public Const Power = "Power"
    Public Const HP = "HP"
    Public Const MP = "MP"
    Public Const Mana = "Mana"
    Public Const Unassigned = "Unassigned"
    Public Const UnarmedMaximumDamage = "Unarmed Maximum Damage"
    Public Const BaseMaximumDefend = "Base Maximum Defend"
    Public Const Wounds = "Wounds"
    Public Const Stress = "Stress"
    Public Const Money = "Money"
    Public Const Fatigue = "Fatigue"
    Public Const XP = "XP"
    Public Const XPGoal = "XP Goal"
    Public Const Drunkenness = "Drunkenness"
    Public Const Highness = "Highness"
    Public Const Hunger = "Hunger"
    Public Const FoodPoisoning = "Food Poisoning"
    Public Const Chafing = "Chafing"
    Public Const Immobilization = "Immobilization"
End Module
