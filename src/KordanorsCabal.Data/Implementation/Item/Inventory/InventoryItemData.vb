Public Class InventoryItemData
    Inherits BaseData
    Implements IInventoryItemData
    Friend Const TableName = "InventoryItems"
    Friend Const InventoryIdColumn = InventoryData.InventoryIdColumn
    Friend Const ItemIdColumn = ItemData.ItemIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Sub Write(inventoryId As Long, itemId As Long) Implements IInventoryItemData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            TableName,
            (InventoryIdColumn, inventoryId),
            (ItemIdColumn, itemId))
    End Sub

    Public Function ReadItems(inventoryId As Long) As IEnumerable(Of Long) Implements IInventoryItemData.ReadItems
        Return Store.Record.WithValues(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            ItemIdColumn,
            (InventoryIdColumn, inventoryId))
    End Function

    Public Sub ClearForItem(itemId As Long) Implements IInventoryItemData.ClearForItem
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            TableName,
            (ItemIdColumn, itemId))
    End Sub
End Class
