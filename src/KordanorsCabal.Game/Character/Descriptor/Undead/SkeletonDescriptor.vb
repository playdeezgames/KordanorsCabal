﻿Friend Class SkeletonDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 2},
                    {CharacterStatisticType.Strength, 4},
                    {CharacterStatisticType.Dexterity, 2},
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

    Private ReadOnly spawnCountTable As IReadOnlyDictionary(Of DungeonLevel, Long) =
        New Dictionary(Of DungeonLevel, Long) From
        {
            {DungeonLevel.Level1, 30},
            {DungeonLevel.Level2, 45},
            {DungeonLevel.Level3, 30},
            {DungeonLevel.Level4, 15}
        }

    Public Overrides ReadOnly Property SpawnCount(level As DungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level, result), result, 0)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "skeleton"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 1
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d4") > 1 Then
            location.Inventory.Add(Item.Create(ItemType.SkullFragment))
        End If

    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = CharacterType.N00b
    End Function
    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return location.LocationType = LocationType.Dungeon
    End Function
    Public Overrides Function RollMoneyDrop() As Long
        Return RNG.RollDice("2d3")
    End Function
    Public Overrides ReadOnly Property IsUndead As Boolean
        Get
            Return True
        End Get
    End Property
End Class
