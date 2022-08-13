Friend Class CabalLeaderDescriptor
    Inherits CharacterType

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub
    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id = 4 AndAlso location.LocationType = LocationType.FromName(DungeonBoss)
    End Function
End Class
