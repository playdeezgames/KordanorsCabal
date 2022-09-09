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
                    (Columns.LocationTypeIdColumn, locationType)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForAListOfLocationsWithAGivenLocationType()
        WithSubobject(
            Sub(store, subject)
                Dim locationType = 1L
                subject.ReadForLocationType(locationType).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadRecordsWithColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Locations,
                    Columns.LocationIdColumn,
                    (Columns.LocationTypeIdColumn, 1)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheLocationTypeOfAGivenLocation()
        WithSubobject(
            Sub(store, subject)
                Dim locationId = 1L
                subject.ReadLocationType(locationId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Locations,
                    Columns.LocationTypeIdColumn,
                    (Columns.LocationIdColumn, 1)))
            End Sub)
    End Sub
End Class
