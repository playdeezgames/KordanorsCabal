Public Interface ICharacterStatistics
    Inherits IBaseThingie
    Sub SetStatistic(statisticType As IStatisticType, statisticValue As Long)
    Function GetStatistic(statisticType As IStatisticType) As Long?
    Sub ChangeStatistic(statisticType As IStatisticType, delta As Long)
End Interface
