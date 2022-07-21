Friend Class ZombieDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 3},
                    {CharacterStatisticType.Strength, 4},
                    {CharacterStatisticType.Dexterity, 3},
                    {CharacterStatisticType.HP, 1},
                    {CharacterStatisticType.UnarmedMaximumDamage, 2},
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
                Case 2
                    Return 30
                Case 3
                    Return 45
                Case 4
                    Return 30
                Case Else
                    Return 15
            End Select
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Zombie"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 1
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d4") > 1 Then
            location.Inventory.Add(Item.Create(ItemType.ZombieTaint))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = CharacterType.N00b
    End Function
    Public Overrides Function CanSpawn(location As Location, level As Long) As Boolean
        Select Case level
            Case 1
                Return location.LocationType = LocationType.DungeonDeadEnd
            Case Else
                Return location.LocationType.IsDungeon
        End Select
    End Function
    Public Overrides Function RollMoneyDrop() As Long
        Return RNG.RollDice("2d4")
    End Function
    Public Overrides ReadOnly Property IsUndead As Boolean
        Get
            Return True
        End Get
    End Property
End Class
