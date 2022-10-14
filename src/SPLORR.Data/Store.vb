﻿Imports Microsoft.Data.Sqlite
Public Class Backer
    Implements IBacker
    Public Property Connection As SqliteConnection Implements IBacker.Connection
    Public ReadOnly Property LastInsertRowId As Long Implements IBacker.LastInsertRowId
        Get
            Using command = Connection.CreateCommand()
                command.CommandText = "SELECT last_insert_rowid();"
                Return CLng(command.ExecuteScalar())
            End Using
        End Get
    End Property

    Public Sub BackupTo(other As IBacker) Implements IBacker.BackupTo
        Connection.BackupDatabase(other.Connection)
    End Sub

    Public Sub Connect(filename As String) Implements IBacker.Connect
        Connection = New SqliteConnection($"Data Source={filename}")
        Connection.Open()
    End Sub
End Class
Public Class Store
    Implements IStore
    Private backer As IBacker
    Private templateFilename As String
    Public Sub New(filename As String)
        Me.backer = New Backer
        Me.templateFilename = filename
    End Sub

    Public Sub Reset() Implements IStore.Reset
        ShutDown()
        backer.Connection = New SqliteConnection("Data Source=:memory:")
        backer.Connection.Open()
        Using loadConnection As New SqliteConnection($"Data Source={templateFilename}")
            loadConnection.Open()
            loadConnection.BackupDatabase(backer.Connection)
        End Using
    End Sub
    Public Function Renew() As IBacker Implements IStore.Renew
        Dim result As IBacker = New Backer()
        result.Connection = backer.Connection
        backer.Connection = Nothing
        Reset()
        Return result
    End Function
    Public Sub Restore(oldBacker As IBacker) Implements IStore.Restore
        ShutDown()
        backer.Connection = oldBacker.Connection
    End Sub
    Public Sub ShutDown() Implements IStore.ShutDown
        If backer.Connection IsNot Nothing Then
            backer.Connection.Close()
            backer.Connection = Nothing
        End If
    End Sub
    Public Sub Save(filename As String) Implements IStore.Save
        Using saveConnection As New SqliteConnection($"Data Source={filename}")
            backer.Connection.BackupDatabase(saveConnection)
        End Using
    End Sub
    Public Sub Load(filename As String) Implements IStore.Load
        Dim oldFilename = templateFilename
        templateFilename = filename
        Reset()
        templateFilename = oldFilename
    End Sub
    Private Function CreateCommand(query As String, ParamArray parameters() As SqliteParameter) As SqliteCommand
        Dim command = backer.Connection.CreateCommand()
        command.CommandText = query
        For Each parameter In parameters
            command.Parameters.Add(parameter)
        Next
        Return command
    End Function
    Public Sub ExecuteNonQuery(sql As String, ParamArray parameters() As (String, Object)) Implements IStore.ExecuteNonQuery
        Using command = CreateCommand(sql, parameters.Select(Function(x) New SqliteParameter(x.Item1, x.Item2)).ToArray)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Private Function ExecuteScalar(Of TResult As Structure)(query As String, ParamArray parameters() As (String, Object)) As TResult?
        Using command = CreateCommand(query, parameters.Select(Function(x) New SqliteParameter(x.Item1, x.Item2)).ToArray)
            Return ExecuteScalar(Of TResult)(command)
        End Using
    End Function
    Private Function ExecuteScalar(Of TResult As Class)(transform As Func(Of Object, TResult), query As String, ParamArray parameters() As (String, Object)) As TResult
        Using command = CreateCommand(query, parameters.Select(Function(x) New SqliteParameter(x.Item1, x.Item2)).ToArray)
            Return transform(command.ExecuteScalar)
        End Using
    End Function
    Private Function ExecuteReader(Of TResult)(transform As Func(Of SqliteDataReader, TResult), query As String, ParamArray parameters() As (String, Object)) As List(Of TResult)
        Using command = CreateCommand(query, parameters.Select(Function(x) New SqliteParameter(x.Item1, x.Item2)).ToArray)
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
            Return backer.LastInsertRowId
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
            ($"@{inputColumnValue.Item1}", inputColumnValue.Item2))
    End Function
    Public Function ReadColumnValue(Of TFirstInputColumn, TSecondInputColumn, TOutputColumn As Structure)(initializer As Action, tableName As String, outputColumnName As String, firstColumnValue As (String, TFirstInputColumn), secondColumnValue As (String, TSecondInputColumn)) As TOutputColumn? Implements IStore.ReadColumnValue
        initializer()
        Return ExecuteScalar(Of TOutputColumn)(
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} AND  [{secondColumnValue.Item1}]=@{secondColumnValue.Item1};",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
    End Function
    Public Function ReadColumnValue(Of TFirstInputColumn, TSecondInputColumn, TThirdInputColumn, TOutputColumn As Structure)(initializer As Action, tableName As String, outputColumnName As String, firstColumnValue As (String, TFirstInputColumn), secondColumnValue As (String, TSecondInputColumn), thirdColumnValue As (String, TThirdInputColumn)) As TOutputColumn? Implements IStore.ReadColumnValue
        initializer()
        Return ExecuteScalar(Of TOutputColumn)(
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} AND [{secondColumnValue.Item1}]=@{secondColumnValue.Item1} AND [{thirdColumnValue.Item1}]=@{thirdColumnValue.Item1};",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2))
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
            ($"@{firstInputColumnValue.Item1}", firstInputColumnValue.Item2),
            ($"@{secondInputColumnValue.Item1}", secondInputColumnValue.Item2))
    End Function
    Public Function ReadColumnString(Of TInput)(initializer As Action, tableName As String, outputColumnName As String, inputColumnValue As (String, TInput)) As String Implements IStore.ReadColumnString
        initializer()
        Return ExecuteScalar(
            Function(o) If(o Is Nothing OrElse TypeOf o Is DBNull, Nothing, CStr(o)),
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{inputColumnValue.Item1}]=@{inputColumnValue.Item1};",
            ($"@{inputColumnValue.Item1}", inputColumnValue.Item2))
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
            ($"@{whereColumn.Item1}", whereColumn.Item2),
            ($"@{setColumn.Item1}", setColumn.Item2))
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
            ($"@{forColumnValue.Item1}", forColumnValue.Item2))
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
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
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
            ($"@{forColumnValue.Item1}", forColumnValue.Item2))
    End Function
    Public Sub ClearForColumnValue(Of TColumn)(initializer As Action, tableName As String, columnValue As (String, TColumn)) Implements IStore.ClearForColumnValue
        initializer()
        ExecuteNonQuery($"DELETE FROM [{tableName}] WHERE [{columnValue.Item1}]=@{columnValue.Item1};", ($"@{columnValue.Item1}", columnValue.Item2))
    End Sub
    Public Sub ClearForColumnValues(Of TFirstColumn, TSecondColumn)(
                                                                   initializer As Action,
                                                                   tableName As String,
                                                                   firstColumnValue As (String, TFirstColumn),
                                                                   secondColumnValue As (String, TSecondColumn)) Implements IStore.ClearForColumnValues
        initializer()
        ExecuteNonQuery(
            $"DELETE FROM [{tableName}] WHERE [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} AND [{secondColumnValue.Item1}]=@{secondColumnValue.Item1};",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
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
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
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
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2))
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
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            ($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2))
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
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            ($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2),
            ($"@{fifthColumnValue.Item1}", fifthColumnValue.Item2))
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
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            ($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2),
            ($"@{fifthColumnValue.Item1}", fifthColumnValue.Item2),
            ($"@{sixthColumnValue.Item1}", sixthColumnValue.Item2))
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
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            ($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2),
            ($"@{fifthColumnValue.Item1}", fifthColumnValue.Item2),
            ($"@{sixthColumnValue.Item1}", sixthColumnValue.Item2),
            ($"@{seventhColumnValue.Item1}", seventhColumnValue.Item2))
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
            ($"@{columnValue.Item1}", columnValue.Item2))
        Return LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn)) As Long Implements IStore.CreateRecord
        initializer()
        ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1});",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
        Return LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn)) As Long Implements IStore.CreateRecord
        initializer()
        ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}],[{thirdColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1},@{thirdColumnValue.Item1});",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2))
        Return LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn, TFourthColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn), fourthColumnValue As (String, TFourthColumn)) As Long Implements IStore.CreateRecord
        initializer()
        ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}],[{thirdColumnValue.Item1}],[{fourthColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1},@{thirdColumnValue.Item1}, @{fourthColumnValue.Item1});",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            ($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2))
        Return LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn, TFourthColumn, TFifthColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn), fourthColumnValue As (String, TFourthColumn), fifthColumnValue As (String, TFifthColumn)) As Long Implements IStore.CreateRecord
        initializer()
        ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}],[{thirdColumnValue.Item1}],[{fourthColumnValue.Item1}],[{fifthColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1},@{thirdColumnValue.Item1}, @{fourthColumnValue.Item1}, @{fifthColumnValue.Item1});",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            ($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2),
            ($"@{fifthColumnValue.Item1}", fifthColumnValue.Item2))
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
            ($"@{inputColumnValue.Item1}", inputColumnValue.Item2)).Value
    End Function
End Class
