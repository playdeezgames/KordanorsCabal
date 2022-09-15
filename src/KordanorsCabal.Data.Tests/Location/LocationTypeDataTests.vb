Public Class LocationTypeDataTests
    Inherits WorldDataSubobjectTests(Of ILocationTypeData)
    Sub New()
        MyBase.New(Function(x) x.LocationType)
    End Sub

    <Fact>
    Sub location_types_have_names_fetched_from_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const locationTypeId = 1L
                subject.ReadName(locationTypeId)
                store.Verify(Function(x) x.ReadColumnString(Of Long)(
                                 It.IsAny(Of Action),
                                 Tables.LocationTypes,
                                 Columns.LocationTypeNameColumn,
                                 (Columns.LocationTypeIdColumn, locationTypeId)))
            End Sub)
    End Sub
End Class
