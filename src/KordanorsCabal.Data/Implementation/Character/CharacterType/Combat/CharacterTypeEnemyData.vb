Public Class CharacterTypeEnemyData
    Inherits BaseData
    Implements ICharacterTypeEnemyData
    Friend Const TableName = "CharacterTypeEnemies"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const EnemyCharacterTypeIdColumn = "EnemyCharacterTypeId"
    Friend Sub Initialize()
        Store.Primitive.Execute($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{CharacterTypeIdColumn}],
                    [{EnemyCharacterTypeIdColumn}]) AS
                (VALUES
                    (1,11),
                    (2,11),
                    (3,11),
                    (4,11),
                    (5,11),
                    (6,11),
                    (7,11),
                    (8,11),
                    (9,11),
                    (10,11),
                    (12,11),
                    (13,11),
                    (14,11),
                    (15,11),
                    (16,11),
                    (11,1),
                    (11,2),
                    (11,3),
                    (11,4),
                    (11,5),
                    (11,6),
                    (11,7),
                    (11,8),
                    (11,9),
                    (11,10),
                    (11,12),
                    (11,13),
                    (11,14),
                    (11,15),
                    (11,16)
                )
                SELECT 
                    [{CharacterTypeIdColumn}],
                    [{EnemyCharacterTypeIdColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(characterTypeId As Long, enemyCharacterTypeId As Long) As Boolean Implements ICharacterTypeEnemyData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            EnemyCharacterTypeIdColumn,
            (CharacterTypeIdColumn, characterTypeId),
            (EnemyCharacterTypeIdColumn, enemyCharacterTypeId)).HasValue
    End Function
End Class
