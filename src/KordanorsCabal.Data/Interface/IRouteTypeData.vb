Public Interface IRouteTypeData
    Function ReadAbbreviation(routeTypeId As Long) As String
    Function ReadIsSingleUse(routeTypeId As Long) As Boolean
End Interface
