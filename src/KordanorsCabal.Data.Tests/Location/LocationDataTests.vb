Public Class LocationDataTests
    Inherits WorldDataSubobjectTests(Of ILocationData)
    Sub New()
        MyBase.New(Function(x) x.Location)
    End Sub
    <Fact>
    Sub ShouldCreateANewLocationInTheStore()
        WithSubobject(
            Sub(store, subject)
                Dim locationType = 1L
                subject.Create(locationType).ShouldBe(0)
                store.Verify(
                    Function(x) x.CreateRecord(Of Long)(
                    It.IsAny(Of Action),
                    Tables.Locations,
                    (Columns.LocationTypeColumn, locationType)))
            End Sub)
    End Sub
End Class
