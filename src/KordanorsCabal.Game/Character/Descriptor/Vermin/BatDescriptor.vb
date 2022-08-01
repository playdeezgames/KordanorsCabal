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

    Private ReadOnly spawnCountTable As IReadOnlyDictionary(Of DungeonLevel, Long) =
        New Dictionary(Of DungeonLevel, Long) From
        {
            {DungeonLevel.Level1, 15},
            {DungeonLevel.Level2, 30},
            {DungeonLevel.Level3, 45},
            {DungeonLevel.Level4, 30},
            {DungeonLevel.Level5, 15}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level, result), result, 0)
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
        If RNG.RollDice("1d2") > 1 Then
            location.Inventory.Add(Item.Create(ItemType.BatWing))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = CharacterType.N00b
    End Function
End Class
