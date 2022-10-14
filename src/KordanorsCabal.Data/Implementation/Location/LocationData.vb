Public Class LocationData
    Inherits BaseData
    Implements ILocationData
    Public Const TableName = "Locations"
    Public Const LocationIdColumn = "LocationId"
    Friend Const LocationTypeIdColumn = "LocationTypeId"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Sub Initialize()
        Store.Primitive.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{LocationIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{LocationTypeIdColumn}] INT NOT NULL
            );")
    End Sub

    Public Function ReadForLocationType(locationType As Long) As IEnumerable(Of Long) Implements ILocationData.ReadForLocationType
        Return Store.Record.ReadRecordsWithColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationIdColumn,
            (LocationTypeIdColumn, locationType))
    End Function

    Public Sub WriteLocationType(locationId As Long, locationType As Long) Implements ILocationData.WriteLocationType
        Store.Column.Write(
            AddressOf Initialize,
            TableName,
            (LocationTypeIdColumn, locationType),
            (LocationIdColumn, locationId))
    End Sub

    Public Function ReadLocationType(locationId As Long) As Long? Implements ILocationData.ReadLocationType
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationTypeIdColumn,
            (LocationIdColumn, locationId))
    End Function

    Public Function Create(locationType As Long) As Long Implements ILocationData.Create
        Return Store.Create.Entry(
            AddressOf Initialize,
            TableName,
            (LocationTypeIdColumn, locationType))
    End Function
End Class
