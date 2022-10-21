Public Class ItemLoreData
    Inherits BaseData
    Implements IItemLoreData

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadForItem(itemId As Long) As Long? Implements IItemLoreData.ReadForItem
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Tables.ItemLores,
            Columns.LoreIdColumn,
            (Columns.ItemIdColumn, itemId))
    End Function
End Class
