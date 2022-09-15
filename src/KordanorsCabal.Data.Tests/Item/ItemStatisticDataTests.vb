Public Class ItemStatisticDataTests
    Inherits WorldDataSubobjectTests(Of IItemStatisticData)

    Sub New()
        MyBase.New(Function(x) x.ItemStatistic)
    End Sub

    <Fact>
    Sub item_statistics_can_be_cleared_for_an_item()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                subject.ClearForItem(itemId)
                store.Verify(
                    Sub(x) x.ClearForColumnValue(
                        It.IsAny(Of Action),
                        Tables.ItemStatistics,
                        (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
End Class
