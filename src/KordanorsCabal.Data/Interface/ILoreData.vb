Public Interface ILoreData
    Function ReadAll() As IEnumerable(Of Long)
    Function ReadText(loreId As Long) As String
    Function ReadItemName(loreId As Long) As String
End Interface
