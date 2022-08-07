Friend Class DirectionDescriptor
    Sub New(
           name As String,
           abbreviation As String,
           opposite As Direction,
           Optional nextDirection As Direction? = Nothing,
           Optional previousDirection As Direction? = Nothing,
           Optional isCardinal As Boolean = False)
        Me.Name = name
        Me.Abbreviation = abbreviation
        Me.Opposite = opposite
        Me.NextDirection = nextDirection
        Me.PreviousDirection = previousDirection
        Me.IsCardinal = isCardinal
    End Sub
    ReadOnly Property Name As String
    ReadOnly Property Abbreviation As String
    ReadOnly Property Opposite As Direction
    ReadOnly Property IsCardinal As Boolean
    ReadOnly Property NextDirection As Direction?
    ReadOnly Property PreviousDirection As Direction?
End Class
Friend Module DirectionDescriptorUtility
    Friend DirectionDescriptors As IReadOnlyDictionary(Of Direction, DirectionDescriptor) =
        New Dictionary(Of Direction, DirectionDescriptor) From
        {
            {Direction.Down, New DirectionDescriptor("Down", "D", Direction.Up)},
            {Direction.East, New DirectionDescriptor("East", "E", Direction.West, Direction.South, Direction.North, True)},
            {Direction.Inward, New DirectionDescriptor("In", "In", Direction.Outward)},
            {Direction.North, New DirectionDescriptor("North", "N", Direction.South, Direction.East, Direction.West, True)},
            {Direction.Outward, New DirectionDescriptor("Out", "Out", Direction.Inward)},
            {Direction.South, New DirectionDescriptor("South", "S", Direction.North, Direction.West, Direction.East, True)},
            {Direction.Up, New DirectionDescriptor("Up", "U", Direction.Down)},
            {Direction.West, New DirectionDescriptor("West", "W", Direction.East, Direction.North, Direction.South, True)}
        }
    Friend ReadOnly Property AllDirections As IEnumerable(Of Direction)
        Get
            Return DirectionDescriptors.Keys
        End Get
    End Property
End Module
