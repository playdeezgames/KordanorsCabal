Public Class CharacterQuestDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterQuestData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterQuest)
    End Sub
    <Fact>
    Sub ShouldClearAQuestForACharacter()
        WithSubobject(
            Sub(store, subject)
                Dim characterId = 1L
                Dim quest = 2L
                subject.Clear(characterId, quest)
                store.Verify(Sub(x) x.ClearForColumnValues(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuests,
                                 (CharacterIdColumn, characterId),
                                 (QuestColumn, quest)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldClearAllQuestsForACharacter()
        WithSubobject(
            Sub(store, subject)
                Dim characterId = 1L
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.ClearForColumnValue(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuests,
                                 (CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheExistenceOfAQuestForACharacter()
        WithSubobject(
            Sub(store, subject)
                Dim characterId = 1L
                Dim quest = 2L
                subject.Read(characterId, quest).ShouldBeFalse
                store.Verify(Sub(x) x.ReadRecordsWithColumnValues(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuests,
                                 CharacterIdColumn,
                                 (CharacterIdColumn, characterId),
                                 (QuestColumn, quest)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreWithAQuestForACharacter()
        WithSubobject(
            Sub(store, subject)
                Dim characterId = 1L
                Dim quest = 2L
                subject.Write(characterId, quest)
                store.Verify(Sub(x) x.ReplaceRecord(Of Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuests,
                                 (CharacterIdColumn, characterId),
                                 (QuestColumn, quest)))
            End Sub)
    End Sub
End Class
