Public Class CharacterEquipSlotDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterEquipSlotData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterEquipSlot)
    End Sub

    <Fact>
    Public Sub ShouldClearOutDataForAnItem()
        WithSubobject(
            Sub(store, subject)
                Const itemId = 1L
                subject.ClearForItem(itemId)
                store.Verify(Sub(x) x.ClearForColumnValue(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             (Columns.ItemIdColumn, itemId)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldClearOutDataForACharacter()
        WithSubobject(
            Sub(store, subject)
                Const characterId = 1L
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.ClearForColumnValue(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             (Columns.CharacterIdColumn, characterId)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldClearOutDataForACharactersEquipSlot()
        WithSubobject(
            Sub(store, subject)
                Const characterId = 1L
                Const equipSlot = 2L
                subject.Clear(characterId, equipSlot)
                store.Verify(Sub(x) x.ClearForColumnValues(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             (Columns.CharacterIdColumn, characterId),
                             (Columns.EquipSlotColumn, equipSlot)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheStoreForTheInUseEquipSlotsOfACharacter()
        WithSubobject(
            Sub(store, subject)
                Const characterId = 1L
                subject.ReadEquipSlotsForCharacter(characterId).ShouldBeNull
                store.Verify(Sub(x) x.ReadRecordsWithColumnValue(Of Long, Long)(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             Columns.EquipSlotColumn,
                             (Columns.CharacterIdColumn, characterId)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheStoreForTheContentsOfAnEquipSlotOfACharacter()
        WithSubobject(
            Sub(store, subject)
                Const characterId = 1L
                Const equipSlot = 2L
                subject.ReadForCharacterEquipSlot(characterId, equipSlot).ShouldBeNull
                store.Verify(Sub(x) x.ReadColumnValue(Of Long, Long, Long)(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             Columns.ItemIdColumn,
                             (Columns.CharacterIdColumn, characterId),
                             (Columns.EquipSlotColumn, equipSlot)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheStoreForTheEquippedItemsOfACharacter()
        WithSubobject(
            Sub(store, subject)
                Const characterId = 1L
                subject.ReadItemsForCharacter(characterId).ShouldBeNull
                store.Verify(Sub(x) x.ReadRecordsWithColumnValue(Of Long, Long)(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             Columns.ItemIdColumn,
                             (Columns.CharacterIdColumn, characterId)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldReplaceTheStoreContentsOfAnEquipSlotOfACharacter()
        WithSubobject(
            Sub(store, subject)
                Const characterId = 1L
                Const equipSlot = 2L
                Const itemId = 3L
                subject.Write(characterId, equipSlot, itemId)
                store.Verify(Sub(x) x.ReplaceRecord(Of Long, Long, Long)(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             (Columns.CharacterIdColumn, characterId),
                             (Columns.EquipSlotColumn, equipSlot),
                             (Columns.ItemIdColumn, itemId)),
                             Times.Once)
            End Sub)
    End Sub
End Class
