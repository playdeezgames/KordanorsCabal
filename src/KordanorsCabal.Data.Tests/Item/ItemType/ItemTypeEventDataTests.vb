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
                subject.Read(itemTypeId, eventId).ShouldBeNull
            End Sub)
    End Sub
End Class
