Public Class FeatureTypeDataTests
    Inherits WorldDataSubobjectTests(Of IFeatureTypeData)
    Sub New()
        MyBase.New(Function(x) x.FeatureType)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForAllFeatureTypes()
        WithSubobject(
            Sub(store, checker, subject)
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadAll().ShouldBeNull
                store.Verify(
                    Function(x) x.Record.All(Of Long)(
                    It.IsAny(Of Action),
                    Tables.FeatureTypes,
                    Columns.FeatureTypeIdColumn))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheInteractionModeOfAGivenFeatureType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim featureType = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadInteractionMode(featureType).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.FeatureTypes,
                    Columns.InteractionModeColumn,
                    (Columns.FeatureTypeIdColumn, featureType)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheLocationTypeOfAGivenFeatureType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim featureType = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadLocationType(featureType).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.FeatureTypes,
                    Columns.LocationTypeIdColumn,
                    (Columns.FeatureTypeIdColumn, featureType)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheNameOfAGivenFeatureType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim featureType = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadName(featureType).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(Of Long)(
                    It.IsAny(Of Action),
                    Tables.FeatureTypes,
                    Columns.FeatureTypeNameColumn,
                    (Columns.FeatureTypeIdColumn, featureType)))
            End Sub)
    End Sub
End Class
