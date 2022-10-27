Public Interface IItemEventData
    Sub Write(itemId As Long, eventId As Long, eventName As String)
    Sub ClearForItem(itemId As Long)
End Interface
