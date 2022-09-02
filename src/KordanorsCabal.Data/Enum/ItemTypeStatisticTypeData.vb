Public Class ItemTypeStatisticTypeData
    Inherits BaseData
    Friend Const TableName = "ItemTypeStatisticTypes"
    Friend Const ItemTypeStatisticTypeIdColumn = "ItemTypeStatisticTypeId"
    Friend Const ItemTypeStatisticTypeNameColumn = "ItemTypeStatisticTypeName"

    Public Function ReadName(itemTypeStatisticTypeId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            ItemTypeStatisticTypeNameColumn,
            (ItemTypeStatisticTypeIdColumn, itemTypeStatisticTypeId))
    End Function
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeStatisticTypeIdColumn}],
                    [{ItemTypeStatisticTypeNameColumn}]) AS
                (VALUES
                    (1,'Encumbrance'),
                    (2,'AttackDice'),
                    (3,'MaximumDamage'),
                    (4,'DefendDice'),
                    (5,'MaximumDurability'),
                    (6,'Offer'),
                    (7,'Price'),
                    (8,'RepairPrice'))
                SELECT 
                    [{ItemTypeStatisticTypeIdColumn}],
                    [{ItemTypeStatisticTypeNameColumn}]
                FROM [cte];")
    End Sub
    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
    End Sub
End Class
