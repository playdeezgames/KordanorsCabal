Public Class ItemTypeEquipShould
    Inherits ThingieShould(Of IItemTypeEquip)

    Public Sub New()
        MyBase.New(Function(w, i) ItemTypeEquip.FromId(w, i))
    End Sub
    <Fact>
    Sub item_types_have_equip_slots()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.Setup(Function(x) x.ItemTypeEquipSlot.ReadForItemType(It.IsAny(Of Long)))
                subject.EquipSlots.ShouldBeEmpty
                worldData.Verify(Function(x) x.ItemTypeEquipSlot.ReadForItemType(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_equipped_buffs()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                Const statisticTypeId = 2L
                worldData.Setup(Function(x) x.ItemTypeCharacterStatisticBuff.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.EquippedBuff(StatisticType.FromId(worldData.Object, statisticTypeId)).ShouldBeNull
                worldData.Verify(Function(x) x.ItemTypeCharacterStatisticBuff.Read(itemTypeId, statisticTypeId))
            End Sub)
    End Sub
End Class
