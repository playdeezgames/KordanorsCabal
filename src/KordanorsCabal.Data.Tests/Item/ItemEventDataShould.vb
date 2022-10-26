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

End Class
