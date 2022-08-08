﻿Public Module LocationDungeonLevelData
    Friend Const TableName = "LocationDungeonLevels"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const DungeonLevelColumn = "DungeonLevel"
    Private ReadOnly Store As SPLORR.Data.Store = StaticStore.Store
    Friend Sub Initialize()
        LocationData.Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{LocationIdColumn}] INT NOT NULL UNIQUE,
                [{DungeonLevelColumn}] INT NOT NULL
            );")
    End Sub
    Public Function Read(locationId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
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
        Return Store.ReadRecordsWithColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationIdColumn,
            (DungeonLevelColumn, dungeonLevel))
    End Function
End Module
