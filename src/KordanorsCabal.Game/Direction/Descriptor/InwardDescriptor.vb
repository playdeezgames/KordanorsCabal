Friend Class InwardDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.Outward
        End Get
    End Property
End Class
