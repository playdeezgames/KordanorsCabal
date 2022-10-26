Public Class ItemTypeEventDataShould
    Inherits WorldDataSubobjectTests(Of IItemTypeEventData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeEvent)
    End Sub
    <Fact>
    Sub read_all_event_ids_and_names_for_a_give_item_type()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemTypeId = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadAll(itemTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Record.WithValue(Of Long, Long, String)(
                        It.IsAny(Of Action),
                        ItemTypeEvents,
                        (EventIdColumn, EventNameColumn),
                        (ItemTypeIdColumn, itemTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub ReadTheNameOfTheEventAssociatedWithAGiveItemTypeAndGivenEventId()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemTypeId = 1L
                Const eventId = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read(itemTypeId, eventId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(
                        It.IsAny(Of Action),
                        Tables.ItemTypeEvents,
                        Columns.EventNameColumn,
                        (Columns.ItemTypeIdColumn, itemTypeId),
                        (Columns.EventIdColumn, eventId)))
            End Sub)
    End Sub
End Class
