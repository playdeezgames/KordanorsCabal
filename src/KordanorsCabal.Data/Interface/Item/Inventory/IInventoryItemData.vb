﻿Public Interface IInventoryItemData
    Sub ClearForItem(itemId As Long)
    Function ReadItems(inventoryId As Long) As IEnumerable(Of Long)
    Sub Write(inventoryId As Long, itemId As Long)
    Function ReadForItem(itemId As Long) As Long?
End Interface
