Friend Class GoblinEliteDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {OldCharacterStatisticType.BaseMaximumDefend, 1},
                    {OldCharacterStatisticType.Strength, 6},
                    {OldCharacterStatisticType.Dexterity, 1},
                    {OldCharacterStatisticType.HP, 2},
                    {OldCharacterStatisticType.Immobilization, 0},
                    {OldCharacterStatisticType.Influence, 1},
                    {OldCharacterStatisticType.MP, 1},
                    {OldCharacterStatisticType.Stress, 0},
                    {OldCharacterStatisticType.UnarmedMaximumDamage, 3},
                    {OldCharacterStatisticType.Willpower, 2},
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
            {1, 5},
            {2, 15},
            {3, 30},
            {4, 45},
            {5, 30}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level.Id, result), result, 0)
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

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id <> 1 OrElse location.LocationType = LocationType.FromName(DungeonDeadEnd)
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
