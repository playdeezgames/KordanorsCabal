Public Interface IFeatureType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property LocationType As ILocationType
    ReadOnly Property InteractionMode As Long
End Interface
