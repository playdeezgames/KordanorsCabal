Public Class LocationStatisticData
    Inherits BaseData
    Friend Const TableName = "LocationStatistics"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const StatisticTypeColumn = "StatisticType"
    Friend Const StatisticValueColumn = "StatisticValue"

    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        World.Location.Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{LocationIdColumn}] INT NOT NULL,
                [{StatisticTypeColumn}] INT NOT NULL, 
                [{StatisticValueColumn}] INT NOT NULL,
                UNIQUE([{LocationIdColumn}],[{StatisticTypeColumn}]),
                FOREIGN KEY ([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
            );")
    End Sub

    Public Function ReadForStatisticValue(statisticType As Long, statisticValue As Long) As IEnumerable(Of Long)
        Return Store.ReadRecordsWithColumnValues(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName, LocationIdColumn,
            (StatisticTypeColumn, statisticType),
            (StatisticValueColumn, statisticValue))
    End Function

    Public Function Read(locationId As Long, statisticType As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            StatisticValueColumn,
            (LocationIdColumn, locationId),
            (StatisticTypeColumn, statisticType))
    End Function

    Public Sub Write(locationId As Long, statisticType As Long, statisticValue As Long?)
        If Not statisticValue.HasValue Then
            Store.ClearForColumnValues(
                AddressOf Initialize,
                TableName,
                (LocationIdColumn, locationId),
                (StatisticTypeColumn, statisticType))
            Return
        End If
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (StatisticTypeColumn, statisticType),
            (StatisticValueColumn, statisticValue.Value))
    End Sub
End Class
