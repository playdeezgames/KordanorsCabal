Public Class ItemTypeStatisticData
    Inherits BaseData
    Friend Const TableName = "ItemTypeStatistics"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const ItemTypeStatisticTypeIdColumn = ItemTypeStatisticTypeData.ItemTypeStatisticTypeIdColumn
    Friend Const ItemTypeStatisticValueColumn = "ItemTypeStatisticValue"
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{ItemTypeStatisticTypeIdColumn}],
                    [{ItemTypeStatisticValueColumn}]) AS
                (VALUES
                    (17,1,20),
                    (16,1,2),
                    (15,1,5),
                    (10,1,1),
                    (18,1,5),
                    (20,1,40),
                    (19,1,10),
                    (7,1,2),
                    (26,1,2),
                    (39,1,2),
                    (43,1,5),
                    (30,1,1)
                )
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{ItemTypeStatisticTypeIdColumn}],
                    [{ItemTypeStatisticValueColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, itemTypeStatisticTypeId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            ItemTypeStatisticValueColumn,
            (ItemTypeIdColumn, itemTypeId),
            (ItemTypeStatisticTypeIdColumn, itemTypeStatisticTypeId))
    End Function
End Class
