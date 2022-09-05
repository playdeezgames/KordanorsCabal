Public Class CharacterEquipSlotDataTests
    Inherits WorldDataSubobjectTests
    <Fact>
    Public Sub ShouldClearOutDataForAnItem()
        WithSubobject(
            Function(x) x.CharacterEquipSlot,
            Sub(store, subject)
                Const itemId = 1L
                subject.ClearForItem(itemId)
                store.Verify(Sub(x) x.ClearForColumnValue(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             (Columns.ItemId, itemId)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldClearOutDataForACharacter()
        WithSubobject(
            Function(x) x.CharacterEquipSlot,
            Sub(store, subject)
                Const characterId = 1L
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.ClearForColumnValue(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             (Columns.CharacterId, characterId)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldClearOutDataForACharactersEquipSlot()
        WithSubobject(
            Function(x) x.CharacterEquipSlot,
            Sub(store, subject)
                Const characterId = 1L
                Const equipSlot = 2L
                subject.Clear(characterId, equipSlot)
                store.Verify(Sub(x) x.ClearForColumnValues(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             (Columns.CharacterId, characterId),
                             (Columns.EquipSlot, equipSlot)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheStoreForTheInUseEquipSlotsOfACharacter()
        WithSubobject(
            Function(x) x.CharacterEquipSlot,
            Sub(store, subject)
                Const characterId = 1L
                subject.ReadEquipSlotsForCharacter(characterId).ShouldBeNull
                store.Verify(Sub(x) x.ReadRecordsWithColumnValue(Of Long, Long)(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             Columns.EquipSlot,
                             (Columns.CharacterId, characterId)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheStoreForTheContentsOfAnEquipSlotOfACharacter()
        WithSubobject(
            Function(x) x.CharacterEquipSlot,
            Sub(store, subject)
                Const characterId = 1L
                Const equipSlot = 2L
                subject.ReadForCharacterEquipSlot(characterId, equipSlot).ShouldBeNull
                store.Verify(Sub(x) x.ReadColumnValue(Of Long, Long, Long)(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             Columns.ItemId,
                             (Columns.CharacterId, characterId),
                             (Columns.EquipSlot, equipSlot)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheStoreForTheEquippedItemsOfACharacter()
        WithSubobject(
            Function(x) x.CharacterEquipSlot,
            Sub(store, subject)
                Const characterId = 1L
                subject.ReadItemsForCharacter(characterId).ShouldBeNull
                store.Verify(Sub(x) x.ReadRecordsWithColumnValue(Of Long, Long)(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             Columns.ItemId,
                             (Columns.CharacterId, characterId)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldReplaceTheStoreContentsOfAnEquipSlotOfACharacter()
        WithSubobject(
            Function(x) x.CharacterEquipSlot,
            Sub(store, subject)
                Const characterId = 1L
                Const equipSlot = 2L
                Const itemId = 3L
                subject.Write(characterId, equipSlot, itemId)
                store.Verify(Sub(x) x.ReplaceRecord(Of Long, Long, Long)(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             (Columns.CharacterId, characterId),
                             (Columns.EquipSlot, equipSlot),
                             (Columns.ItemId, itemId)),
                             Times.Once)
            End Sub)
    End Sub
End Class
