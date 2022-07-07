﻿Friend Class CabalLeaderDescriptor
    Inherits CharacterTypeDescriptor

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Get
            Return New Dictionary(Of CharacterStatisticType, Long) From
                {
                    {CharacterStatisticType.BaseMaximumDefend, 4},
                    {CharacterStatisticType.Strength, 8},
                    {CharacterStatisticType.Dexterity, 4},
                    {CharacterStatisticType.HP, 3},
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
            Return 0
        End Get
    End Property

    Public Overrides ReadOnly Property SpawnCount(level As Long) As Long
        Get
            Select Case level
                Case 4
                    Return 1
                Case Else
                    Return 0
            End Select
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Leader"
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
            Case 4
                Return location.LocationType = LocationType.DungeonBoss
            Case Else
                Return False
        End Select
    End Function
End Class