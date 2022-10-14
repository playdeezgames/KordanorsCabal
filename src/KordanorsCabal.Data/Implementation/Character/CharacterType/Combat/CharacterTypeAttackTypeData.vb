Public Class CharacterTypeAttackTypeData
    Inherits BaseData
    Implements ICharacterTypeAttackTypeData
    Friend Const TableName = "CharacterTypeAttackTypes"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const AttackTypeColumn = "AttackType"
    Friend Const WeightColumn = "Weight"
    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{CharacterTypeIdColumn}],
                    [{AttackTypeColumn}],
                    [{WeightColumn}]) AS
                (VALUES
                    (1,1,1),
                    (2,1,1),
                    (3,1,1),
                    (4,1,1),
                    (5,1,1),
                    (6,1,1),
                    (7,1,1),
                    (8,1,1),
                    (9,1,3),
                    (9,2,1),
                    (10,1,1),
                    (11,1,1),
                    (12,1,1),
                    (13,1,1),
                    (14,1,1),
                    (15,1,1),
                    (16,1,1)
                )
                SELECT 
                    [{CharacterTypeIdColumn}],
                    [{AttackTypeColumn}],
                    [{WeightColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(characterTypeId As Long) As IReadOnlyDictionary(Of Long, Integer) Implements ICharacterTypeAttackTypeData.Read
        Dim results = Store.Record.ReadRecordsWithColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            (AttackTypeColumn, WeightColumn),
            (CharacterTypeIdColumn, characterTypeId))
        If results Is Nothing Then
            Return Nothing
        End If
        Return results.ToDictionary(
                Function(x) x.Item1,
                Function(x) CInt(x.Item2))
    End Function
End Class
