Friend Class RatDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of OldCharacterStatisticType, Long)
        Get
            Return New Dictionary(Of OldCharacterStatisticType, Long) From
                {
                    {OldCharacterStatisticType.BaseMaximumDefend, 1},
                    {OldCharacterStatisticType.Strength, 2},
                    {OldCharacterStatisticType.Dexterity, 1},
                    {OldCharacterStatisticType.HP, 1},
                    {OldCharacterStatisticType.Immobilization, 0},
                    {OldCharacterStatisticType.Influence, 0},
                    {OldCharacterStatisticType.MP, 1},
                    {OldCharacterStatisticType.Stress, 0},
                    {OldCharacterStatisticType.UnarmedMaximumDamage, 1},
                    {OldCharacterStatisticType.Willpower, 1},
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
            Return 0
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Rat"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 1
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        location.Inventory.Add(Item.Create(ItemType.RatTail))
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = CharacterType.N00b
    End Function
End Class
