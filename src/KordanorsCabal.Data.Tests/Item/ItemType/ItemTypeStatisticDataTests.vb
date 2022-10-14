Public Class ItemTypeStatisticDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeStatisticData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeStatistic)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheStatisticValuesOfAGivenItemType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim itemTypeId = 1L
                Dim statisticType = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read(itemTypeId, statisticType).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long, Long)(
                    It.IsAny(Of Action),
                    Tables.ItemTypeStatistics,
                    Columns.ItemTypeStatisticValueColumn,
                    (Columns.ItemTypeIdColumn, itemTypeId),
                    (Columns.ItemTypeStatisticTypeIdColumn, statisticType)))
            End Sub)
    End Sub
End Class
