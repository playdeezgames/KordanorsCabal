Public Interface ILocationData
    Function Create(locationType As Long) As Long
    Function ReadForLocationType(locationType As Long) As IEnumerable(Of Long)
    Function ReadLocationType(locationId As Long) As Long?
    Sub WriteLocationType(locationId As Long, locationType As Long)
End Interface
