﻿Public Class LocationData
    Inherits BaseData
    Implements ILocationData
    Public Const TableName = "Locations"
    Public Const LocationIdColumn = "LocationId"
    Friend Const LocationTypeColumn = "LocationType"

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Public Sub Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{LocationIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{LocationTypeColumn}] INT NOT NULL
            );")
    End Sub

    Public Function ReadForLocationType(locationType As Long) As IEnumerable(Of Long) Implements ILocationData.ReadForLocationType
        Return Store.ReadRecordsWithColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationIdColumn,
            (LocationTypeColumn, locationType))
    End Function

    Public Sub WriteLocationType(locationId As Long, locationType As Long) Implements ILocationData.WriteLocationType
        Store.WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (LocationTypeColumn, locationType),
            (LocationIdColumn, locationId))
    End Sub

    Public Function ReadLocationType(locationId As Long) As Long? Implements ILocationData.ReadLocationType
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationTypeColumn,
            (LocationIdColumn, locationId))
    End Function

    Public Function Create(locationType As Long) As Long Implements ILocationData.Create
        Return Store.CreateRecord(
            AddressOf Initialize,
            TableName,
            (LocationTypeColumn, locationType))
    End Function
End Class
