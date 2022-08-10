Friend Class KordanorDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 5},
                    {CharacterStatisticType.Strength, 8},
                    {CharacterStatisticType.Dexterity, 5},
                    {CharacterStatisticType.HP, 10},
                    {CharacterStatisticType.Immobilization, 0},
                    {CharacterStatisticType.Influence, 4},
                    {CharacterStatisticType.MP, 4},
                    {CharacterStatisticType.Stress, 0},
                    {CharacterStatisticType.UnarmedMaximumDamage, 4},
                    {CharacterStatisticType.Willpower, 4},
                    {CharacterStatisticType.Wounds, 0}
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
            Return If(level.Id = 5, 1, 0)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Kordanor"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 5
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        location.Inventory.Add(Item.Create(ItemType.HornsOfKordanor))
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id = 5 AndAlso location.LocationType = OldLocationType.DungeonBoss
    End Function
End Class
