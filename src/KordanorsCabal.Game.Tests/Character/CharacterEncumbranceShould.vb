﻿Public Class CharacterEncumbranceShould
    Inherits ThingieShould(Of ICharacterEncumbrance)
    Sub New()
        MyBase.New(Function(w, i) CharacterEncumbrance.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub have_encumbrance()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.CurrentEncumbrance.ShouldBe(0)
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
End Class