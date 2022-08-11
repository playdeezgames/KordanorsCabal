Public MustInherit Class CharacterStatisticType
    ReadOnly Property Id As Long
    Sub New(characterStatisticTypeId As Long)
        Id = characterStatisticTypeId
    End Sub
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
Public Module CharacterStatisticTypeUtility
    Friend ReadOnly CharacterStatisticTypeDescriptors As IReadOnlyDictionary(Of OldCharacterStatisticType, CharacterStatisticType) =
        New Dictionary(Of OldCharacterStatisticType, CharacterStatisticType) From
        {
            {OldCharacterStatisticType.BaseMaximumDefend, New BaseMaximumDefendDescriptor(OldCharacterStatisticType.BaseMaximumDefend)},
            {OldCharacterStatisticType.Chafing, New ChafingDescriptor(OldCharacterStatisticType.Chafing)},
            {OldCharacterStatisticType.Dexterity, New DexterityDescriptor(OldCharacterStatisticType.Dexterity)},
            {OldCharacterStatisticType.Drunkenness, New DrunkennessDescriptor(OldCharacterStatisticType.Drunkenness)},
            {OldCharacterStatisticType.Fatigue, New FatigueDescriptor(OldCharacterStatisticType.Fatigue)},
            {OldCharacterStatisticType.FoodPoisoning, New FoodPoisoningDescriptor(OldCharacterStatisticType.FoodPoisoning)},
            {OldCharacterStatisticType.Highness, New HighnessDescriptor(OldCharacterStatisticType.Highness)},
            {OldCharacterStatisticType.HP, New HpDescriptor(OldCharacterStatisticType.HP)},
            {OldCharacterStatisticType.Hunger, New HungerDescriptor(OldCharacterStatisticType.Hunger)},
            {OldCharacterStatisticType.Immobilization, New ImmobilizationDescriptor(OldCharacterStatisticType.Immobilization)},
            {OldCharacterStatisticType.Influence, New InfluenceDescriptor(OldCharacterStatisticType.Influence)},
            {OldCharacterStatisticType.Mana, New ManaDescriptor(OldCharacterStatisticType.Mana)},
            {OldCharacterStatisticType.Money, New MoneyDescriptor(OldCharacterStatisticType.Money)},
            {OldCharacterStatisticType.MP, New MpDescriptor(OldCharacterStatisticType.MP)},
            {OldCharacterStatisticType.Power, New PowerDescriptor(OldCharacterStatisticType.Power)},
            {OldCharacterStatisticType.Strength, New StrengthDescriptor(OldCharacterStatisticType.Strength)},
            {OldCharacterStatisticType.Stress, New StressDescriptor(OldCharacterStatisticType.Stress)},
            {OldCharacterStatisticType.UnarmedMaximumDamage, New UnarmedMaximumDamageDescriptor(OldCharacterStatisticType.UnarmedMaximumDamage)},
            {OldCharacterStatisticType.Unassigned, New UnassignedDescriptor(OldCharacterStatisticType.Unassigned)},
            {OldCharacterStatisticType.Willpower, New WillpowerDescriptor(OldCharacterStatisticType.Willpower)},
            {OldCharacterStatisticType.Wounds, New WoundsDescriptor(OldCharacterStatisticType.Wounds)},
            {OldCharacterStatisticType.XP, New XPDescriptor(OldCharacterStatisticType.XP)},
            {OldCharacterStatisticType.XPGoal, New XPGoalDescriptor(OldCharacterStatisticType.XPGoal)}
        }
End Module
