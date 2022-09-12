Public MustInherit Class BaseThingieTests(Of TThingie)
    Private ReadOnly ThingieMaker As Func(Of IWorldData, Long?, TThingie)
    Sub New(thingieMaker As Func(Of IWorldData, Long?, TThingie))
        Me.ThingieMaker = thingieMaker
    End Sub
    Protected Sub WithAnySubject(stuffToDo As Action(Of Long, Mock(Of IWorldData), TThingie))
        Dim itemTypeId = 1L
        Dim worldData As New Mock(Of IWorldData)
        Dim subject As TThingie = ThingieMaker(worldData.Object, itemTypeId)
        stuffToDo(itemTypeId, worldData, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
End Class
