Public MustInherit Class StatisticTypeDescriptor
    Overridable ReadOnly Property DefaultValue As Long
        Get
            Return 0
        End Get
    End Property

    MustOverride ReadOnly Property Name() As String
End Class
Friend Module StatisticTypeDescriptorUtility
    Friend ReadOnly StatisticTypeDescriptors As IReadOnlyDictionary(Of StatisticType, StatisticTypeDescriptor) =
        New Dictionary(Of StatisticType, StatisticTypeDescriptor) From
        {
            {StatisticType.Dexterity, New DexterityDescriptor},
            {StatisticType.HP, New HpDescriptor},
            {StatisticType.Influence, New InfluenceDescriptor},
            {StatisticType.Mana, New ManaDescriptor},
            {StatisticType.MP, New MpDescriptor},
            {StatisticType.Power, New PowerDescriptor},
            {StatisticType.Strength, New StrengthDescriptor},
            {StatisticType.Unassigned, New UnassignedDescriptor},
            {StatisticType.Willpower, New WillpowerDescriptor}
        }
End Module
