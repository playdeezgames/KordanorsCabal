Public Interface IStoreCreate
    Function Entry(
                         initializer As Action,
                         tableName As String) As Long
    Function Entry(Of TColumn)(
                                     initializer As Action,
                                     tableName As String,
                                     columnValue As (String, TColumn)
                                     ) As Long
    Function Entry(Of TFirstColumn,
                              TSecondColumn)(
                                            initializer As Action,
                                            tableName As String,
                                            firstColumnValue As (String, TFirstColumn),
                                            secondColumnValue As (String, TSecondColumn)
                                            ) As Long
    Function Entry(Of TFirstColumn,
                              TSecondColumn,
                              TThirdColumn)(
                                           initializer As Action,
                                           tableName As String,
                                           firstColumnValue As (String, TFirstColumn),
                                           secondColumnValue As (String, TSecondColumn),
                                           thirdColumnValue As (String, TThirdColumn)
                                           ) As Long
    Function Entry(Of TFirstColumn,
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
    Function Entry(Of TFirstColumn,
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
