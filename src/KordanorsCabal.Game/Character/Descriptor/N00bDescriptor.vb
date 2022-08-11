Friend Class N00bDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {OldCharacterStatisticType.BaseMaximumDefend, 1},
                    {OldCharacterStatisticType.Chafing, 0},
                    {OldCharacterStatisticType.Dexterity, 1},
                    {OldCharacterStatisticType.Drunkenness, 0},
                    {OldCharacterStatisticType.Fatigue, 0},
                    {OldCharacterStatisticType.FoodPoisoning, 0},
                    {OldCharacterStatisticType.Highness, 0},
                    {OldCharacterStatisticType.HP, 3},
                    {OldCharacterStatisticType.Immobilization, 0},
                    {OldCharacterStatisticType.Hunger, 0},
                    {OldCharacterStatisticType.Influence, 1},
                    {OldCharacterStatisticType.Mana, 1},
                    {OldCharacterStatisticType.Money, 0},
                    {OldCharacterStatisticType.MP, 3},
                    {OldCharacterStatisticType.Power, 1},
                    {OldCharacterStatisticType.Strength, 1},
                    {OldCharacterStatisticType.Stress, 0},
                    {OldCharacterStatisticType.UnarmedMaximumDamage, 1},
                    {OldCharacterStatisticType.Unassigned, 8},
                    {OldCharacterStatisticType.Willpower, 1},
                    {OldCharacterStatisticType.Wounds, 0},
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
