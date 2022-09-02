Public Class ItemTypeSpawnCountData
    Inherits BaseData
    '[ItemTypeSpawnCounts]([ItemTypeId],[DungeonLevelId],[SpawnDice])
    Friend Const TableName = "ItemTypeSpawnCounts"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const DungeonLevelIdColumn = DungeonLevelData.DungeonLevelIdColumn
    Friend Const SpawnDice = "SpawnDice"
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{DungeonLevelIdColumn}],
                    [{SpawnDice}]) AS
                (VALUES
                    (7,1,'3d6'),
                    (7,2,'3d6'),
                    (7,3,'3d6'),
                    (7,4,'3d6'),
                    (7,5,'3d6'),
                    (10,1,'4d6'),
                    (10,2,'2d6'),
                    (11,2,'1d1'),
                    (12,4,'1d1'),
                    (13,3,'1d1'),
                    (14,1,'1d1'),
                    (15,1,'3d6'),
                    (16,1,'3d6'),
                    (17,1,'1d6'),
                    (18,1,'2d6'),
                    (18,2,'1d6'),
                    (19,2,'2d6'),
                    (19,3,'1d6'),
                    (20,2,'1d6'),
                    (43,6,'1d1'),
                    (45,1,'1d1'),
                    (48,4,'1d1'),
                    (50,3,'1d1'),
                    (51,8,'1d1')
                )
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{DungeonLevelIdColumn}],
                    [{SpawnDice}]
                FROM [cte];")
    End Sub

    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, dungeonLevelId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            SpawnDice,
            (ItemTypeIdColumn, itemTypeId),
            (DungeonLevelIdColumn, dungeonLevelId))
    End Function
End Class
