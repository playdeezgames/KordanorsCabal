Public Class ItemTypeStatisticData
    Inherits BaseData
    Implements IItemTypeStatisticData
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const ItemTypeStatisticTypeIdColumn = ItemTypeStatisticTypeData.ItemTypeStatisticTypeIdColumn
    Friend Const ItemTypeStatisticValueColumn = "ItemTypeStatisticValue"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, itemTypeStatisticTypeId As Long) As Long? Implements IItemTypeStatisticData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            ItemTypeStatistics,
            ItemTypeStatisticValueColumn,
            (ItemTypeIdColumn, itemTypeId),
            (ItemTypeStatisticTypeIdColumn, itemTypeStatisticTypeId))
    End Function
End Class
