Public Interface IItemData
    Sub WriteItemType(itemId As Long, itemType As Long)
    Function Create(itemType As Long) As Long
    Function ReadItemType(itemId As Long) As Long?
    Sub Clear(itemId As Long)
End Interface
