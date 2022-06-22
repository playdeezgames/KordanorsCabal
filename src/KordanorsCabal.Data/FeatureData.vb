Public Module FeatureData
    Friend Const TableName = "Features"
    Friend Const FeatureIdColumn = "FeatureId"
    Friend Const FeatureTypeColumn = "FeatureType"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Sub Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{FeatureIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{FeatureTypeColumn}] INT NOT NULL,
                [{LocationIdColumn}] INT NOT NULL UNIQUE,
                FOREIGN KEY([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
            );")
    End Sub

    Public Function Create(featureType As Long, locationId As Long) As Long
        Return CreateRecord(AddressOf Initialize, TableName, (FeatureTypeColumn, featureType), (LocationIdColumn, locationId))
    End Function

    Public Function ReadForLocation(locationId As Long) As Long?
        Return ReadColumnValue(Of Long, Long)(AddressOf Initialize, TableName, FeatureIdColumn, (LocationIdColumn, locationId))
    End Function
End Module
