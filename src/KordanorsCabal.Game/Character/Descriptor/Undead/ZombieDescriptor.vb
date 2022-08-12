Friend Class ZombieDescriptor
    Inherits CharacterType

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 3},
                    {1, 4},
                    {2, 3},
                    {6, 1},
                    {23, 0},
                    {10, 2},
                    {12, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d4") > 1 Then
            location.Inventory.Add(Item.Create(ItemType.ZombieTaint))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = OldCharacterType.N00b
    End Function
    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Select Case level.Id
            Case 1
                Return location.LocationType = LocationType.FromName(DungeonDeadEnd)
            Case Else
                Return location.LocationType.IsDungeon
        End Select
    End Function
End Class
