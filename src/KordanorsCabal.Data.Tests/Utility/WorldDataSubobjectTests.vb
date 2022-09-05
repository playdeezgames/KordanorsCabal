Public Class WorldDataSubobjectTests(Of TSubobject)
    Protected Shared Sub WithSubobject(
                                        accessor As Func(Of IWorldData, TSubobject),
                                        stuffToDo As Action(Of Mock(Of IStore), TSubobject))
        Dim store As New Mock(Of IStore)
        Dim worldData As New WorldData(store.Object)
        stuffToDo(store, accessor(worldData))
        store.VerifyNoOtherCalls()
    End Sub
End Class
