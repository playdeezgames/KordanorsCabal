Public Interface ICharacterStatistics
    Inherits IBaseThingie
    Sub SetStatistic(statisticType As ICharacterStatisticType, statisticValue As Long)
    Function GetStatistic(statisticType As ICharacterStatisticType) As Long?
    Sub ChangeStatistic(statisticType As ICharacterStatisticType, delta As Long)
End Interface
