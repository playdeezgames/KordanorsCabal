Friend Class MalcontentDescriptor
    Inherits CharacterTypeDescriptor
    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 2},
                    {CharacterStatisticType.Strength, 6},
                    {CharacterStatisticType.Dexterity, 4},
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

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Private ReadOnly spawnCountTable As IReadOnlyDictionary(Of OldDungeonLevel, Long) =
        New Dictionary(Of OldDungeonLevel, Long) From
        {
            {OldDungeonLevel.Level1, 30},
            {OldDungeonLevel.Level2, 15}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long
            Return If(spawnCountTable.TryGetValue(level.ToOld, result), result, 5)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Malcontent"
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


    Private ReadOnly foodTable As IReadOnlyDictionary(Of ItemType, Integer) =
        New Dictionary(Of ItemType, Integer) From
        {
            {ItemType.None, 2},
            {ItemType.Food, 1},
            {ItemType.RottenFood, 2}
        }

    Public Overrides Sub DropLoot(location As Location)
        location.Inventory.Add(Item.Create(ItemType.MembershipCard))
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
            Case ItemType.Pr0n
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Shared ReadOnly attackTable As IReadOnlyDictionary(Of AttackType, Integer) =
        New Dictionary(Of AttackType, Integer) From
        {
            {AttackType.Physical, 3},
            {AttackType.Mental, 1}
        }

    Public Overrides Function GenerateAttackType(character As Character) As AttackType
        Return RNG.FromGenerator(attackTable)
    End Function
End Class
