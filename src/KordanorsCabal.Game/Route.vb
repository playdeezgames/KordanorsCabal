Public Class Route
    Public ReadOnly Id As Long
    Public Sub New(routeId As Long)
        Id = routeId
    End Sub
    Public Shared Function FromId(routeId As Long) As Route
        Return New Route(routeId)
    End Function
    Public Shared Function Create(location As Location, direction As Direction, routeType As RouteType, toLocation As Location) As Route
        Return FromId(RouteData.Create(location.Id, direction, routeType, toLocation.Id))
    End Function
End Class
