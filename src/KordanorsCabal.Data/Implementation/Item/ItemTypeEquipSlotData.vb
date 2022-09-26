Public Class ItemTypeEquipSlotData
    Inherits BaseData
    Implements IItemTypeEquipSlotData
    Friend Const TableName = "ItemTypeEquipSlots"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const EquipSlotIdColumn = EquipSlotData.EquipSlotIdColumn

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{EquipSlotIdColumn}]) AS
                (VALUES
                    (10,1),
                    (15,2),
                    (16,3),
                    (17,4),
                    (18,1),
                    (19,1),
                    (20,4),
                    (27,5),
                    (30,1),
                    (43,1),
                    (45,6),
                    (46,7),
                    (46,8),
                    (48,6),
                    (49,6),
                    (50,6),
                    (51,6),
                    (52,6)
                )
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{EquipSlotIdColumn}]
                FROM [cte];")
    End Sub
End Class
