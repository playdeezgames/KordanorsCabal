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
                    Return 25
                Case Else
                    Return 5
            End Select
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

    Public Overrides Sub DropLoot(location As Location)
        location.Inventory.Add(Item.Create(ItemType.MembershipCard))
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
            Case ItemType.Pr0n
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Shared table As IReadOnlyDictionary(Of AttackType, Integer) =
        New Dictionary(Of AttackType, Integer) From
        {
            {AttackType.Physical, 3},
            {AttackType.Mental, 1}
        }

    Public Overrides Function GenerateAttackType(character As Character) As AttackType
        Return RNG.FromGenerator(table)
    End Function
End Class
