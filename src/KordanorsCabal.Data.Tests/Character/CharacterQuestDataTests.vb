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
End Class
