Public Class Feature
    Inherits BaseThingie
    Implements IFeature
    Sub New(worldData As IWorldData, featureId As Long)
        MyBase.New(worldData, featureId)
    End Sub
    Shared Function FromId(worldData As IWorldData, featureId As Long?) As IFeature
        Return If(featureId.HasValue, New Feature(worldData, featureId.Value), Nothing)
    End Function

    Friend Shared Function Create(worldData As IWorldData, featureType As FeatureType, location As ILocation) As IFeature
        Return FromId(worldData, worldData.Feature.Create(featureType.Id, location.Id))
    End Function

    ReadOnly Property FeatureType As FeatureType Implements IFeature.FeatureType
        Get
            Return New FeatureType(WorldData, WorldData.Feature.ReadFeatureType(Id).Value)
        End Get
    End Property

    ReadOnly Property Name As String Implements IFeature.Name
        Get
            Return FeatureType.Name
        End Get
    End Property

    ReadOnly Property InteractionMode As PlayerMode Implements IFeature.InteractionMode
        Get
            Return FeatureType.InteractionMode
        End Get
    End Property
End Class
