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

    Public Sub New(store As Store)
        MyBase.New(store)
    End Sub

    Public Function Read(characterTypeId As Long) As IReadOnlyDictionary(Of String, Integer)
        Throw New NotImplementedException()
    End Function
End Class
