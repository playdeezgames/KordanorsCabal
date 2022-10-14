Public Class LocationDataTests
    Inherits WorldDataSubobjectTests(Of ILocationData)
    Sub New()
        MyBase.New(Function(x) x.Location)
    End Sub
    <Fact>
    Sub ShouldCreateANewLocationInTheStore()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationType = 1L
                store.SetupGet(Function(x) x.Create).Returns((New Mock(Of IStoreCreate)).Object)
                subject.Create(locationType).ShouldBe(0)
                store.Verify(
                    Function(x) x.Create.CreateRecord(Of Long)(
                    It.IsAny(Of Action),
                    Tables.Locations,
                    (Columns.LocationTypeIdColumn, locationType)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForAListOfLocationsWithAGivenLocationType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationType = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadForLocationType(locationType).ShouldBeNull
                store.Verify(
                    Function(x) x.Record.ReadRecordsWithColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Locations,
                    Columns.LocationIdColumn,
                    (Columns.LocationTypeIdColumn, locationType)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheLocationTypeOfAGivenLocation()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadLocationType(locationId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Locations,
                    Columns.LocationTypeIdColumn,
                    (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreWithANewLocationTypeForAGivenLocation()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationId = 1L
                Dim locationType = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.WriteLocationType(locationId, locationType)
                store.Verify(
                    Sub(x) x.Column.Write(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Locations,
                    (Columns.LocationTypeIdColumn, locationType),
                    (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
End Class
