Public Interface IStatisticTypeData
    Inherits INameCacheData
    Function ReadName(statisticType As Long) As String
    Function ReadMinimumValue(statisticType As Long) As Long?
    Function ReadMaximumValue(statisticType As Long) As Long?
    Function ReadDefaultValue(statisticType As Long) As Long?
    Function ReadAbbreviation(statisticType As Long) As String
End Interface
