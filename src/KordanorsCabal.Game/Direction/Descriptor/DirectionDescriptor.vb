Public Class DirectionDescriptor
    Sub New(directionId As Long)
        Id = directionId
    End Sub
    Sub New(directionName As String)
        Me.New(StaticWorldData.World.Direction.ReadForName(directionName).Value)
    End Sub
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
    ReadOnly Property Opposite As DirectionDescriptor
        Get
            Return New DirectionDescriptor(StaticWorldData.World.Direction.ReadOpposite(Id).Value)
        End Get
    End Property
    ReadOnly Property IsCardinal As Boolean
        Get
            Return StaticWorldData.World.Direction.ReadIsCardinal(Id)
        End Get
    End Property
    ReadOnly Property NextDirection As DirectionDescriptor
        Get
            Dim result = StaticWorldData.World.Direction.ReadNext(Id)
            Return If(result.HasValue, New DirectionDescriptor(result.Value), Nothing)
        End Get
    End Property
    ReadOnly Property PreviousDirection As DirectionDescriptor
        Get
            Dim result = StaticWorldData.World.Direction.ReadPrevious(Id)
            Return If(result.HasValue, New DirectionDescriptor(result.Value), Nothing)
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
    Friend ReadOnly Property CardinalDirections As IEnumerable(Of Direction)
        Get
            Return AllDirections.Where(Function(x) x.ToDescriptor.IsCardinal)
        End Get
    End Property
End Module
