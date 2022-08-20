Public Class Feature
    ReadOnly Property Id As Long
    Sub New(featureId As Long)
        Id = featureId
    End Sub
    Shared Function FromId(featureId As Long?) As Feature
        Return If(featureId.HasValue, New Feature(featureId.Value), Nothing)
    End Function

    Friend Shared Function Create(featureType As FeatureType, location As Location) As Feature
        Return FromId(StaticWorldData.World.Feature.Create(featureType.Id, location.Id))
    End Function

    ReadOnly Property FeatureType As FeatureType
        Get
            Return New FeatureType(StaticWorldData.World.Feature.ReadFeatureType(Id).Value)
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
