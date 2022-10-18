Module Program
    Sub Main(args As String())
        Dim start = DateTimeOffset.Now
        IO.File.Delete("test.db")
        Using connection As New SqliteConnection("Data Source=test.db")
            connection.Open()
            Scaffold(connection)
            Populate(connection)
        End Using
        Dim elapsed = DateTimeOffset.Now - start
        Console.WriteLine(elapsed)
        Console.ReadLine()
    End Sub
End Module
