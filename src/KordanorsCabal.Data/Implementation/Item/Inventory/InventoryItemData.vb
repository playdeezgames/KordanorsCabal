Public Class InventoryItemData
    Inherits BaseData
    Implements IInventoryItemData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Sub Write(inventoryId As Long, itemId As Long) Implements IInventoryItemData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            InventoryItems,
            (InventoryIdColumn, inventoryId),
            (ItemIdColumn, itemId))
    End Sub

    Public Function ReadItems(inventoryId As Long) As IEnumerable(Of Long) Implements IInventoryItemData.ReadItems
        Return Store.Record.WithValues(Of Long, Long)(
            AddressOf NoInitializer,
            InventoryItems,
            ItemIdColumn,
            (InventoryIdColumn, inventoryId))
    End Function

    Public Sub ClearForItem(itemId As Long) Implements IInventoryItemData.ClearForItem
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            InventoryItems,
            (ItemIdColumn, itemId))
    End Sub

    Public Function ReadForItem(itemId As Long) As Long? Implements IInventoryItemData.ReadForItem
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Tables.InventoryItems,
            Columns.InventoryIdColumn,
            (Columns.ItemIdColumn, itemId))
    End Function
End Class
