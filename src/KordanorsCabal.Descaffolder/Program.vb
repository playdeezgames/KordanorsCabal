Imports Microsoft.Data.Sqlite
Imports SQLitePCL

Module Program
    Function FetchTableSql(connection As SqliteConnection) As IReadOnlyDictionary(Of String, String)
        Dim tableSql As New Dictionary(Of String, String)
        Using command = New SqliteCommand("SELECT [name],[sql] FROM [sqlite_master] WHERE [type]='table';", connection)
            Using reader = command.ExecuteReader()
                While reader.Read()
                    Dim tableName = CStr(reader(0))
                    If tableName = "sqlite_sequence" Then
                        Continue While
                    End If
                    Dim sql = CStr(reader(1))
                    tableSql(tableName) = sql
                End While
            End Using
        End Using
        Return tableSql
    End Function
    Sub Main(args As String())
        Using connection = New SqliteConnection("Data Source=boilerplate.db")
            connection.Open()
            Dim tableSql = FetchTableSql(connection)
            CreateTablesFile(tableSql.Keys)
            Dim allColumns = FetchColumnNames(connection, tableSql.Keys)
            CreateColumnsFile(allColumns)
        End Using
    End Sub

    Private Sub CreateColumnsFile(allColumns As HashSet(Of String))
        Using writer = System.IO.File.CreateText("Columns.vb")
            writer.WriteLine("Public Module Columns")
            For Each key In allColumns.OrderBy(Function(x) $"{x}Column")
                writer.WriteLine($"    Public Const {key}Column=""{key}""")
            Next
            writer.WriteLine("End Module")
        End Using
    End Sub

    Private Function FetchColumnNames(
                                     connection As SqliteConnection,
                                     tableNames As IEnumerable(Of String)) As HashSet(Of String)
        Dim result = New HashSet(Of String)
        For Each tablename In tableNames
            Using command = New SqliteCommand($"PRAGMA table_info({tablename})", connection)
                Using reader = command.ExecuteReader
                    While reader.Read
                        result.Add(CStr(reader(1)))
                    End While
                End Using
            End Using
        Next
        Return result
    End Function

    Sub CreateTablesFile(tableNames As IEnumerable(Of String))
        Using writer = System.IO.File.CreateText("Tables.vb")
            writer.WriteLine("Public Module Tables")
            For Each key In tableNames.OrderBy(Function(x) x)
                writer.WriteLine($"    Public Const {key}=""{key}""")
            Next
            writer.WriteLine("End Module")
        End Using
    End Sub
End Module
