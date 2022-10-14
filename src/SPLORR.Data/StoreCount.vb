Public Class StoreCount
    Inherits StoreBase
    Implements IStoreCount

    Public Sub New(backer As IBacker)
        MyBase.New(backer)
    End Sub
    Public Function ReadCountForColumnValue(Of TInputColumn)(initializer As Action, tableName As String, inputColumnValue As (String, TInputColumn)) As Long Implements IStoreCount.ReadCountForColumnValue
        initializer()
        Return ExecuteScalar(Of Long)(
            $"SELECT 
                COUNT(1) 
            FROM [{tableName}] 
            WHERE 
                [{inputColumnValue.Item1}]=@{inputColumnValue.Item1};",
            ($"@{inputColumnValue.Item1}", inputColumnValue.Item2)).Value
    End Function
End Class
