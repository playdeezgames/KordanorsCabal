Public Class FeatureData
    Inherits BaseData
    Friend Const TableName = "Features"
    Friend Const FeatureIdColumn = "FeatureId"
    Friend Const FeatureTypeColumn = "FeatureType"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn

    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{FeatureIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{FeatureTypeColumn}] INT NOT NULL,
                [{LocationIdColumn}] INT NOT NULL UNIQUE,
                FOREIGN KEY([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
            );")
    End Sub

    Public Function ReadFeatureType(featureId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            FeatureTypeColumn,
            (FeatureIdColumn, featureId))
    End Function

    Public Function Create(featureType As Long, locationId As Long) As Long
        Return Store.CreateRecord(
            AddressOf Initialize,
            TableName,
            (FeatureTypeColumn, featureType),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadForLocation(locationId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            FeatureIdColumn,
            (LocationIdColumn, locationId))
    End Function
End Class
