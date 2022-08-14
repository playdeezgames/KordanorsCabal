Public Class Feature
    ReadOnly Property Id As Long
    Sub New(featureId As Long)
        Id = featureId
    End Sub
    Shared Function FromId(featureId As Long?) As Feature
        Return If(featureId.HasValue, New Feature(featureId.Value), Nothing)
    End Function

    Friend Shared Function Create(featureType As OldFeatureType, location As Location) As Feature
        Return FromId(StaticWorldData.World.Feature.Create(featureType, location.Id))
    End Function

    ReadOnly Property FeatureType As OldFeatureType
        Get
            Return CType(StaticWorldData.World.Feature.ReadFeatureType(Id).Value, OldFeatureType)
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
