Friend Class BishopDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 3},
                    {OldCharacterStatisticType.Strength, 6},
                    {OldCharacterStatisticType.Dexterity, 3},
                    {OldCharacterStatisticType.HP, 4},
                    {23, 0},
                    {3, 6},
                    {7, 3},
                    {13, 0},
                    {10, 3},
                    {4, 3},
                    {12, 0}
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
            {3, 1},
            {4, 10},
            {5, 25}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level.Id, result), result, 0)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Bishop"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 5
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d2/2") = 1 Then
            location.Inventory.Add(Item.Create(ItemType.Potion))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Select Case level.Id
            Case 3
                Return location.LocationType = LocationType.FromName(DungeonBoss)
            Case Else
                Return True
        End Select
    End Function
End Class
