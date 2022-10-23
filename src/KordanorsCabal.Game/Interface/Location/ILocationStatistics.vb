Public Interface ILocationStatistics
    Inherits IBaseThingie
    Sub SetStatistic(statisticType As OldLocationStatisticType, statisticValue As Long?)
    Function GetStatistic(statisticType As OldLocationStatisticType) As Long?
End Interface
