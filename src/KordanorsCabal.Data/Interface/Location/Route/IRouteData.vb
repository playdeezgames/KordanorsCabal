﻿Public Interface IRouteData
    Sub Clear(routeId As Long)
    Function Create(locationId As Long, direction As Long, routeType As Long, toLocationId As Long) As Long
    Function ReadCountForLocation(locationId As Long) As Long
    Function ReadDirectionRouteForLocation(locationId As Long) As IEnumerable(Of Tuple(Of Long, Long))
    Function ReadDirectionRouteTypeForLocation(locationId As Long) As IEnumerable(Of Tuple(Of Long, Long))
    Function ReadForLocationDirection(locationId As Long, direction As Long) As Long?
    Function ReadForLocationRouteType(locationId As Long, routeType As Long) As IEnumerable(Of Long)
    Function ReadToLocation(routeId As Long) As Long?
    Function ReadRouteType(routeId As Long) As Long?
    Sub WriteRouteType(routeId As Long, routeType As Long)
End Interface
