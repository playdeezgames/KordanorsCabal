Public Class Routes
    Inherits BaseThingie
    Implements IRoutes
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As IRoutes
        Return If(id.HasValue, New Routes(worldData, id.Value), Nothing)
    End Function
    Public Function Find(direction As IDirection) As IRoute Implements IRoutes.Find
        Return Route.FromId(WorldData, WorldData.Route.ReadForLocationDirection(Id, direction.Id))
    End Function
    Public Function Exists(direction As IDirection) As Boolean Implements IRoutes.Exists
        Return Find(direction) IsNot Nothing
    End Function
    Public ReadOnly Property RouteDirections As IEnumerable(Of IDirection) Implements IRoutes.RouteDirections
        Get
            Return WorldData.Route.ReadDirectionRouteForLocation(Id).Select(Function(x) New Direction(WorldData, x.Item1))
        End Get
    End Property
    Public ReadOnly Property RouteTypes As IEnumerable(Of IRouteType) Implements IRoutes.RouteTypes
        Get
            Return WorldData.Route.ReadDirectionRouteTypeForLocation(Id).Select(Function(x) RouteType.FromId(WorldData, x.Item2))
        End Get
    End Property
    ReadOnly Property RouteCount As Long Implements IRoutes.RouteCount
        Get
            Return WorldData.Route.ReadCountForLocation(Id)
        End Get
    End Property
    Sub DestroyRoute(direction As IDirection) Implements IRoutes.DestroyRoute
        Find(direction)?.Destroy()
    End Sub
    Public ReadOnly Property HasStairs As Boolean Implements IRoutes.HasStairs
        Get
            Return WorldData.Route.ReadForLocationRouteType(Id, 3).Any()
        End Get
    End Property
End Class
