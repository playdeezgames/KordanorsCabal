Public Class RoutesShould
    Inherits ThingieShould(Of IRoutes)
    Sub New()
        MyBase.New(AddressOf Routes.FromId)
    End Sub
    <Fact>
    Sub has_route()
        WithSubject(
            Sub(worldData, id, subject)
                Const directionId = 2L
                worldData.Setup(Function(x) x.Route.ReadForLocationDirection(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.HasRoute(Direction.FromId(worldData.Object, directionId)).ShouldBeFalse
                worldData.Verify(Function(x) x.Route.ReadForLocationDirection(id, directionId))
            End Sub)
    End Sub
    <Fact>
    Sub have_routes()
        WithSubject(
            Sub(worldData, id, subject)
                Const directionId = 2L
                worldData.Setup(Function(x) x.Route.ReadForLocationDirection(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.GetRoute(Direction.FromId(worldData.Object, directionId))
                worldData.Verify(Function(x) x.Route.ReadForLocationDirection(id, directionId))
            End Sub)
    End Sub
End Class
