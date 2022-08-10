Friend Class SnakeDescriptor
    Inherits CharacterTypeDescriptor
    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 1},
                    {CharacterStatisticType.Strength, 3},
                    {CharacterStatisticType.Dexterity, 1},
                    {CharacterStatisticType.HP, 1},
                    {CharacterStatisticType.Immobilization, 0},
                    {CharacterStatisticType.Influence, 0},
                    {CharacterStatisticType.MP, 1},
                    {CharacterStatisticType.Stress, 0},
                    {CharacterStatisticType.UnarmedMaximumDamage, 2},
                    {CharacterStatisticType.Willpower, 1},
                    {CharacterStatisticType.Wounds, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return location.LocationType = LocationType.Dungeon
    End Function

    Private ReadOnly spawnCountTable As IReadOnlyDictionary(Of OldDungeonLevel, Long) =
        New Dictionary(Of OldDungeonLevel, Long) From
        {
            {OldDungeonLevel.Level1, 15},
            {OldDungeonLevel.Level2, 30},
            {OldDungeonLevel.Level3, 45},
            {OldDungeonLevel.Level4, 30},
            {OldDungeonLevel.Level5, 15}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level.ToOld, result), result, 0)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Snake"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 1
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d2") > 1 Then
            location.Inventory.Add(Item.Create(ItemType.SnakeFang))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = CharacterType.N00b
    End Function
End Class
