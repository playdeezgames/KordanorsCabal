Public MustInherit Class BaseThingieTests(Of TThingie)
    Private ReadOnly ThingieMaker As Func(Of IWorldData, Long?, TThingie)
    Sub New(thingieMaker As Func(Of IWorldData, Long?, TThingie))
        Me.ThingieMaker = thingieMaker
    End Sub
    Protected Sub WithSubject(stuffToDo As Action(Of Long, Mock(Of IWorldData), TThingie))
        Dim id = 9999L
        Dim worldData As New Mock(Of IWorldData)
        Dim subject As TThingie = ThingieMaker(worldData.Object, id)
        stuffToDo(id, worldData, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
    Protected Function FreshMock(Of TMock As Class)() As Mock(Of TMock)
        Return New Mock(Of TMock)()
    End Function
End Class
