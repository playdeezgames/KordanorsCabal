Friend Class OutwardDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.Inward
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "out"
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "OUT"
        End Get
    End Property
End Class
