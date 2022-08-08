Public Module StaticStore
    Public ReadOnly Store As New Store
    Public Sub ReplaceRecord(Of TFirstColumn, TSecondColumn, TThirdColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn))
        Store.ReplaceRecord(initializer, tableName, firstColumnValue, secondColumnValue, thirdColumnValue)
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
                                               fourthColumnValue As (String, TFourthColumn))
        Store.ReplaceRecord(
            initializer, tableName, firstColumnValue, secondColumnValue, thirdColumnValue, fourthColumnValue)
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
                                      fifthColumnValue As (String, TFifthColumn))
        Store.ReplaceRecord(
            initializer, tableName, firstColumnValue, secondColumnValue, thirdColumnValue, fourthColumnValue, fifthColumnValue)
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
                                      sixthColumnValue As (String, TSixthColumn))
        Store.ReplaceRecord(
            initializer, tableName, firstColumnValue, secondColumnValue, thirdColumnValue, fourthColumnValue, fifthColumnValue, sixthColumnValue)
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
                                      seventhColumnValue As (String, TSeventhColumn))
        Store.ReplaceRecord(
            initializer, tableName, firstColumnValue, secondColumnValue, thirdColumnValue, fourthColumnValue, fifthColumnValue, sixthColumnValue, seventhColumnValue)
    End Sub
    Public Function CreateRecord(initializer As Action, tableName As String) As Long
        Return Store.CreateRecord(initializer, tableName)
    End Function
    Public Function CreateRecord(Of TColumn)(initializer As Action, tableName As String, columnValue As (String, TColumn)) As Long
        Return Store.CreateRecord(initializer, tableName, columnValue)
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn)) As Long
        Return Store.CreateRecord(initializer, tableName, firstColumnValue, secondColumnValue)
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn)) As Long
        Return Store.CreateRecord(initializer, tableName, firstColumnValue, secondColumnValue, thirdColumnValue)
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn, TFourthColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn), fourthColumnValue As (String, TFourthColumn)) As Long
        Return Store.CreateRecord(initializer, tableName, firstColumnValue, secondColumnValue, thirdColumnValue, fourthColumnValue)
    End Function
    Public Function CreateRecord(Of TFirstColumn, TSecondColumn, TThirdColumn, TFourthColumn, TFifthColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn), fourthColumnValue As (String, TFourthColumn), fifthColumnValue As (String, TFifthColumn)) As Long
        Return Store.CreateRecord(initializer, tableName, firstColumnValue, secondColumnValue, thirdColumnValue, fourthColumnValue, fifthColumnValue)
    End Function
    Public Function ReadCountForColumnValue(Of TInputColumn)(initializer As Action, tableName As String, inputColumnValue As (String, TInputColumn)) As Long
        Return Store.ReadCountForColumnValue(initializer, tableName, inputColumnValue)
    End Function
End Module