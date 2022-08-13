Friend Class RatDescriptor
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
        Return character.CharacterType = OldCharacterType.N00b
    End Function
End Class
