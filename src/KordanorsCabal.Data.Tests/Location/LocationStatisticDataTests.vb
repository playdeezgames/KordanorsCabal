﻿Public Class LocationStatisticDataTests
    Inherits WorldDataSubobjectTests(Of ILocationStatisticData)
    Sub New()
        MyBase.New(Function(x) x.LocationStatistic)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheStatisticValueOfAGivenStatisticAndAGivenLocation()
        WithSubobject(
            Sub(store, subject)
                Dim locationId = 1L
                Dim statisticType = 2L
                subject.Read(locationId, statisticType).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long, Long)(
                    It.IsAny(Of Action),
                    Tables.LocationStatistics,
                    Columns.StatisticValueColumn,
                    (Columns.LocationIdColumn, locationId),
                    (Columns.LocationStatisticTypeIdColumn, statisticType)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreWithAStatisticValueForAGivenCombinationOfLocationAndStatisticType()
        WithSubobject(
            Sub(store, subject)
                Dim locationId = 1L
                Dim statisticType = 2L
                Dim statisticValue = 3L
                subject.Write(locationId, statisticType, statisticValue)
                store.Verify(
                    Sub(x) x.ReplaceRecord(
                    It.IsAny(Of Action),
                    Tables.LocationStatistics,
                    (Columns.LocationIdColumn, locationId),
                    (Columns.LocationStatisticTypeIdColumn, statisticType),
                    (Columns.StatisticValueColumn, statisticValue)))
            End Sub)
    End Sub
End Class
