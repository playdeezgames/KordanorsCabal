Friend Class CellarDescriptor
    Inherits LocationDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Cellar"
        End Get
    End Property

    Public Overrides ReadOnly Property IsDungeon As Boolean
        Get
            Return True
        End Get
    End Property
End Class
