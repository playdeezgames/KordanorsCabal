Public Interface IItemTypeEventData
    Function Read(itemTypeId As Long, eventId As Long) As String
    Function ReadAll(itemTypeId As Long) As List(Of Tuple(Of Long, String))
End Interface
