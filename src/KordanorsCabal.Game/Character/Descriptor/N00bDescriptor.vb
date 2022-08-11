Friend Class N00bDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 1},
                    {22, 0},
                    {2, 1},
                    {18, 0},
                    {15, 0},
                    {21, 0},
                    {19, 0},
                    {6, 3},
                    {23, 0},
                    {20, 0},
                    {3, 1},
                    {8, 1},
                    {14, 0},
                    {7, 3},
                    {5, 1},
                    {1, 1},
                    {13, 0},
                    {10, 1},
                    {9, 8},
                    {4, 1},
                    {12, 0},
                    {16, 0},
                    {17, 10}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 50 + If(character.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Strength)), 0) * 10
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
