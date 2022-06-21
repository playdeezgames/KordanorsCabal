Friend Class EastDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.West
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "east"
        End Get
    End Property
    Public Overrides ReadOnly Property IsCardinal As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "E"
        End Get
    End Property
End Class
