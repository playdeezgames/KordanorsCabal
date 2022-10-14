Imports Microsoft.Data.Sqlite

Public Interface IBacker
    Property Connection As SqliteConnection
    ReadOnly Property LastInsertRowId() As Long
    Sub BackupTo(other As IBacker)
    Sub Connect(filename As String)
    Sub ShutDown()
    Sub ExecuteNonQuery(sql As String, ParamArray parameters() As (String, Object))
    Function ExecuteScalar(Of TResult As Structure)(query As String, ParamArray parameters() As (String, Object)) As TResult?
    Function ExecuteScalar(Of TResult As Class)(transform As Func(Of Object, TResult), query As String, ParamArray parameters() As (String, Object)) As TResult
    Function ExecuteReader(Of TResult)(transform As Func(Of Func(Of String, Object), TResult), query As String, ParamArray parameters() As (String, Object)) As List(Of TResult)
End Interface
