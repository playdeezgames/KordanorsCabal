Public Class Feature
    ReadOnly Property Id As Long
    Sub New(featureId As Long)
        Id = featureId
    End Sub
    Shared Function FromId(featureId As Long?) As Feature
        Return If(featureId.HasValue, New Feature(featureId.Value), Nothing)
    End Function

    Friend Shared Function Create(featureType As FeatureType, location As Location) As Feature
        Return FromId(FeatureData.Create(featureType, location.Id))
    End Function
End Class
