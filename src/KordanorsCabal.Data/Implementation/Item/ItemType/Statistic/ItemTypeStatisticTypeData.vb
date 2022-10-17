Public Class ItemTypeStatisticTypeData
    Inherits BaseData
    Implements IItemTypeStatisticTypeData
    Public Function ReadName(itemTypeStatisticTypeId As Long) As String Implements IItemTypeStatisticTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            ItemTypeStatisticTypes,
            ItemTypeStatisticTypeNameColumn,
            (ItemTypeStatisticTypeIdColumn, itemTypeStatisticTypeId))
    End Function
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
End Class
