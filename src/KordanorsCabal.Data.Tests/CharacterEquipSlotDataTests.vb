Public Class CharacterEquipSlotDataTests
    Private Shared Sub WithCharacterEquipSlotData(
                                                 stuffToDo As Action(Of
                                                 Mock(Of IStore),
                                                 ICharacterEquipSlotData))
        Dim store As New Mock(Of IStore)
        Dim worldData As New WorldData(store.Object)
        Dim subject = worldData.CharacterEquipSlot
        stuffToDo(store, subject)
        store.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Public Sub ShouldClearOutDataForAnItem()
        WithCharacterEquipSlotData(
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
        WithCharacterEquipSlotData(
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
        WithCharacterEquipSlotData(
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
End Class
