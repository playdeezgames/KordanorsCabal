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
                    (7,7,15),
                    (8,6,5),
                    (9,6,5),
                    (10,1,1),
                    (10,2,2),
                    (10,3,1),
                    (10,5,10),
                    (10,6,1),
                    (10,7,5),
                    (15,1,5),
                    (15,4,2),
                    (15,5,10),
                    (15,6,3),
                    (15,7,15),
                    (16,1,2),
                    (16,4,2),
                    (16,5,10),
                    (16,6,2),
                    (16,7,10),
                    (17,1,20),
                    (17,3,2),
                    (17,4,2),
                    (17,6,10),
                    (17,7,50),
                    (18,1,5),
                    (18,3,2),
                    (18,2,4),
                    (18,5,20),
                    (18,6,5),
                    (18,7,25),
                    (19,1,10),
                    (19,2,6),
                    (19,3,3),
                    (19,5,40),
                    (19,6,20),
                    (19,7,100),
                    (20,1,40),
                    (20,4,4),
                    (20,5,50),
                    (20,6,50),
                    (20,7,250),
                    (21,6,1),
                    (22,7,10),
                    (23,7,50),
                    (24,7,2),
                    (25,6,100),
                    (26,1,2),
                    (26,7,5),
                    (27,7,100),
                    (28,7,10),
                    (29,7,5000),
                    (30,1,1),
                    (30,2,2),
                    (30,3,1),
                    (30,5,3),
                    (31,7,100),
                    (32,6,10),
                    (33,7,25),
                    (34,7,5),
                    (36,6,25),
                    (38,6,5),
                    (39,1,2),
                    (39,6,5),
                    (39,7,25),
                    (40,6,3),
                    (41,6,3),
                    (43,1,5),
                    (43,3,10),
                    (43,4,5),
                    (43,6,100),
                    (47,7,50),
                    (52,7,1000)
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
