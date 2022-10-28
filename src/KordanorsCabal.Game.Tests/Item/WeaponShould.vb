Public Class WeaponShould
    Private Sub WithSubject(stuffToDo As Action(Of Mock(Of IWorldData), Long, IWeapon))
        Const itemId = 1L
        Dim worldData As New Mock(Of IWorldData)
        Dim weapon As IWeapon = Game.Weapon.FromId(worldData.Object, itemId)
        stuffToDo(worldData, itemId, weapon)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub have_an_is_weapon_property()
        WithSubject(
            Sub(worldData, itemId, item)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.IsWeapon.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemStatistic.Read(itemId, 30L))
            End Sub)
    End Sub
    <Fact>
    Sub have_attack_dice()
        WithSubject(
            Sub(worldData, itemId, item)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.AttackDice.ShouldBe(0)
                worldData.Verify(Function(x) x.ItemStatistic.Read(itemId, 30L))
            End Sub)
    End Sub
    <Fact>
    Sub have_maximum_damage()
        WithSubject(
            Sub(worldData, itemId, item)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.MaximumDamage.ShouldBeNull
                worldData.Verify(Function(x) x.ItemStatistic.Read(itemId, 31L))
            End Sub)
    End Sub
End Class
