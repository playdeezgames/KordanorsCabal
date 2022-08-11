Friend Class KordanorDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 5},
                    {OldCharacterStatisticType.Strength, 8},
                    {OldCharacterStatisticType.Dexterity, 5},
                    {OldCharacterStatisticType.HP, 10},
                    {23, 0},
                    {3, 4},
                    {7, 4},
                    {13, 0},
                    {10, 4},
                    {4, 4},
                    {12, 0}
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
        Return level.Id = 5 AndAlso location.LocationType = LocationType.FromName(DungeonBoss)
    End Function
End Class
