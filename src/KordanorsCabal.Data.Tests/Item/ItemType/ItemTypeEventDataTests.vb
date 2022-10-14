Public Class ItemTypeEventDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeEventData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeEvent)
    End Sub
    <Fact>
    Sub ShouldReadTheNameOfTheEventAssociatedWithAGiveItemTypeAndGivenEventId()
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
