﻿Public Class FeatureDataTests
    Inherits WorldDataSubobjectTests(Of IFeatureData)
    Sub New()
        MyBase.New(Function(x) x.Feature)
    End Sub
    <Fact>
    Sub ShouldCreateAFeatureInTheStore()
        WithSubobject(
            Sub(store, subject)
                Dim featureType = 1L
                Dim locationId = 2L
                subject.Create(featureType, locationId).ShouldBe(0)
                store.Verify(
                    Function(x) x.CreateRecord(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Features,
                    (Columns.FeatureTypeColumn, featureType),
                    (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
End Class
