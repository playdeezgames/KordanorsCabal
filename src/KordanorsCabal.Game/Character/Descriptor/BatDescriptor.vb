Friend Class BatDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides Function CanSpawn(location As Location, level As Long) As Boolean
        Return location.LocationType = LocationType.Dungeon
    End Function

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 2},
                    {CharacterStatisticType.Strength, 2},
                    {CharacterStatisticType.Dexterity, 4},
                    {CharacterStatisticType.HP, 1},
                    {CharacterStatisticType.Influence, 0},
                    {CharacterStatisticType.MP, 1},
                    {CharacterStatisticType.Stress, 0},
                    {CharacterStatisticType.UnarmedMaximumDamage, 1},
                    {CharacterStatisticType.Willpower, 1},
                    {CharacterStatisticType.Wounds, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Single
        Get
            Return 0!
        End Get
    End Property

    Public Overrides ReadOnly Property SpawnCount(level As Long) As Long
        Get
            Select Case level
                Case 1
                    Return 15
                Case 2
                    Return 30
                Case 3
                    Return 45
                Case 4
                    Return 30
                Case 5
                    Return 15
                Case Else
                    Return 0
            End Select
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Bat"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 1
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = CharacterType.N00b
    End Function
End Class
