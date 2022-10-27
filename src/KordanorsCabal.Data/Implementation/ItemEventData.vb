Public Class ItemEventData
    Inherits BaseData
    Implements IItemEventData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Sub Write(itemId As Long, eventId As Long, eventName As String) Implements IItemEventData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            ItemEvents,
            (ItemIdColumn, itemId),
            (EventIdColumn, eventId),
            (EventNameColumn, eventName))
    End Sub
    Public Sub ClearForItem(itemId As Long) Implements IItemEventData.ClearForItem
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            ItemEvents,
            (ItemIdColumn, itemId))
    End Sub
    Public Function Read(itemId As Long, eventId As Long) As String Implements IItemEventData.Read
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            ItemEvents,
            Columns.EventNameColumn,
            (Columns.ItemIdColumn, itemId),
            (Columns.EventIdColumn, eventId))
    End Function
End Class
