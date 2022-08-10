Friend Class ChurchEntranceDescriptor
    Inherits LocationType

    Public Sub New()
        MyBase.New(3)
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Church Entrance"
        End Get
    End Property

    Public Overrides ReadOnly Property IsDungeon As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property CanMap As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property RequiresMP As Boolean
        Get
            Return False
        End Get
    End Property
End Class
