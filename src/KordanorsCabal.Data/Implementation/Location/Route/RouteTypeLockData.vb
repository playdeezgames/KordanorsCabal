Public Class RouteTypeLockData
    Inherits BaseData
    Implements IRouteTypeLockData
    Friend Const TableName = "RouteTypeLocks"
    Friend Const RouteTypeIdColumn = RouteTypeData.RouteTypeIdColumn
    Friend Const UnlockedRouteTypeIdColumn = "UnlockedRouteTypeId"
    Friend Const UnlockItemTypeId = "UnlockItemTypeId"
    Friend Sub Initialize()
        Store.Primitive.Execute($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{RouteTypeIdColumn}],
                    [{UnlockedRouteTypeIdColumn}],
                    [{UnlockItemTypeId}]) AS
                (VALUES
                    (4,2,1),
                    (5,2,2),
                    (6,2,3),
                    (7,2,4),
                    (8,2,5),
                    (9,2,6))
                SELECT 
                    [{RouteTypeIdColumn}],
                    [{UnlockedRouteTypeIdColumn}],
                    [{UnlockItemTypeId}]
                FROM [cte];")
    End Sub
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadUnlockItem(routeTypeId As Long) As Long? Implements IRouteTypeLockData.ReadUnlockItem
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            UnlockItemTypeId,
            (RouteTypeIdColumn, routeTypeId))
    End Function

    Public Function ReadUnlockedRouteType(routeTypeId As Long) As Long? Implements IRouteTypeLockData.ReadUnlockedRouteType
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            UnlockedRouteTypeIdColumn,
            (RouteTypeIdColumn, routeTypeId))
    End Function
End Class
