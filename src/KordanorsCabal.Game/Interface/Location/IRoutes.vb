Public Interface IRoutes
    Inherits IBaseThingie
    Function Exists(direction As IDirection) As Boolean
    Function Find(direction As IDirection) As IRoute
    ReadOnly Property Count As Long
    ReadOnly Property Directions As IEnumerable(Of IDirection)
    Sub DestroyRoute(direction As IDirection)
    ReadOnly Property HasStairs As Boolean
    ReadOnly Property RouteTypes As IEnumerable(Of IRouteType)
End Interface
