﻿Public Interface IItemEventData
    Sub Write(itemId As Long, eventId As Long, eventName As String)
    Sub ClearForItem(itemId As Long)
    Function Read(itemId As Long, eventId As Long) As String
End Interface
