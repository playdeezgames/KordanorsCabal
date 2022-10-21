Public Interface IItemData
    Sub Clear(itemId As Long)
    Function Create(itemType As Long) As Long
    Function ReadItemType(itemId As Long) As Long?
    Sub WriteItemType(itemId As Long, itemType As Long)
    Function ReadName(itemId As Long) As String
End Interface
