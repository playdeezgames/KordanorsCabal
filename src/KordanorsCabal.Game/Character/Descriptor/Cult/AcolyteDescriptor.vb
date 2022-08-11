Friend Class AcolyteDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of OldCharacterStatisticType, Long)
        Get
            Return New Dictionary(Of OldCharacterStatisticType, Long) From
                {
                    {OldCharacterStatisticType.BaseMaximumDefend, 2},
                    {OldCharacterStatisticType.Strength, 6},
                    {OldCharacterStatisticType.Dexterity, 2},
                    {OldCharacterStatisticType.HP, 2},
                    {OldCharacterStatisticType.Immobilization, 0},
                    {OldCharacterStatisticType.Influence, 0},
                    {OldCharacterStatisticType.MP, 2},
                    {OldCharacterStatisticType.Stress, 0},
                    {OldCharacterStatisticType.UnarmedMaximumDamage, 3},
                    {OldCharacterStatisticType.Willpower, 2},
                    {OldCharacterStatisticType.Wounds, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
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

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level.Id, result), result, 0)
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
        If RNG.RollDice("1d2/2") = 1 Then
            location.Inventory.Add(Item.Create(ItemType.HolyWater))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Select Case level.Id
            Case 1
                Return location.LocationType = LocationType.FromName(DungeonBoss)
            Case Else
                Return True
        End Select
    End Function
End Class
