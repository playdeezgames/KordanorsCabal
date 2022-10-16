Public Class CharacterShould
    Inherits ThingieShould(Of ICharacter)
    Sub New()
        MyBase.New(AddressOf Character.FromId)
    End Sub
    <Fact>
    Sub have_can_gamble()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 14L

                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CanGamble.ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_movement_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Movement.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_character_type()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterTypeId = 1L
                Dim characterData As New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadCharacterType(id)).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)

                subject.CharacterType.ShouldNotBeNull

                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_name()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterTypeId = 1L
                Dim characterData As New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadCharacterType(id)).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)
                Dim characterTypeData As New Mock(Of ICharacterTypeData)
                worldData.SetupGet(Function(x) x.CharacterType).Returns(characterTypeData.Object)

                subject.Name.ShouldBeNull

                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
                worldData.Verify(Function(x) x.CharacterType.ReadName(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_has_equipment()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.HasEquipment.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub equip()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemId = 2L
                Const itemTypeId = 3L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEquipSlot.ReadForItemType(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.ItemType.ReadName(It.IsAny(Of Long)))
                subject.Equip(Item.FromId(worldData.Object, itemId))
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeEquipSlot.ReadForItemType(itemTypeId))
                worldData.Verify(Function(x) x.ItemType.ReadName(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub unequip()
        WithSubject(
            Sub(worldData, id, subject)
                Const equipSlotId = 2L
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadForCharacterEquipSlot(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Unequip(EquipSlot.FromId(worldData.Object, equipSlotId))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadForCharacterEquipSlot(id, equipSlotId))
            End Sub)
    End Sub
    <Fact>
    Sub have_equipment()
        WithSubject(
            Sub(worldData, id, subject)
                Const equipSlotId = 2L
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadForCharacterEquipSlot(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.CurrentEquipment(EquipSlot.FromId(worldData.Object, equipSlotId))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadForCharacterEquipSlot(id, equipSlotId))
            End Sub)
    End Sub
    <Fact>
    Sub have_equipped_slots()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadEquipSlotsForCharacter(It.IsAny(Of Long)))
                subject.EquippedSlots.ShouldBeEmpty
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadEquipSlotsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub gamble()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.Gamble()
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 14))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(14))
            End Sub)
    End Sub
    <Fact>
    Sub have_mode()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Player.ReadPlayerMode()).Returns(0)
                subject.Mode.ShouldBe(Game.Constants.PlayerModes.None)
                worldData.Verify(Function(x) x.Player.ReadPlayerMode())
            End Sub)
    End Sub
    <Fact>
    Sub destroy()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Sub(x) x.Character.Clear(It.IsAny(Of Long)))
                subject.Destroy()
                worldData.Verify(Sub(x) x.Character.Clear(id))
            End Sub)
    End Sub
    <Fact>
    Sub do_armor_wear()
        WithSubject(
            Sub(worldData, id, subject)
                Const wear = 2L
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.DoArmorWear(wear).ShouldBeEmpty
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub do_weapon_wear()
        WithSubject(
            Sub(worldData, id, subject)
                Const wear = 2L
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.DoWeaponWear(wear).ShouldBeEmpty
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub enqueue_messages_without_sfx()
        WithSubject(
            Sub(worldData, id, subject)
                subject.EnqueueMessage("text")
            End Sub)
    End Sub
    <Fact>
    Sub enqueue_messages_with_sfx()
        WithSubject(
            Sub(worldData, id, subject)
                subject.EnqueueMessage(Sfx.Miss, "text")
            End Sub)
    End Sub
    <Fact>
    Sub have_quest_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Quest.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_health_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Health.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_physicalcombat_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.PhysicalCombat.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_advancement_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Advancement.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_mentalcombat_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.MentalCombat.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_spellbook_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Spellbook.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_mana_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Mana.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_statistics_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Statistics.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_items_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Items.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_repair_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Repair.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_encumbrance_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Encumbrance.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_statuses_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Statuses.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_interactions_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Interaction.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_equipment_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Equipment.ShouldNotBeNull
            End Sub)
    End Sub
End Class
