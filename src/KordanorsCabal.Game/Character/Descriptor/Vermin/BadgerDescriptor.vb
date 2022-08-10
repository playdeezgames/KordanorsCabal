Friend Class BadgerDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 1},
                    {CharacterStatisticType.Strength, 4},
                    {CharacterStatisticType.Dexterity, 2},
                    {CharacterStatisticType.HP, 2},
                    {CharacterStatisticType.Immobilization, 0},
                    {CharacterStatisticType.Influence, 0},
                    {CharacterStatisticType.MP, 3},
                    {CharacterStatisticType.Stress, 0},
                    {CharacterStatisticType.UnarmedMaximumDamage, 2},
                    {CharacterStatisticType.Willpower, 3},
                    {CharacterStatisticType.Wounds, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Private ReadOnly spawnCountTable As IReadOnlyDictionary(Of OldDungeonLevel, Long) =
        New Dictionary(Of OldDungeonLevel, Long) From
        {
            {OldDungeonLevel.Level1, 24},
            {OldDungeonLevel.Level2, 12}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long
            Return If(spawnCountTable.TryGetValue(level.ToOld, result), result, 6)
        End Get
    End Property

    Public Overrides Function CanSpawn(location As Location, level As OldDungeonLevel) As Boolean
        Select Case level
            Case OldDungeonLevel.Level1
                Return location.LocationType = LocationType.DungeonDeadEnd
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
