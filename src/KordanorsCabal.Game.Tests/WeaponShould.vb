Public Class WeaponShould
    Private Sub WithWeapon(stuffToDo As Action(Of Mock(Of IWorldData), Long, IWeapon))
        Const itemId = 1L
        Dim worldData As New Mock(Of IWorldData)
        Dim weapon As IWeapon = Game.Weapon.FromId(worldData.Object, itemId)
        stuffToDo(worldData, itemId, weapon)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub have_an_is_weapon_property()
        WithWeapon(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.IsWeapon.ShouldBeFalse
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 2L))
            End Sub)
    End Sub
    <Fact>
    Sub have_attack_dice()
        WithWeapon(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.AttackDice.ShouldBe(0)
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 2L))
            End Sub)
    End Sub
End Class
