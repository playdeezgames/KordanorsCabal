Public Module ItemStatisticData
    Friend Const TableName = "ItemStatistics"
    Friend Const ItemIdColumn = ItemData.ItemIdColumn
    Friend Const StatisticTypeColumn = "StatisticType"
    Friend Const StatisticValueColumn = "StatisticValue"
    Private ReadOnly Store As SPLORR.Data.Store = StaticStore.Store
    Friend Sub Initialize()
        ItemData.Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{ItemIdColumn}] INT NOT NULL,
                [{StatisticTypeColumn}] INT NOT NULL,
                [{StatisticValueColumn}] INT NOT NULL,
                UNIQUE([{ItemIdColumn}],[{StatisticTypeColumn}]),
                FOREIGN KEY ([{ItemIdColumn}]) REFERENCES [{ItemData.TableName}]([{ItemData.ItemIdColumn}])
            );")
    End Sub
    Public Function Read(itemId As Long, statisticType As Long) As Long?
        Return ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            StatisticValueColumn,
            (ItemIdColumn, itemId),
            (StatisticTypeColumn, statisticType))
    End Function

    Public Sub Write(itemId As Long, statisticType As Long, value As Long)
        ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (ItemIdColumn, itemId),
            (StatisticTypeColumn, statisticType),
            (StatisticValueColumn, value))
    End Sub

    Friend Sub ClearForItem(itemId As Long)
        ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (ItemIdColumn, itemId))
    End Sub
End Module
