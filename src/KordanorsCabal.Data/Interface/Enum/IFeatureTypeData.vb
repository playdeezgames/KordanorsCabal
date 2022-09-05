Public Interface IFeatureTypeData
    Function ReadName(featureType As Long) As String
    Function ReadLocationType(featureType As Long) As Long?
    Function ReadInteractionMode(featureType As Long) As Long?
    Function ReadAll() As IEnumerable(Of Long)
End Interface
