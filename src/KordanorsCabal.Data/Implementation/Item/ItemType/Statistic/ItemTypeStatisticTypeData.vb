Public Class ItemTypeStatisticTypeData
    Inherits BaseData
    Implements IItemTypeStatisticTypeData
    Friend Const TableName = "ItemTypeStatisticTypes"
    Friend Const ItemTypeStatisticTypeIdColumn = "ItemTypeStatisticTypeId"
    Friend Const ItemTypeStatisticTypeNameColumn = "ItemTypeStatisticTypeName"

    Public Function ReadName(itemTypeStatisticTypeId As Long) As String Implements IItemTypeStatisticTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            ItemTypeStatisticTypeNameColumn,
            (ItemTypeStatisticTypeIdColumn, itemTypeStatisticTypeId))
    End Function
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
End Class
