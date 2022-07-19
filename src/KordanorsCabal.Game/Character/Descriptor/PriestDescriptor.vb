﻿Friend Class PriestDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 3},
                    {CharacterStatisticType.Strength, 6},
                    {CharacterStatisticType.Dexterity, 3},
                    {CharacterStatisticType.HP, 3},
                    {CharacterStatisticType.Influence, 3},
                    {CharacterStatisticType.MP, 3},
                    {CharacterStatisticType.Stress, 0},
                    {CharacterStatisticType.UnarmedMaximumDamage, 3},
                    {CharacterStatisticType.Willpower, 3},
                    {CharacterStatisticType.Wounds, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Single
        Get
            Return 0
        End Get
    End Property

    Public Overrides ReadOnly Property SpawnCount(level As Long) As Long
        Get
            Select Case level
                Case 2
                    Return 1
                Case 3
                    Return 15
                Case 4
                    Return 30
                Case 5
                    Return 45
                Case Else
                    Return 0
            End Select
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Priest"
        End Get
    End Property

    Public Overrides ReadOnly Property XPValue As Long
        Get
            Return 5
        End Get
    End Property

    Public Overrides Sub DropLoot(location As Location)
        'TODO
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function

    Public Overrides Function CanSpawn(location As Location, level As Long) As Boolean
        Select Case level
            Case 2
                Return location.LocationType = LocationType.DungeonBoss
            Case Else
                Return Trued
        End Select
    End Function
End Class
