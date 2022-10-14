Public Class ItemTypeSpawnLocationTypeData
    Inherits BaseData
    Implements IItemTypeSpawnLocationTypeData
    Friend Const TableName = "ItemTypeSpawnLocationTypes"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const DungeonLevelIdColumn = DungeonLevelData.DungeonLevelIdColumn
    Friend Const LocationTypeIdColumn = LocationTypeData.LocationTypeIdColumn
    Friend Sub Initialize()
        Store.Primitive.Execute($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{DungeonLevelIdColumn}],
                    [{LocationTypeIdColumn}]) AS
                (VALUES
                    (1,1,4),
                    (1,2,4),
                    (1,3,4),
                    (1,4,4),
                    (1,5,4),
                    (2,1,5),
                    (3,2,5),
                    (4,3,5),
                    (5,4,5),
                    (7,1,4),
                    (7,1,5),
                    (7,2,4),
                    (7,2,5),
                    (7,3,4),
                    (7,3,5),
                    (7,4,4),
                    (7,4,5),
                    (7,5,4),
                    (7,5,5),
                    (10,1,4),
                    (10,2,4),
                    (10,3,4),
                    (10,4,4),
                    (10,5,4),
                    (11,2,6),
                    (12,4,6),
                    (14,1,6),
                    (15,1,4),
                    (15,1,5),
                    (15,2,4),
                    (15,2,5),
                    (15,3,4),
                    (15,3,5),
                    (15,4,4),
                    (15,4,5),
                    (15,5,4),
                    (15,5,5),
                    (16,1,4),
                    (16,1,5),
                    (16,2,4),
                    (16,2,5),
                    (16,3,4),
                    (16,3,5),
                    (16,4,4),
                    (16,4,5),
                    (16,5,4),
                    (16,5,5),
                    (17,1,5),
                    (17,2,5),
                    (17,2,4),
                    (17,3,5),
                    (17,3,4),
                    (17,4,5),
                    (17,4,4),
                    (17,5,5),
                    (17,5,4),
                    (18,1,4),
                    (18,2,4),
                    (18,3,4),
                    (18,4,4),
                    (18,5,4),
                    (19,1,4),
                    (19,2,4),
                    (19,3,4),
                    (19,4,4),
                    (19,5,4),
                    (20,2,5),
                    (20,3,4),
                    (20,3,5),
                    (20,4,4),
                    (20,4,5),
                    (20,5,4),
                    (20,5,5),
                    (43,6,8),
                    (45,1,5),
                    (45,1,6),
                    (48,4,5),
                    (48,4,6),
                    (50,3,5),
                    (50,3,6),
                    (51,2,5),
                    (51,2,6)
                )
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{DungeonLevelIdColumn}],
                    [{LocationTypeIdColumn}]
                FROM [cte];")

    End Sub

    Public Function ReadAll(itemTypeId As Long, dungeonLevelId As Long) As IEnumerable(Of Long) Implements IItemTypeSpawnLocationTypeData.ReadAll
        Return Store.Record.ReadRecordsWithColumnValues(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationTypeIdColumn,
            (ItemTypeIdColumn, itemTypeId),
            (DungeonLevelIdColumn, dungeonLevelId))
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
End Class
