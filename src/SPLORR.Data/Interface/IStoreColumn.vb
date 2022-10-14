Public Interface IStoreColumn
    Function ReadValues(Of TOutputColumn)(
                                               initializer As Action,
                                               tableName As String,
                                               outputColumnName As String
                                               ) As IEnumerable(Of TOutputColumn)
    Function ReadValue(Of TInputColumn,
                                 TOutputColumn As Structure)(
                                                            initializer As Action,
                                                            tableName As String,
                                                            outputColumnName As String,
                                                            inputColumnValue As (String, TInputColumn)
                                                            ) As TOutputColumn?
    Function ReadValue(Of TFirstInputColumn,
                                 TSecondInputColumn,
                                 TOutputColumn As Structure)(
                                                            initializer As Action,
                                                            tableName As String,
                                                            outputColumnName As String,
                                                            firstColumnValue As (String, TFirstInputColumn),
                                                            secondColumnValue As (String, TSecondInputColumn)
                                                            ) As TOutputColumn?
    Function ReadValue(Of TFirstInputColumn,
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
    Function ReadString(Of TFirst,
                                  TSecond)(
                                          initializer As Action,
                                          tableName As String,
                                          outputColumnName As String,
                                          firstInputColumnValue As (String, TFirst),
                                          secondInputColumnValue As (String, TSecond)
                                          ) As String
    Function ReadString(Of TInput)(
                                        initializer As Action,
                                        tableName As String,
                                        outputColumnName As String,
                                        inputColumnValue As (String, TInput)
                                        ) As String
    Sub Write(Of TWhereColumn,
                             TSetColumn)(
                                        initializer As Action,
                                        tableName As String,
                                        setColumn As (String, TSetColumn),
                                        whereColumn As (String, TWhereColumn))
End Interface
