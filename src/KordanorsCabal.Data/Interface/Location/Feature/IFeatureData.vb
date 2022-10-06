Public Interface IFeatureData
    Function Create(featureType As Long, locationId As Long) As Long
    Function ReadFeatureType(featureId As Long) As Long?
    Function ReadForLocation(locationId As Long) As Long?
End Interface
