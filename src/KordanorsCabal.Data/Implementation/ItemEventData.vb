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
End Class
