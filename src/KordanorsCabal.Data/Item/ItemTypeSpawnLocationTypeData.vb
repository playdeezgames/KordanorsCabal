Public Class ItemTypeSpawnLocationTypeData
    Inherits BaseData
    '[ItemTypeSpawnLocationTypes]([ItemTypeId],[DungeonLevelId],[LocationTypeId])
    Friend Const TableName = "ItemTypeSpawnLocationTypes"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const DungeonLevelIdColumn = DungeonLevelData.DungeonLevelIdColumn
    Friend Const LocationTypeIdColumn = LocationTypeData.LocationTypeIdColumn
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{DungeonLevelIdColumn}],
                    [{LocationTypeIdColumn}]) AS
                (VALUES
                    (,,),
                    (,,),
                    (,,),
                    (,,),
                    (,,),
                    (,,),
                    (,,),
                    (,,),
                    (,,),
                    (,,),
                    (,,),
                    (,,),
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{DungeonLevelIdColumn}],
                    [{LocationTypeIdColumn}]
                FROM [cte];")

    End Sub

    Public Function ReadAll(itemTypeId As Long, dungeonLevelId As Long) As IEnumerable(Of Long)
        Throw New NotImplementedException()
    End Function

    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
    End Sub
End Class
