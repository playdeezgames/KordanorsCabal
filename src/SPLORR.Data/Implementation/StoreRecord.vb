Public Class StoreRecord
    Inherits StoreBase
    Implements IStoreRecord

    Public Sub New(backer As IBacker)
        MyBase.New(backer)
    End Sub
    Public Function All(Of TOutputColumn)(initializer As Action, tableName As String, outputColumnName As String) As List(Of TOutputColumn) Implements IStoreRecord.All
        initializer()
        Return ExecuteReader(
            Function(reader) CType(reader(outputColumnName), TOutputColumn),
            $"SELECT [{outputColumnName}] FROM [{tableName}];")
    End Function
    Public Function WithValues(Of TInputColumn, TOutputColumn)(initializer As Action, tableName As String, outputColumnName As String, forColumnValue As (String, TInputColumn)) As List(Of TOutputColumn) Implements IStoreRecord.WithValues
        initializer()
        Return ExecuteReader(
            Function(reader) CType(reader(outputColumnName), TOutputColumn),
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{forColumnValue.Item1}]=@{forColumnValue.Item1};",
            ($"@{forColumnValue.Item1}", forColumnValue.Item2))
    End Function
    Public Function WithValues(Of TFirstInputColumn, TSecondInputColumn, TOutputColumn)(initializer As Action, tableName As String, outputColumnName As String, firstColumnValue As (String, TFirstInputColumn), secondColumnValue As (String, TSecondInputColumn)) As List(Of TOutputColumn) Implements IStoreRecord.WithValues
        initializer()
        Return ExecuteReader(
            Function(reader) CType(reader(outputColumnName), TOutputColumn),
            $"SELECT 
                [{outputColumnName}] 
            FROM [{tableName}] 
            WHERE 
                [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} 
                AND [{secondColumnValue.Item1}]=@{secondColumnValue.Item1};",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
    End Function
    Public Function WithValue(
            Of TInputColumn,
                TFirstOutputColumn,
                TSecondOutputColumn)(
                    initializer As Action,
                    tableName As String,
                    outputColumnNames As (String, String),
                    forColumnValue As (String, TInputColumn)) As List(Of Tuple(Of TFirstOutputColumn, TSecondOutputColumn)) Implements IStoreRecord.WithValue
        initializer()
        Dim firstColumnName = outputColumnNames.Item1
        Dim secondColumnName = outputColumnNames.Item2
        Return ExecuteReader(
            Function(reader)
                Dim firstColumnValue = reader(firstColumnName)
                Dim secondColumnValue = reader(secondColumnName)
                Return New Tuple(Of TFirstOutputColumn, TSecondOutputColumn)(
                    CType(If(TypeOf firstColumnValue Is DBNull, Nothing, firstColumnValue), TFirstOutputColumn),
                    CType(secondColumnValue, TSecondOutputColumn))
            End Function,
            $"SELECT [{outputColumnNames.Item1}],[{outputColumnNames.Item2}] FROM [{tableName}] WHERE [{forColumnValue.Item1}]=@{forColumnValue.Item1};",
            ($"@{forColumnValue.Item1}", forColumnValue.Item2))
    End Function
End Class
