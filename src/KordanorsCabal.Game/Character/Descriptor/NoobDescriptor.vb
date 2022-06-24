Friend Class NoobDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.Dexterity, 1},
                    {CharacterStatisticType.HP, 3},
                    {CharacterStatisticType.Influence, 1},
                    {CharacterStatisticType.Mana, 1},
                    {CharacterStatisticType.MP, 3},
                    {CharacterStatisticType.Power, 1},
                    {CharacterStatisticType.Strength, 1},
                    {CharacterStatisticType.Unassigned, 8},
                    {CharacterStatisticType.Willpower, 1}
                }
        End Get
    End Property
End Class
