Public Class ItemTypeEventData
    Inherits BaseData
    Implements IItemTypeEventData

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, eventId As Long) As String Implements IItemTypeEventData.Read
        Return Nothing
    End Function
End Class
