Public Module StaticStore
    Public ReadOnly Store As New Store
    Public Function ReadCountForColumnValue(Of TInputColumn)(initializer As Action, tableName As String, inputColumnValue As (String, TInputColumn)) As Long
        Return Store.ReadCountForColumnValue(initializer, tableName, inputColumnValue)
    End Function
End Module