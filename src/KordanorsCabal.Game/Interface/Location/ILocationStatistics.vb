Public Interface ILocationStatistics
    Inherits IBaseThingie
    Sub SetStatistic(statisticType As LocationStatisticType, statisticValue As Long?)
    Function GetStatistic(statisticType As LocationStatisticType) As Long?
End Interface
