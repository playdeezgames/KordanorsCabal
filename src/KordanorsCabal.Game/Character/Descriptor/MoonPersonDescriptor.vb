Friend Class MoonPersonDescriptor
    Inherits CharacterType

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 4},
                    {1, 8},
                    {2, 4},
                    {6, 3},
                    {23, 0},
                    {3, 6},
                    {7, 3},
                    {13, 0},
                    {10, 4},
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

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Return If(level.Id = 6, 100, 0)
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d2") > 1 Then
            location.Inventory.Add(Item.Create(ItemType.ShoeLaces))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = OldCharacterType.N00b
    End Function

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id = 6
    End Function

    Private Shared ReadOnly bribeItems As IReadOnlyList(Of ItemType) =
        New List(Of ItemType) From
        {
            ItemType.Bottle
        }

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub

    Public Overrides Function CanBeBribedWith(itemType As ItemType) As Boolean
        Return bribeItems.Contains(itemType)
    End Function
End Class
