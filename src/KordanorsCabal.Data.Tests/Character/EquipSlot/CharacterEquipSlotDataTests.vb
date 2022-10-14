Imports SQLitePCL

Public Class CharacterEquipSlotDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterEquipSlotData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterEquipSlot)
    End Sub

    <Fact>
    Public Sub ShouldClearOutDataForAnItem()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                store.Setup(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ClearForItem(itemId)
                store.Verify(Sub(x) x.Clear.ClearForColumnValue(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             (Columns.ItemIdColumn, itemId)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldClearOutDataForACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Const characterId = 1L
                store.Setup(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.Clear.ClearForColumnValue(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             (Columns.CharacterIdColumn, characterId)),
                             Times.Once)
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldClearOutDataForACharactersEquipSlot()
        WithSubobject(
            Sub(store, checker, subject)
                Const characterId = 1L
                Const equipSlot = 2L
                store.Setup(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.Clear(characterId, equipSlot)
                store.Verify(Sub(x) x.Clear.ClearForColumnValues(
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
            Sub(store, checker, subject)
                Const characterId = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadEquipSlotsForCharacter(characterId).ShouldBeNull
                store.Verify(Function(x) x.Record.ReadRecordsWithColumnValue(Of Long, Long)(
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
            Sub(store, checker, subject)
                Const characterId = 1L
                Const equipSlot = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadForCharacterEquipSlot(characterId, equipSlot).ShouldBeNull
                store.Verify(Sub(x) x.Column.ReadColumnValue(Of Long, Long, Long)(
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
            Sub(store, checker, subject)
                Const characterId = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadItemsForCharacter(characterId).ShouldBeNull
                store.Verify(Function(x) x.Record.ReadRecordsWithColumnValue(Of Long, Long)(
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
            Sub(store, checker, subject)
                Const characterId = 1L
                Const equipSlot = 2L
                Const itemId = 3L
                store.SetupGet(Function(x) x.Replace).Returns((New Mock(Of IStoreReplace)).Object)
                subject.Write(characterId, equipSlot, itemId)
                store.Verify(Sub(x) x.Replace.ReplaceRecord(Of Long, Long, Long)(
                             It.IsAny(Of Action),
                             Tables.CharacterEquipSlots,
                             (Columns.CharacterIdColumn, characterId),
                             (Columns.EquipSlotColumn, equipSlot),
                             (Columns.ItemIdColumn, itemId)),
                             Times.Once)
            End Sub)
    End Sub
End Class
