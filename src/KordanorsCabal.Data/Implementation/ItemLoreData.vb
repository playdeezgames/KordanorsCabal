Public Class ItemLoreData
    Inherits BaseData
    Implements IItemLoreData

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Sub ClearForItem(itemId As Long) Implements IItemLoreData.ClearForItem
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            Tables.ItemLores,
            (Columns.ItemIdColumn, itemId))
    End Sub

    Public Sub Write(itemId As Long, loreId As Long) Implements IItemLoreData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            Tables.ItemLores,
            (Columns.ItemIdColumn, itemId),
            (Columns.LoreIdColumn, loreId))
    End Sub

    Public Function ReadForItem(itemId As Long) As Long? Implements IItemLoreData.ReadForItem
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Tables.ItemLores,
            Columns.LoreIdColumn,
            (Columns.ItemIdColumn, itemId))
    End Function

    Public Function ReadAllLore() As IEnumerable(Of Long) Implements IItemLoreData.ReadAllLore
        Return Store.Record.All(Of Long)(AddressOf NoInitializer, Tables.ItemLores, Columns.LoreIdColumn)
    End Function
End Class
