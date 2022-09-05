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
End Class
