Friend Class MoonLocationTypeDescriptor
    Inherits LocationDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Moon"
        End Get
    End Property

    Public Overrides ReadOnly Property IsDungeon As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property CanMap As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property RequiresMP As Boolean
        Get
            Return True
        End Get
    End Property
End Class
