Public Class FeatureData
    Inherits BaseData
    Implements IFeatureData
    Friend Const TableName = "Features"
    Friend Const FeatureIdColumn = "FeatureId"
    Friend Const FeatureTypeIdColumn = FeatureTypeData.FeatureTypeIdColumn
    Friend Const LocationIdColumn = LocationData.LocationIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{FeatureIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{FeatureTypeIdColumn}] INT NOT NULL,
                [{LocationIdColumn}] INT NOT NULL UNIQUE,
                FOREIGN KEY([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
            );")
    End Sub

    Public Function ReadFeatureType(featureId As Long) As Long? Implements IFeatureData.ReadFeatureType
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            FeatureTypeIdColumn,
            (FeatureIdColumn, featureId))
    End Function

    Public Function Create(featureType As Long, locationId As Long) As Long Implements IFeatureData.Create
        Return Store.CreateRecord(
            AddressOf Initialize,
            TableName,
            (FeatureTypeIdColumn, featureType),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadForLocation(locationId As Long) As Long? Implements IFeatureData.ReadForLocation
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            FeatureIdColumn,
            (LocationIdColumn, locationId))
    End Function
End Class
