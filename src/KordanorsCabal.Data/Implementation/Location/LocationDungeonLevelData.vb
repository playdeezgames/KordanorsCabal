Public Class LocationDungeonLevelData
    Inherits BaseData
    Implements ILocationDungeonLevelData
    Friend Const TableName = "LocationDungeonLevels"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const DungeonLevelIdColumn = "DungeonLevelId"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Location, LocationData).Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{LocationIdColumn}] INT NOT NULL UNIQUE,
                [{DungeonLevelIdColumn}] INT NOT NULL
            );")
    End Sub
    Public Function Read(locationId As Long) As Long? Implements ILocationDungeonLevelData.Read
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            DungeonLevelIdColumn,
            (LocationIdColumn, locationId))
    End Function

    Public Sub Write(locationId As Long, dungeonLevel As Long) Implements ILocationDungeonLevelData.Write
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (DungeonLevelIdColumn, dungeonLevel))
    End Sub

    Public Function ReadForDungeonLevel(dungeonLevel As Long) As IEnumerable(Of Long) Implements ILocationDungeonLevelData.ReadForDungeonLevel
        Return Store.ReadRecordsWithColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationIdColumn,
            (DungeonLevelIdColumn, dungeonLevel))
    End Function
End Class
