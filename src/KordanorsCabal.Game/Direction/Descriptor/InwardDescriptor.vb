Friend Class InwardDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.Outward
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "in"
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "IN"
        End Get
    End Property
End Class
