Friend Class MoonPersonDescriptor
    Inherits CharacterType
    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = OldCharacterType.N00b
    End Function

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id = 6
    End Function

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub
End Class
