Public Class WorldDataSubobjectTests(Of TSubobject)
    Private ReadOnly _accessor As Func(Of IWorldData, TSubobject)
    Protected Sub WithSubobject(
                                        stuffToDo As Action(Of Mock(Of IStore), Mock(Of IChecker), TSubobject))
        Dim store As New Mock(Of IStore)
        Dim checker As New Mock(Of IChecker)
        Dim worldData As New WorldData(store.Object, checker.Object)
        stuffToDo(store, checker, _accessor.Invoke(worldData))
        store.VerifyNoOtherCalls()
        checker.VerifyNoOtherCalls()
    End Sub
    Protected Sub New(accessor As Func(Of IWorldData, TSubobject))
        _accessor = accessor
    End Sub
End Class
