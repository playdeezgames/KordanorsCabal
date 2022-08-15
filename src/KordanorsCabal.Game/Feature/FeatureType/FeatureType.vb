Public Class FeatureType
    ReadOnly Property Id As Long
    ReadOnly Property Name As String
    ReadOnly Property LocationType As LocationType
    ReadOnly Property InteractionMode As PlayerMode
    Sub New(featureTypeId As Long, name As String, locationType As LocationType, mode As PlayerMode)
        Me.Id = featureTypeId
        Me.Name = name
        Me.LocationType = locationType
        Me.InteractionMode = mode
    End Sub
End Class
