Public Class ItemStatisticData
    Inherits BaseData
    Implements IItemStatisticData
    Friend Const TableName = "ItemStatistics"
    Friend Const ItemIdColumn = ItemData.ItemIdColumn
    Friend Const ItemStatisticTypeIdColumn = "ItemStatisticTypeId"
    Friend Const StatisticValueColumn = "StatisticValue"

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Item, ItemData).Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{ItemIdColumn}] INT NOT NULL,
                [{ItemStatisticTypeIdColumn}] INT NOT NULL,
                [{StatisticValueColumn}] INT NOT NULL,
                UNIQUE([{ItemIdColumn}],[{ItemStatisticTypeIdColumn}]),
                FOREIGN KEY ([{ItemIdColumn}]) REFERENCES [{ItemData.TableName}]([{ItemData.ItemIdColumn}])
            );")
    End Sub
    Public Function Read(itemId As Long, statisticType As Long) As Long? Implements IItemStatisticData.Read
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            StatisticValueColumn,
            (ItemIdColumn, itemId),
            (ItemStatisticTypeIdColumn, statisticType))
    End Function

    Public Sub Write(itemId As Long, statisticType As Long, value As Long) Implements IItemStatisticData.Write
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (ItemIdColumn, itemId),
            (ItemStatisticTypeIdColumn, statisticType),
            (StatisticValueColumn, value))
    End Sub

    Public Sub ClearForItem(itemId As Long) Implements IItemStatisticData.ClearForItem
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (ItemIdColumn, itemId))
    End Sub
End Class
