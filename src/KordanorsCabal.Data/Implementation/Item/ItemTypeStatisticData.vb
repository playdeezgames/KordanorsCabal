Public Class ItemTypeStatisticData
    Inherits BaseData
    Implements IItemTypeStatisticData
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
                    (7,1,2),
                    (10,1,1),
                    (10,2,2),
                    (10,3,1),
                    (15,1,5),
                    (16,1,2),
                    (17,1,20),
                    (17,3,2),
                    (18,1,5),
                    (18,3,2),
                    (18,2,4),
                    (19,1,10),
                    (19,2,6),
                    (19,3,3),
                    (20,1,40),
                    (26,1,2),
                    (30,1,1),
                    (30,2,2),
                    (30,3,1),
                    (39,1,2),
                    (43,1,5),
                    (43,3,10)
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

    Public Function Read(itemTypeId As Long, itemTypeStatisticTypeId As Long) As Long? Implements IItemTypeStatisticData.Read
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            ItemTypeStatisticValueColumn,
            (ItemTypeIdColumn, itemTypeId),
            (ItemTypeStatisticTypeIdColumn, itemTypeStatisticTypeId))
    End Function
End Class
