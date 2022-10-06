Public Class CharacterTypeBribeData
    Inherits BaseData
    Implements ICharacterTypeBribeData
    Friend Const TableName = "CharacterTypeBribes"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{CharacterTypeIdColumn}],
                    [{ItemTypeIdColumn}]) AS
                (VALUES
                    (6,26),
                    (6,28),
                    (7,26),
                    (9,28),
                    (10,30).
                    (2,37)
                )
                SELECT 
                    [{CharacterTypeIdColumn}],
                    [{ItemTypeIdColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(characterTypeId As Long, itemType As Long) As Boolean Implements ICharacterTypeBribeData.Read
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            ItemTypeIdColumn,
            (CharacterTypeIdColumn, characterTypeId),
            (ItemTypeIdColumn, itemType)).HasValue
    End Function
End Class
