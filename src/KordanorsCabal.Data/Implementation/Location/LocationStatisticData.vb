﻿Public Class LocationStatisticData
    Inherits BaseData
    Implements ILocationStatisticData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function ReadForStatisticValue(statisticType As Long, statisticValue As Long) As IEnumerable(Of Long)
        Return Store.Record.WithValues(Of Long, Long, Long)(
            AddressOf NoInitializer,
            LocationStatistics, LocationIdColumn,
            (StatisticTypeIdColumn, statisticType),
            (StatisticValueColumn, statisticValue))
    End Function

    Public Function Read(locationId As Long, statisticType As Long) As Long? Implements ILocationStatisticData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            LocationStatistics,
            StatisticValueColumn,
            (LocationIdColumn, locationId),
            (StatisticTypeIdColumn, statisticType))
    End Function

    Public Sub Write(locationId As Long, statisticType As Long, statisticValue As Long?) Implements ILocationStatisticData.Write
        If Not statisticValue.HasValue Then
            Store.Clear.ForValues(
                AddressOf NoInitializer,
                LocationStatistics,
                (LocationIdColumn, locationId),
                (StatisticTypeIdColumn, statisticType))
            Return
        End If
        Store.Replace.Entry(
            AddressOf NoInitializer,
            LocationStatistics,
            (LocationIdColumn, locationId),
            (StatisticTypeIdColumn, statisticType),
            (StatisticValueColumn, statisticValue.Value))
    End Sub
End Class
