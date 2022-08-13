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
End Class
