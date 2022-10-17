Public Class RouteTypeLockData
    Inherits BaseData
    Implements IRouteTypeLockData
    Friend Const TableName = "RouteTypeLocks"
    Friend Const RouteTypeIdColumn = RouteTypeData.RouteTypeIdColumn
    Friend Const UnlockedRouteTypeIdColumn = "UnlockedRouteTypeId"
    Friend Const UnlockItemTypeId = "UnlockItemTypeId"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadUnlockItem(routeTypeId As Long) As Long? Implements IRouteTypeLockData.ReadUnlockItem
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            UnlockItemTypeId,
            (RouteTypeIdColumn, routeTypeId))
    End Function

    Public Function ReadUnlockedRouteType(routeTypeId As Long) As Long? Implements IRouteTypeLockData.ReadUnlockedRouteType
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            UnlockedRouteTypeIdColumn,
            (RouteTypeIdColumn, routeTypeId))
    End Function
End Class
