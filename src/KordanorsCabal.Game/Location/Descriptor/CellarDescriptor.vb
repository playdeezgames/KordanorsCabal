Friend Class CellarDescriptor
    Inherits LocationType

    Public Sub New()
        MyBase.New(7)
    End Sub

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

    Public Overrides ReadOnly Property CanMap As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property RequiresMP As Boolean
        Get
            Return True
        End Get
    End Property
End Class
