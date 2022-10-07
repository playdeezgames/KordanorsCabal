﻿Public Class InventoryItemData
    Inherits BaseData
    Implements IInventoryItemData
    Friend Const TableName = "InventoryItems"
    Friend Const InventoryIdColumn = InventoryData.InventoryIdColumn
    Friend Const ItemIdColumn = ItemData.ItemIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Item, ItemData).Initialize()
        CType(World.Inventory, InventoryData).Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{InventoryIdColumn}] INT NOT NULL,
                [{ItemIdColumn}] INT NOT NULL UNIQUE
            );")
    End Sub
    Public Sub Write(inventoryId As Long, itemId As Long) Implements IInventoryItemData.Write
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (InventoryIdColumn, inventoryId),
            (ItemIdColumn, itemId))
    End Sub

    Public Function ReadItems(inventoryId As Long) As IEnumerable(Of Long) Implements IInventoryItemData.ReadItems
        Return Store.ReadRecordsWithColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            ItemIdColumn,
            (InventoryIdColumn, inventoryId))
    End Function

    Public Sub ClearForItem(itemId As Long) Implements IInventoryItemData.ClearForItem
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (ItemIdColumn, itemId))
    End Sub
End Class