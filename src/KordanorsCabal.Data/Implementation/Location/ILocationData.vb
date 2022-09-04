Public Interface ILocationData
    Function ReadLocationType(locationId As Long) As Long?
    Sub WriteLocationType(locationId As Long, locationType As Long)
    Function ReadForLocationType(locationType As Long) As IEnumerable(Of Long)
    Function Create(locationType As Long) As Long
End Interface
