Public Class LocationData
    Inherits BaseData
    Implements ILocationData
    Public Const TableName = "Locations"
    Public Const LocationIdColumn = "LocationId"
    Friend Const LocationTypeIdColumn = "LocationTypeId"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function ReadForLocationType(locationType As Long) As IEnumerable(Of Long) Implements ILocationData.ReadForLocationType
        Return Store.Record.WithValues(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            LocationIdColumn,
            (LocationTypeIdColumn, locationType))
    End Function

    Public Sub WriteLocationType(locationId As Long, locationType As Long) Implements ILocationData.WriteLocationType
        Store.Column.Write(
            AddressOf NoInitializer,
            TableName,
            (LocationTypeIdColumn, locationType),
            (LocationIdColumn, locationId))
    End Sub

    Public Function ReadLocationType(locationId As Long) As Long? Implements ILocationData.ReadLocationType
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            LocationTypeIdColumn,
            (LocationIdColumn, locationId))
    End Function

    Public Function Create(locationType As Long) As Long Implements ILocationData.Create
        Return Store.Create.Entry(
            AddressOf NoInitializer,
            TableName,
            (LocationTypeIdColumn, locationType))
    End Function
End Class
