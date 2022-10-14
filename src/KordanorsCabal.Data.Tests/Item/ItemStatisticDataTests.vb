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
                store.Setup(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ClearForItem(itemId)
                store.Verify(
                    Sub(x) x.Clear.ClearForColumnValue(
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
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read(itemId, statisticTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadColumnValue(Of Long, Long, Long)(
                        It.IsAny(Of Action),
                        Tables.ItemStatistics,
                        Columns.StatisticValueColumn,
                        (Columns.ItemIdColumn, itemId),
                        (Columns.ItemStatisticTypeIdColumn, statisticTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub item_statistics_can_write_a_statistic_value_to_a_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                Const statisticTypeId = 2L
                Const statisticValue = 3L
                subject.Write(itemId, statisticTypeId, statisticValue)
                store.Verify(
                    Sub(x) x.ReplaceRecord(Of Long, Long, Long)(
                        It.IsAny(Of Action),
                        Tables.ItemStatistics,
                        (Columns.ItemIdColumn, itemId),
                        (Columns.ItemStatisticTypeIdColumn, statisticTypeId),
                        (Columns.StatisticValueColumn, statisticValue)))
            End Sub)
    End Sub
End Class
