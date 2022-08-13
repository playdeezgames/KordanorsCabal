Public Class CharacterTypeLootData
    Inherits BaseData
    Friend Const TableName = "CharacterTypeLoots"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const WeightColumn = "Weight"

    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{CharacterTypeIdColumn}],
                    [{ItemTypeIdColumn}],
                    [{WeightColumn}]) AS
                (VALUES
                    (1,0,1),
                    (1,22,1),
                    (2,0,2),
                    (2,36,1),
                    (3,0,1),
                    (3,40,1),
                    (4,0,1),
                    (4,7,1),
                    (6,0,2),
                    (6,8,4),
                    (6,24,1),
                    (6,35,1),
                    (7,0,2),
                    (7,8,5),
                    (7,24,2),
                    (7,35,1),
                    (8,44,1),
                    (9,0,2),
                    (9,24,1),
                    (9,35,2),
                    (10,0,1),
                    (10,42,1),
                    (12,0,1),
                    (12,34,1),
                    (13,21,1),
                    (14,0,1),
                    (14,9,3),
                    (15,0,2),
                    (15,41,1),
                    (16,0,1),
                    (16,38,3)
                )
                SELECT 
                    [{CharacterTypeIdColumn}],
                    [{ItemTypeIdColumn}],
                    [{WeightColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As Store)
        MyBase.New(store)
    End Sub

    Public Function Read(characterTypeId As Long) As IReadOnlyDictionary(Of Long, Integer)
        Return Store.ReadRecordsWithColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            (ItemTypeIdColumn, WeightColumn),
            (CharacterTypeIdColumn, characterTypeId)).ToDictionary(
                Function(x) x.Item1,
                Function(x) CInt(x.Item2))
    End Function
End Class
