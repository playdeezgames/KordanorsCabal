Public Class CharacterTypeSpawnLocationData
    Inherits BaseData
    Implements ICharacterTypeSpawnLocationData
    Friend Const TableName = "CharacterTypeSpawnLocations"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const DungeonLevelIdColumn = DungeonLevelData.DungeonLevelIdColumn
    Friend Const LocationTypeIdColumn = LocationTypeData.LocationTypeIdColumn
    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{CharacterTypeIdColumn}],
                    [{DungeonLevelIdColumn}],
                    [{LocationTypeIdColumn}]) AS
                (VALUES
                        (1,1,6),
                        (1,2,4),
                        (1,2,5),
                        (1,3,4),
                        (1,3,5),
                        (1,4,4),
                        (1,4,5),
                        (1,5,4),
                        (1,5,5),
                        (2,1,5),
                        (2,2,4),
                        (2,2,5),
                        (2,3,4),
                        (2,3,5),
                        (2,4,4),
                        (2,4,5),
                        (2,5,4),
                        (2,5,5),
                        (3,1,4),
                        (3,2,4),
                        (3,3,4),
                        (3,4,4),
                        (3,5,4),
                        (4,3,6),
                        (4,4,4),
                        (4,4,5),
                        (4,5,4),
                        (4,5,5),
                        (5,4,6),
                        (6,1,4),
                        (6,2,4),
                        (6,3,4),
                        (6,4,4),
                        (6,5,4),
                        (7,1,5),
                        (7,2,4),
                        (7,2,5),
                        (7,3,4),
                        (7,3,5),
                        (7,4,4),
                        (7,4,5),
                        (7,5,4),
                        (7,5,5),
                        (8,5,6),
                        (9,1,5),
                        (9,2,4),
                        (9,2,5),
                        (9,3,4),
                        (9,3,5),
                        (9,4,4),
                        (9,4,5),
                        (9,5,4),
                        (9,5,5),
                        (10,6,8),
                        (12,2,6),
                        (12,3,4),
                        (12,3,5),
                        (12,4,4),
                        (12,4,5),
                        (12,5,4),
                        (12,5,5),
                        (14,1,4),
                        (14,2,4),
                        (14,3,4),
                        (14,4,4),
                        (14,5,4),
                        (15,1,4),
                        (15,2,4),
                        (15,3,4),
                        (15,4,4),
                        (15,5,4),
                        (16,1,5),
                        (16,2,4),
                        (16,2,5),
                        (16,3,4),
                        (16,3,5),
                        (16,4,4),
                        (16,4,5),
                        (16,5,4),
                        (16,5,5)
                )
                SELECT 
                    [{CharacterTypeIdColumn}],
                    [{DungeonLevelIdColumn}],
                    [{LocationTypeIdColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(
                        characterTypeId As Long,
                        dungeonLevel As Long,
                        locationType As Long) As Boolean Implements ICharacterTypeSpawnLocationData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterTypeIdColumn,
            (CharacterTypeIdColumn, characterTypeId),
            (DungeonLevelIdColumn, dungeonLevel),
            (LocationTypeIdColumn, locationType)).HasValue
    End Function
End Class
