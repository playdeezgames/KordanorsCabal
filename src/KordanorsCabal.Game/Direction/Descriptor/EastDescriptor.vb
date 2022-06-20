Friend Class EastDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.West
        End Get
    End Property
End Class
