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

    ReadOnly Property FeatureType As FeatureType
        Get
            Return CType(FeatureData.ReadFeatureType(Id).Value, FeatureType)
        End Get
    End Property

    ReadOnly Property Name As String
        Get
            Return FeatureType.Name
        End Get
    End Property

    Friend Function InteractionMode() As PlayerMode
        Return FeatureType.InteractionMode
    End Function
End Class
