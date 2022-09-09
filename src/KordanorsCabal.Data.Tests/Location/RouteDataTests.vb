﻿Public Class RouteDataTests
    Inherits WorldDataSubobjectTests(Of IRouteData)
    Sub New()
        MyBase.New(Function(x) x.Route)
    End Sub
    <Fact>
    Sub ShouldClearTheStoreOfAGivenRoute()
        WithSubobject(
            Sub(store, subject)
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
            Sub(store, subject)
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
End Class