Public Class FeatureType
    Inherits BaseThingie
    Implements IFeatureType
    Shared Function FromId(worldData As IWorldData, id As Long?) As IFeatureType
        Return If(id.HasValue, New FeatureType(worldData, id.Value), Nothing)
    End Function
    ReadOnly Property Name As String Implements IFeatureType.Name
        Get
            Return WorldData.FeatureType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property LocationType As ILocationType Implements IFeatureType.LocationType
        Get
            Return Game.LocationType.FromId(WorldData, WorldData.FeatureType.ReadLocationType(Id))
        End Get
    End Property
    ReadOnly Property InteractionMode As Long Implements IFeatureType.InteractionMode
        Get
            Return WorldData.FeatureType.ReadInteractionMode(Id).Value
        End Get
    End Property
    Sub New(worldData As IWorldData, featureTypeId As Long)
        MyBase.New(worldData, featureTypeId)
    End Sub
End Class
