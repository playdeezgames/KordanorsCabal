Friend Class CabalLeaderDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {OldCharacterStatisticType.BaseMaximumDefend, 4},
                    {OldCharacterStatisticType.Strength, 8},
                    {OldCharacterStatisticType.Dexterity, 4},
                    {OldCharacterStatisticType.HP, 5},
                    {OldCharacterStatisticType.Immobilization, 0},
                    {OldCharacterStatisticType.Influence, 6},
                    {OldCharacterStatisticType.MP, 3},
                    {OldCharacterStatisticType.Stress, 0},
                    {OldCharacterStatisticType.UnarmedMaximumDamage, 4},
                    {OldCharacterStatisticType.Willpower, 3},
                    {OldCharacterStatisticType.Wounds, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Return If(level.Id = 4, 1, 0)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Leader"
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

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id = 4 AndAlso location.LocationType = LocationType.FromName(DungeonBoss)
    End Function
End Class
