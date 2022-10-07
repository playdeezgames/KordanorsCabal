Public Class ItemShould
    Private Sub WithItem(stuffToDo As Action(Of Mock(Of IWorldData), Long, IItem))
        Const itemId = 1L
        Dim worldData As New Mock(Of IWorldData)
        Dim item As IItem = Game.Item.FromId(worldData.Object, itemId)
        stuffToDo(worldData, itemId, item)
        worldData.VerifyNoOtherCalls()
    End Sub

    <Fact>
    Sub have_a_name()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemType.ReadName(It.IsAny(Of Long)))
                item.Name.ShouldBeNull
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemType.ReadName(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_an_item_type()
        WithItem(
            Sub(worldData, itemId, item)
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long)))
                item.ItemType.ShouldBeNull
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
            End Sub)
    End Sub
    <Fact>
    Sub have_an_is_weapon_property()
        WithItem(
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
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.AttackDice.ShouldBe(0)
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 2L))
            End Sub)
    End Sub
    <Fact>
    Sub have_maximum_damage()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.MaximumDamage.ShouldBeNull
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 3L))
            End Sub)
    End Sub
    <Fact>
    Sub have_durability()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1L)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.Durability.ShouldBe(1L)
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 5L))
                worldData.Verify(Function(x) x.ItemStatistic.Read(itemId, 1L))
            End Sub)
    End Sub
    <Fact>
    Sub have_maximum_durability()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.MaximumDurability.ShouldBeNull
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 5L))
            End Sub)
    End Sub
    <Fact>
    Sub reduce_durability()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1L)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.ReduceDurability(1L)
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 5L))
                worldData.Verify(Function(x) x.ItemStatistic.Read(itemId, 1L))
                worldData.Verify(Sub(x) x.ItemStatistic.Write(itemId, 1L, 1L))
            End Sub)
    End Sub
    <Fact>
    Sub have_is_broken_flag()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1L)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.IsBroken.ShouldBeFalse
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 5L))
                worldData.Verify(Function(x) x.ItemStatistic.Read(itemId, 1L))
            End Sub)
    End Sub
    <Fact>
    Sub have_needs_repair_flag()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(2L)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1L)
                item.NeedsRepair.ShouldBeTrue
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 5L))
                worldData.Verify(Function(x) x.ItemStatistic.Read(itemId, 1L))
            End Sub)
    End Sub
End Class
