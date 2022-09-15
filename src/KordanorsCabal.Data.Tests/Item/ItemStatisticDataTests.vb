Public Class ItemStatisticDataTests
    Inherits WorldDataSubobjectTests(Of IItemStatisticData)

    Sub New()
        MyBase.New(Function(x) x.ItemStatistic)
    End Sub

    <Fact>
    Sub item_statistics_can_be_cleared_for_an_item_from_the_data_store()
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
    <Fact>
    Sub item_statistics_can_read_a_statistic_value_from_a_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                Const statisticTypeId = 2L
                subject.Read(itemId, statisticTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long, Long)(
                        It.IsAny(Of Action),
                        Tables.ItemStatistics,
                        Columns.StatisticValueColumn,
                        (Columns.ItemIdColumn, itemId),
                        (Columns.ItemStatisticTypeIdColumn, statisticTypeId)))
            End Sub)
    End Sub
End Class
