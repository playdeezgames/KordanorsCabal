Friend Class DownDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.Up
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "down"
        End Get
    End Property
End Class
