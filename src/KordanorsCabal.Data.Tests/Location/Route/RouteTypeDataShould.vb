Public Class RouteTypeDataShould
    Inherits WorldDataSubobjectTests(Of IRouteTypeData)
    Public Sub New()
        MyBase.New(Function(x) x.RouteType)
    End Sub
    <Fact>
    Sub should_have_abbreviation()
        WithSubobject(
            Sub(store, evenets, subject)
                Const routeTypeId = 1L
                subject.ReadAbbreviation(routeTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnString(
                        It.IsAny(Of Action),
                        Tables.RouteTypes,
                        Columns.AbbreviationColumn,
                        (Columns.RouteTypeIdColumn, routeTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub should_have_is_single_use()
        WithSubobject(
            Sub(store, evenets, subject)
                Const routeTypeId = 1L
                subject.ReadIsSingleUse(routeTypeId).ShouldBeFalse
                store.Verify(Function(x) x.ReadColumnValue(Of Long, Long)(It.IsAny(Of Action), Tables.RouteTypes, Columns.IsSingleUseColumn, (Columns.RouteTypeIdColumn, routeTypeId)))
            End Sub)
    End Sub
End Class
