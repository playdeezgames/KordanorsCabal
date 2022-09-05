Public Class CharacterDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterData)
    <Fact>
    Sub ShouldClearOutDataWhenClearIsCalled()
        WithSubobject(
            Function(x) x.Character,
            Sub(store, subject)
                Const characterId = 1L
                subject.Clear(characterId)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterQuests, (Columns.CharacterIdColumn, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterQuestCompletions, (Columns.CharacterIdColumn, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterEquipSlots, (Columns.CharacterIdColumn, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.Inventories, (Columns.CharacterIdColumn, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterLocations, (Columns.CharacterIdColumn, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterStatistics, (Columns.CharacterIdColumn, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.Players, (Columns.CharacterIdColumn, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterSpells, (Columns.CharacterIdColumn, characterId)), Times.Once)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.Characters, (Columns.CharacterIdColumn, characterId)), Times.Once)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldCreateRecordsInTheCharactersTable()
        WithSubobject(
            Function(x) x.Character,
            Sub(store, subject)
                Const characterType = 1L
                Const locationId = 2L
                subject.Create(characterType, locationId)
                store.Verify(Function(x) x.CreateRecord(
                     It.IsAny(Of Action),
                     Tables.Characters,
                     (Columns.CharacterTypeColumn, characterType),
                     (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForTheCharacterTypeOfAGivenCharacter()
        WithSubobject(
            Function(x) x.Character,
            Sub(store, subject)
                Dim characterId = 1L
                subject.ReadCharacterType(characterId)
                store.Verify(Function(x) x.ReadColumnValue(Of Long,
                             Long)(
                             It.IsAny(Of Action),
                             Tables.Characters,
                             Columns.CharacterTypeColumn,
                             (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForCharactersWithAGivenLocation()
        WithSubobject(
            Function(x) x.Character,
            Sub(store, subject)
                Dim locationId = 1L
                subject.ReadForLocation(locationId)
                store.Verify(Function(x) x.ReadRecordsWithColumnValue(Of Long,
                                 Long)(
                                 It.IsAny(Of Action),
                                 Tables.Characters,
                                 Columns.CharacterIdColumn,
                                 (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForCharacterLocation()
        WithSubobject(
            Function(x) x.Character,
            Sub(store, subject)
                Dim characterId = 1L
                subject.ReadLocation(characterId)
                store.Verify(Function(x) x.ReadColumnValue(Of Long,
                                 Long)(
                                 It.IsAny(Of Action),
                                 Tables.Characters,
                                 Columns.LocationIdColumn,
                                 (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheLocationOfAGivenCharacter()
        WithSubobject(
            Function(x) x.Character,
            Sub(store, subject)
                Dim characterId = 1L
                Dim locationId = 2L
                subject.WriteLocation(characterId, locationId)
                store.Verify(Sub(x) x.WriteColumnValue(
                                 It.IsAny(Of Action),
                                 Tables.Characters,
                                 (Columns.LocationIdColumn, locationId),
                                 (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
End Class
