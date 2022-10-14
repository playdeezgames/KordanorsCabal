Public Class ItemData
    Inherits BaseData
    Implements IItemData
    Friend Const TableName = "Items"
    Friend Const ItemIdColumn = "ItemId"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{ItemIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{ItemTypeIdColumn}] INT NOT NULL
            );")
    End Sub
    Public Function Create(itemType As Long) As Long Implements IItemData.Create
        Return Store.CreateRecord(
            AddressOf Initialize,
            TableName,
            (ItemTypeIdColumn, itemType))
    End Function

    Public Function ReadItemType(itemId As Long) As Long? Implements IItemData.ReadItemType
        Return Store.Column.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            ItemTypeIdColumn,
            (ItemIdColumn, itemId))
    End Function

    Public Sub Clear(itemId As Long) Implements IItemData.Clear
        World.CharacterEquipSlot.ClearForItem(itemId)
        World.InventoryItem.ClearForItem(itemId)
        World.ItemStatistic.ClearForItem(itemId)
        Store.Clear.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (ItemIdColumn, itemId))
    End Sub

    Public Sub WriteItemType(itemId As Long, itemType As Long) Implements IItemData.WriteItemType
        Store.Column.WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (ItemTypeIdColumn, itemType),
            (ItemIdColumn, itemId))
    End Sub
End Class
