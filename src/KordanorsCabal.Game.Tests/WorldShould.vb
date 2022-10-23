Public Class WorldShould
    Private Sub WithWorld(stuffToDo As Action(Of Mock(Of IWorldData), IWorld))
        Dim worldData As New Mock(Of IWorldData)
        Dim subject As IWorld = World.FromWorldData(worldData.Object)
        stuffToDo(worldData, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub have_is_valid()
        WithWorld(
            Sub(worldData, subject)
                worldData.SetupGet(Function(x) x.Player).Returns((New Mock(Of IPlayerData)).Object)
                subject.IsValid.ShouldBeFalse
                worldData.Verify(Function(x) x.Player.Read)
            End Sub)
    End Sub
End Class
