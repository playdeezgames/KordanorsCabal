﻿Friend Class BatDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return location.LocationType = LocationType.FromName(Dungeon)
    End Function

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 2},
                    {OldCharacterStatisticType.Strength, 2},
                    {OldCharacterStatisticType.Dexterity, 4},
                    {OldCharacterStatisticType.HP, 1},
                    {23, 0},
                    {3, 0},
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

    Private ReadOnly spawnCountTable As IReadOnlyDictionary(Of Long, Long) =
        New Dictionary(Of Long, Long) From
        {
            {1, 15},
            {2, 30},
            {3, 45},
            {4, 30},
            {5, 15}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level.Id, result), result, 0)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Bat"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 1
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d2") > 1 Then
            location.Inventory.Add(Item.Create(ItemType.BatWing))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = CharacterType.N00b
    End Function
End Class
