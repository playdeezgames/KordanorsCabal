Imports Microsoft.Data.Sqlite
Public Interface IBacker
    Property Connection As SqliteConnection
    ReadOnly Property LastInsertRowId() As Long
    Sub BackupTo(other As IBacker)
    Sub Connect(filename As String)
    Sub ShutDown()
    Sub ExecuteNonQuery(sql As String, ParamArray parameters() As (String, Object))
    Function ExecuteScalar(Of TResult As Structure)(query As String, ParamArray parameters() As (String, Object)) As TResult?
    Function ExecuteScalar(Of TResult As Class)(transform As Func(Of Object, TResult), query As String, ParamArray parameters() As (String, Object)) As TResult
    Function ExecuteReader(Of TResult)(transform As Func(Of Func(Of String, Object), TResult), query As String, ParamArray parameters() As (String, Object)) As List(Of TResult)
End Interface
Public Interface IStoreMeta

    Sub Reset()
    Function Renew() As IBacker
    Sub Restore(
               oldBacker As IBacker)
    Sub ShutDown()
    Sub Save(
            filename As String)
    Sub Load(
            filename As String)
End Interface
Public Interface IStorePrimitive
    Sub ExecuteNonQuery(
                       sql As String,
                       ParamArray parameters() As (String, Object))
End Interface
Public Interface IStoreColumn
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
End Interface
Public Interface IStoreRecord
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
End Interface
Public Interface IStoreClear
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
End Interface
Public Interface IStoreReplace
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
End Interface
Public Interface IStoreCreate
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
End Interface
Public Interface IStoreCount
    Function ReadCountForColumnValue(Of TInputColumn)(
                                                     initializer As Action,
                                                     tableName As String,
                                                     inputColumnValue As (String, TInputColumn)
                                                     ) As Long
End Interface
Public Interface IStore
    Inherits IStoreMeta, IStorePrimitive, IStoreColumn, IStoreRecord, IStoreClear, IStoreReplace, IStoreCount, IStoreCreate
End Interface
