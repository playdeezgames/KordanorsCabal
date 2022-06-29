Friend Class N00bDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 1},
                    {CharacterStatisticType.Dexterity, 1},
                    {CharacterStatisticType.HP, 3},
                    {CharacterStatisticType.Influence, 1},
                    {CharacterStatisticType.Mana, 1},
                    {CharacterStatisticType.MP, 3},
                    {CharacterStatisticType.Power, 1},
                    {CharacterStatisticType.Strength, 1},
                    {CharacterStatisticType.UnarmedMaximumDamage, 1},
                    {CharacterStatisticType.Unassigned, 8},
                    {CharacterStatisticType.Willpower, 1}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Single
        Get
            Return 50.0! + If(character.GetStatistic(CharacterStatisticType.Strength), 0) * 10.0!
        End Get
    End Property

    Public Overrides ReadOnly Property SpawnCount(level As Long) As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType <> CharacterType.N00b
    End Function
End Class
