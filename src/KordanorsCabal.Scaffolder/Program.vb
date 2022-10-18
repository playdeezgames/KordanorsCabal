Module Program
    Sub Main(args As String())
        Dim filename = args(0)
        IO.File.Delete(filename)
        Using connection As New SqliteConnection($"Data Source={filename}")
            connection.Open()
            Scaffold(connection)
            Populate(connection)
        End Using
    End Sub
End Module
