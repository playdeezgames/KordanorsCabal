﻿Public Class ItemTypeShopTypeData
    Inherits BaseData
    Implements IItemTypeShopTypeData
    Friend Const TableName = "ItemTypeShopTypes"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const ShopTypeIdColumn = "ShopTypeId"
    Friend Const TransactionTypeIdColumn = "TransactionTypeId"

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{ShopTypeIdColumn}],
                    [{TransactionTypeIdColumn}]) AS
                (VALUES
                    (10,2,1),
                    (15,2,1),
                    (16,2,1),
                    (18,2,1),
                    (19,2,1),
                    (20,2,1)
                )
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{ShopTypeIdColumn}],
                    [{TransactionTypeIdColumn}]
                FROM [cte];")

    End Sub

    Public Function ReadForTransactionType(itemTypeId As Long, transationTypeId As Long) As IEnumerable(Of Long) Implements IItemTypeShopTypeData.ReadForTransactionType
        Return Store.ReadRecordsWithColumnValues(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            ShopTypeIdColumn,
            (ItemTypeIdColumn, itemTypeId),
            (TransactionTypeIdColumn, transationTypeId))
    End Function
End Class
