Public Class CharacterQuestCompletionDataTests
    Inherits WorldDataSubobjectTests
    <Fact>
    Sub ShouldClearTheStoreOfAnyQuestCompletionsAssociatedWithACharacter()
        WithSubobject(
            Function(x) x.CharacterQuestCompletion,
            Sub(store, subject)
                Dim characterId = 1L
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.ClearForColumnValue(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuestCompletions,
                                 (CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForHowManTimesACharacterHasCompletedAQuest()
        WithSubobject(
            Function(x) x.CharacterQuestCompletion,
            Sub(store, subject)
                Dim characterId = 1L
                Dim questId = 2L
                subject.Read(characterId, questId).ShouldBeNull
                store.Verify(Function(x) x.ReadColumnValue(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuestCompletions,
                                 Columns.CompletionsColumn,
                                 (CharacterIdColumn, characterId),
                                 (QuestColumn, questId)))
            End Sub)
    End Sub
End Class
