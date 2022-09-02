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
                    (,,''),
                    (,,''),
                    (,,''),
                    (,,''),
                    (,,''),
                    (,,''),
                    (,,''),
                    (,,''),
                    (,,''),
                    (,,''),
                    (,,''),
                    (,,''),
                    (,,''),
                    (,,''),
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
