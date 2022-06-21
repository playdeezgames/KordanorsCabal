Friend Class WestDescriptor
    Inherits DirectionDescriptor

    Public Overrides ReadOnly Property Opposite As Direction
        Get
            Return Direction.East
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "west"
        End Get
    End Property
    Public Overrides ReadOnly Property IsCardinal As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "W"
        End Get
    End Property

    Public Overrides ReadOnly Property NextDirection As Direction?
        Get
            Return Direction.North
        End Get
    End Property

    Public Overrides ReadOnly Property PreviousDirection As Direction?
        Get
            Return Direction.South
        End Get
    End Property
End Class
