Friend Class PriestDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 3},
                    {CharacterStatisticType.Strength, 6},
                    {CharacterStatisticType.Dexterity, 3},
                    {CharacterStatisticType.HP, 3},
                    {CharacterStatisticType.Immobilization, 0},
                    {CharacterStatisticType.Influence, 3},
                    {CharacterStatisticType.MP, 3},
                    {CharacterStatisticType.Stress, 0},
                    {CharacterStatisticType.UnarmedMaximumDamage, 3},
                    {CharacterStatisticType.Willpower, 3},
                    {CharacterStatisticType.Wounds, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Single
        Get
            Return 0
        End Get
    End Property

    Private ReadOnly spawnCountTable As IReadOnlyDictionary(Of DungeonLevel, Long) =
        New Dictionary(Of DungeonLevel, Long) From
        {
            {DungeonLevel.Level2, 1},
            {DungeonLevel.Level3, 15},
            {DungeonLevel.Level4, 30},
            {DungeonLevel.Level5, 45}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level, result), result, 0)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Priest"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 5
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d2/2") = 1 Then
            location.Inventory.Add(Item.Create(ItemType.Herb))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Select Case level
            Case DungeonLevel.Level2
                Return location.LocationType = LocationType.DungeonBoss
            Case Else
                Return True
        End Select
    End Function
End Class
