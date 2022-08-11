Friend Class MoonPersonDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of OldCharacterStatisticType, Long)
        Get
            Return New Dictionary(Of OldCharacterStatisticType, Long) From
                {
                    {OldCharacterStatisticType.BaseMaximumDefend, 4},
                    {OldCharacterStatisticType.Strength, 8},
                    {OldCharacterStatisticType.Dexterity, 4},
                    {OldCharacterStatisticType.HP, 3},
                    {OldCharacterStatisticType.Immobilization, 0},
                    {OldCharacterStatisticType.Influence, 6},
                    {OldCharacterStatisticType.MP, 3},
                    {OldCharacterStatisticType.Stress, 0},
                    {OldCharacterStatisticType.UnarmedMaximumDamage, 4},
                    {OldCharacterStatisticType.Willpower, 3},
                    {OldCharacterStatisticType.Wounds, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Return If(level.Id = 6, 100, 0)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Moon Person"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 3
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d2") > 1 Then
            location.Inventory.Add(Item.Create(ItemType.ShoeLaces))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = CharacterType.N00b
    End Function

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id = 6
    End Function

    Private Shared ReadOnly bribeItems As IReadOnlyList(Of ItemType) =
        New List(Of ItemType) From
        {
            ItemType.Bottle
        }

    Public Overrides Function CanBeBribedWith(itemType As ItemType) As Boolean
        Return bribeItems.Contains(itemType)
    End Function
End Class
