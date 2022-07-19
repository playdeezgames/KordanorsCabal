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

    Public Overrides ReadOnly Property SpawnCount(level As Long) As Long
        Get
            Select Case level
                Case 1
                    Return 5
                Case 2
                    Return 15
                Case 3
                    Return 30
                Case 4
                    Return 45
                Case 5
                    Return 30
                Case Else
                    Return 0
            End Select
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

    Public Overrides Function CanSpawn(location As Location, level As Long) As Boolean
        Select Case level
            Case 1
                Return location.LocationType = LocationType.DungeonDeadEnd
            Case Else
                Return True
        End Select
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
