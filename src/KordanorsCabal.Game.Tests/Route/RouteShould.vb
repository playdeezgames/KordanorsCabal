Public Class RouteShould
    Inherits ThingieShould(Of IRoute)

    Public Sub New()
        MyBase.New(AddressOf Route.FromId)
    End Sub
    <Fact>
    Sub have_route_type()
        WithSubject(
            Sub(worldData, id, subject)
                Const routeTypeId = 2L
                worldData.Setup(Function(x) x.Route.ReadRouteType(It.IsAny(Of Long))).Returns(routeTypeId)
                subject.RouteType.Id.ShouldBe(routeTypeId)
                worldData.Verify(Function(x) x.Route.ReadRouteType(id))
            End Sub)
    End Sub
    <Fact>
    Sub destroy()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Sub(x) x.Route.Clear(It.IsAny(Of Long)))
                subject.Destroy()
                worldData.Verify(Sub(x) x.Route.Clear(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_move()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                Const routeTypeId = 3L
                worldData.Setup(Function(x) x.Route.ReadRouteType(It.IsAny(Of Long))).Returns(routeTypeId)
                worldData.Setup(Function(x) x.RouteTypeLock.ReadUnlockItem(It.IsAny(Of Long)))
                subject.CanMove(Character.FromId(worldData.Object, characterId))
                worldData.Verify(Function(x) x.Route.ReadRouteType(id))
                worldData.Verify(Function(x) x.RouteTypeLock.ReadUnlockItem(routeTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_to_location()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Route.ReadToLocation(It.IsAny(Of Long)))
                subject.ToLocation.ShouldBeNull
                worldData.Verify(Function(x) x.Route.ReadToLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub move()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                Const routeTypeId = 3L
                worldData.Setup(Function(x) x.Route.ReadRouteType(It.IsAny(Of Long))).Returns(routeTypeId)
                worldData.Setup(Function(x) x.RouteTypeLock.ReadUnlockItem(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.RouteType.ReadIsSingleUse(It.IsAny(Of Long)))
                subject.Move(Character.FromId(worldData.Object, characterId))
                worldData.Verify(Function(x) x.Route.ReadRouteType(id))
                worldData.Verify(Function(x) x.Route.ReadToLocation(id))
                worldData.Verify(Function(x) x.RouteTypeLock.ReadUnlockItem(routeTypeId))
                worldData.Verify(Function(x) x.RouteType.ReadIsSingleUse(routeTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
