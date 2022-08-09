Friend Class GoblinEliteDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 1},
                    {CharacterStatisticType.Strength, 6},
                    {CharacterStatisticType.Dexterity, 1},
                    {CharacterStatisticType.HP, 2},
                    {CharacterStatisticType.Immobilization, 0},
                    {CharacterStatisticType.Influence, 1},
                    {CharacterStatisticType.MP, 1},
                    {CharacterStatisticType.Stress, 0},
                    {CharacterStatisticType.UnarmedMaximumDamage, 3},
                    {CharacterStatisticType.Willpower, 2},
                    {CharacterStatisticType.Wounds, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Single
        Get
            Return 0!
        End Get
    End Property

    Private ReadOnly spawnCountTable As IReadOnlyDictionary(Of OldDungeonLevel, Long) =
        New Dictionary(Of OldDungeonLevel, Long) From
        {
            {OldDungeonLevel.Level1, 5},
            {OldDungeonLevel.Level2, 15},
            {OldDungeonLevel.Level3, 30},
            {OldDungeonLevel.Level4, 45},
            {OldDungeonLevel.Level5, 30}
        }

    Public Overrides ReadOnly Property SpawnCount(level As OldDungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level, result), result, 0)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Goblin Elite"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 2
        End Get
    End Property

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function
    Public Overrides Function RollMoneyDrop() As Long
        Return RNG.RollDice("3d6")
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
            {ItemType.Food, 2},
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

    Public Overrides Function CanSpawn(location As Location, level As OldDungeonLevel) As Boolean
        Return level <> OldDungeonLevel.Level1 OrElse location.LocationType = LocationType.DungeonDeadEnd
    End Function

    Public Overrides Function CanBeBribedWith(itemType As ItemType) As Boolean
        Select Case itemType
            Case ItemType.Beer
                Return True
            Case Else
                Return False
        End Select
    End Function
End Class
