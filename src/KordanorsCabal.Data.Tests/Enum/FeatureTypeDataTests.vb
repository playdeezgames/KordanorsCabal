Public Class FeatureTypeDataTests
    Inherits WorldDataSubobjectTests(Of IFeatureTypeData)
    Sub New()
        MyBase.New(Function(x) x.FeatureType)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForAllFeatureTypes()
        WithSubobject(
            Sub(store, subject)
                subject.ReadAll().ShouldBeNull
                store.Verify(
                    Function(x) x.ReadRecords(Of Long)(
                    It.IsAny(Of Action),
                    Tables.FeatureTypes,
                    Columns.FeatureTypeIdColumn))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheInteractionModeOfAGivenFeatureType()
        WithSubobject(
            Sub(store, subject)
                Dim featureType = 1L
                subject.ReadInteractionMode(featureType).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.FeatureTypes,
                    Columns.InteractionModeColumn,
                    (Columns.FeatureTypeIdColumn, featureType)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheLocationTypeOfAGivenFeatureType()
        WithSubobject(
            Sub(store, subject)
                Dim featureType = 1L
                subject.ReadLocationType(featureType).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.FeatureTypes,
                    Columns.LocationTypeColumn,
                    (Columns.FeatureTypeIdColumn, featureType)))
            End Sub)
    End Sub
End Class
