Public Class CharacterQuestDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterQuestData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterQuest)
    End Sub
    <Fact>
    Sub ShouldClearAQuestForACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                Dim quest = 2L
                store.Setup(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.Clear(characterId, quest)
                store.Verify(Sub(x) x.Clear.ClearForColumnValues(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuests,
                                 (CharacterIdColumn, characterId),
                                 (QuestTypeIdColumn, quest)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldClearAllQuestsForACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                store.Setup(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.Clear.ClearForColumnValue(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuests,
                                 (CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheExistenceOfAQuestForACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                Dim quest = 2L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.Read(characterId, quest).ShouldBeFalse
                store.Verify(Sub(x) x.Record.ReadRecordsWithColumnValues(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuests,
                                 CharacterIdColumn,
                                 (CharacterIdColumn, characterId),
                                 (QuestTypeIdColumn, quest)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreWithAQuestForACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                Dim quest = 2L
                store.SetupGet(Function(x) x.Replace).Returns((New Mock(Of IStoreReplace)).Object)
                subject.Write(characterId, quest)
                store.Verify(Sub(x) x.Replace.ReplaceRecord(Of Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterQuests,
                                 (CharacterIdColumn, characterId),
                                 (QuestTypeIdColumn, quest)))
            End Sub)
    End Sub
End Class
