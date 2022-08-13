﻿Friend Class BatDescriptor
    Inherits CharacterType

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return location.LocationType = LocationType.FromName(Dungeon)
    End Function
    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub
End Class
