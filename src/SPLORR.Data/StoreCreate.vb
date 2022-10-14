﻿Public Class StoreCreate
    Inherits StoreBase
    Implements IStoreCreate

    Public Sub New(backer As IBacker)
        MyBase.New(backer)
    End Sub
    Public Function CreateRecord(initializer As Action, tableName As String) As Long Implements IStoreCreate.CreateRecord
        initializer()
        backer.ExecuteNonQuery($"INSERT INTO [{tableName}] DEFAULT VALUES;")
        Return backer.LastInsertRowId
    End Function
    Public Function CreateRecord(Of TColumn)(initializer As Action, tableName As String, columnValue As (String, TColumn)) As Long Implements IStoreCreate.CreateRecord
        initializer()
        backer.ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{columnValue.Item1}]) VALUES(@{columnValue.Item1});",
            ($"@{columnValue.Item1}", columnValue.Item2))
        Return backer.LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn)) As Long Implements IStoreCreate.CreateRecord
        initializer()
        backer.ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1});",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
        Return backer.LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn)) As Long Implements IStoreCreate.CreateRecord
        initializer()
        backer.ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}],[{thirdColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1},@{thirdColumnValue.Item1});",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2))
        Return backer.LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn, TFourthColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn), fourthColumnValue As (String, TFourthColumn)) As Long Implements IStoreCreate.CreateRecord
        initializer()
        backer.ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}],[{thirdColumnValue.Item1}],[{fourthColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1},@{thirdColumnValue.Item1}, @{fourthColumnValue.Item1});",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            ($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2))
        Return backer.LastInsertRowId
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn, TFourthColumn, TFifthColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn), fourthColumnValue As (String, TFourthColumn), fifthColumnValue As (String, TFifthColumn)) As Long Implements IStoreCreate.CreateRecord
        initializer()
        backer.ExecuteNonQuery(
            $"INSERT INTO [{tableName}] ([{firstColumnValue.Item1}],[{secondColumnValue.Item1}],[{thirdColumnValue.Item1}],[{fourthColumnValue.Item1}],[{fifthColumnValue.Item1}]) VALUES(@{firstColumnValue.Item1},@{secondColumnValue.Item1},@{thirdColumnValue.Item1}, @{fourthColumnValue.Item1}, @{fifthColumnValue.Item1});",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2),
            ($"@{thirdColumnValue.Item1}", thirdColumnValue.Item2),
            ($"@{fourthColumnValue.Item1}", fourthColumnValue.Item2),
            ($"@{fifthColumnValue.Item1}", fifthColumnValue.Item2))
        Return backer.LastInsertRowId
    End Function
End Class
