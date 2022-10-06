Public Interface IFeatureTypeData
    Function ReadAll() As IEnumerable(Of Long)
    Function ReadInteractionMode(featureType As Long) As Long?
    Function ReadLocationType(featureType As Long) As Long?
    Function ReadName(featureType As Long) As String
End Interface
