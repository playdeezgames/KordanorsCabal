﻿Friend Class CabalLeaderDescriptor
    Inherits CharacterType

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 4},
                    {1, 8},
                    {2, 4},
                    {6, 5},
                    {23, 0},
                    {3, 6},
                    {7, 3},
                    {13, 0},
                    {10, 4},
                    {4, 3},
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
            Return If(level.Id = 4, 1, 0)
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

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id = 4 AndAlso location.LocationType = LocationType.FromName(DungeonBoss)
    End Function
End Class
