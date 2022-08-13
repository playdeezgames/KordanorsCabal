﻿Friend Class KordanorDescriptor
    Inherits CharacterType

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 5},
                    {1, 8},
                    {2, 5},
                    {6, 10},
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

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id = 5 AndAlso location.LocationType = LocationType.FromName(DungeonBoss)
    End Function
End Class
