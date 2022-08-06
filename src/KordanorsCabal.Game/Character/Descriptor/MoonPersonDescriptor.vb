Friend Class MoonPersonDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 4},
                    {CharacterStatisticType.Strength, 8},
                    {CharacterStatisticType.Dexterity, 4},
                    {CharacterStatisticType.HP, 3},
                    {CharacterStatisticType.Immobilization, 0},
                    {CharacterStatisticType.Influence, 6},
                    {CharacterStatisticType.MP, 3},
                    {CharacterStatisticType.Stress, 0},
                    {CharacterStatisticType.UnarmedMaximumDamage, 4},
                    {CharacterStatisticType.Willpower, 3},
                    {CharacterStatisticType.Wounds, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Single
        Get
            Return 0!
        End Get
    End Property

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Return If(level = DungeonLevel.Moon, 100, 0)
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
        Return level = DungeonLevel.Moon
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
