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
End Class
