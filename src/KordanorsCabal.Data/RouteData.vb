Public Module RouteData
    Friend Const TableName = "Routes"
    Friend Const RouteIdColumn = "RouteId"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const DirectionColumn = "Direction"
    Friend Const RouteTypeColumn = "RouteType"
    Friend Const ToLocationIdColumn = "To" + LocationData.LocationIdColumn
    Friend Sub Initialize()
        LocationData.Initialize()
        ExecuteNonQuery(
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

    Public Sub WriteRouteType(routeId As Long, routeType As Long)
        WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (RouteTypeColumn, routeType),
            (RouteIdColumn, routeId))
    End Sub

    Public Function ReadRouteType(routeId As Long) As Long?
        Return ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            RouteTypeColumn,
            (RouteIdColumn, routeId))
    End Function

    Public Function ReadToLocation(routeId As Long) As Long?
        Return ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            ToLocationIdColumn,
            (RouteIdColumn, routeId))
    End Function

    Public Function ReadForLocation(locationId As Long) As IEnumerable(Of Tuple(Of Long, Long))
        Return ReadRecordsWithColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            (DirectionColumn, RouteIdColumn),
            (LocationIdColumn, locationId))
    End Function

    Public Function Create(locationId As Long, direction As Long, routeType As Long, toLocationId As Long) As Long
        Return CreateRecord(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (DirectionColumn, direction),
            (RouteTypeColumn, routeType),
            (ToLocationIdColumn, toLocationId))
    End Function
End Module
