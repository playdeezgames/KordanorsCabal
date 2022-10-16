Public Class PlayerCharacterShould
    <Fact>
    Sub have_mode()
        Const id = 1L
        Dim worldData As New Mock(Of IWorldData)
        worldData.Setup(Function(x) x.Player.Read()).Returns(id)
        Dim subject As IPlayerCharacter = New PlayerCharacter(worldData.Object)
        worldData.Setup(Function(x) x.Player.ReadPlayerMode()).Returns(0)
        subject.Mode.ShouldBe(Game.Constants.PlayerModes.None)
        worldData.Verify(Function(x) x.Player.ReadPlayerMode())
        worldData.Verify(Function(x) x.Player.Read())
        worldData.VerifyNoOtherCalls()
    End Sub
End Class
