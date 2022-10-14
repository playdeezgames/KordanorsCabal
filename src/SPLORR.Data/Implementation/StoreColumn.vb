Public Class StoreColumn
    Inherits StoreBase
    Implements IStoreColumn

    Public Sub New(backer As IBacker)
        MyBase.New(backer)
    End Sub

    Public Function ReadColumnValues(Of TOutputColumn)(
                                                      initializer As Action,
                                                      tableName As String,
                                                      outputColumnName As String) As IEnumerable(Of TOutputColumn) Implements IStoreColumn.ReadColumnValues
        initializer()
        Return ExecuteReader(
            Function(reader) CType(reader(outputColumnName), TOutputColumn),
            $"SELECT [{outputColumnName}] FROM [{tableName}];")
    End Function
    Public Function ReadColumnValue(Of TInputColumn, TOutputColumn As Structure)(initializer As Action, tableName As String, outputColumnName As String, inputColumnValue As (String, TInputColumn)) As TOutputColumn? Implements IStoreColumn.ReadColumnValue
        initializer()
        Return ExecuteScalar(Of TOutputColumn)(
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{inputColumnValue.Item1}]=@{inputColumnValue.Item1};",
            ($"@{inputColumnValue.Item1}", inputColumnValue.Item2))
    End Function
    Public Function ReadColumnValue(Of TFirstInputColumn, TSecondInputColumn, TOutputColumn As Structure)(initializer As Action, tableName As String, outputColumnName As String, firstColumnValue As (String, TFirstInputColumn), secondColumnValue As (String, TSecondInputColumn)) As TOutputColumn? Implements IStoreColumn.ReadColumnValue
        initializer()
        Return ExecuteScalar(Of TOutputColumn)(
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} AND  [{secondColumnValue.Item1}]=@{secondColumnValue.Item1};",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
    End Function
    Public Function ReadColumnValue(Of TFirstInputColumn, TSecondInputColumn, TThirdInputColumn, TOutputColumn As Structure)(initializer As Action, tableName As String, outputColumnName As String, firstColumnValue As (String, TFirstInputColumn), secondColumnValue As (String, TSecondInputColumn), thirdColumnValue As (String, TThirdInputColumn)) As TOutputColumn? Implements IStoreColumn.ReadColumnValue
        initializer()
        Return ExecuteScalar(Of TOutputColumn)(
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} AND [{secondColumnValue.Item1}]=@{secondColumnValue.Item1} AND [{thirdColumnValue.Item1}]=@{thirdColumnValue.Item1};",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2))
    End Function
    Public Function ReadColumnString(Of TFirst, TSecond)(initializer As Action, tableName As String, outputColumnName As String, firstInputColumnValue As (String, TFirst), secondInputColumnValue As (String, TSecond)) As String Implements IStoreColumn.ReadColumnString
        initializer()
        Return ExecuteScalar(
            Function(o) If(o Is Nothing OrElse TypeOf o Is DBNull, Nothing, CStr(o)),
            $"SELECT 
                [{outputColumnName}] 
            FROM 
                [{tableName}] 
            WHERE 
                [{firstInputColumnValue.Item1}]=@{firstInputColumnValue.Item1} 
                AND [{secondInputColumnValue.Item1}]=@{secondInputColumnValue.Item1};",
            ($"@{firstInputColumnValue.Item1}", firstInputColumnValue.Item2),
            ($"@{secondInputColumnValue.Item1}", secondInputColumnValue.Item2))
    End Function
    Public Function ReadColumnString(Of TInput)(initializer As Action, tableName As String, outputColumnName As String, inputColumnValue As (String, TInput)) As String Implements IStoreColumn.ReadColumnString
        initializer()
        Return ExecuteScalar(
            Function(o) If(o Is Nothing OrElse TypeOf o Is DBNull, Nothing, CStr(o)),
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{inputColumnValue.Item1}]=@{inputColumnValue.Item1};",
            ($"@{inputColumnValue.Item1}", inputColumnValue.Item2))
    End Function
    Public Sub WriteColumnValue(Of TWhereColumn, TSetColumn)(initializer As Action, tableName As String, setColumn As (String, TSetColumn), whereColumn As (String, TWhereColumn)) Implements IStoreColumn.WriteColumnValue
        initializer()
        backer.Execute(
            $"UPDATE 
                [{tableName}] 
            SET 
                [{setColumn.Item1}]=@{setColumn.Item1} 
            WHERE 
                [{whereColumn.Item1}]=@{whereColumn.Item1};",
            ($"@{whereColumn.Item1}", whereColumn.Item2),
            ($"@{setColumn.Item1}", setColumn.Item2))
    End Sub
End Class
