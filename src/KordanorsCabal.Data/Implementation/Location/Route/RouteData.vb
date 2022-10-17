Public Class RouteData
    Inherits BaseData
    Implements IRouteData
    Friend Const RouteIdColumn = "RouteId"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const DirectionIdColumn = "DirectionId"
    Friend Const RouteTypeIdColumn = "RouteTypeId"
    Friend Const ToLocationIdColumn = "To" + LocationData.LocationIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Sub Clear(routeId As Long) Implements IRouteData.Clear
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            Routes,
            (RouteIdColumn, routeId))
    End Sub

    Public Sub WriteRouteType(routeId As Long, routeType As Long) Implements IRouteData.WriteRouteType
        Store.Column.Write(
            AddressOf NoInitializer,
            Routes,
            (RouteTypeIdColumn, routeType),
            (RouteIdColumn, routeId))
    End Sub

    Public Function ReadRouteType(routeId As Long) As Long? Implements IRouteData.ReadRouteType
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Routes,
            RouteTypeIdColumn,
            (RouteIdColumn, routeId))
    End Function

    Public Function ReadToLocation(routeId As Long) As Long? Implements IRouteData.ReadToLocation
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Routes,
            ToLocationIdColumn,
            (RouteIdColumn, routeId))
    End Function

    Public Function ReadForLocationRouteType(locationId As Long, routeType As Long) As IEnumerable(Of Long) Implements IRouteData.ReadForLocationRouteType
        Return Store.Record.WithValues(Of Long, Long, Long)(
            AddressOf NoInitializer,
            Routes,
            RouteIdColumn,
            (LocationIdColumn, locationId),
            (RouteTypeIdColumn, routeType))
    End Function

    Public Function ReadDirectionRouteForLocation(locationId As Long) As IEnumerable(Of Tuple(Of Long, Long)) Implements IRouteData.ReadDirectionRouteForLocation
        Return Store.Record.WithValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            Routes,
            (DirectionIdColumn, RouteIdColumn),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadDirectionRouteTypeForLocation(locationId As Long) As IEnumerable(Of Tuple(Of Long, Long)) Implements IRouteData.ReadDirectionRouteTypeForLocation
        Return Store.Record.WithValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            Routes,
            (DirectionIdColumn, RouteTypeIdColumn),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadForLocationDirection(locationId As Long, direction As Long) As Long? Implements IRouteData.ReadForLocationDirection
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            Routes,
            RouteIdColumn,
            (LocationIdColumn, locationId),
            (DirectionIdColumn, direction))
    End Function

    Public Function Create(locationId As Long, direction As Long, routeType As Long, toLocationId As Long) As Long Implements IRouteData.Create
        Return Store.Create.Entry(
            AddressOf NoInitializer,
            Routes,
            (LocationIdColumn, locationId),
            (DirectionIdColumn, direction),
            (RouteTypeIdColumn, routeType),
            (ToLocationIdColumn, toLocationId))
    End Function

    Public Function ReadCountForLocation(locationId As Long) As Long Implements IRouteData.ReadCountForLocation
        Return Store.Count.ForValue(AddressOf NoInitializer, Routes, (LocationIdColumn, locationId))
    End Function
End Class
