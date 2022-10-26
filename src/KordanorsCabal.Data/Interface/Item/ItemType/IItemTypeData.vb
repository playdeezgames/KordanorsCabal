Public Interface IItemTypeData
    Function ReadName(itemTypeId As Long) As String
    Function ReadAll() As IEnumerable(Of Long)
End Interface
