Friend Class SouthDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.North
        End Get
    End Property
End Class
