Public Interface ILocationStatisticData
    Sub Write(locationId As Long, statisticType As Long, statisticValue As Long?)
    Function Read(locationId As Long, statisticType As Long) As Long?
End Interface
