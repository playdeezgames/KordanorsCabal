Public Class StoreBase
    Protected backer As IBacker
    Sub New(backer As IBacker)
        Me.backer = backer
    End Sub
    Protected Function ExecuteScalar(Of TResult As Structure)(query As String, ParamArray parameters() As (String, Object)) As TResult?
        Return backer.ScalarOf(Of TResult)(query, parameters)
    End Function
    Protected Function ExecuteScalar(Of TResult As Class)(transform As Func(Of Object, TResult), query As String, ParamArray parameters() As (String, Object)) As TResult
        Return backer.ScalarOf(transform, query, parameters)
    End Function
    Protected Function ExecuteReader(Of TResult)(transform As Func(Of Func(Of String, Object), TResult), query As String, ParamArray parameters() As (String, Object)) As List(Of TResult)
        Return backer.ReadereOf(transform, query, parameters)
    End Function
End Class
