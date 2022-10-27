Imports SQLitePCL

Public Class ItemEventDataShould
    Inherits WorldDataSubobjectTests(Of IItemEventData)
    Sub New()
        MyBase.New(Function(x) x.ItemEvent)
    End Sub
    <Fact>
    Sub write_event_entries_into_an_item()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                Const eventId = 2L
                Const eventName = "three"
                store.SetupGet(Function(x) x.Replace).Returns((New Mock(Of IStoreReplace)).Object)
                subject.Write(itemId, eventId, eventName)
                store.Verify(Sub(x) x.Replace.Entry(
                                 It.IsAny(Of Action),
                                 ItemEvents,
                                 (ItemIdColumn, itemId),
                                 (EventIdColumn, eventId),
                                 (EventNameColumn, eventName)))
            End Sub)
    End Sub
    <Fact>
    Sub clear_events_for_an_item()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                store.SetupGet(Function(x) x.Clear).Returns((New Mock(Of IStoreClear)).Object)
                subject.ClearForItem(itemId)
                store.Verify(
                    Sub(x) x.Clear.ForValue(
                        It.IsAny(Of Action),
                        Tables.ItemEvents,
                        (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
    <Fact>
    Sub have_event_names()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                Const eventId = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read(itemId, eventId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(
                        It.IsAny(Of Action),
                        ItemEvents,
                        EventNameColumn,
                        (ItemIdColumn, itemId),
                        (EventIdColumn, eventId)))
            End Sub)
    End Sub
End Class
