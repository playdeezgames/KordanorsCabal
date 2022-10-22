Public Class LoreData
    Inherits BaseData
    Implements ILoreData

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadAll() As IEnumerable(Of Long) Implements ILoreData.ReadAll
        Return Store.Record.All(Of Long)(
            AddressOf NoInitializer,
            Tables.Lores,
            Columns.LoreIdColumn)
    End Function

    Public Function ReadText(loreId As Long) As String Implements ILoreData.ReadText
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            Tables.Lores,
            Columns.LoreTextColumn,
            (Columns.LoreIdColumn, loreId))
    End Function

    Public Function ReadItemName(loreId As Long) As String Implements ILoreData.ReadItemName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            Tables.Lores,
            Columns.ItemNameColumn,
            (Columns.LoreIdColumn, loreId))
    End Function
End Class
