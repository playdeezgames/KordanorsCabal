Public Interface IItemLoreData
    Sub ClearForItem(itemId As Long)
    Sub Write(itemId As Long, loreId As Long)
    Function ReadForItem(itemId As Long) As Long?
    Function ReadAllLore() As IEnumerable(Of Long)
End Interface
