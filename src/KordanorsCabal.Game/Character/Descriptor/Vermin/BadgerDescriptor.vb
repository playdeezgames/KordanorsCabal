Friend Class BadgerDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {OldCharacterStatisticType.BaseMaximumDefend, 1},
                    {OldCharacterStatisticType.Strength, 4},
                    {OldCharacterStatisticType.Dexterity, 2},
                    {OldCharacterStatisticType.HP, 2},
                    {OldCharacterStatisticType.Immobilization, 0},
                    {OldCharacterStatisticType.Influence, 0},
                    {OldCharacterStatisticType.MP, 3},
                    {OldCharacterStatisticType.Stress, 0},
                    {OldCharacterStatisticType.UnarmedMaximumDamage, 2},
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

    Private ReadOnly spawnCountTable As IReadOnlyDictionary(Of Long, Long) =
        New Dictionary(Of Long, Long) From
        {
            {1, 24},
            {2, 12}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long
            Return If(spawnCountTable.TryGetValue(level.Id, result), result, 6)
        End Get
    End Property

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Select Case level.Id
            Case 1
                Return location.LocationType = LocationType.FromName(DungeonDeadEnd)
            Case Else
                Return True
        End Select
    End Function

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Badger"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 2
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d3") = 1 Then
            location.Inventory.Add(Item.Create(ItemType.Mushroom))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = CharacterType.N00b
    End Function

    Private ReadOnly bribes As IReadOnlyList(Of ItemType) =
        New List(Of ItemType) From {ItemType.RottenEgg}

    Public Overrides Function CanBeBribedWith(itemType As ItemType) As Boolean
        Return bribes.Contains(itemType)
    End Function
End Class
