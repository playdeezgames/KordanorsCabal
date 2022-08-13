Friend Class PriestDescriptor
    Inherits CharacterType

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Select Case level.Id
            Case 2
                Return location.LocationType = LocationType.FromName(DungeonBoss)
            Case Else
                Return True
        End Select
    End Function
End Class
