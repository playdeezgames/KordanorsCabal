Public Class StoreClear
    Inherits StoreBase
    Implements IStoreClear

    Public Sub New(backer As IBacker)
        MyBase.New(backer)
    End Sub

    Public Sub ClearForColumnValue(Of TColumn)(initializer As Action, tableName As String, columnValue As (String, TColumn)) Implements IStoreClear.ClearForColumnValue
        initializer()
        backer.Execute($"DELETE FROM [{tableName}] WHERE [{columnValue.Item1}]=@{columnValue.Item1};", ($"@{columnValue.Item1}", columnValue.Item2))
    End Sub

    Public Sub ClearForColumnValues(Of TFirstColumn, TSecondColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn)) Implements IStoreClear.ClearForColumnValues
        initializer()
        backer.Execute(
            $"DELETE FROM [{tableName}] WHERE [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} AND [{secondColumnValue.Item1}]=@{secondColumnValue.Item1};",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
    End Sub
End Class
