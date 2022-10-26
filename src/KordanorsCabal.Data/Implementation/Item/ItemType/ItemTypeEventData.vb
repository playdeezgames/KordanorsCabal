Public Class ItemTypeEventData
    Inherits BaseData
    Implements IItemTypeEventData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, eventId As Long) As String Implements IItemTypeEventData.Read
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            ItemTypeEvents,
            EventNameColumn,
            (ItemTypeIdColumn, itemTypeId),
            (EventIdColumn, eventId))
    End Function

    Public Function ReadAll(itemTypeId As Long) As List(Of Tuple(Of Long, String)) Implements IItemTypeEventData.ReadAll
        Return Store.Record.WithValue(Of Long, Long, String)(
            AddressOf NoInitializer,
            Tables.ItemTypeEvents,
            (EventIdColumn, EventNameColumn),
            (ItemTypeIdColumn, itemTypeId))
    End Function
End Class
