Public Class RouteDataTests
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
End Class
