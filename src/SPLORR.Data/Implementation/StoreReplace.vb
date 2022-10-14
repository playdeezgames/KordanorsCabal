Public Class StoreReplace
    Inherits StoreBase
    Implements IStoreReplace

    Public Sub New(backer As IBacker)
        MyBase.New(backer)
    End Sub
    Public Sub ReplaceRecord(Of TFirstColumn, TSecondColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn)) Implements IStoreReplace.ReplaceRecord
        initializer()
        backer.Execute(
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
    Public Sub ReplaceRecord(Of TFirstColumn, TSecondColumn, TThirdColumn)(initializer As Action, tableName As String, firstColumnValue As (String, TFirstColumn), secondColumnValue As (String, TSecondColumn), thirdColumnValue As (String, TThirdColumn)) Implements IStoreReplace.ReplaceRecord
        initializer()
        backer.Execute(
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
                                               fourthColumnValue As (String, TFourthColumn)) Implements IStoreReplace.ReplaceRecord
        initializer()
        backer.Execute(
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
                                      fifthColumnValue As (String, TFifthColumn)) Implements IStoreReplace.ReplaceRecord
        initializer()
        backer.Execute(
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
                                      sixthColumnValue As (String, TSixthColumn)) Implements IStoreReplace.ReplaceRecord
        initializer()
        backer.Execute(
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
                                      seventhColumnValue As (String, TSeventhColumn)) Implements IStoreReplace.ReplaceRecord
        initializer()
        backer.Execute(
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
End Class
