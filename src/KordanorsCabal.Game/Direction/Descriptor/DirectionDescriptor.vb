Friend Class DirectionDescriptor
    Sub New(directionId As Long)
        Id = directionId
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
        Get
            Dim result = StaticWorldData.World.Direction.ReadNext(Id)
            Return If(result.HasValue, CType(result.Value, Direction), Nothing)
        End Get
    End Property
    ReadOnly Property PreviousDirection As Direction?
        Get
            Dim result = StaticWorldData.World.Direction.ReadPrevious(Id)
            Return If(result.HasValue, CType(result.Value, Direction), Nothing)
        End Get
    End Property
End Class
Friend Module DirectionDescriptorUtility
    Friend DirectionDescriptors As IReadOnlyDictionary(Of Direction, DirectionDescriptor) =
        New Dictionary(Of Direction, DirectionDescriptor) From
        {
            {Direction.Down, New DirectionDescriptor(Direction.Down)},
            {Direction.East, New DirectionDescriptor(Direction.East)},
            {Direction.Inward, New DirectionDescriptor(Direction.Inward)},
            {Direction.North, New DirectionDescriptor(Direction.North)},
            {Direction.Outward, New DirectionDescriptor(Direction.Outward)},
            {Direction.South, New DirectionDescriptor(Direction.South)},
            {Direction.Up, New DirectionDescriptor(Direction.Up)},
            {Direction.West, New DirectionDescriptor(Direction.West)}
        }
    Friend ReadOnly Property AllDirections As IEnumerable(Of Direction)
        Get
            Return DirectionDescriptors.Keys
        End Get
    End Property
End Module
