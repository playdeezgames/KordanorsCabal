Friend Class N00bDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 1},
                    {CharacterStatisticType.Chafing, 0},
                    {CharacterStatisticType.Dexterity, 1},
                    {CharacterStatisticType.Drunkenness, 0},
                    {CharacterStatisticType.Fatigue, 0},
                    {CharacterStatisticType.FoodPoisoning, 0},
                    {CharacterStatisticType.Highness, 0},
                    {CharacterStatisticType.HP, 3},
                    {CharacterStatisticType.Immobilization, 0},
                    {CharacterStatisticType.Hunger, 0},
                    {CharacterStatisticType.Influence, 1},
                    {CharacterStatisticType.Mana, 1},
                    {CharacterStatisticType.Money, 0},
                    {CharacterStatisticType.MP, 3},
                    {CharacterStatisticType.Power, 1},
                    {CharacterStatisticType.Strength, 1},
                    {CharacterStatisticType.Stress, 0},
                    {CharacterStatisticType.UnarmedMaximumDamage, 1},
                    {CharacterStatisticType.Unassigned, 8},
                    {CharacterStatisticType.Willpower, 1},
                    {CharacterStatisticType.Wounds, 0},
                    {CharacterStatisticType.XP, 0},
                    {CharacterStatisticType.XPGoal, 10}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Single
        Get
            Return 50.0! + If(character.GetStatistic(CharacterStatisticType.Strength), 0) * 10.0!
        End Get
    End Property

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "n00b"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        'drop nothing
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType <> CharacterType.N00b
    End Function
End Class
