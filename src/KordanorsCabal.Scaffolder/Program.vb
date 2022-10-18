Module Program
    Sub Main(args As String())
        System.IO.File.Delete("test.db")
        Using connection As New SqliteConnection("Data Source=test.db")
            connection.Open()
            Scaffold.Scaffold(connection)
            Populate.Populate(connection)
        End Using
    End Sub
End Module
