Public Interface IStoreReplace
    Sub Entry(Of TFirstColumn,
                          TSecondColumn)(
                                        initializer As Action,
                                        tableName As String,
                                        firstColumnValue As (String, TFirstColumn),
                                        secondColumnValue As (String, TSecondColumn))
    Sub Entry(Of TFirstColumn,
                          TSecondColumn,
                          TThirdColumn)(
                                       initializer As Action,
                                       tableName As String,
                                       firstColumnValue As (String, TFirstColumn),
                                       secondColumnValue As (String, TSecondColumn),
                                       thirdColumnValue As (String, TThirdColumn))
    Sub Entry(Of TFirstColumn,
                        TSecondColumn,
                        TThirdColumn,
                        TFourthColumn)(
                                    initializer As Action,
                                    tableName As String,
                                    firstColumnValue As (String, TFirstColumn),
                                    secondColumnValue As (String, TSecondColumn),
                                    thirdColumnValue As (String, TThirdColumn),
                                    fourthColumnValue As (String, TFourthColumn))
    Sub Entry(Of TFirstColumn,
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
    Sub Entry(Of TFirstColumn,
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
    Sub Entry(Of TFirstColumn,
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
