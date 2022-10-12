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
                subject.Exists(Direction.FromId(worldData.Object, directionId)).ShouldBeFalse
                worldData.Verify(Function(x) x.Route.ReadForLocationDirection(id, directionId))
            End Sub)
    End Sub
    <Fact>
    Sub have_routes()
        WithSubject(
            Sub(worldData, id, subject)
                Const directionId = 2L
                worldData.Setup(Function(x) x.Route.ReadForLocationDirection(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Find(Direction.FromId(worldData.Object, directionId))
                worldData.Verify(Function(x) x.Route.ReadForLocationDirection(id, directionId))
            End Sub)
    End Sub
    <Fact>
    Sub have_route_types()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Route.ReadDirectionRouteTypeForLocation(It.IsAny(Of Long)))
                subject.RouteTypes.ShouldBeEmpty
                worldData.Verify(Function(x) x.Route.ReadDirectionRouteTypeForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_route_directions()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Route.ReadDirectionRouteForLocation(It.IsAny(Of Long)))
                subject.RouteDirections.ShouldBeEmpty
                worldData.Verify(Function(x) x.Route.ReadDirectionRouteForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub destroys_route()
        WithSubject(
            Sub(worldData, id, subject)
                Const directionId = 2L
                worldData.Setup(Function(x) x.Route.ReadForLocationDirection(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.DestroyRoute(Direction.FromId(worldData.Object, directionId))
                worldData.Verify(Function(x) x.Route.ReadForLocationDirection(id, directionId))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_route_count()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Route.ReadCountForLocation(It.IsAny(Of Long)))
                subject.RouteCount.ShouldBe(0)
                worldData.Verify(Function(x) x.Route.ReadCountForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_has_stairs()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Route.ReadForLocationRouteType(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.HasStairs.ShouldBeFalse
                worldData.Verify(Function(x) x.Route.ReadForLocationRouteType(id, 3))
            End Sub)
    End Sub
End Class
