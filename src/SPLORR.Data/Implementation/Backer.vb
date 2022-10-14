Imports Microsoft.Data.Sqlite

Public Class Backer
    Implements IBacker
    Public Property Connection As SqliteConnection
    Public ReadOnly Property LastRowIndex As Long Implements IBacker.LastRowIndex
        Get
            Using command = Connection.CreateCommand()
                command.CommandText = "SELECT last_insert_rowid();"
                Return CLng(command.ExecuteScalar())
            End Using
        End Get
    End Property

    Public Sub CopyTo(other As IBacker) Implements IBacker.CopyTo
        Connection.BackupDatabase(DirectCast(other, Backer).Connection)
    End Sub

    Public Sub Open(filename As String) Implements IBacker.Open
        Connection = New SqliteConnection($"Data Source={filename}")
        Connection.Open()
    End Sub

    Public Sub ShutDown() Implements IBacker.ShutDown
        If Connection IsNot Nothing Then
            Connection.Close()
            Connection = Nothing
        End If
    End Sub

    Public Sub Execute(sql As String, ParamArray parameters() As (String, Object)) Implements IBacker.Execute
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

    Public Function ScalarOf(Of TResult As Structure)(query As String, ParamArray parameters() As (String, Object)) As TResult? Implements IBacker.ScalarOf
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

    Public Function ScalarOf(Of TResult As Class)(transform As Func(Of Object, TResult), query As String, ParamArray parameters() As (String, Object)) As TResult Implements IBacker.ScalarOf
        Using command = CreateCommand(query, parameters.Select(Function(x) New SqliteParameter(x.Item1, x.Item2)).ToArray)
            Return transform(command.ExecuteScalar)
        End Using
    End Function

    Public Function ReadereOf(Of TResult)(transform As Func(Of Func(Of String, Object), TResult), query As String, ParamArray parameters() As (String, Object)) As List(Of TResult) Implements IBacker.ReadereOf
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

    Public Sub Clone(backer As IBacker) Implements IBacker.Clone
        Connection = DirectCast(backer, Backer).Connection
    End Sub

    Public Sub Close() Implements IBacker.Close
        Connection = Nothing
    End Sub
End Class
