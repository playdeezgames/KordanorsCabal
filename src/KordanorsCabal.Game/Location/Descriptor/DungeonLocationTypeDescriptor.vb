Friend Class DungeonLocationTypeDescriptor
    Inherits LocationDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Dungeon"
        End Get
    End Property

    Public Overrides ReadOnly Property IsDungeon As Boolean
        Get
            Return False
        End Get
    End Property
End Class
