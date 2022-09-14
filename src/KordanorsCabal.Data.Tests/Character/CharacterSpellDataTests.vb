﻿Public Class CharacterSpellDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterSpellData)

    Sub New()
        MyBase.New(Function(x) x.CharacterSpell)
    End Sub
    <Fact>
    Sub ShouldRemoveAllSpellLevelsForACharacterFromTheStore()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.ClearForColumnValue(
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
                subject.Read(characterId, spellType).ShouldBeNull
                store.Verify(Sub(x) x.ReadColumnValue(Of Long, Long, Long)(
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
                subject.ReadForCharacter(characterId).ShouldBeNull
                store.Verify(Sub(x) x.ReadRecordsWithColumnValue(Of Long, Long, Long)(
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
                subject.Write(characterId, spellType, level)
                store.Verify(Sub(x) x.ReplaceRecord(
                                 It.IsAny(Of Action),
                                 CharacterSpells,
                                 (CharacterIdColumn, characterId),
                                 (SpellTypeIdColumn, spellType),
                                 (SpellLevelColumn, level)))
            End Sub)
    End Sub
End Class
