Public Interface IStoreClear
    Sub ForValue(Of TColumn)(
                                       initializer As Action,
                                       tableName As String,
                                       columnValue As (String, TColumn))
    Sub ForValues(Of TFirstColumn,
                                 TSecondColumn)(
                                                initializer As Action,
                                                tableName As String,
                                                firstColumnValue As (String, TFirstColumn),
                                                secondColumnValue As (String, TSecondColumn))
End Interface
