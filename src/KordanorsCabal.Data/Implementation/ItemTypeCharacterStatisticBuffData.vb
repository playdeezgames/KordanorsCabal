Friend Class ItemTypeCharacterStatisticBuffData
    Inherits BaseData
    Implements IItemTypeCharacterStatisticBuffData
    Friend Const TableName = "ItemTypeCharacterStatisticBuffs"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const CharacterStatisticTypeIdColumn = CharacterStatisticTypeData.CharacterStatisticTypeIdColumn
    Friend Const BuffColumn = "Buff"
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{CharacterStatisticTypeIdColumn}],
                    [{BuffColumn}]) AS
                (VALUES
                    (45,6,1),
                    (46,6,1),
                    (49,2,1),
                    (50,5,1),
                    (51,8,1)
                )
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{CharacterStatisticTypeIdColumn}],
                    [{BuffColumn}]
                FROM [cte];")
    End Sub


    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, characterStatisticTypeId As Long) As Long? Implements IItemTypeCharacterStatisticBuffData.Read
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            BuffColumn,
            (ItemTypeIdColumn, itemTypeId),
            (CharacterStatisticTypeIdColumn, characterStatisticTypeId))
    End Function
End Class
