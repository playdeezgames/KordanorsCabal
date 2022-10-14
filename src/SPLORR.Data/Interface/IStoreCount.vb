Public Interface IStoreCount
    Function ForValue(Of TInputColumn)(
                                                     initializer As Action,
                                                     tableName As String,
                                                     inputColumnValue As (String, TInputColumn)
                                                     ) As Long
End Interface
