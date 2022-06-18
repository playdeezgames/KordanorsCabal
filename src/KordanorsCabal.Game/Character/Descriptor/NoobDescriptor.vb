Friend Class NoobDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of StatisticType, Long)
        Get
            Return New Dictionary(Of StatisticType, Long) From
                {
                    {StatisticType.Dexterity, 1},
                    {StatisticType.HP, 3},
                    {StatisticType.Influence, 1},
                    {StatisticType.Mana, 1},
                    {StatisticType.MP, 3},
                    {StatisticType.Power, 1},
                    {StatisticType.Strength, 1},
                    {StatisticType.Unassigned, 8},
                    {StatisticType.Willpower, 1}
                }
        End Get
    End Property
End Class
