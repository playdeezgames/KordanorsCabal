Public Class CharacterEquipmentShould
    Inherits ThingieShould(Of ICharacterEquipment)
    Sub New()
        MyBase.New(Function(w, i) CharacterEquipment.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub have_has_equipment()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.HasEquipment.ShouldBeFalse
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
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
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub unequip()
        WithSubject(
            Sub(worldData, id, subject)
                Const equipSlotId = 2L
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadForCharacterEquipSlot(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Unequip(EquipSlot.FromId(worldData.Object, equipSlotId))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadForCharacterEquipSlot(id, equipSlotId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_equipment()
        WithSubject(
            Sub(worldData, id, subject)
                Const equipSlotId = 2L
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadForCharacterEquipSlot(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.CurrentEquipment(EquipSlot.FromId(worldData.Object, equipSlotId))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadForCharacterEquipSlot(id, equipSlotId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_equipped_slots()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadEquipSlotsForCharacter(It.IsAny(Of Long)))
                subject.EquippedSlots.ShouldBeEmpty
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadEquipSlotsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub do_armor_wear()
        WithSubject(
            Sub(worldData, id, subject)
                Const wear = 2L
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.DoArmorWear(wear).ShouldBeEmpty
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub do_weapon_wear()
        WithSubject(
            Sub(worldData, id, subject)
                Const wear = 2L
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.DoWeaponWear(wear).ShouldBeEmpty
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
