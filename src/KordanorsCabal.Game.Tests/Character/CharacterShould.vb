Imports SQLitePCL

Public Class CharacterShould
    Inherits ThingieShould(Of ICharacter)
    Sub New()
        MyBase.New(AddressOf Character.FromId)
    End Sub
    <Fact>
    Sub can_accept_quests()
        WithSubject(
            Sub(worldData, id, subject)
                Const questTypeId = 1L
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                worldData.Setup(Function(x) x.QuestType.ReadCanAcceptEventName(It.IsAny(Of Long)))
                subject.AcceptQuest(QuestType.FromId(worldData.Object, questTypeId))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, Nothing, {1L}))
                worldData.Verify(Function(x) x.QuestType.ReadCanAcceptEventName(questTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub change_current_mp()
        WithSubject(
            Sub(worldData, id, subject)
                Const stress = 2L
                Const statisticTypeId = 13L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                subject.AddStress(stress)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub add_xp()
        WithSubject(
            Sub(worldData, id, subject)
                Const xp = 2L
                Const statisticTypeId = 16L
                Const otherStatisticTypeId = 17L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                subject.AddXP(xp).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, otherStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(otherStatisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub assign_points()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 9

                Dim statisticType As New Mock(Of ICharacterStatisticType)
                worldData.Setup(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.Setup(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.AssignPoint(statisticType.Object)

                statisticType.VerifyNoOtherCalls()
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_accept_quest()
        WithSubject(
            Sub(worldData, id, subject)
                Const questTypeId = 2L
                worldData.Setup(Function(x) x.CharacterQuest.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                worldData.Setup(Function(x) x.QuestType.ReadCanAcceptEventName(It.IsAny(Of Long)))
                subject.CanAcceptQuest(QuestType.FromId(worldData.Object, questTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterQuest.Read(id, questTypeId))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, Nothing, {1}))
                worldData.Verify(Function(x) x.QuestType.ReadCanAcceptEventName(questTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_be_bribed_with()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterTypeId = 2L
                Const itemTypeId = 14L

                Dim characterData = New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadCharacterType(It.IsAny(Of Long))).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)
                worldData.SetupGet(Function(X) X.CharacterTypeBribe).Returns((New Mock(Of ICharacterTypeBribeData)).Object)

                subject.CanBeBribedWith(Game.ItemType.FromId(worldData.Object, itemTypeId)).ShouldBeFalse

                characterData.Verify(Function(x) x.ReadCharacterType(id))
                worldData.Verify(Function(x) x.CharacterTypeBribe.Read(characterTypeId, itemTypeId))
                characterData.VerifyNoOtherCalls()
            End Sub)
    End Sub
    <Fact>
    Sub have_can_cast_spell()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 1L
                worldData.SetupGet(Function(x) x.SpellType).Returns((New Mock(Of ISpellTypeData)).Object)
                worldData.SetupGet(Function(x) x.Events).Returns((New Mock(Of IEventData)).Object)
                Dim spellType = Game.SpellType.FromId(worldData.Object, spellTypeId)

                subject.CanCastSpell(spellType).ShouldBeFalse

                worldData.Verify(Function(x) x.SpellType.ReadCastCheck(spellTypeId))
                worldData.Verify(Function(x) x.Events.Test(It.IsAny(Of IWorldData), Nothing, id))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_do_intimidation()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 3

                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CanDoIntimidation().ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_fight()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanFight().ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
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
    Sub have_can_interact()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanInteract().ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_intimidate()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 4L

                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CanIntimidate.ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_learn()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 1L
                Dim spellType = Game.SpellType.FromId(worldData.Object, spellTypeId)

                worldData.SetupGet(Function(x) x.SpellType).Returns((New Mock(Of ISpellTypeData)).Object)
                worldData.SetupGet(Function(x) x.CharacterSpell).Returns((New Mock(Of ICharacterSpellData)).Object)

                subject.CanLearn(spellType).ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterSpell.Read(id, spellTypeId))
                worldData.Verify(Function(x) x.SpellType.ReadMaximumLevel(spellTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_map()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanMap.ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
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
    Sub have_location()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationId = 1L
                Dim characterData As New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadLocation(id)).Returns(locationId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)

                subject.Location.ShouldNotBeNull

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
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
    Sub have_current_hp()
        WithSubject(
            Sub(worldData, id, subject)
                Const firstStatisticTypeId = 6L
                Const secondStatisticTypeId = 12L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CurrentHP.ShouldBe(0)

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, secondStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(secondStatisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_is_dead()
        WithSubject(
            Sub(worldData, id, subject)
                Const firstStatisticTypeId = 6L
                Const secondStatisticTypeId = 12L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.IsDead.ShouldBeTrue

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, secondStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(secondStatisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_is_enemy()
        WithSubject(
            Sub(worldData, id, subject)
                Const otherCharacterId = 2L
                Const characterTypeId = 3L
                Const otherCharacterTypeId = 4L
                Dim otherCharacter = Character.FromId(worldData.Object, otherCharacterId)
                Dim characterData = FreshMock(Of ICharacterData)()
                characterData.Setup(Function(x) x.ReadCharacterType(id)).Returns(characterTypeId)
                characterData.Setup(Function(x) x.ReadCharacterType(otherCharacterId)).Returns(otherCharacterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)
                worldData.SetupGet(Function(x) x.CharacterTypeEnemy).Returns(FreshMock(Of ICharacterTypeEnemyData).Object)

                subject.IsEnemy(otherCharacter).ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
                worldData.Verify(Function(x) x.Character.ReadCharacterType(otherCharacterId))
                worldData.Verify(Function(x) x.CharacterTypeEnemy.Read(characterTypeId, otherCharacterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_is_demoralized()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.IsDemoralized.ShouldBeTrue
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 4))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 7))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 13))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(4))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(7))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(13))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_is_undead()
        WithSubject(
            Sub(worldData, id, subject)
                subject.IsUndead.ShouldBeFalse
            End Sub)
    End Sub
    <Fact>
    Sub have_maximum_hp()
        WithSubject(
            Sub(worldData, id, subject)
                subject.MaximumHP.ShouldBe(0)
            End Sub)
    End Sub
    <Fact>
    Sub have_maximum_mana()
        WithSubject(
            Sub(worldData, id, subject)
                subject.MaximumMana.ShouldBe(0)
            End Sub)
    End Sub
    <Fact>
    Sub have_parting_shot()
        WithSubject(
            Sub(worldData, id, subject)
                subject.PartingShot.ShouldBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_statistics()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 2L
                subject.Statistic(CharacterStatisticType.FromId(worldData.Object, statisticTypeId)).ShouldBe(0)
            End Sub)
    End Sub
    <Fact>
    Sub have_has_statistics()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 2L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.HasStatistic(CharacterStatisticType.FromId(worldData.Object, statisticTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_inventory()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                subject.Inventory.Id.ShouldBe(0)
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_hunger()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.Hunger.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 20))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(20))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_highness()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.Highness.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 19))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(19))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_food_poisoning()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.FoodPoisoning.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 21))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(21))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_drunkenness()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.Drunkenness.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 18))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(18))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_current_mp()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.CurrentMP.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 7))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(7))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 13))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(13))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_current_mana()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.CurrentMana.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 8))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(8))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 15))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(15))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_chafing()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.Chafing.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 22))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(22))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_money()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Money.ShouldBe(0)
            End Sub)
    End Sub
    <Fact>
    Sub have_encumbrance()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.Encumbrance.ShouldBe(0)
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(0))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_maximum_encumbrance()
        WithSubject(
            Sub(worldData, id, subject)
                subject.MaximumEncumbrance.ShouldBe(0)
            End Sub)
    End Sub
    <Fact>
    Sub have_is_encumbered()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.IsEncumbered.ShouldBeFalse
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(0))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 1))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 24))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 25))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(1))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(24))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(25))
            End Sub)
    End Sub
    <Fact>
    Sub have_has_item_type()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                subject.HasItemType(ItemType.FromId(worldData.Object, itemTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(0))
            End Sub)
    End Sub
    <Fact>
    Sub have_has_items_to_repair()
        WithSubject(
            Sub(worldData, id, subject)
                Const shoppeTypeId = 2L
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.HasItemsToRepair(ShoppeType.FromId(worldData.Object, shoppeTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(0))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_items_to_repair()
        WithSubject(
            Sub(worldData, id, subject)
                Const shoppeTypeId = 2L
                subject.ItemsToRepair(ShoppeType.FromId(worldData.Object, shoppeTypeId)).ShouldBeEmpty
            End Sub)
    End Sub
    <Fact>
    Sub have_needs_healing()
        WithSubject(
            Sub(worldData, id, subject)
                subject.NeedsHealing.ShouldBeFalse
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
    Sub have_has_spells()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterSpell.ReadForCharacter(It.IsAny(Of Long)))
                subject.HasSpells.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterSpell.ReadForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub interact()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Interact()
            End Sub)
    End Sub
    <Fact>
    Sub have_can_move_forward()
        WithSubject(
            Sub(worldData, id, subject)
                Const directionId = 2L
                worldData.Setup(Function(x) x.Player.ReadDirection()).Returns(directionId)
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Character.ReadLocation(It.IsAny(Of Long)))
                subject.CanMoveForward()
                worldData.Verify(Function(x) x.Player.ReadDirection())
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(0))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 1))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 24))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 25))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(1))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(24))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(25))
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_use_item()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemId = 2L
                subject.UseItem(Item.FromId(worldData.Object, itemId))
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
                subject.Unequip(EquipSlot.FromId(worldData.Object, equipSlotId))
            End Sub)
    End Sub
    <Fact>
    Sub cast()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 2L
                worldData.Setup(Function(x) x.SpellType.ReadCastCheck(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                subject.Cast(SpellType.FromId(worldData.Object, spellTypeId))
                worldData.Verify(Function(x) x.SpellType.ReadCastCheck(spellTypeId))
                worldData.Verify(Function(x) x.SpellType.ReadName(spellTypeId))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, Nothing, {id}))
            End Sub)
    End Sub
    <Fact>
    Sub have_equipment()
        WithSubject(
            Sub(worldData, id, subject)
                Const equipSlotId = 2L
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadForCharacterEquipSlot(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Equipment(EquipSlot.FromId(worldData.Object, equipSlotId))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadForCharacterEquipSlot(id, equipSlotId))
            End Sub)
    End Sub
    <Fact>
    Sub have_has_visited()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationId = 2L
                worldData.Setup(Function(x) x.CharacterLocation.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.HasVisited(Location.FromId(worldData.Object, locationId))
                worldData.Verify(Function(x) x.CharacterLocation.Read(id, locationId))
            End Sub)
    End Sub
    <Fact>
    Sub have_spells()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Spells.ShouldBeNull
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
    Sub complete_quest()
        WithSubject(
            Sub(worldData, id, subject)
                Const questTypeId = 2L
                worldData.Setup(Function(x) x.CharacterQuest.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.CompleteQuest(QuestType.FromId(worldData.Object, questTypeId))
                worldData.Verify(Function(x) x.CharacterQuest.Read(id, questTypeId))
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
    Sub do_intimidation()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.DoIntimidation()
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 3))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(3))
            End Sub)
    End Sub
    <Fact>
    Sub fight()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Character.ReadLocation(It.IsAny(Of Long)))
                subject.Fight()
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub purify_items()
        WithSubject(
            Sub(worldData, id, subject)
                subject.PurifyItems()
            End Sub)
    End Sub
    <Fact>
    Sub have_direction()
        WithSubject(
            Sub(worldData, id, subject)
                Const directionId = 2L
                worldData.Setup(Function(x) x.Player.ReadDirection()).Returns(directionId)
                subject.Direction.Id.ShouldBe(directionId)
                worldData.Verify(Function(x) x.Player.ReadDirection())
            End Sub)
    End Sub
    <Fact>
    Sub run()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Run()
            End Sub)
    End Sub
    <Fact>
    Sub have_mode()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Mode.ShouldBe(PlayerMode.Neutral)
            End Sub)
    End Sub
    <Fact>
    Sub have_is_fully_assigned()
        WithSubject(
            Sub(worldData, id, subject)
                subject.IsFullyAssigned.ShouldBeFalse
            End Sub)
    End Sub
    <Fact>
    Sub set_statistic()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 2L
                Const statisticValue = 3L
                subject.SetStatistic(CharacterStatisticType.FromId(worldData.Object, statisticTypeId), statisticValue)
            End Sub)
    End Sub
    <Fact>
    Sub change_statistic()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 2L
                Const delta = 3L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.ChangeStatistic(CharacterStatisticType.FromId(worldData.Object, statisticTypeId), delta)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub get_statistic()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 2L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.GetStatistic(CharacterStatisticType.FromId(worldData.Object, statisticTypeId)).ShouldBeNull
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_has_quest()
        WithSubject(
            Sub(worldData, id, subject)
                Const questTypeId = 2L
                worldData.Setup(Function(x) x.CharacterQuest.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.HasQuest(QuestType.FromId(worldData.Object, questTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterQuest.Read(id, questTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub learn()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 2L
                subject.Learn(SpellType.FromId(worldData.Object, spellTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub kill()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                Dim actual = subject.Kill(Character.FromId(worldData.Object, characterId))
                actual.Item1.ShouldBeNull
                actual.Item2.ShouldBeNull
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
    Sub heal()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Heal()
            End Sub)
    End Sub
    <Fact>
    Sub do_counter_attacks()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationId = 2L
                worldData.Setup(Function(x) x.Character.ReadLocation(It.IsAny(Of Long))).Returns(locationId)
                subject.DoCounterAttacks()
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Function(x) x.Character.ReadForLocation(locationId))
            End Sub)
    End Sub
    <Fact>
    Sub determine_damage()
        WithSubject(
            Sub(worldData, id, subject)
                Const damage = 2L
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                subject.DetermineDamage(damage).ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 10))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(10))
            End Sub)
    End Sub
    <Fact>
    Sub do_damage()
        WithSubject(
            Sub(worldData, id, subject)
                Const damage = 2L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.DoDamage(damage)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 12))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(12))
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
    Sub do_immobilization()
        WithSubject(
            Sub(worldData, id, subject)
                Const wear = 2L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.DoImmobilization(wear)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 23))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(23))
            End Sub)
    End Sub
    <Fact>
    Sub do_fatigue()
        WithSubject(
            Sub(worldData, id, subject)
                Const wear = 2L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.DoFatigue(wear)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 15))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(15))
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
    Sub roll_willpower()
        WithSubject(
            Sub(worldData, id, subject)
                subject.RollWillpower().ShouldBe(0)
            End Sub)
    End Sub
    <Fact>
    Sub roll_defend()
        WithSubject(
            Sub(worldData, id, subject)
                subject.RollDefend().ShouldBe(0)
            End Sub)
    End Sub
    <Fact>
    Sub roll_attack()
        WithSubject(
            Sub(worldData, id, subject)
                subject.RollAttack().ShouldBe(0)
            End Sub)
    End Sub
    <Fact>
    Sub roll_influence()
        WithSubject(
            Sub(worldData, id, subject)
                subject.RollInfluence().ShouldBe(0)
            End Sub)
    End Sub
    <Fact>
    Sub roll_power()
        WithSubject(
            Sub(worldData, id, subject)
                subject.RollPower().ShouldBe(0)
            End Sub)
    End Sub
    <Fact>
    Sub roll_spell_dice()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 2L
                subject.RollSpellDice(SpellType.FromId(worldData.Object, spellTypeId)).ShouldBe(0)
            End Sub)
    End Sub
End Class
