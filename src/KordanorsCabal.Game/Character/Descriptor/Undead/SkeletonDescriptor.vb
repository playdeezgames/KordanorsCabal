Friend Class SkeletonDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of OldCharacterStatisticType, Long)
        Get
            Return New Dictionary(Of OldCharacterStatisticType, Long) From
                {
                    {OldCharacterStatisticType.BaseMaximumDefend, 2},
                    {OldCharacterStatisticType.Strength, 4},
                    {OldCharacterStatisticType.Dexterity, 2},
                    {OldCharacterStatisticType.HP, 1},
                    {OldCharacterStatisticType.Immobilization, 0},
                    {OldCharacterStatisticType.UnarmedMaximumDamage, 2},
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
            {1, 30},
            {2, 45},
            {3, 30},
            {4, 15}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level.Id, result), result, 0)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "skeleton"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 1
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d4") > 1 Then
            location.Inventory.Add(Item.Create(ItemType.SkullFragment))
        End If

    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = CharacterType.N00b
    End Function
    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return location.LocationType = LocationType.FromName(Dungeon)
    End Function
    Public Overrides Function RollMoneyDrop() As Long
        Return RNG.RollDice("2d3")
    End Function
    Public Overrides ReadOnly Property IsUndead As Boolean
        Get
            Return True
        End Get
    End Property
End Class
