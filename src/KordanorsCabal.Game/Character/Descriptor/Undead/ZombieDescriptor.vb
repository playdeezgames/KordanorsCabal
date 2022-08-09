﻿Friend Class ZombieDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 3},
                    {CharacterStatisticType.Strength, 4},
                    {CharacterStatisticType.Dexterity, 3},
                    {CharacterStatisticType.HP, 1},
                    {CharacterStatisticType.Immobilization, 0},
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

    Private ReadOnly spawnCountTable As IReadOnlyDictionary(Of OldDungeonLevel, Long) =
        New Dictionary(Of OldDungeonLevel, Long) From
        {
            {OldDungeonLevel.Level2, 30},
            {OldDungeonLevel.Level3, 45},
            {OldDungeonLevel.Level4, 30}
        }

    Public Overrides ReadOnly Property SpawnCount(level As OldDungeonLevel) As Long
        Get
            Dim result As Long = 0
            Return If(spawnCountTable.TryGetValue(level, result), result, 15)
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
    Public Overrides Function CanSpawn(location As Location, level As OldDungeonLevel) As Boolean
        Select Case level
            Case OldDungeonLevel.Level1
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
