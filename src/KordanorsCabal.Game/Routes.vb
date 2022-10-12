Public Class Routes
    Inherits BaseThingie
    Implements IRoutes
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As IRoutes
        Return If(id.HasValue, New Routes(worldData, id.Value), Nothing)
    End Function
    Public Function GetRoute(direction As IDirection) As IRoute Implements IRoutes.GetRoute
        Return Route.FromId(WorldData, WorldData.Route.ReadForLocationDirection(Id, direction.Id))
    End Function
    Public Function HasRoute(direction As IDirection) As Boolean Implements IRoutes.HasRoute
        Return GetRoute(direction) IsNot Nothing
    End Function
End Class
