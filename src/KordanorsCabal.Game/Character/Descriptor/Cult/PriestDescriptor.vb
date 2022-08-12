Friend Class PriestDescriptor
    Inherits CharacterType

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 3},
                    {1, 6},
                    {2, 3},
                    {6, 3},
                    {23, 0},
                    {3, 3},
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
            {2, 1},
            {3, 15},
            {4, 30},
            {5, 45}
        }

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level.Id, result), result, 0)
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
        Select Case level.Id
            Case 2
                Return location.LocationType = LocationType.FromName(DungeonBoss)
            Case Else
                Return True
        End Select
    End Function
End Class
