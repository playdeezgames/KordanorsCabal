Public Class FeatureDataTests
    Inherits WorldDataSubobjectTests(Of IFeatureData)
    Sub New()
        MyBase.New(Function(x) x.Feature)
    End Sub
    <Fact>
    Sub ShouldCreateAFeatureInTheStore()
        WithSubobject(
            Sub(store, checker, subject)
                Dim featureType = 1L
                Dim locationId = 2L
                store.SetupGet(Function(x) x.Create).Returns((New Mock(Of IStoreCreate)).Object)
                subject.Create(featureType, locationId).ShouldBe(0)
                store.Verify(
                    Function(x) x.Create.Entry(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Features,
                    (Columns.FeatureTypeIdColumn, featureType),
                    (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheFeatureTypeOfAGivenFeature()
        WithSubobject(
            Sub(store, checker, subject)
                Dim featureId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadFeatureType(featureId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Features,
                    Columns.FeatureTypeIdColumn,
                    (Columns.FeatureIdColumn, featureId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheFeatureWithinALocation()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadForLocation(locationId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Features,
                    Columns.FeatureIdColumn,
                    (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
End Class
