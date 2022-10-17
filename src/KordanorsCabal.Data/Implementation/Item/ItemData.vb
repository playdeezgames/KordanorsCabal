﻿Public Class ItemData
    Inherits BaseData
    Implements IItemData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function Create(itemType As Long) As Long Implements IItemData.Create
        Return Store.Create.Entry(
            AddressOf NoInitializer,
            Items,
            (ItemTypeIdColumn, itemType))
    End Function

    Public Function ReadItemType(itemId As Long) As Long? Implements IItemData.ReadItemType
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Items,
            ItemTypeIdColumn,
            (ItemIdColumn, itemId))
    End Function

    Public Sub Clear(itemId As Long) Implements IItemData.Clear
        World.CharacterEquipSlot.ClearForItem(itemId)
        World.InventoryItem.ClearForItem(itemId)
        World.ItemStatistic.ClearForItem(itemId)
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            Items,
            (ItemIdColumn, itemId))
    End Sub

    Public Sub WriteItemType(itemId As Long, itemType As Long) Implements IItemData.WriteItemType
        Store.Column.Write(
            AddressOf NoInitializer,
            Items,
            (ItemTypeIdColumn, itemType),
            (ItemIdColumn, itemId))
    End Sub
End Class
