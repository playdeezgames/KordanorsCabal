Public Module LocationDungeonLevelData
    Friend Const TableName = "LocationDungeonLevels"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const DungeonLevelColumn = "DungeonLevel"
    Friend Sub Initialize()
        LocationData.Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{LocationIdColumn}] INT NOT NULL UNIQUE,
                [{DungeonLevelColumn}] INT NOT NULL
            );")
    End Sub
    Public Function Read(locationId As Long) As Long?
        Return ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            DungeonLevelColumn,
            (LocationIdColumn, locationId))
    End Function

    Public Sub Write(locationId As Long, dungeonLevel As Long)
        ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (DungeonLevelColumn, dungeonLevel))
    End Sub

    Public Function ReadForDungeonLevel(dungeonLevel As Long) As IEnumerable(Of Long)
        Return ReadRecordsWithColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationIdColumn,
            (DungeonLevelColumn, dungeonLevel))
    End Function
End Module
