Public Class CharacterQuestCompletionDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterQuestCompletionData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterQuestCompletion)
    End Sub

    <Fact>
    Sub ShouldClearTheStoreOfAnyQuestCompletionsAssociatedWithACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                store.Setup(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.Clear.ClearForColumnValue(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuestCompletions,
                                 (CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForHowManTimesACharacterHasCompletedAQuest()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                Dim questId = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read(characterId, questId).ShouldBeNull
                store.Verify(Function(x) x.Column.ReadColumnValue(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuestCompletions,
                                 Columns.CompletionsColumn,
                                 (CharacterIdColumn, characterId),
                                 (QuestTypeIdColumn, questId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreWithHowManyTimesAGivenCharacterHasCompletedAGivenQuest()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                Dim questId = 2L
                Dim completions = 3L
                subject.Write(characterId, questId, completions)
                store.Verify(Sub(x) x.ReplaceRecord(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuestCompletions,
                                 (CharacterIdColumn, characterId),
                                 (QuestTypeIdColumn, questId),
                                 (CompletionsColumn, completions)))
            End Sub)
    End Sub
End Class
