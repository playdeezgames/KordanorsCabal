Public Class CharacterTypeData
    Inherits BaseData
    Friend Const TableName = "CharacterTypes"
    Friend Const CharacterTypeIdColumn = "CharacterTypeId"
    Friend Const CharacterTypeNameColumn = "CharacterTypeName"

    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{CharacterTypeIdColumn}],
                    [{CharacterTypeNameColumn}]) AS
                (VALUES
                        (1,'Acolyte'),
                        (2,'Badger'),
                        (3,'Bat'),
                        (4,'Bishop'),
                        (5,'Cabal Leader'),
                        (6,'Goblin'),
                        (7,'Goblin Elite'),
                        (8,'Kordanor'),
                        (9,'Malcontent'),
                        (10,'MoonPerson'),
                        (11,'N00b'),
                        (12,'Priest'),
                        (13,'Rat'),
                        (14,'Skeleton'),
                        (15,'Snake'),
                        (16,'Zombie')
                )
                SELECT 
                    [{CharacterTypeIdColumn}],
                    [{CharacterTypeNameColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As Store)
        MyBase.New(store)
    End Sub

    Public Function ReadName(characterTypeId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            CharacterTypeNameColumn,
            (CharacterTypeIdColumn, characterTypeId))
    End Function
End Class
