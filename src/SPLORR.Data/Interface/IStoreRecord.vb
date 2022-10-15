Public Interface IStoreRecord
    Function All(Of TOutputColumn)(
                                          initializer As Action,
                                          tableName As String,
                                          outputColumnName As String
                                          ) As List(Of TOutputColumn)
    Function WithValues(Of TInputColumn,
                                            TOutputColumn)(
                                                          initializer As Action,
                                                          tableName As String,
                                                          outputColumnName As String,
                                                          forColumnValue As (String, TInputColumn)
                                                          ) As List(Of TOutputColumn)
    Function WithValues(Of TFirstInputColumn,
                                             TSecondInputColumn,
                                             TOutputColumn)(
                                                           initializer As Action,
                                                           tableName As String,
                                                           outputColumnName As String,
                                                           firstColumnValue As (String, TFirstInputColumn),
                                                           secondColumnValue As (String, TSecondInputColumn)
                                                           ) As List(Of TOutputColumn)
    Function WithValue(Of TInputColumn,
                                            TFirstOutputColumn,
                                            TSecondOutputColumn)(
                                                initializer As Action,
                                                tableName As String,
                                                outputColumnNames As (String, String),
                                                forColumnValue As (String, TInputColumn)
                                                ) As List(Of Tuple(Of TFirstOutputColumn, TSecondOutputColumn))
End Interface
