Public Class CharacterTypeSpawnCountData
    Inherits BaseData
    Friend Const TableName = "CharacterTypeSpawnCounts"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const DungeonLevelIdColumn = DungeonLevelData.DungeonLevelIdColumn
    Friend Const SpawnCountColumn = "SpawnCount"
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{CharacterTypeIdColumn}],
                    [{DungeonLevelIdColumn}],
                    [{SpawnCountColumn}]) AS
                (VALUES
                    (1,1,1),
                    (1,2,10),
                    (1,3,25),
                    (2,1,24),
                    (2,2,12),
                    (3,1,15),
                    (3,2,30),
                    (3,3,45),
                    (3,4,30),
                    (3,5,15),
                    (4,3,1),
                    (4,4,10),
                    (4,5,25),
                    (5,4,1),
                    (6,1,30),
                    (6,2,45),
                    (6,3,30),
                    (6,4,15),
                    (7,1,5),
                    (7,2,15),
                    (7,3,30),
                    (7,4,45),
                    (7,5,30),
                    (8,5,1),
                    (9,1,30),
                    (9,2,15),
                    (10,6,100),
                    (12,2,1),
                    (12,3,15),
                    (12,4,30),
                    (12,5,45),
                    (14,1,30),
                    (14,2,45),
                    (14,3,30),
                    (14,4,15),
                    (15,1,15),
                    (15,2,30),
                    (15,3,45),
                    (15,4,30),
                    (15,5,15),
                    (16,2,30),
                    (16,3,45),
                    (16,4,30)
                )
                SELECT 
                    [{CharacterTypeIdColumn}],
                    [{DungeonLevelIdColumn}],
                    [{SpawnCountColumn}]
                FROM [cte];")
    End Sub

    Public Function ReadSpawnCount(characterTypeId As Long, dungeonLevelId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            SpawnCountColumn,
            (CharacterTypeIdColumn, characterTypeId),
            (DungeonLevelIdColumn, dungeonLevelId))
    End Function

    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
    End Sub
End Class
