Friend Class AcolyteDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 2},
                    {CharacterStatisticType.Strength, 6},
                    {CharacterStatisticType.Dexterity, 2},
                    {CharacterStatisticType.HP, 2},
                    {CharacterStatisticType.Influence, 0},
                    {CharacterStatisticType.MP, 2},
                    {CharacterStatisticType.Stress, 0},
                    {CharacterStatisticType.UnarmedMaximumDamage, 3},
                    {CharacterStatisticType.Willpower, 2},
                    {CharacterStatisticType.Wounds, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Single
        Get
            Return 0
        End Get
    End Property

    Private ReadOnly spawnCountTable As IReadOnlyDictionary(Of Long, Long) =
        New Dictionary(Of Long, Long) From
        {
            {1, 1},
            {2, 10},
            {3, 25}
        }

    Public Overrides ReadOnly Property SpawnCount(level As Long) As Long
        Get
            Dim result As Long = 0
            spawnCountTable.TryGetValue(level, result)
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Acolyte"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 5
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        'TODO
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function

    Public Overrides Function CanSpawn(location As Location, level As Long) As Boolean
        Select Case level
            Case 1
                Return location.LocationType = LocationType.DungeonBoss
            Case Else
                Return True
        End Select
    End Function
End Class
