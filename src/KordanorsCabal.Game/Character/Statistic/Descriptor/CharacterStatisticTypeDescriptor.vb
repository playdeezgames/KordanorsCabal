﻿Public MustInherit Class CharacterStatisticTypeDescriptor
    Overridable ReadOnly Property DefaultValue As Long?
        Get
            Return Nothing
        End Get
    End Property

    MustOverride ReadOnly Property Name() As String

    Overridable ReadOnly Property MinimumValue() As Long
        Get
            Return 0
        End Get
    End Property

    Overridable ReadOnly Property MaximumValue() As Long
        Get
            Return Long.MaxValue
        End Get
    End Property

    MustOverride ReadOnly Property Abbreviation As String
End Class
Friend Module CharacterStatisticTypeDescriptorUtility
    Friend ReadOnly CharacterStatisticTypeDescriptors As IReadOnlyDictionary(Of CharacterStatisticType, CharacterStatisticTypeDescriptor) =
        New Dictionary(Of CharacterStatisticType, CharacterStatisticTypeDescriptor) From
        {
            {CharacterStatisticType.BaseMaximumDefend, New BaseMaximumDefendDescriptor},
            {CharacterStatisticType.Chafing, New ChafingDescriptor},
            {CharacterStatisticType.Dexterity, New DexterityDescriptor},
            {CharacterStatisticType.Drunkenness, New DrunkennessDescriptor},
            {CharacterStatisticType.Fatigue, New FatigueDescriptor},
            {CharacterStatisticType.FoodPoisoning, New FoodPoisoningDescriptor},
            {CharacterStatisticType.Highness, New HighnessDescriptor},
            {CharacterStatisticType.HP, New HpDescriptor},
            {CharacterStatisticType.Hunger, New HungerDescriptor},
            {CharacterStatisticType.Immobilization, New ImmobilizationDescriptor},
            {CharacterStatisticType.Influence, New InfluenceDescriptor},
            {CharacterStatisticType.Mana, New ManaDescriptor},
            {CharacterStatisticType.Money, New MoneyDescriptor},
            {CharacterStatisticType.MP, New MpDescriptor},
            {CharacterStatisticType.Power, New PowerDescriptor},
            {CharacterStatisticType.Strength, New StrengthDescriptor},
            {CharacterStatisticType.Stress, New StressDescriptor},
            {CharacterStatisticType.UnarmedMaximumDamage, New UnarmedMaximumDamageDescriptor},
            {CharacterStatisticType.Unassigned, New UnassignedDescriptor},
            {CharacterStatisticType.Willpower, New WillpowerDescriptor},
            {CharacterStatisticType.Wounds, New WoundsDescriptor},
            {CharacterStatisticType.XP, New XPDescriptor},
            {CharacterStatisticType.XPGoal, New XPGoalDescriptor}
        }
End Module
