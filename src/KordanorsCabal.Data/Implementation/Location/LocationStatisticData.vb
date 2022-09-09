Public Class LocationStatisticData
    Inherits BaseData
    Implements ILocationStatisticData
    Friend Const TableName = "LocationStatistics"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const LocationStatisticTypeIdColumn = "LocationStatisticTypeId"
    Friend Const StatisticValueColumn = "StatisticValue"

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Location, LocationData).Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{LocationIdColumn}] INT NOT NULL,
                [{LocationStatisticTypeIdColumn}] INT NOT NULL, 
                [{StatisticValueColumn}] INT NOT NULL,
                UNIQUE([{LocationIdColumn}],[{LocationStatisticTypeIdColumn}]),
                FOREIGN KEY ([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
            );")
    End Sub

    Public Function ReadForStatisticValue(statisticType As Long, statisticValue As Long) As IEnumerable(Of Long)
        Return Store.ReadRecordsWithColumnValues(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName, LocationIdColumn,
            (LocationStatisticTypeIdColumn, statisticType),
            (StatisticValueColumn, statisticValue))
    End Function

    Public Function Read(locationId As Long, statisticType As Long) As Long? Implements ILocationStatisticData.Read
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            StatisticValueColumn,
            (LocationIdColumn, locationId),
            (LocationStatisticTypeIdColumn, statisticType))
    End Function

    Public Sub Write(locationId As Long, statisticType As Long, statisticValue As Long?) Implements ILocationStatisticData.Write
        If Not statisticValue.HasValue Then
            Store.ClearForColumnValues(
                AddressOf Initialize,
                TableName,
                (LocationIdColumn, locationId),
                (LocationStatisticTypeIdColumn, statisticType))
            Return
        End If
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (LocationStatisticTypeIdColumn, statisticType),
            (StatisticValueColumn, statisticValue.Value))
    End Sub
End Class
