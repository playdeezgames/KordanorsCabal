Public Class RouteDataTests
    Inherits WorldDataSubobjectTests(Of IRouteData)
    Sub New()
        MyBase.New(Function(x) x.Route)
    End Sub
    <Fact>
    Sub ShouldClearTheStoreOfAGivenRoute()
        WithSubobject(
            Sub(store, checker, subject)
                Dim routeId = 1L
                store.Setup(Sub(x) x.Clear.ForValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.Clear(routeId)
                store.Verify(
                    Sub(x) x.Clear.ForValue(Of Long)(
                            It.IsAny(Of Action),
                            Tables.Routes,
                            (Columns.RouteIdColumn, routeId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldCreateARoute()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationId = 1L
                Dim direction = 2L
                Dim routeType = 3L
                Dim toLocationId = 4L
                store.SetupGet(Function(x) x.Create).Returns((New Mock(Of IStoreCreate)).Object)
                subject.Create(locationId, direction, routeType, toLocationId)
                store.Verify(
                    Function(x) x.Create.Entry(Of Long, Long, Long, Long)(
                            It.IsAny(Of Action),
                            Tables.Routes,
                            (Columns.LocationIdColumn, locationId),
                            (Columns.DirectionIdColumn, direction),
                            (Columns.RouteTypeIdColumn, routeType),
                            (Columns.ToLocationIdColumn, toLocationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForHowManyRoutesAreAtAGivenLocation()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationId = 1L
                store.Setup(Function(x) x.Count.ForValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ReadCountForLocation(locationId).ShouldBe(0)
                store.Verify(
                    Function(x) x.Count.ForValue(
                            It.IsAny(Of Action),
                            Tables.Routes,
                            (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForDirectionsAndRoutesForAGivenLocation()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationId = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadDirectionRouteForLocation(locationId).ShouldBeNull
                store.Verify(
                    Function(x) x.Record.ReadRecordsWithColumnValue(Of Long, Long, Long)(
                            It.IsAny(Of Action),
                            Tables.Routes,
                            (Columns.DirectionIdColumn, Columns.RouteIdColumn),
                            (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForDirectionsAndRoutesTypesForAGivenLocation()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationId = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadDirectionRouteTypeForLocation(locationId).ShouldBeNull
                store.Verify(
                    Function(x) x.Record.ReadRecordsWithColumnValue(Of Long, Long, Long)(
                            It.IsAny(Of Action),
                            Tables.Routes,
                            (Columns.DirectionIdColumn, Columns.RouteTypeIdColumn),
                            (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForARouteWithAGivenLocationAndGivenDirection()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationId = 1L
                Dim direction = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadForLocationDirection(locationId, direction).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long, Long)(
                            It.IsAny(Of Action),
                            Tables.Routes,
                            Columns.RouteIdColumn,
                            (Columns.LocationIdColumn, locationId),
                            (Columns.DirectionIdColumn, direction)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForRoutesInAGivenLocationWithAGivenRouteType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationId = 1L
                Dim routeType = 2L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadForLocationRouteType(locationId, routeType).ShouldBeNull
                store.Verify(
                    Function(x) x.Record.ReadRecordsWithColumnValues(Of Long, Long, Long)(
                            It.IsAny(Of Action),
                            Tables.Routes,
                            Columns.RouteIdColumn,
                            (Columns.LocationIdColumn, locationId),
                            (Columns.RouteTypeIdColumn, routeType)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheToLocationOfAGivenRoute()
        WithSubobject(
            Sub(store, checker, subject)
                Dim routeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadToLocation(routeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                            It.IsAny(Of Action),
                            Tables.Routes,
                            Columns.ToLocationIdColumn,
                            (Columns.RouteIdColumn, routeId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheRouteTypeOfAGivenRoute()
        WithSubobject(
            Sub(store, checker, subject)
                Dim routeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadRouteType(routeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                            It.IsAny(Of Action),
                            Tables.Routes,
                            Columns.RouteTypeIdColumn,
                            (Columns.RouteIdColumn, routeId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreAndWriteAGivenRouteTypeToAGivenRoute()
        WithSubobject(
            Sub(store, checker, subject)
                Dim routeId = 1L
                Dim routeTypeId = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.WriteRouteType(routeId, routeTypeId)
                store.Verify(
                    Sub(x) x.Column.Write(Of Long, Long)(
                            It.IsAny(Of Action),
                            Tables.Routes,
                            (Columns.RouteTypeIdColumn, routeTypeId),
                            (Columns.RouteIdColumn, routeId)))
            End Sub)
    End Sub
End Class
