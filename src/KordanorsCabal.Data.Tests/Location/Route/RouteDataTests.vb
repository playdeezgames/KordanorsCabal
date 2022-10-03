﻿Public Class RouteDataTests
    Inherits WorldDataSubobjectTests(Of IRouteData)
    Sub New()
        MyBase.New(Function(x) x.Route)
    End Sub
    <Fact>
    Sub ShouldClearTheStoreOfAGivenRoute()
        WithSubobject(
            Sub(store, checker, subject)
                Dim routeId = 1L
                subject.Clear(routeId)
                store.Verify(
                    Sub(x) x.ClearForColumnValue(Of Long)(
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
                subject.Create(locationId, direction, routeType, toLocationId)
                store.Verify(
                    Function(x) x.CreateRecord(Of Long, Long, Long, Long)(
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
                subject.ReadCountForLocation(locationId).ShouldBe(0)
                store.Verify(
                    Function(x) x.ReadCountForColumnValue(
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
                subject.ReadDirectionRouteForLocation(locationId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadRecordsWithColumnValue(Of Long, Long, Long)(
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
                subject.ReadDirectionRouteTypeForLocation(locationId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadRecordsWithColumnValue(Of Long, Long, Long)(
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
                subject.ReadForLocationDirection(locationId, direction).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long, Long)(
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
                subject.ReadForLocationRouteType(locationId, routeType).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadRecordsWithColumnValues(Of Long, Long, Long)(
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
                subject.ReadToLocation(routeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
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
                subject.ReadRouteType(routeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
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
                subject.WriteRouteType(routeId, routeTypeId)
                store.Verify(
                    Sub(x) x.WriteColumnValue(Of Long, Long)(
                            It.IsAny(Of Action),
                            Tables.Routes,
                            (Columns.RouteTypeIdColumn, routeTypeId),
                            (Columns.RouteIdColumn, routeId)))
            End Sub)
    End Sub
End Class