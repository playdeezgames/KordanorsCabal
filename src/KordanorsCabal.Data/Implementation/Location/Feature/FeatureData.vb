Public Class FeatureData
    Inherits BaseData
    Implements IFeatureData
    Friend Const FeatureIdColumn = "FeatureId"
    Friend Const FeatureTypeIdColumn = FeatureTypeData.FeatureTypeIdColumn
    Friend Const LocationIdColumn = LocationData.LocationIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function ReadFeatureType(featureId As Long) As Long? Implements IFeatureData.ReadFeatureType
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Features,
            FeatureTypeIdColumn,
            (FeatureIdColumn, featureId))
    End Function

    Public Function Create(featureType As Long, locationId As Long) As Long Implements IFeatureData.Create
        Return Store.Create.Entry(
            AddressOf NoInitializer,
            Features,
            (FeatureTypeIdColumn, featureType),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadForLocation(locationId As Long) As Long? Implements IFeatureData.ReadForLocation
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Features,
            FeatureIdColumn,
            (LocationIdColumn, locationId))
    End Function
End Class
