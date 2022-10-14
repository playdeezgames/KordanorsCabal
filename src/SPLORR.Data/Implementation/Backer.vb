Imports Microsoft.Data.Sqlite

Public Class Backer
    Implements IBacker
    Public Property Connection As SqliteConnection
    Public ReadOnly Property LastInsertRowId As Long Implements IBacker.LastInsertRowId
        Get
            Using command = Connection.CreateCommand()
                command.CommandText = "SELECT last_insert_rowid();"
                Return CLng(command.ExecuteScalar())
            End Using
        End Get
    End Property

    Public Sub BackupTo(other As IBacker) Implements IBacker.BackupTo
        Connection.BackupDatabase(DirectCast(other, Backer).Connection)
    End Sub

    Public Sub Connect(filename As String) Implements IBacker.Connect
        Connection = New SqliteConnection($"Data Source={filename}")
        Connection.Open()
    End Sub

    Public Sub ShutDown() Implements IBacker.ShutDown
        If Connection IsNot Nothing Then
            Connection.Close()
            Connection = Nothing
        End If
    End Sub

    Public Sub ExecuteNonQuery(sql As String, ParamArray parameters() As (String, Object)) Implements IBacker.ExecuteNonQuery
        Using command = CreateCommand(sql, parameters.Select(Function(x) New SqliteParameter(x.Item1, x.Item2)).ToArray)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Private Function ExecuteScalar(Of TResult As Structure)(command As SqliteCommand) As TResult?
        Dim result = command.ExecuteScalar
        If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
            Return CType(result, TResult?)
        End If
        Return Nothing
    End Function

    Public Function ExecuteScalar(Of TResult As Structure)(query As String, ParamArray parameters() As (String, Object)) As TResult? Implements IBacker.ExecuteScalar
        Using command = CreateCommand(query, parameters.Select(Function(x) New SqliteParameter(x.Item1, x.Item2)).ToArray)
            Return ExecuteScalar(Of TResult)(command)
        End Using
    End Function

    Private Function CreateCommand(query As String, ParamArray parameters() As SqliteParameter) As SqliteCommand
        Dim command = Connection.CreateCommand()
        command.CommandText = query
        For Each parameter In parameters
            command.Parameters.Add(parameter)
        Next
        Return command
    End Function

    Public Function ExecuteScalar(Of TResult As Class)(transform As Func(Of Object, TResult), query As String, ParamArray parameters() As (String, Object)) As TResult Implements IBacker.ExecuteScalar
        Using command = CreateCommand(query, parameters.Select(Function(x) New SqliteParameter(x.Item1, x.Item2)).ToArray)
            Return transform(command.ExecuteScalar)
        End Using
    End Function

    Public Function ExecuteReader(Of TResult)(transform As Func(Of Func(Of String, Object), TResult), query As String, ParamArray parameters() As (String, Object)) As List(Of TResult) Implements IBacker.ExecuteReader
        Using command = CreateCommand(query, parameters.Select(Function(x) New SqliteParameter(x.Item1, x.Item2)).ToArray)
            Using reader = command.ExecuteReader
                Dim result As New List(Of TResult)
                While reader.Read
                    result.Add(transform(Function(x) reader(x)))
                End While
                Return result
            End Using
        End Using
    End Function

    Public Sub TakeConnection(backer As IBacker) Implements IBacker.TakeConnection
        Connection = DirectCast(backer, Backer).Connection
    End Sub

    Public Sub Disconnect() Implements IBacker.Disconnect
        Connection = Nothing
    End Sub
End Class
