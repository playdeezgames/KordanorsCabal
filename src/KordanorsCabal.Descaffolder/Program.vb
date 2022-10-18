Imports System.Data
Imports System.IO
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
            Dim replacements = CreateReplacements(tableSql.Keys, allColumns)
            CreateScaffoldFile(tableSql, replacements)
            CreatePopulateFile(connection, tableSql.Keys, replacements)
        End Using
    End Sub

    Private Sub CreatePopulateFile(connection As SqliteConnection, keys As IEnumerable(Of String), replacements As IReadOnlyDictionary(Of String, String))
        Using writer = System.IO.File.CreateText("Populate.vb")
            writer.WriteLine("Public Module Populate")
            writer.WriteLine("    Sub Populate(connection As SqliteConnection)")
            For Each key In keys
                writer.WriteLine($"        Populate{key}(connection)")
            Next
            writer.WriteLine("    End Sub")
            For Each key In keys
                CreatePopulateTable(writer, connection, key, replacements)
            Next
            writer.WriteLine("End Module")
        End Using
    End Sub

    Private Sub CreatePopulateTable(writer As StreamWriter, connection As SqliteConnection, key As String, replacements As IReadOnlyDictionary(Of String, String))
        Dim columns As New List(Of (String, String))
        Using command = New SqliteCommand($"PRAGMA table_info({key})", connection)
            Using reader = command.ExecuteReader()
                While reader.Read
                    Dim columnName = CStr(reader(1))
                    Dim columnType = CStr(reader(2))
                    Dim notNull = CLng(reader(3)) > 0
                    columns.Add((columnName,
                                If(columnType = "TEXT", "String",
                                If(columnType = "INTEGER", "Long",
                                If(notNull, "Long", "Long?")))))
                End While
            End Using
        End Using
        writer.Write($"    Sub Populate{key}Record(connection As SqliteConnection")
        For Each column In columns
            writer.Write($", {column.Item1} As {column.Item2}")
        Next
        writer.WriteLine($")")
        writer.Write($"        Using command = New SqliteCommand(""INSERT INTO [{key}](")
        writer.Write(String.Join(", ", columns.Select(Function(x) $"[{x.Item1}]")))
        writer.Write(") VALUES (")
        writer.Write(String.Join(", ", columns.Select(Function(x) $"@{x.Item1}")))
        writer.WriteLine(");"", connection)")
        For Each column In columns
            If column.Item2 = "Long?" Then
                writer.WriteLine($"            command.Parameters.AddWithValue(""@{column.Item1}"", If({column.Item1} Is Nothing, CObj(DBNull.Value) ,{column.Item1}))")
            Else
                writer.WriteLine($"            command.Parameters.AddWithValue(""@{column.Item1}"", {column.Item1})")
            End If
        Next
        writer.WriteLine("            command.ExecuteNonQuery()")
        writer.WriteLine("        End Using")
        writer.WriteLine("    End Sub")
        writer.WriteLine($"    Sub Populate{key}(connection As SqliteConnection)")
        Using command = New SqliteCommand($"SELECT * FROM [{key}];", connection)
            Using reader = command.ExecuteReader
                While reader.Read
                    writer.Write($"        Populate{key}Record(connection")
                    Dim index = 0
                    For Each column In columns
                        Select Case column.Item2
                            Case "String"
                                writer.Write($",""{CStr(reader(index)).Replace("""", """""")}""")
                            Case "Long?"
                                If TypeOf reader(index) Is DBNull Then
                                    writer.Write(", Nothing")
                                Else
                                    writer.Write($", {CLng(reader(index))}")
                                End If
                            Case "Long"
                                writer.Write($", {CLng(reader(index))}")
                        End Select
                        index += 1
                    Next
                    writer.WriteLine(")")
                End While
            End Using
        End Using
        writer.WriteLine("    End Sub")
    End Sub

    Private Function CreateReplacements(keys As IEnumerable(Of String), allColumns As HashSet(Of String)) As IReadOnlyDictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)
        For Each key In keys
            result($"[{key}]") = $"[{{Tables.{key}}}]"
        Next
        For Each column In allColumns
            result($"[{column}]") = $"[{{Columns.{column}Column}}]"
        Next
        Return result
    End Function

    Private Sub CreateScaffoldFile(tableSql As IReadOnlyDictionary(Of String, String), replacements As IReadOnlyDictionary(Of String, String))
        Using writer = System.IO.File.CreateText("Scaffold.vb")
            writer.WriteLine("Public Module Scaffold")
            writer.WriteLine("    Private Sub ScaffoldTable(connection as SqliteConnection, sql As String)")
            writer.WriteLine("        Using command = New SqliteCommand(sql, connection)")
            writer.WriteLine("            command.ExecuteNonQuery()")
            writer.WriteLine("        End Using")
            writer.WriteLine("    End Sub")
            writer.WriteLine("    Public Sub Scaffold(connection as SqliteConnection)")
            For Each value In tableSql.Values
                For Each replacement In replacements
                    value = value.Replace(replacement.Key, replacement.Value)
                Next
                writer.WriteLine($"        ScaffoldTable(connection, $""{value.Replace("""", """""")}"")")
            Next
            writer.WriteLine("    End Sub")
            writer.WriteLine("End Module")
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
