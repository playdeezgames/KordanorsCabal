﻿Public Class ItemTypeStatisticDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeStatisticData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeStatistic)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheStatisticValuesOfAGivenItemType()
        WithSubobject(
            Sub(store, subject)
                Dim itemTypeId = 1L
                Dim statisticType = 2L
                subject.Read(itemTypeId, statisticType).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long, Long)(
                    It.IsAny(Of Action),
                    Tables.ItemTypeStatistics,
                    Columns.ItemTypeStatisticValueColumn,
                    (Columns.ItemTypeIdColumn, itemTypeId),
                    (Columns.ItemTypeStatisticTypeIdColumn, statisticType)))
            End Sub)
    End Sub
End Class