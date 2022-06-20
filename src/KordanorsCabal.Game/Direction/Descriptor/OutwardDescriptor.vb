Friend Class OutwardDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.Inward
        End Get
    End Property
End Class
