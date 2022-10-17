﻿Public Class ItemStatisticData
    Inherits BaseData
    Implements IItemStatisticData
    Friend Const ItemIdColumn = ItemData.ItemIdColumn
    Friend Const ItemStatisticTypeIdColumn = "ItemStatisticTypeId"
    Friend Const StatisticValueColumn = "StatisticValue"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function Read(itemId As Long, statisticType As Long) As Long? Implements IItemStatisticData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            ItemStatistics,
            StatisticValueColumn,
            (ItemIdColumn, itemId),
            (ItemStatisticTypeIdColumn, statisticType))
    End Function

    Public Sub Write(itemId As Long, statisticType As Long, value As Long) Implements IItemStatisticData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            ItemStatistics,
            (ItemIdColumn, itemId),
            (ItemStatisticTypeIdColumn, statisticType),
            (StatisticValueColumn, value))
    End Sub

    Public Sub ClearForItem(itemId As Long) Implements IItemStatisticData.ClearForItem
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            ItemStatistics,
            (ItemIdColumn, itemId))
    End Sub
End Class
