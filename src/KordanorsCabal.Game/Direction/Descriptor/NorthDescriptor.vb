Friend Class NorthDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.South
        End Get
    End Property
End Class
