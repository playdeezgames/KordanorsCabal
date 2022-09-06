﻿Public Class CharacterQuestCompletionDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterQuestCompletionData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterQuestCompletion)
    End Sub

    <Fact>
    Sub ShouldClearTheStoreOfAnyQuestCompletionsAssociatedWithACharacter()
        WithSubobject(
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
    <Fact>
    Sub ShouldUpdateTheStoreWithHowManyTimesAGivenCharacterHasCompletedAGivenQuest()
        WithSubobject(
            Sub(store, subject)
                Dim characterId = 1L
                Dim questId = 2L
                Dim completions = 3L
                subject.Write(characterId, questId, completions)
                store.Verify(Sub(x) x.ReplaceRecord(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuestCompletions,
                                 (CharacterIdColumn, characterId),
                                 (QuestColumn, questId),
                                 (CompletionsColumn, completions)))
            End Sub)
    End Sub
End Class