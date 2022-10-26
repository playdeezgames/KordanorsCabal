Public Interface IItemTypeStatisticData
    Function Read(itemType As Long, statisticType As Long) As Long?
    Function ReadAll(itemType As Long) As IEnumerable(Of (Long, Long))
End Interface
