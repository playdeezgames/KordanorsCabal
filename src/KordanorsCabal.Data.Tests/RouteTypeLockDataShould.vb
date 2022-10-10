﻿Public Class RouteTypeLockDataShould
    Inherits WorldDataSubobjectTests(Of IRouteTypeLockData)
    Sub New()
        MyBase.New(Function(x) x.RouteTypeLock)
    End Sub
    <Fact>
    Sub have_unlocked_route_type()
        WithSubobject(
            Sub(store, events, subject)
                Const routeTypeId = 1L
                subject.ReadUnlockedRouteType(routeTypeId).ShouldBeNull
                store.Verify(Function(x) x.ReadColumnValue(Of Long, Long)(It.IsAny(Of Action), Tables.RouteTypeLocks, Columns.UnlockedRouteTypeIdColumn, (Columns.RouteTypeIdColumn, routeTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub have_unlock_item()
        WithSubobject(
            Sub(store, events, subject)
                Const routeTypeId = 1L
                subject.ReadUnlockItem(routeTypeId).ShouldBeNull
                store.Verify(Function(x) x.ReadColumnValue(Of Long, Long)(It.IsAny(Of Action), Tables.RouteTypeLocks, Columns.UnlockItemTypeIdColumn, (Columns.RouteTypeIdColumn, routeTypeId)))
            End Sub)
    End Sub
End Class
