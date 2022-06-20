Friend Class DownDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.Up
        End Get
    End Property
End Class
