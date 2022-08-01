Friend Class GoblinDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 1},
                    {CharacterStatisticType.Strength, 4},
                    {CharacterStatisticType.Dexterity, 1},
                    {CharacterStatisticType.HP, 1},
                    {CharacterStatisticType.Influence, 0},
                    {CharacterStatisticType.MP, 1},
                    {CharacterStatisticType.Stress, 0},
                    {CharacterStatisticType.UnarmedMaximumDamage, 2},
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

    Private Shared ReadOnly spawnCountTable As IReadOnlyDictionary(Of DungeonLevel, Long) =
        New Dictionary(Of DungeonLevel, Long) From
        {
            {DungeonLevel.Level1, 30},
            {DungeonLevel.Level2, 45},
            {DungeonLevel.Level3, 30},
            {DungeonLevel.Level4, 15}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long
            Return If(spawnCountTable.TryGetValue(level, result), result, 0)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "goblin"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 1
        End Get
    End Property

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = CharacterType.N00b
    End Function
    Public Overrides Function CanSpawn(location As Location, level As Long) As Boolean
        Return location.LocationType = LocationType.Dungeon
    End Function
    Public Overrides Function RollMoneyDrop() As Long
        Return RNG.RollDice("2d6")
    End Function
    Private ReadOnly partingShots As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "@#$% you!"
        }
    Public Overrides Function PartingShot() As String

        Return RNG.FromEnumerable(partingShots)
    End Function

    Private ReadOnly foodTable As IReadOnlyDictionary(Of ItemType, Integer) =
        New Dictionary(Of ItemType, Integer) From
        {
            {ItemType.None, 2},
            {ItemType.Food, 1},
            {ItemType.RottenFood, 1}
        }

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d2") > 1 Then
            location.Inventory.Add(Item.Create(ItemType.GoblinEar))
        End If
        Dim foodType = RNG.FromGenerator(foodTable)
        If foodType <> ItemType.None Then
            location.Inventory.Add(Item.Create(foodType))
        End If
    End Sub

    Private Shared ReadOnly bribeItems As IReadOnlyList(Of ItemType) =
        New List(Of ItemType) From
        {
            ItemType.Beer,
            ItemType.Pr0n
        }

    Public Overrides Function CanBeBribedWith(itemType As ItemType) As Boolean
        Return bribeItems.Contains(itemType)
    End Function
End Class
