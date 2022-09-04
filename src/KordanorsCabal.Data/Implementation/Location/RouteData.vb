Public Class RouteData
    Inherits BaseData
    Friend Const TableName = "Routes"
    Friend Const RouteIdColumn = "RouteId"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const DirectionColumn = "Direction"
    Friend Const RouteTypeColumn = "RouteType"
    Friend Const ToLocationIdColumn = "To" + LocationData.LocationIdColumn

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Location, LocationData).Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{RouteIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{LocationIdColumn}] INT NOT NULL,
                [{DirectionColumn}] INT NOT NULL,
                [{RouteTypeColumn}] INT NOT NULL,
                [{ToLocationIdColumn}] INT NOT NULL,
                UNIQUE([{LocationIdColumn}],[{DirectionColumn}]),
                FOREIGN KEY([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}]),
                FOREIGN KEY([{ToLocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
            );")
    End Sub

    Public Sub Clear(routeId As Long)
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (RouteIdColumn, routeId))
    End Sub

    Public Sub WriteRouteType(routeId As Long, routeType As Long)
        Store.WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (RouteTypeColumn, routeType),
            (RouteIdColumn, routeId))
    End Sub

    Public Function ReadRouteType(routeId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            RouteTypeColumn,
            (RouteIdColumn, routeId))
    End Function

    Public Function ReadToLocation(routeId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            ToLocationIdColumn,
            (RouteIdColumn, routeId))
    End Function

    Public Function ReadForLocationRouteType(locationId As Long, routeType As Long) As IEnumerable(Of Long)
        Return Store.ReadRecordsWithColumnValues(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            RouteIdColumn,
            (LocationIdColumn, locationId),
            (RouteTypeColumn, routeType))
    End Function

    Public Function ReadDirectionRouteForLocation(locationId As Long) As IEnumerable(Of Tuple(Of Long, Long))
        Return Store.ReadRecordsWithColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            (DirectionColumn, RouteIdColumn),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadDirectionRouteTypeForLocation(locationId As Long) As IEnumerable(Of Tuple(Of Long, Long))
        Return Store.ReadRecordsWithColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            (DirectionColumn, RouteTypeColumn),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadForLocationDirection(locationId As Long, direction As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            RouteIdColumn,
            (LocationIdColumn, locationId),
            (DirectionColumn, direction))
    End Function

    Public Function Create(locationId As Long, direction As Long, routeType As Long, toLocationId As Long) As Long
        Return Store.CreateRecord(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (DirectionColumn, direction),
            (RouteTypeColumn, routeType),
            (ToLocationIdColumn, toLocationId))
    End Function

    Public Function ReadCountForLocation(locationId As Long) As Long
        Return Store.ReadCountForColumnValue(AddressOf Initialize, TableName, (LocationIdColumn, locationId))
    End Function
End Class
