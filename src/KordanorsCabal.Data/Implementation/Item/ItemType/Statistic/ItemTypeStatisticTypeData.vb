﻿Public Class ItemTypeStatisticTypeData
    Inherits BaseData
    Implements IItemTypeStatisticTypeData
    Friend Const TableName = "ItemTypeStatisticTypes"
    Friend Const ItemTypeStatisticTypeIdColumn = "ItemTypeStatisticTypeId"
    Friend Const ItemTypeStatisticTypeNameColumn = "ItemTypeStatisticTypeName"

    Public Function ReadName(itemTypeStatisticTypeId As Long) As String Implements IItemTypeStatisticTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf Initialize,
            TableName,
            ItemTypeStatisticTypeNameColumn,
            (ItemTypeStatisticTypeIdColumn, itemTypeStatisticTypeId))
    End Function
    Friend Sub Initialize()
        Store.Primitive.Execute($"CREATE TABLE IF NOT EXISTS [ItemTypeStatisticTypes] AS
                WITH [cte](
                    [ItemTypeStatisticTypeId],
                    [ItemTypeStatisticTypeName]) AS
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
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
End Class
