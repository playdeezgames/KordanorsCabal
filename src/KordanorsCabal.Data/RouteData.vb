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
