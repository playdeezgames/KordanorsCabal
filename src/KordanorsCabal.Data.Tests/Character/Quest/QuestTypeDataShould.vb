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
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadCanAcceptEventName(questTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadColumnString(
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
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadAcceptEventName(questTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadColumnString(
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
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadCanCompleteEventName(questTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadColumnString(
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
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadCompleteEventName(questTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadColumnString(
                        It.IsAny(Of Action),
                        Tables.QuestTypes,
                        Columns.CompleteEventNameColumn,
                        (Columns.QuestTypeIdColumn, questTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub read_all()
        WithSubobject(
            Sub(store, events, subject)
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadAll().ShouldBeNull
                store.Verify(Function(x) x.Record.ReadRecords(Of Long)(It.IsAny(Of Action), Tables.QuestTypes, Columns.QuestTypeIdColumn))
            End Sub)
    End Sub
End Class
