Public Class CharacterData_should
    Inherits WorldDataSubobjectTests(Of ICharacterData)

    Public Sub New()
        MyBase.New(Function(x) x.Character)
    End Sub

    <Fact>
    Sub clear_out_data_when_clear_is_called()
        WithSubobject(
            Sub(store, checker, subject)
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
    Sub create_records_in_the_characters_table()
        WithSubobject(
            Sub(store, checker, subject)
                Const characterType = 1L
                Const locationId = 2L
                subject.Create(characterType, locationId)
                store.Verify(Function(x) x.CreateRecord(
                     It.IsAny(Of Action),
                     Tables.Characters,
                     (Columns.CharacterTypeIdColumn, characterType),
                     (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub query_for_the_character_type_of_a_given_character()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                subject.ReadCharacterType(characterId)
                store.Verify(Function(x) x.ReadColumnValue(Of Long,
                             Long)(
                             It.IsAny(Of Action),
                             Tables.Characters,
                             Columns.CharacterTypeIdColumn,
                             (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub query_for_characters_with_a_given_location()
        WithSubobject(
            Sub(store, checker, subject)
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
    Sub query_for_character_location()
        WithSubobject(
            Sub(store, checker, subject)
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
    Sub update_the_location_of_a_given_character()
        WithSubobject(
            Sub(store, checker, subject)
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
