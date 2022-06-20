Friend Class WestDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.East
        End Get
    End Property
End Class
