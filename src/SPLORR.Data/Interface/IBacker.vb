Public Interface IBacker
    ReadOnly Property LastRowIndex() As Long
    Sub CopyTo(other As IBacker)
    Sub Open(filename As String)
    Sub ShutDown()
    Sub Execute(sql As String, ParamArray parameters() As (String, Object))
    Sub Clone(backer As IBacker)
    Sub Close()
    Function ScalarOf(Of TResult As Structure)(query As String, ParamArray parameters() As (String, Object)) As TResult?
    Function ScalarOf(Of TResult As Class)(transform As Func(Of Object, TResult), query As String, ParamArray parameters() As (String, Object)) As TResult
    Function ReadereOf(Of TResult)(transform As Func(Of Func(Of String, Object), TResult), query As String, ParamArray parameters() As (String, Object)) As List(Of TResult)
End Interface
