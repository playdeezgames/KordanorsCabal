Public Module LocationData
    Friend Const TableName = "Locations"
    Friend Const LocationIdColumn = "LocationId"
    Friend Const LocationTypeColumn = "LocationType"
    Friend Sub Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{LocationIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{LocationTypeColumn}] INT NOT NULL
            );")
    End Sub

    Public Function ReadForLocationType(locationType As Long) As IEnumerable(Of Long)
        Return ReadRecordsWithColumnValue(Of Long, Long)(AddressOf Initialize, TableName, LocationIdColumn, (LocationTypeColumn, locationType))
    End Function

    Public Function ReadLocationType(locationId As Long) As Long?
        Return ReadColumnValue(Of Long, Long)(AddressOf Initialize, TableName, LocationTypeColumn, (LocationIdColumn, locationId))
    End Function

    Public Function Create(locationType As Long) As Long
        Return CreateRecord(AddressOf Initialize, TableName, (LocationTypeColumn, locationType))
    End Function
End Module
