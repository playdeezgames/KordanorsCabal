Imports SQLitePCL

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
    <Fact>
    Sub can_repair()
        WithItem(
            Sub(worldData, itemId, item)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.Repair()
                worldData.Verify(Sub(x) x.ItemStatistic.Write(itemId, 1L, 0L))
            End Sub)
    End Sub
    <Fact>
    Sub have_an_is_armor_property()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.IsArmor.ShouldBeFalse
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 4L))
            End Sub)
    End Sub
    <Fact>
    Sub have_defense_dice()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.DefendDice.ShouldBe(0)
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 4L))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_equip_property()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEquipSlot.ReadForItemType(It.IsAny(Of Long)))
                item.CanEquip.ShouldBeFalse
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeEquipSlot.ReadForItemType(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_equip_slots()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEquipSlot.ReadForItemType(It.IsAny(Of Long)))
                item.EquipSlots.ShouldBeEmpty
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeEquipSlot.ReadForItemType(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_equiped_buff()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                Const statisticTypeId = 3L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeCharacterStatisticBuff.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.EquippedBuff(CharacterStatisticType.FromId(worldData.Object, statisticTypeId)).ShouldBeNull
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeCharacterStatisticBuff.Read(itemTypeId, statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_encumbrance()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.Encumbrance.ShouldBe(0)
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 1L))
            End Sub)
    End Sub
    <Fact>
    Sub purify()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.Purify()
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 1L))
            End Sub)
    End Sub
    <Fact>
    Sub have_is_consumed_property()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemType.ReadIsConsumed(It.IsAny(Of Long)))
                item.IsConsumed.ShouldBeFalse
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemType.ReadIsConsumed(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_use_test()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                Const characterId = 3L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.CanUse(Character.FromId(worldData.Object, characterId)).ShouldBeFalse
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 2L))
            End Sub)
    End Sub
    <Fact>
    Sub allow_use_by_a_character()
        WithItem(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                Const characterId = 3L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.Use(Character.FromId(worldData.Object, characterId))
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 3L))
            End Sub)
    End Sub
    <Fact>
    Sub allow_destruction()
        WithItem(
            Sub(worldData, itemId, item)
                worldData.Setup(Sub(x) x.Item.Clear(It.IsAny(Of Long)))
                item.Destroy()
                worldData.Verify(Sub(x) x.Item.Clear(itemId))
            End Sub)
    End Sub
End Class
