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
