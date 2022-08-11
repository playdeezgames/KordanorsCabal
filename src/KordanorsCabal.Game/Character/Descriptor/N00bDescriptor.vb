Friend Class N00bDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 1},
                    {OldCharacterStatisticType.Chafing, 0},
                    {2, 1},
                    {OldCharacterStatisticType.Drunkenness, 0},
                    {OldCharacterStatisticType.Fatigue, 0},
                    {OldCharacterStatisticType.FoodPoisoning, 0},
                    {OldCharacterStatisticType.Highness, 0},
                    {6, 3},
                    {23, 0},
                    {OldCharacterStatisticType.Hunger, 0},
                    {3, 1},
                    {OldCharacterStatisticType.Mana, 1},
                    {OldCharacterStatisticType.Money, 0},
                    {7, 3},
                    {OldCharacterStatisticType.Power, 1},
                    {1, 1},
                    {13, 0},
                    {10, 1},
                    {OldCharacterStatisticType.Unassigned, 8},
                    {4, 1},
                    {12, 0},
                    {OldCharacterStatisticType.XP, 0},
                    {OldCharacterStatisticType.XPGoal, 10}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 50 + If(character.GetStatistic(OldCharacterStatisticType.Strength.ToNew), 0) * 10
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
