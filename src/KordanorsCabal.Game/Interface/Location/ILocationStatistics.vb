Public Interface ILocationStatistics
    Inherits IBaseThingie
    Sub SetStatistic(statisticType As ILocationStatisticType, statisticValue As Long?)
    Function GetStatistic(statisticType As ILocationStatisticType) As Long?
End Interface
