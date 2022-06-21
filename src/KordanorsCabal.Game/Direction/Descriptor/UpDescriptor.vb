Friend Class UpDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.Down
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "up"
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "UP"
        End Get
    End Property
End Class
