﻿Friend Class MalcontentDescriptor
    Inherits CharacterType
    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id <> 1 OrElse location.LocationType = LocationType.FromName(DungeonDeadEnd)
    End Function

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub
End Class
