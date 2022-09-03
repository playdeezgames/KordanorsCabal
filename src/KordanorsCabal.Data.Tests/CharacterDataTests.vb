Public Class CharacterDataTests
    Private Shared Sub WithCharacterData(stuffToDo As Action(Of Mock(Of IStore), CharacterData))
        Dim store As New Mock(Of IStore)
        Dim worldData As New WorldData(store.Object)
        Dim subject = worldData.Character
        stuffToDo(store, subject)
        store.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldClearOutDataWhenClearIsCalled()
        WithCharacterData(
            Sub(store, subject)
                Const characterId = 1L
                subject.Clear(characterId)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterQuests, (Columns.CharacterId, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterQuestCompletions, (Columns.CharacterId, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterEquipSlots, (Columns.CharacterId, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.Inventories, (Columns.CharacterId, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterLocations, (Columns.CharacterId, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterStatistics, (Columns.CharacterId, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.Players, (Columns.CharacterId, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterSpells, (Columns.CharacterId, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.Characters, (Columns.CharacterId, characterId)), Times.Once)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldCreateRecordsInTheCharactersTable()
        WithCharacterData(
            Sub(store, subject)
                Const characterType = 1L
                Const locationId = 2L
                subject.Create(characterType, locationId)
                store.Verify(Function(x) x.CreateRecord(
                     It.IsAny(Of Action),
                     Tables.Characters,
                     (Columns.CharacterType, characterType),
                     (Columns.LocationId, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForTheCharacterTypeOfAGivenCharacter()
        WithCharacterData(
            Sub(store, subject)
                Dim characterId = 1L
                subject.ReadCharacterType(characterId)
                store.Verify(Function(x) x.ReadColumnValue(Of Long,
                             Long)(
                             It.IsAny(Of Action),
                             Tables.Characters,
                             Columns.CharacterType,
                             (Columns.CharacterId, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForCharactersWithAGivenLocation()
        WithCharacterData(
            Sub(store, subject)
                Dim locationId = 1L
                subject.ReadForLocation(locationId)
                store.Verify(Function(x) x.ReadRecordsWithColumnValue(Of Long,
                                 Long)(
                                 It.IsAny(Of Action),
                                 Tables.Characters,
                                 Columns.CharacterId,
                                 (Columns.LocationId, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForCharacterLocation()
        WithCharacterData(
            Sub(store, subject)
                Dim characterId = 1L
                subject.ReadLocation(characterId)
                store.Verify(Function(x) x.ReadColumnValue(Of Long,
                                 Long)(
                                 It.IsAny(Of Action),
                                 Tables.Characters,
                                 Columns.LocationId,
                                 (Columns.CharacterId, characterId)))
            End Sub)
    End Sub
End Class
