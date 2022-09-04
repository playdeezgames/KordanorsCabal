﻿Public Class InventoryItemData
    Inherits BaseData
    Friend Const TableName = "InventoryItems"
    Friend Const InventoryIdColumn = InventoryData.InventoryIdColumn
    Friend Const ItemIdColumn = ItemData.ItemIdColumn

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        World.Item.Initialize()
        World.Inventory.Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{InventoryIdColumn}] INT NOT NULL,
                [{ItemIdColumn}] INT NOT NULL UNIQUE
            );")
    End Sub
    Public Sub Write(inventoryId As Long, itemId As Long)
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (InventoryIdColumn, inventoryId),
            (ItemIdColumn, itemId))
    End Sub

    Public Function ReadItems(inventoryId As Long) As IEnumerable(Of Long)
        Return Store.ReadRecordsWithColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            ItemIdColumn,
            (InventoryIdColumn, inventoryId))
    End Function

    Public Sub ClearForItem(itemId As Long)
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (ItemIdColumn, itemId))
    End Sub
End Class