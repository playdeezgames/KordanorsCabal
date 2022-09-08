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
End Class
