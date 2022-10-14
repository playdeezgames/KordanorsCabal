Public Class CharacterSpellDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterSpellData)

    Sub New()
        MyBase.New(Function(x) x.CharacterSpell)
    End Sub
    <Fact>
    Sub ShouldRemoveAllSpellLevelsForACharacterFromTheStore()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                store.Setup(Sub(x) x.Clear.ForValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.Clear.ForValue(
                                 It.IsAny(Of Action),
                                 CharacterSpells,
                                 (CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheDataStoreForASpellLevelForACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                Dim spellType = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read(characterId, spellType).ShouldBeNull
                store.Verify(Sub(x) x.Column.ReadValue(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 CharacterSpells,
                                 SpellLevelColumn,
                                 (CharacterIdColumn, characterId),
                                 (SpellTypeIdColumn, spellType)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForAllSpellLevelsForACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadForCharacter(characterId).ShouldBeNull
                store.Verify(Function(x) x.Record.ReadRecordsWithColumnValue(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 CharacterSpells,
                                 (SpellTypeIdColumn, SpellLevelColumn),
                                 (CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreWithASpellLevelForACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                Dim spellType = 2L
                Dim level = 3L
                store.SetupGet(Function(x) x.Replace).Returns((New Mock(Of IStoreReplace)).Object)
                subject.Write(characterId, spellType, level)
                store.Verify(Sub(x) x.Replace.ReplaceRecord(
                                 It.IsAny(Of Action),
                                 CharacterSpells,
                                 (CharacterIdColumn, characterId),
                                 (SpellTypeIdColumn, spellType),
                                 (SpellLevelColumn, level)))
            End Sub)
    End Sub
End Class
