Public Class ItemTypeStatisticData
    Inherits BaseData
    Implements IItemTypeStatisticData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, itemTypeStatisticTypeId As Long) As Long? Implements IItemTypeStatisticData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            ItemTypeStatistics,
            StatisticValueColumn,
            (ItemTypeIdColumn, itemTypeId),
            (StatisticTypeIdColumn, itemTypeStatisticTypeId))
    End Function

    Public Function ReadAll(itemType As Long) As List(Of Tuple(Of Long, Long)) Implements IItemTypeStatisticData.ReadAll
        Return Store.Record.WithValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            ItemTypeStatistics,
            (StatisticTypeIdColumn, StatisticValueColumn),
            (ItemTypeIdColumn, itemType))
    End Function
End Class
