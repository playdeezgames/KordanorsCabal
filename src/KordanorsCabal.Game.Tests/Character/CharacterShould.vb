Imports SQLitePCL

Public Class CharacterShould
    Inherits ThingieShould(Of ICharacter)
    Sub New()
        MyBase.New(AddressOf Character.FromId)
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
                Const characterTypeId = 2L
                worldData.Setup(Function(x) x.Character.ReadCharacterType(It.IsAny(Of Long))).Returns(characterTypeId)
                worldData.Setup(Function(x) x.CharacterType.ReadIsUndead(It.IsAny(Of Long)))
                subject.IsUndead.ShouldBeFalse
                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
                worldData.Verify(Function(x) x.CharacterType.ReadIsUndead(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_maximum_mana()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.MaximumMana.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 8))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_statistics()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 2L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.Statistic(CharacterStatisticType.FromId(worldData.Object, statisticTypeId)).ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, It.IsAny(Of Long)))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
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
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.Money.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 14))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(14))
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
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.MaximumEncumbrance.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 1))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 24))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 25))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(1))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(24))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(25))
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
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.ItemsToRepair(ShoppeType.FromId(worldData.Object, shoppeTypeId)).ShouldBeEmpty
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(0))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
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
                worldData.Setup(Function(x) x.Character.ReadLocation(It.IsAny(Of Long)))
                subject.Interact()
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_use_item()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemId = 2L
                Const itemTypeId = 3L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.UseItem(Item.FromId(worldData.Object, itemId))
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 2L))
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
    Sub have_spells()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterSpell.ReadForCharacter(It.IsAny(Of Long)))
                subject.Spells.ShouldBeEmpty
                worldData.Verify(Function(x) x.CharacterSpell.ReadForCharacter(id))
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
    Sub purify_items()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.PurifyItems()
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(0))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
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
    Sub have_is_fully_assigned()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.IsFullyAssigned.ShouldBeTrue
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 9))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(9))
            End Sub)
    End Sub
    <Fact>
    Sub set_statistic()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 2L
                Const statisticValue = 3L
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadMinimumValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadMaximumValue(It.IsAny(Of Long))).Returns(10)
                worldData.Setup(Sub(x) x.CharacterStatistic.Write(It.IsAny(Of Long), It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.SetStatistic(CharacterStatisticType.FromId(worldData.Object, statisticTypeId), statisticValue)
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadMinimumValue(statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadMaximumValue(statisticTypeId))
                worldData.Verify(Sub(x) x.CharacterStatistic.Write(id, statisticTypeId, statisticValue))
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
    Sub learn()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 2L
                worldData.Setup(Function(x) x.CharacterSpell.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.SpellType.ReadMaximumLevel(It.IsAny(Of Long)))
                subject.Learn(SpellType.FromId(worldData.Object, spellTypeId))
                worldData.Verify(Function(x) x.CharacterSpell.Read(id, spellTypeId))
                worldData.Verify(Function(x) x.SpellType.ReadMaximumLevel(spellTypeId))
                worldData.Verify(Function(x) x.SpellType.ReadName(spellTypeId))
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
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.RollWillpower().ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 4))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 18))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 19))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 22))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(4))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(18))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(19))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(22))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub roll_influence()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.RollInfluence().ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 3))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 18))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 19))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 22))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(3))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(18))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(19))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(22))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub roll_power()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.RollPower().ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 5))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 18))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 19))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 22))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(5))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(18))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(19))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(22))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub roll_spell_dice()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 2L
                worldData.Setup(Function(x) x.CharacterSpell.ReadForCharacter(It.IsAny(Of Long)))
                subject.RollSpellDice(SpellType.FromId(worldData.Object, spellTypeId)).ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterSpell.ReadForCharacter(id))
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
    Sub have_combat_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Combat.ShouldNotBeNull
            End Sub)
    End Sub
End Class
