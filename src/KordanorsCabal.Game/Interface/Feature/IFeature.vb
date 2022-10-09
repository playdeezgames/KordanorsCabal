Public Interface IFeature
    Inherits IBaseThingie
    ReadOnly Property FeatureType As FeatureType
    ReadOnly Property Name As String
    ReadOnly Property InteractionMode As PlayerMode
End Interface
