Imports Microsoft.Data.Sqlite

Public Class Store
    Implements IStore
    Private connection As SqliteConnection
    Private templateFilename As String
    Public Sub New(filename As String)
        Me.templateFilename = filename
    End Sub

    Public Sub Reset() Implements IStore.Reset
        ShutDown()
        connection = New SqliteConnection("Data Source=:memory:")
        connection.Open()
        Using loadConnection As New SqliteConnection($"Data Source={templateFilename}")
            loadConnection.Open()
            loadConnection.BackupDatabase(connection)
        End Using
    End Sub
    Public Function Renew() As SqliteConnection Implements IStore.Renew
        Dim result = connection
        connection = Nothing
        Reset()
        Return result
    End Function
    Public Sub Restore(oldConnection As SqliteConnection) Implements IStore.Restore
        ShutDown()
        connection = oldConnection
    End Sub
    Public Sub ShutDown() Implements IStore.ShutDown
        If connection IsNot Nothing Then
            connection.Close()
            connection = Nothing
        End If
    End Sub
    Public Sub Save(filename As String) Implements IStore.Save
        Using saveConnection As New SqliteConnection($"Data Source={filename}")
            connection.BackupDatabase(saveConnection)
        End Using
    End Sub
    Public Sub Load(filename As String) Implements IStore.Load
        Dim oldFilename = templateFilename
        templateFilename = filename
        Reset()
        templateFilename = oldFilename
    End Sub
    Private Function CreateCommand(query As String, ParamArray parameters() As SqliteParameter) As SqliteCommand
        Dim command = connection.CreateCommand()
        command.CommandText = query
        For Each parameter In parameters
            command.Parameters.Add(parameter)
        Next
        Return command
    End Function
    Private Shared Function MakeParameter(Of TParameter)(name As String, value As TParameter) As SqliteParameter
        Return New SqliteParameter(name, value)
    End Function
    Public Sub ExecuteNonQuery(sql As String, ParamArray parameters() As SqliteParameter) Implements IStore.ExecuteNonQuery
        Using command = CreateCommand(sql, parameters)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Private Function ExecuteScalar(Of TResult As Structure)(query As String, ParamArray parameters() As SqliteParameter) As TResult?
        Using command = CreateCommand(query, parameters)
            Return ExecuteScalar(Of TResult)(command)
        End Using
    End Function
    Private Function ExecuteScalar(Of TResult As Class)(transform As Func(Of Object, TResult), query As String, ParamArray parameters() As SqliteParameter) As TResult
        Using command = CreateCommand(query, parameters)
            Return transform(command.ExecuteScalar)
        End Using
    End Function
    Private Function ExecuteReader(Of TResult)(transform As Func(Of SqliteDataReader, TResult), query As String, ParamArray parameters() As SqliteParameter) As List(Of TResult)
        Using command = CreateCommand(query, parameters)
            Using reader = command.ExecuteReader
                Dim result As New List(Of TResult)
                While reader.Read
                    result.Add(transform(reader))
                End While
                Return result
            End Using
        End Using
    End Function
    Private ReadOnly Property LastInsertRowId() As Long
        Get
            Using command = connection.CreateCommand()
                command.CommandText = "SELECT last_insert_rowid();"
                Return CLng(command.ExecuteScalar())
            End Using
        End Get
    End Property
    Private Function ExecuteScalar(Of TResult As Structure)(command As SqliteCommand) As TResult?
        Dim result = command.ExecuteScalar
        If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
            Return CType(result, TResult?)
        End If
        Return Nothing
    End Function
    Public Function ReadColumnValues(Of TOutputColumn)(
                                                      initializer As Action,
                                                      tableName As String,
                                                      outputColumnName As String) As IEnumerable(Of TOutputColumn) Implements IStore.ReadColumnValues
        initializer()
        Return ExecuteReader(
            Function(reader) CType(reader(outputColumnName), TOutputColumn),
            $"SELECT [{outputColumnName}] FROM [{tableName}];")
    End Function
    Public Function ReadColumnValue(Of TInputColumn, TOutputColumn As Structure)(initializer As Action, tableName As String, outputColumnName As String, inputColumnValue As (String, TInputColumn)) As TOutputColumn? Implements IStore.ReadColumnValue
        initializer()
        Return ExecuteScalar(Of TOutputColumn)(
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{inputColumnValue.Item1}]=@{inputColumnValue.Item1};",
            MakeParameter($"@{inputColumnValue.Item1}", inputColumnValue.Item2))
    End Function
    Public Function ReadColumnValue(Of TFirstInputColumn, TSecondInputColumn, TOutputColumn As Structure)(initializer As Action, tableName As String, outputColumnName As String, firstColumnValue As (String, TFirstInputColumn), secondColumnValue As (String, TSecondInputColumn)) As TOutputColumn? Implements IStore.ReadColumnValue
        initializer()
        Return ExecuteScalar(Of TOutputColumn)(
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} AND  [{secondColumnValue.Item1}]=@{secondColumnValue.Item1};",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
    End Function
    Public Function ReadColumnValue(Of TFirstInputColumn, TSecondInputColumn, TThirdInputColumn, TOutputColumn As Structure)(initializer As Action, tableName As String, outputColumnName As String, firstColumnValue As (String, TFirstInputColumn), secondColumnValue As (String, TSecondInputColumn), thirdColumnValue As (String, TThirdInputColumn)) As TOutputColumn? Implements IStore.ReadColumnValue
        initializer()
        Return ExecuteScalar(Of TOutputColumn)(
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} AND [{secondColumnValue.Item1}]=@{secondColumnValue.Item1} AND [{thirdColumnValue.Item1}]=@{thirdColumnValue.Item1};",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            MakeParameter($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2))
    End Function
    Public Function ReadColumnString(Of TFirst, TSecond)(initializer As Action, tableName As String, outputColumnName As String, firstInputColumnValue As (String, TFirst), secondInputColumnValue As (String, TSecond)) As String Implements IStore.ReadColumnString
        initializer()
        Return ExecuteScalar(
            Function(o) If(o Is Nothing OrElse TypeOf o Is DBNull, Nothing, CStr(o)),
            $"SELECT 
                [{outputColumnName}] 
            FROM 
                [{tableName}] 
            WHERE 
                [{firstInputColumnValue.Item1}]=@{firstInputColumnValue.Item1} 
                AND [{secondInputColumnValue.Item1}]=@{secondInputColumnValue.Item1};",
            MakeParameter($"@{firstInputColumnValue.Item1}", firstInputColumnValue.Item2),
            MakeParameter($"@{secondInputColumnValue.Item1}", secondInputColumnValue.Item2))
    End Function
    Public Function ReadColumnString(Of TInput)(initializer As Action, tableName As String, outputColumnName As String, inputColumnValue As (String, TInput)) As String Implements IStore.ReadColumnString
        initializer()
        Return ExecuteScalar(
            Function(o) If(o Is Nothing OrElse TypeOf o Is DBNull, Nothing, CStr(o)),
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{inputColumnValue.Item1}]=@{inputColumnValue.Item1};",
            MakeParameter($"@{inputColumnValue.Item1}", inputColumnValue.Item2))
    End Function
    Public Sub WriteColumnValue(Of TWhereColumn, TSetColumn)(initializer As Action, tableName As String, setColumn As (String, TSetColumn), whereColumn As (String, TWhereColumn)) Implements IStore.WriteColumnValue
        initializer()
        ExecuteNonQuery(
            $"UPDATE 
                [{tableName}] 
            SET 
                [{setColumn.Item1}]=@{setColumn.Item1} 
            WHERE 
                [{whereColumn.Item1}]=@{whereColumn.Item1};",
            MakeParameter($"@{whereColumn.Item1}", whereColumn.Item2),
            MakeParameter($"@{setColumn.Item1}", setColumn.Item2))
    End Sub
    Public Function ReadRecords(Of TOutputColumn)(initializer As Action, tableName As String, outputColumnName As String) As List(Of TOutputColumn) Implements IStore.ReadRecords
        initializer()
        Return ExecuteReader(
            Function(reader) CType(reader(outputColumnName), TOutputColumn),
            $"SELECT [{outputColumnName}] FROM [{tableName}];")
    End Function
    Public Function ReadRecordsWithColumnValue(Of TInputColumn, TOutputColumn)(initializer As Action, tableName As String, outputColumnName As String, forColumnValue As (String, TInputColumn)) As List(Of TOutputColumn) Implements IStore.ReadRecordsWithColumnValue
        initializer()
        Return ExecuteReader(
            Function(reader) CType(reader(outputColumnName), TOutputColumn),
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{forColumnValue.Item1}]=@{forColumnValue.Item1};",
            MakeParameter($"@{forColumnValue.Item1}", forColumnValue.Item2))
    End Function
    Public Function ReadRecordsWithColumnValues(Of TFirstInputColumn, TSecondInputColumn, TOutputColumn)(initializer As Action, tableName As String, outputColumnName As String, firstColumnValue As (String, TFirstInputColumn), secondColumnValue As (String, TSecondInputColumn)) As List(Of TOutputColumn) Implements IStore.ReadRecordsWithColumnValues
        initializer()
        Return ExecuteReader(
            Function(reader) CType(reader(outputColumnName), TOutputColumn),
            $"SELECT 
                [{outputColumnName}] 
            FROM [{tableName}] 
            WHERE 
                [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} 
                AND [{secondColumnValue.Item1}]=@{secondColumnValue.Item1};",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
    End Function
    Public Function ReadRecordsWithColumnValue(
            Of TInputColumn,
                TFirstOutputColumn,
                TSecondOutputColumn)(
                    initializer As Action,
                    tableName As String,
                    outputColumnNames As (String, String),
                    forColumnValue As (String, TInputColumn)) As List(Of Tuple(Of TFirstOutputColumn, TSecondOutputColumn)) Implements IStore.ReadRecordsWithColumnValue
        initializer()
        Return ExecuteReader(
            Function(reader) New Tuple(Of TFirstOutputColumn, TSecondOutputColumn)(CType(reader(outputColumnNames.Item1), TFirstOutputColumn), CType(reader(outputColumnNames.Item2), TSecondOutputColumn)),
            $"SELECT [{outputColumnNames.Item1}],[{outputColumnNames.Item2}] FROM [{tableName}] WHERE [{forColumnValue.Item1}]=@{forColumnValue.Item1};",
            MakeParameter($"@{forColumnValue.Item1}", forColumnValue.Item2))
    End Function
    Public Sub ClearForColumnValue(Of TColumn)(initializer As Action, tableName As String, columnValue As (String, TColumn)) Implements IStore.ClearForColumnValue
        initializer()
        ExecuteNonQuery($"DELETE FROM [{tableName}] WHERE [{columnValue.Item1}]=@{columnValue.Item1};", MakeParameter($"@{columnValue.Item1}", columnValue.Item2))
    End Sub
    Public Sub ClearForColumnValues(Of TFirstColumn, TSecondColumn)(
                                                                   initializer As Action,
                                                                   tableName As String,
                                                                   firstColumnValue As (String, TFirstColumn),
                                                                   secondColumnValue As (String, TSecondColumn)) Implements IStore.ClearForColumnValues
        initializer()
        ExecuteNonQuery(
            $"DELETE FROM [{tableName}] WHERE [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} AND [{secondColumnValue.Item1}]=@{secondColumnValue.Item1};",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
    End Sub
    Public Sub ReplaceRecord(Of TFirstColumn, TSecondColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn)) Implements IStore.ReplaceRecord
        initializer()
        ExecuteNonQuery(
            $"REPLACE INTO [{tableName}]
            (
                [{firstColumnValue.Item1}],
                [{secondColumnValue.Item1}]
            ) 
            VALUES
            (
                @{firstColumnValue.Item1},
                @{secondColumnValue.Item1}
            );",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
    End Sub
    Public Sub ReplaceRecord(Of TFirstColumn, TSecondColumn, TThirdColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn)) Implements IStore.ReplaceRecord
        initializer()
        ExecuteNonQuery(
            $"REPLACE INTO [{tableName}]
            (
                [{firstColumnValue.Item1}],
                [{secondColumnValue.Item1}],
                [{thirdColumnValue.Item1}]
            ) 
            VALUES
            (
                @{firstColumnValue.Item1},
                @{secondColumnValue.Item1},
                @{thirdColumnValue.Item1}
            );",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            MakeParameter($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2))
    End Sub
    Public Sub ReplaceRecord(Of
                                 TFirstColumn,
                                 TSecondColumn,
                                 TThirdColumn,
                                 TFourthColumn)(
                                               initializer As Action,
                                               tableName As String,
                                               firstColumnValue As (String, TFirstColumn),
                                               secondColumnValue As (String, TSecondColumn),
                                               thirdColumnValue As (String, TThirdColumn),
                                               fourthColumnValue As (String, TFourthColumn)) Implements IStore.ReplaceRecord
        initializer()
        ExecuteNonQuery(
            $"REPLACE INTO [{tableName}]
            (
                [{firstColumnValue.Item1}],
                [{secondColumnValue.Item1}],
                [{thirdColumnValue.Item1}],
                [{fourthColumnValue.Item1}]
            ) 
            VALUES
            (
                @{firstColumnValue.Item1},
                @{secondColumnValue.Item1},
                @{thirdColumnValue.Item1},
                @{fourthColumnValue.Item1}
            );",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            MakeParameter($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            MakeParameter($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2))
    End Sub
    Public Sub ReplaceRecord(
                     Of TFirstColumn,
                         TSecondColumn,
                         TThirdColumn,
                         TFourthColumn,
                         TFifthColumn)(
                                      initializer As Action,
                                      tableName As String,
                                      firstColumnValue As (String, TFirstColumn),
                                      secondColumnValue As (String, TSecondColumn),
                                      thirdColumnValue As (String, TThirdColumn),
                                      fourthColumnValue As (String, TFourthColumn),
                                      fifthColumnValue As (String, TFifthColumn)) Implements IStore.ReplaceRecord
        initializer()
        ExecuteNonQuery(
            $"REPLACE INTO [{tableName}]
            (
                [{firstColumnValue.Item1}],
                [{secondColumnValue.Item1}],
                [{thirdColumnValue.Item1}],
                [{fourthColumnValue.Item1}],
                [{fifthColumnValue.Item1}]
            ) 
            VALUES
            (
                @{firstColumnValue.Item1},
                @{secondColumnValue.Item1},
                @{thirdColumnValue.Item1},
                @{fourthColumnValue.Item1},
                @{fifthColumnValue.Item1}
            );",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            MakeParameter($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            MakeParameter($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2),
            MakeParameter($"@{fifthColumnValue.Item1}", fifthColumnValue.Item2))
    End Sub
    Public Sub ReplaceRecord(
                     Of TFirstColumn,
                         TSecondColumn,
                         TThirdColumn,
                         TFourthColumn,
                         TFifthColumn,
                         TSixthColumn)(
                                      initializer As Action,
                                      tableName As String,
                                      firstColumnValue As (String, TFirstColumn),
                                      secondColumnValue As (String, TSecondColumn),
                                      thirdColumnValue As (String, TThirdColumn),
                                      fourthColumnValue As (String, TFourthColumn),
                                      fifthColumnValue As (String, TFifthColumn),
                                      sixthColumnValue As (String, TSixthColumn)) Implements IStore.ReplaceRecord
        initializer()
        ExecuteNonQuery(
            $"REPLACE INTO [{tableName}]
            (
                [{firstColumnValue.Item1}],
                [{secondColumnValue.Item1}],
                [{thirdColumnValue.Item1}],
                [{fourthColumnValue.Item1}],
                [{fifthColumnValue.Item1}],
                [{sixthColumnValue.Item1}]
            ) 
            VALUES
            (
                @{firstColumnValue.Item1},
                @{secondColumnValue.Item1},
                @{thirdColumnValue.Item1},
                @{fourthColumnValue.Item1},
                @{fifthColumnValue.Item1},
                @{sixthColumnValue.Item1}
            );",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            MakeParameter($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            MakeParameter($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2),
            MakeParameter($"@{fifthColumnValue.Item1}", fifthColumnValue.Item2),
            MakeParameter($"@{sixthColumnValue.Item1}", sixthColumnValue.Item2))
    End Sub
    Public Sub ReplaceRecord(
                     Of TFirstColumn,
                         TSecondColumn,
                         TThirdColumn,
                         TFourthColumn,
                         TFifthColumn,
                         TSixthColumn,
                         TSeventhColumn)(
                                      initializer As Action,
                                      tableName As String,
                                      firstColumnValue As (String, TFirstColumn),
                                      secondColumnValue As (String, TSecondColumn),
                                      thirdColumnValue As (String, TThirdColumn),
                                      fourthColumnValue As (String, TFourthColumn),
                                      fifthColumnValue As (String, TFifthColumn),
                                      sixthColumnValue As (String, TSixthColumn),
                                      seventhColumnValue As (String, TSeventhColumn)) Implements IStore.ReplaceRecord
        initializer()
        ExecuteNonQuery(
            $"REPLACE INTO [{tableName}]
            (
                [{firstColumnValue.Item1}],
                [{secondColumnValue.Item1}],
                [{thirdColumnValue.Item1}],
                [{fourthColumnValue.Item1}],
                [{fifthColumnValue.Item1}],
                [{sixthColumnValue.Item1}],
                [{seventhColumnValue.Item1}]
            ) 
            VALUES
            (
                @{firstColumnValue.Item1},
                @{secondColumnValue.Item1},
                @{thirdColumnValue.Item1},
                @{fourthColumnValue.Item1},
                @{fifthColumnValue.Item1},
                @{sixthColumnValue.Item1},
                @{seventhColumnValue.Item1}
            );",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            MakeParameter($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            MakeParameter($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2),
            MakeParameter($"@{fifthColumnValue.Item1}", fifthColumnValue.Item2),
            MakeParameter($"@{sixthColumnValue.Item1}", sixthColumnValue.Item2),
            MakeParameter($"@{seventhColumnValue.Item1}", seventhColumnValue.Item2))
    End Sub
    Public Function CreateRecord(initializer As Action, tableName As String) As Long Implements IStore.CreateRecord
        initializer()
        ExecuteNonQuery($"INSERT INTO [{tableName}] DEFAULT VALUES;")
        Return LastInsertRowId
    End Function
    Public Function CreateRecord(Of TColumn)(initializer As Action, tableName As String, columnValue As (String, TColumn)) As Long Implements IStore.CreateRecord
        initializer()
        ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{columnValue.Item1}]) VALUES(@{columnValue.Item1});",
            MakeParameter($"@{columnValue.Item1}", columnValue.Item2))
        Return LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn)) As Long Implements IStore.CreateRecord
        initializer()
        ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1});",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
        Return LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn)) As Long Implements IStore.CreateRecord
        initializer()
        ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}],[{thirdColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1},@{thirdColumnValue.Item1});",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            MakeParameter($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2))
        Return LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn, TFourthColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn), fourthColumnValue As (String, TFourthColumn)) As Long Implements IStore.CreateRecord
        initializer()
        ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}],[{thirdColumnValue.Item1}],[{fourthColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1},@{thirdColumnValue.Item1}, @{fourthColumnValue.Item1});",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            MakeParameter($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            MakeParameter($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2))
        Return LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn, TFourthColumn, TFifthColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn), fourthColumnValue As (String, TFourthColumn), fifthColumnValue As (String, TFifthColumn)) As Long Implements IStore.CreateRecord
        initializer()
        ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}],[{thirdColumnValue.Item1}],[{fourthColumnValue.Item1}],[{fifthColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1},@{thirdColumnValue.Item1}, @{fourthColumnValue.Item1}, @{fifthColumnValue.Item1});",
            MakeParameter($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            MakeParameter($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            MakeParameter($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            MakeParameter($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2),
            MakeParameter($"@{fifthColumnValue.Item1}", fifthColumnValue.Item2))
        Return LastInsertRowId
    End Function
    Public Function ReadCountForColumnValue(Of TInputColumn)(initializer As Action, tableName As String, inputColumnValue As (String, TInputColumn)) As Long Implements IStore.ReadCountForColumnValue
        initializer()
        Return ExecuteScalar(Of Long)(
            $"SELECT 
                COUNT(1) 
            FROM [{tableName}] 
            WHERE 
                [{inputColumnValue.Item1}]=@{inputColumnValue.Item1};",
            MakeParameter($"@{inputColumnValue.Item1}", inputColumnValue.Item2)).Value
    End Function
End Class
