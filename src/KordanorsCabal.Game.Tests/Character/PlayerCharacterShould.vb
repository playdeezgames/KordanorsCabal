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
    <Fact>
    Sub have_shoppe_type()
        Dim worldData As New Mock(Of IWorldData)
        Const characterId = 2L
        worldData.Setup(Function(x) x.Player.Read()).Returns(characterId)
        Dim subject As IPlayerCharacter = New PlayerCharacter(worldData.Object)
        subject.ShoppeType.ShouldBeNull
        worldData.Verify(Function(x) x.Player.Read())
        worldData.Verify(Function(x) x.Player.ReadShoppeType())
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub set_shoppe_type()
        Const shoppeTypeId = 2L
        Dim worldData As New Mock(Of IWorldData)
        Const characterId = 2L
        worldData.Setup(Function(x) x.Player.Read()).Returns(characterId)
        Dim subject As IPlayerCharacter = New PlayerCharacter(worldData.Object)
        subject.ShoppeType = ShoppeType.FromId(worldData.Object, shoppeTypeId)
        worldData.Verify(Function(x) x.Player.Read())
        worldData.Verify(Sub(x) x.Player.WriteShoppeType(shoppeTypeId))
        worldData.VerifyNoOtherCalls()
    End Sub
End Class
