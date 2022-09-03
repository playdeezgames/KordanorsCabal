Public Class Feature
    Inherits BaseThingie
    Sub New(worldData As IWorldData, featureId As Long)
        MyBase.New(worldData, featureId)
    End Sub
    Shared Function FromId(worldData As IWorldData, featureId As Long?) As Feature
        Return If(featureId.HasValue, New Feature(worldData, featureId.Value), Nothing)
    End Function

    Friend Shared Function Create(worldData As IWorldData, featureType As FeatureType, location As Location) As Feature
        Return FromId(worldData, worldData.Feature.Create(featureType.Id, location.Id))
    End Function

    ReadOnly Property FeatureType As FeatureType
        Get
            Return New FeatureType(WorldData, WorldData.Feature.ReadFeatureType(Id).Value)
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
