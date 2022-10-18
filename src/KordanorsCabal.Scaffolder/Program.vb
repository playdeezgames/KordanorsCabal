Module Program
    Sub Main(args As String())
        Dim start = DateTimeOffset.Now
        System.IO.File.Delete("test.db")
        Using connection As New SqliteConnection("Data Source=test.db")
            connection.Open()
            Scaffold.Scaffold(connection)
            Populate.Populate(connection)
        End Using
        Dim elapsed = DateTimeOffset.Now - start
        Console.WriteLine(elapsed)
        Console.ReadLine()
    End Sub
End Module
