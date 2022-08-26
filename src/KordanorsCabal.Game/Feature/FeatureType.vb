Public Class FeatureType
    Inherits BaseThingie
    ReadOnly Property Name As String
        Get
            Return WorldData.FeatureType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property LocationType As LocationType
        Get
            Return LocationType.FromId(WorldData, WorldData.FeatureType.ReadLocationType(Id))
        End Get
    End Property
    ReadOnly Property InteractionMode As PlayerMode
        Get
            Return CType(WorldData.FeatureType.ReadInteractionMode(Id).Value, PlayerMode)
        End Get
    End Property
    Sub New(worldData As WorldData, featureTypeId As Long)
        MyBase.New(worldData, featureTypeId)
    End Sub
End Class
