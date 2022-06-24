Public Module LocationStatisticData
    Friend Const TableName = "LocationStatistics"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const StatisticTypeColumn = "StatisticType"
    Friend Const StatisticValueColumn = "StatisticValue"
    Friend Sub Initialize()
        LocationData.Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{LocationIdColumn}] INT NOT NULL,
                [{StatisticTypeColumn}] INT NOT NULL, 
                [{StatisticValueColumn}] INT NOT NULL,
                UNIQUE([{LocationIdColumn}],[{StatisticTypeColumn}]),
                FOREIGN KEY ([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
            );")
    End Sub
    Public Sub Write(locationId As Long, statisticType As Long, statisticValue As Long?)
        If Not statisticValue.HasValue Then
            ClearForColumnValues(
                AddressOf Initialize,
                TableName,
                (LocationIdColumn, locationId),
                (StatisticTypeColumn, statisticType))
            Return
        End If
        ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (StatisticTypeColumn, statisticType),
            (StatisticValueColumn, statisticValue.Value))
    End Sub
End Module
