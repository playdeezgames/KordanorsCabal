Public Class CharacterTypePartingShotData
    Inherits BaseData
    Friend Const TableName = "CharacterTypePartingShots"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const PartingShotColumn = "PartingShot"
    Friend Const WeightColumn = "Weight"
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{CharacterTypeIdColumn}],
                    [{PartingShotColumn}],
                    [{WeightColumn}]) AS
                (VALUES
                    (6,'@#$% you!',1),
                    (7,'@#$% you!',1),
                    (16,'Breains!',1)
                )
                SELECT 
                    [{CharacterTypeIdColumn}],
                    [{PartingShotColumn}],
                    [{WeightColumn}]
                FROM [cte];")

    End Sub

    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(characterTypeId As Long) As IReadOnlyDictionary(Of String, Integer)
        Return Store.ReadRecordsWithColumnValue(Of Long, String, Long)(
            AddressOf Initialize,
            TableName,
            (PartingShotColumn, WeightColumn),
            (CharacterTypeIdColumn, characterTypeId)).
            ToDictionary(Function(x) x.Item1, Function(x) CInt(x.Item2))
    End Function
End Class
