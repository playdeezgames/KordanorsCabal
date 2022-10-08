Imports SQLitePCL

Public Class ItemShould
    Private Sub WithSubject(stuffToDo As Action(Of Mock(Of IWorldData), Long, IItem))
        Const itemId = 1L
        Dim worldData As New Mock(Of IWorldData)
        Dim item As IItem = Game.Item.FromId(worldData.Object, itemId)
        stuffToDo(worldData, itemId, item)
        worldData.VerifyNoOtherCalls()
    End Sub

    <Fact>
    Sub have_a_name()
        WithSubject(
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
        WithSubject(
            Sub(worldData, itemId, item)
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long)))
                item.ItemType.ShouldBeNull
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_equip_property()
        WithSubject(
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
        WithSubject(
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
        WithSubject(
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
        WithSubject(
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
        WithSubject(
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
        WithSubject(
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
        WithSubject(
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
        WithSubject(
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
        WithSubject(
            Sub(worldData, itemId, item)
                worldData.Setup(Sub(x) x.Item.Clear(It.IsAny(Of Long)))
                item.Destroy()
                worldData.Verify(Sub(x) x.Item.Clear(itemId))
            End Sub)
    End Sub
    <Fact>
    Sub have_weapon_subobject()
        WithSubject(
            Sub(worldData, itemId, item)
                item.Weapon.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_durability_subobject()
        WithSubject(
            Sub(worldData, itemId, item)
                item.Durability.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_repair_subobject()
        WithSubject(
            Sub(worldData, itemId, item)
                item.Repair.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_armor_subobject()
        WithSubject(
            Sub(worldData, itemId, item)
                item.Armor.ShouldNotBeNull
            End Sub)
    End Sub
End Class
