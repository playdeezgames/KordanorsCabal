﻿Public Class ItemData
    Inherits BaseData
    Friend Const TableName = "Items"
    Friend Const ItemIdColumn = "ItemId"
    Friend Const ItemTypeColumn = "ItemType"

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{ItemIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{ItemTypeColumn}] INT NOT NULL
            );")
    End Sub
    Public Function Create(itemType As Long) As Long
        Return Store.CreateRecord(
            AddressOf Initialize,
            TableName,
            (ItemTypeColumn, itemType))
    End Function

    Public Function ReadItemType(itemId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            ItemTypeColumn,
            (ItemIdColumn, itemId))
    End Function

    Public Sub Clear(itemId As Long)
        World.CharacterEquipSlot.ClearForItem(itemId)
        World.InventoryItem.ClearForItem(itemId)
        World.ItemStatistic.ClearForItem(itemId)
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (ItemIdColumn, itemId))
    End Sub

    Public Sub WriteItemType(itemId As Long, itemType As Long)
        Store.WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (ItemTypeColumn, itemType),
            (ItemIdColumn, itemId))
    End Sub
End Class