Friend Class DirectionDescriptor
    Sub New(
           directionId As Long,
           Optional nextDirection As Direction? = Nothing,
           Optional previousDirection As Direction? = Nothing)
        Id = directionId
        Me.NextDirection = nextDirection
        Me.PreviousDirection = previousDirection
    End Sub
    '[Directions]([DirectionId],[DirectionName],[Abbreviation],[IsCardinal],[OppositeDirectionId],[NextDirectionId],[PreviousDirectionId])
    ReadOnly Property Id As Long
    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.Direction.ReadName(Id)
        End Get
    End Property
    ReadOnly Property Abbreviation As String
        Get
            Return StaticWorldData.World.Direction.ReadAbbreviation(Id)
        End Get
    End Property
    ReadOnly Property Opposite As Direction
        Get
            Return CType(StaticWorldData.World.Direction.ReadOpposite(Id).Value, Direction)
        End Get
    End Property
    ReadOnly Property IsCardinal As Boolean
        Get
            Return StaticWorldData.World.Direction.ReadIsCardinal(Id)
        End Get
    End Property
    ReadOnly Property NextDirection As Direction?
    ReadOnly Property PreviousDirection As Direction?
End Class
Friend Module DirectionDescriptorUtility
    Friend DirectionDescriptors As IReadOnlyDictionary(Of Direction, DirectionDescriptor) =
        New Dictionary(Of Direction, DirectionDescriptor) From
        {
            {Direction.Down, New DirectionDescriptor(Direction.Down)},
            {Direction.East, New DirectionDescriptor(Direction.East, Direction.South, Direction.North)},
            {Direction.Inward, New DirectionDescriptor(Direction.Inward)},
            {Direction.North, New DirectionDescriptor(Direction.North, Direction.East, Direction.West)},
            {Direction.Outward, New DirectionDescriptor(Direction.Outward)},
            {Direction.South, New DirectionDescriptor(Direction.South, Direction.West, Direction.East)},
            {Direction.Up, New DirectionDescriptor(Direction.Up)},
            {Direction.West, New DirectionDescriptor(Direction.West, Direction.North, Direction.South)}
        }
    Friend ReadOnly Property AllDirections As IEnumerable(Of Direction)
        Get
            Return DirectionDescriptors.Keys
        End Get
    End Property
End Module
