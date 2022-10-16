Public MustInherit Class ThingieShould(Of TThingie)
    Private ReadOnly ThingieMaker As Func(Of IWorldData, Long?, TThingie)
    Sub New(thingieMaker As Func(Of IWorldData, Long?, TThingie))
        Me.ThingieMaker = thingieMaker
    End Sub
    Protected Sub WithSubject(stuffToDo As Action(Of Mock(Of IWorldData), Long, TThingie))
        Const id = 1L
        Dim worldData As New Mock(Of IWorldData)
        worldData.Setup(Function(x) x.Player.Read())
        Dim subject As TThingie = ThingieMaker(worldData.Object, id)
        stuffToDo(worldData, id, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
    Protected Function FreshMock(Of TMock As Class)() As Mock(Of TMock)
        Return New Mock(Of TMock)()
    End Function
End Class
