Public Interface IFeature
    Inherits IBaseThingie
    ReadOnly Property FeatureType As FeatureType
    ReadOnly Property Name As String
    ReadOnly Property InteractionMode As Long
End Interface
