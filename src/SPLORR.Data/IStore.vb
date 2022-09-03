Imports Microsoft.Data.Sqlite

Public Interface IStore
    Sub Reset()
    Function Renew() As SqliteConnection
    Sub Restore(
               oldConnection As SqliteConnection)
    Sub ShutDown()
    Sub Save(
            filename As String)
    Sub Load(
            filename As String)
    Sub ExecuteNonQuery(
                       sql As String,
                       ParamArray parameters() As SqliteParameter)
    Function ReadColumnValues(Of TOutputColumn)(
                                               initializer As Action,
                                               tableName As String,
                                               outputColumnName As String
                                               ) As IEnumerable(Of TOutputColumn)
    Function ReadColumnValue(Of TInputColumn,
                                 TOutputColumn As Structure)(
                                                            initializer As Action,
                                                            tableName As String,
                                                            outputColumnName As String,
                                                            inputColumnValue As (String, TInputColumn)
                                                            ) As TOutputColumn?
    Function ReadColumnValue(Of TFirstInputColumn,
                                 TSecondInputColumn,
                                 TOutputColumn As Structure)(
                                                            initializer As Action,
                                                            tableName As String,
                                                            outputColumnName As String,
                                                            firstColumnValue As (String, TFirstInputColumn),
                                                            secondColumnValue As (String, TSecondInputColumn)
                                                            ) As TOutputColumn?
    Function ReadColumnValue(Of TFirstInputColumn,
                                 TSecondInputColumn,
                                 TThirdInputColumn,
                                 TOutputColumn As Structure)(
                                                            initializer As Action,
                                                            tableName As String,
                                                            outputColumnName As String,
                                                            firstColumnValue As (String, TFirstInputColumn),
                                                            secondColumnValue As (String, TSecondInputColumn),
                                                            thirdColumnValue As (String, TThirdInputColumn)
                                                            ) As TOutputColumn?
    Function ReadColumnString(Of TFirst,
                                  TSecond)(
                                          initializer As Action,
                                          tableName As String,
                                          outputColumnName As String,
                                          firstInputColumnValue As (String, TFirst),
                                          secondInputColumnValue As (String, TSecond)
                                          ) As String
    Function ReadColumnString(Of TInput)(
                                        initializer As Action,
                                        tableName As String,
                                        outputColumnName As String,
                                        inputColumnValue As (String, TInput)
                                        ) As String
    Sub WriteColumnValue(Of TWhereColumn,
                             TSetColumn)(
                                        initializer As Action,
                                        tableName As String,
                                        setColumn As (String, TSetColumn),
                                        whereColumn As (String, TWhereColumn))
    Function ReadRecords(Of TOutputColumn)(
                                          initializer As Action,
                                          tableName As String,
                                          outputColumnName As String
                                          ) As List(Of TOutputColumn)
    Function ReadRecordsWithColumnValue(Of TInputColumn,
                                            TOutputColumn)(
                                                          initializer As Action,
                                                          tableName As String,
                                                          outputColumnName As String,
                                                          forColumnValue As (String, TInputColumn)
                                                          ) As List(Of TOutputColumn)
    Function ReadRecordsWithColumnValues(Of TFirstInputColumn,
                                             TSecondInputColumn,
                                             TOutputColumn)(
                                                           initializer As Action,
                                                           tableName As String,
                                                           outputColumnName As String,
                                                           firstColumnValue As (String, TFirstInputColumn),
                                                           secondColumnValue As (String, TSecondInputColumn)
                                                           ) As List(Of TOutputColumn)
    Function ReadRecordsWithColumnValue(Of TInputColumn,
                                            TFirstOutputColumn,
                                            TSecondOutputColumn)(
                                                initializer As Action,
                                                tableName As String,
                                                outputColumnNames As (String, String),
                                                forColumnValue As (String, TInputColumn)
                                                ) As List(Of Tuple(Of TFirstOutputColumn, TSecondOutputColumn))
    Sub ClearForColumnValue(Of TColumn)(
                                       initializer As Action,
                                       tableName As String,
                                       columnValue As (String, TColumn))
    Sub ClearForColumnValues(Of TFirstColumn,
                                 TSecondColumn)(
                                                initializer As Action,
                                                tableName As String,
                                                firstColumnValue As (String, TFirstColumn),
                                                secondColumnValue As (String, TSecondColumn))
    Sub ReplaceRecord(Of TFirstColumn,
                          TSecondColumn)(
                                        initializer As Action,
                                        tableName As String,
                                        firstColumnValue As (String, TFirstColumn),
                                        secondColumnValue As (String, TSecondColumn))
    Sub ReplaceRecord(Of TFirstColumn,
                          TSecondColumn,
                          TThirdColumn)(
                                       initializer As Action,
                                       tableName As String,
                                       firstColumnValue As (String, TFirstColumn),
                                       secondColumnValue As (String, TSecondColumn),
                                       thirdColumnValue As (String, TThirdColumn))
    Sub ReplaceRecord(Of TFirstColumn,
                        TSecondColumn,
                        TThirdColumn,
                        TFourthColumn)(
                                    initializer As Action,
                                    tableName As String,
                                    firstColumnValue As (String, TFirstColumn),
                                    secondColumnValue As (String, TSecondColumn),
                                    thirdColumnValue As (String, TThirdColumn),
                                    fourthColumnValue As (String, TFourthColumn))
    Sub ReplaceRecord(Of TFirstColumn,
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
    Sub ReplaceRecord(Of TFirstColumn,
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
    Sub ReplaceRecord(Of TFirstColumn,
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
    Function CreateRecord(
                         initializer As Action,
                         tableName As String) As Long
    Function CreateRecord(Of TColumn)(
                                     initializer As Action,
                                     tableName As String,
                                     columnValue As (String, TColumn)
                                     ) As Long
    Function CreateRecord(Of TFirstColumn,
                              TSecondColumn)(
                                            initializer As Action,
                                            tableName As String,
                                            firstColumnValue As (String, TFirstColumn),
                                            secondColumnValue As (String, TSecondColumn)
                                            ) As Long
    Function CreateRecord(Of TFirstColumn,
                              TSecondColumn,
                              TThirdColumn)(
                                           initializer As Action,
                                           tableName As String,
                                           firstColumnValue As (String, TFirstColumn),
                                           secondColumnValue As (String, TSecondColumn),
                                           thirdColumnValue As (String, TThirdColumn)
                                           ) As Long
    Function CreateRecord(Of TFirstColumn,
                              TSecondColumn,
                              TThirdColumn,
                              TFourthColumn)(
                                            initializer As Action,
                                            tableName As String,
                                            firstColumnValue As (String, TFirstColumn),
                                            secondColumnValue As (String, TSecondColumn),
                                            thirdColumnValue As (String, TThirdColumn),
                                            fourthColumnValue As (String, TFourthColumn)
                                            ) As Long
    Function CreateRecord(Of TFirstColumn,
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
                                           fifthColumnValue As (String, TFifthColumn)
                                           ) As Long
    Function ReadCountForColumnValue(Of TInputColumn)(
                                                     initializer As Action,
                                                     tableName As String,
                                                     inputColumnValue As (String, TInputColumn)
                                                     ) As Long
End Interface
