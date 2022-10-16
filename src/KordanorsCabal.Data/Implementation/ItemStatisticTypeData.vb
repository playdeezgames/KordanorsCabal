Public Class ItemStatisticTypeData
    Inherits BaseData
    Implements IItemStatisticTypeData
    Friend Const TableName = "ItemStatisticTypes"
    Friend Const ItemStatisticTypeIdColumn = "ItemStatisticTypeId"
    Friend Const DefaultValueColumn = "DefaultValue"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadDefaultValue(statisticTypeId As Long) As Long? Implements IItemStatisticTypeData.ReadDefaultValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            DefaultValueColumn,
            (ItemStatisticTypeIdColumn, statisticTypeId))
    End Function
End Class
