Public Interface IItemStatisticData
    Sub ClearForItem(itemId As Long)
    Function Read(itemId As Long, statisticType As Long) As Long?
    Sub Write(itemId As Long, statisticType As Long, value As Long)
End Interface
