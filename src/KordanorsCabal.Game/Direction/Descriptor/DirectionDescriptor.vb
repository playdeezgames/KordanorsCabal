Public Class DirectionDescriptor
    Sub New(directionId As Long)
        Id = directionId
    End Sub
    Private Sub New(directionName As String)
        Me.New(StaticWorldData.World.Direction.ReadForName(directionName).Value)
    End Sub
    Shared Function FromName(directionName As String) As DirectionDescriptor
        Return New DirectionDescriptor(directionName)
    End Function
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
    Friend ReadOnly Property AllDirections As IEnumerable(Of DirectionDescriptor)
        Get
            Return StaticWorldData.World.Direction.ReadAll.Select(Function(x) New DirectionDescriptor(x))
        End Get
    End Property
    Friend ReadOnly Property CardinalDirections As IEnumerable(Of DirectionDescriptor)
        Get
            Return AllDirections.Where(Function(x) x.IsCardinal)
        End Get
    End Property
End Module
