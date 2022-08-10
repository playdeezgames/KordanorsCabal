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
    Public Const North = "North"
    Public Const East = "East"
    Public Const South = "South"
    Public Const West = "West"
    Public Const Up = "Up"
    Public Const Down = "Down"
    Public Const Inward = "In"
    Public Const Outward = "Out"
End Module
