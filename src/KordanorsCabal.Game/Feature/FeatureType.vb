Public Class FeatureType
    Inherits BaseThingie
    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.FeatureType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property LocationType As LocationType
        Get
            Return LocationType.FromId(StaticWorldData.World, StaticWorldData.World.FeatureType.ReadLocationType(Id))
        End Get
    End Property
    ReadOnly Property InteractionMode As PlayerMode
        Get
            Return CType(StaticWorldData.World.FeatureType.ReadInteractionMode(Id).Value, PlayerMode)
        End Get
    End Property
    Sub New(worldData As WorldData, featureTypeId As Long)
        MyBase.New(worldData, featureTypeId)
    End Sub
End Class
