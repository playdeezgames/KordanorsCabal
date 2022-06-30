Public MustInherit Class CharacterStatisticTypeDescriptor
    Overridable ReadOnly Property DefaultValue As Long?
        Get
            Return Nothing
        End Get
    End Property

    MustOverride ReadOnly Property Name() As String
End Class
Friend Module CharacterStatisticTypeDescriptorUtility
    Friend ReadOnly CharacterStatisticTypeDescriptors As IReadOnlyDictionary(Of CharacterStatisticType, CharacterStatisticTypeDescriptor) =
        New Dictionary(Of CharacterStatisticType, CharacterStatisticTypeDescriptor) From
        {
            {CharacterStatisticType.BaseMaximumDefend, New BaseMaximumDefendDescriptor},
            {CharacterStatisticType.Dexterity, New DexterityDescriptor},
            {CharacterStatisticType.HP, New HpDescriptor},
            {CharacterStatisticType.Influence, New InfluenceDescriptor},
            {CharacterStatisticType.Mana, New ManaDescriptor},
            {CharacterStatisticType.MP, New MpDescriptor},
            {CharacterStatisticType.Power, New PowerDescriptor},
            {CharacterStatisticType.Strength, New StrengthDescriptor},
            {CharacterStatisticType.Stress, New StressDescriptor},
            {CharacterStatisticType.UnarmedMaximumDamage, New UnarmedMaximumDamageDescriptor},
            {CharacterStatisticType.Unassigned, New UnassignedDescriptor},
            {CharacterStatisticType.Willpower, New WillpowerDescriptor},
            {CharacterStatisticType.Wounds, New WoundsDescriptor}
        }
End Module
