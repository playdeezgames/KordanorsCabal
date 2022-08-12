Public Module DirectionUtility
    Friend ReadOnly Property AllDirections As IEnumerable(Of Direction)
        Get
            Return StaticWorldData.World.Direction.ReadAll.Select(Function(x) New Direction(x))
        End Get
    End Property
    Friend ReadOnly Property CardinalDirections As IEnumerable(Of Direction)
        Get
            Return AllDirections.Where(Function(x) x.IsCardinal)
        End Get
    End Property
    Public Const North = 1L
    Public Const East = 2L
    Public Const South = 3L
    Public Const West = 4L
    Public Const Up = 5L
    Public Const Down = 6L
    Public Const Inward = 7L
    Public Const Outward = 8L
End Module
