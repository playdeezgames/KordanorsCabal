Public Class RouteData
    Inherits BaseData
    Implements IRouteData
    Friend Const TableName = "Routes"
    Friend Const RouteIdColumn = "RouteId"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const DirectionIdColumn = "DirectionId"
    Friend Const RouteTypeIdColumn = "RouteTypeId"
    Friend Const ToLocationIdColumn = "To" + LocationData.LocationIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Location, LocationData).Initialize()
        Store.Primitive.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{RouteIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{LocationIdColumn}] INT NOT NULL,
                [{DirectionIdColumn}] INT NOT NULL,
                [{RouteTypeIdColumn}] INT NOT NULL,
                [{ToLocationIdColumn}] INT NOT NULL,
                UNIQUE([{LocationIdColumn}],[{DirectionIdColumn}]),
                FOREIGN KEY([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}]),
                FOREIGN KEY([{ToLocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
            );")
    End Sub

    Public Sub Clear(routeId As Long) Implements IRouteData.Clear
        Store.Clear.ForValue(
            AddressOf Initialize,
            TableName,
            (RouteIdColumn, routeId))
    End Sub

    Public Sub WriteRouteType(routeId As Long, routeType As Long) Implements IRouteData.WriteRouteType
        Store.Column.WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (RouteTypeIdColumn, routeType),
            (RouteIdColumn, routeId))
    End Sub

    Public Function ReadRouteType(routeId As Long) As Long? Implements IRouteData.ReadRouteType
        Return Store.Column.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            RouteTypeIdColumn,
            (RouteIdColumn, routeId))
    End Function

    Public Function ReadToLocation(routeId As Long) As Long? Implements IRouteData.ReadToLocation
        Return Store.Column.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            ToLocationIdColumn,
            (RouteIdColumn, routeId))
    End Function

    Public Function ReadForLocationRouteType(locationId As Long, routeType As Long) As IEnumerable(Of Long) Implements IRouteData.ReadForLocationRouteType
        Return Store.Record.ReadRecordsWithColumnValues(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            RouteIdColumn,
            (LocationIdColumn, locationId),
            (RouteTypeIdColumn, routeType))
    End Function

    Public Function ReadDirectionRouteForLocation(locationId As Long) As IEnumerable(Of Tuple(Of Long, Long)) Implements IRouteData.ReadDirectionRouteForLocation
        Return Store.Record.ReadRecordsWithColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            (DirectionIdColumn, RouteIdColumn),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadDirectionRouteTypeForLocation(locationId As Long) As IEnumerable(Of Tuple(Of Long, Long)) Implements IRouteData.ReadDirectionRouteTypeForLocation
        Return Store.Record.ReadRecordsWithColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            (DirectionIdColumn, RouteTypeIdColumn),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadForLocationDirection(locationId As Long, direction As Long) As Long? Implements IRouteData.ReadForLocationDirection
        Return Store.Column.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            RouteIdColumn,
            (LocationIdColumn, locationId),
            (DirectionIdColumn, direction))
    End Function

    Public Function Create(locationId As Long, direction As Long, routeType As Long, toLocationId As Long) As Long Implements IRouteData.Create
        Return Store.Create.CreateRecord(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (DirectionIdColumn, direction),
            (RouteTypeIdColumn, routeType),
            (ToLocationIdColumn, toLocationId))
    End Function

    Public Function ReadCountForLocation(locationId As Long) As Long Implements IRouteData.ReadCountForLocation
        Return Store.Count.ReadCountForColumnValue(AddressOf Initialize, TableName, (LocationIdColumn, locationId))
    End Function
End Class
