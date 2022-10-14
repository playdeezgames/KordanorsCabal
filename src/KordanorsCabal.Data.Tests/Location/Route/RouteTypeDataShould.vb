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
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadAbbreviation(routeTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(
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
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadIsSingleUse(routeTypeId).ShouldBeFalse
                store.Verify(Function(x) x.Column.ReadValue(Of Long, Long)(It.IsAny(Of Action), Tables.RouteTypes, Columns.IsSingleUseColumn, (Columns.RouteTypeIdColumn, routeTypeId)))
            End Sub)
    End Sub
End Class
