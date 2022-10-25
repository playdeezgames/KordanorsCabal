Public Class EquipmentShould
    Inherits ThingieShould(Of IEquipment)
    Sub New()
        MyBase.New(AddressOf Equipment.FromId)
    End Sub
    <Fact>
    Sub have_can_equip_property()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEquipSlot.ReadForItemType(It.IsAny(Of Long)))
                subject.CanEquip.ShouldBeFalse
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeEquipSlot.ReadForItemType(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_equip_slots()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEquipSlot.ReadForItemType(It.IsAny(Of Long)))
                subject.EquipSlots.ShouldBeEmpty
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeEquipSlot.ReadForItemType(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_equiped_buff()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                Const statisticTypeId = 3L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeCharacterStatisticBuff.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.EquippedBuff(StatisticType.FromId(worldData.Object, statisticTypeId)).ShouldBeNull
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeCharacterStatisticBuff.Read(itemTypeId, statisticTypeId))
            End Sub)
    End Sub
End Class
