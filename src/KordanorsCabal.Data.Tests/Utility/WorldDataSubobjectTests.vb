Public Class WorldDataSubobjectTests(Of TSubobject)
    Private ReadOnly _accessor As Func(Of IWorldData, TSubobject)
    Protected Sub WithSubobject(
                                        stuffToDo As Action(Of Mock(Of IStore), TSubobject))
        Dim store As New Mock(Of IStore)
        Dim worldData As New WorldData(store.Object)
        stuffToDo(store, _accessor.Invoke(worldData))
        store.VerifyNoOtherCalls()
    End Sub
    Protected Sub New(accessor As Func(Of IWorldData, TSubobject))
        _accessor = accessor
    End Sub
End Class
