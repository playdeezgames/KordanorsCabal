Public Class RouteTypeLockData
    Inherits BaseData
    Implements IRouteTypeLockData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadUnlockItem(routeTypeId As Long) As Long? Implements IRouteTypeLockData.ReadUnlockItem
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            RouteTypeLocks,
            UnlockItemTypeIdColumn,
            (RouteTypeIdColumn, routeTypeId))
    End Function

    Public Function ReadUnlockedRouteType(routeTypeId As Long) As Long? Implements IRouteTypeLockData.ReadUnlockedRouteType
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            RouteTypeLocks,
            UnlockedRouteTypeIdColumn,
            (RouteTypeIdColumn, routeTypeId))
    End Function
End Class
