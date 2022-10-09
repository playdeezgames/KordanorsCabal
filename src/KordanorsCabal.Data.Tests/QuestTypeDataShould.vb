Public Class QuestTypeDataShould
    Inherits WorldDataSubobjectTests(Of IQuestTypeData)
    Sub New()
        MyBase.New(Function(x) x.QuestType)
    End Sub
    <Fact>
    Sub have_can_accept_event_names()
        WithSubobject(
            Sub(store, events, subject)
                Const questTypeId = 1L
                subject.ReadCanAcceptEventName(questTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnString(
                        It.IsAny(Of Action),
                        Tables.QuestTypes,
                        Columns.CanAcceptEventNameColumn,
                        (Columns.QuestTypeIdColumn, questTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub have_accept_event_names()
        WithSubobject(
            Sub(store, events, subject)
                Const questTypeId = 1L
                subject.ReadAcceptEventName(questTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnString(
                        It.IsAny(Of Action),
                        Tables.QuestTypes,
                        Columns.AcceptEventNameColumn,
                        (Columns.QuestTypeIdColumn, questTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_complete_event_names()
        WithSubobject(
            Sub(store, events, subject)
                Const questTypeId = 1L
                subject.ReadCanCompleteEventName(questTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnString(
                        It.IsAny(Of Action),
                        Tables.QuestTypes,
                        Columns.CanCompleteEventNameColumn,
                        (Columns.QuestTypeIdColumn, questTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub have_complete_event_names()
        WithSubobject(
            Sub(store, events, subject)
                Const questTypeId = 1L
                subject.ReadCompleteEventName(questTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnString(
                        It.IsAny(Of Action),
                        Tables.QuestTypes,
                        Columns.CompleteEventNameColumn,
                        (Columns.QuestTypeIdColumn, questTypeId)))
            End Sub)
    End Sub
End Class
