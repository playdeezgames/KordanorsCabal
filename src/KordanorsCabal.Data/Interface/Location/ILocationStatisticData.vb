Public Interface ILocationStatisticData
    Function Read(locationId As Long, statisticType As Long) As Long?
    Sub Write(locationId As Long, statisticType As Long, statisticValue As Long?)
End Interface
