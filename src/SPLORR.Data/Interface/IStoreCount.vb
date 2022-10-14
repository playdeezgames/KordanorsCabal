Public Interface IStoreCount
    Function ReadCountForColumnValue(Of TInputColumn)(
                                                     initializer As Action,
                                                     tableName As String,
                                                     inputColumnValue As (String, TInputColumn)
                                                     ) As Long
End Interface
