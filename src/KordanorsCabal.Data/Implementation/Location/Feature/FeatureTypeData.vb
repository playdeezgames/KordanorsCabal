Public Class FeatureTypeData
    Inherits BaseData
    Implements IFeatureTypeData
    Friend Const FeatureTypeIdColumn = "FeatureTypeId"
    Friend Const FeatureTypeNameColumn = "FeatureTypeName"
    Friend Const LocationTypeIdColumn = LocationTypeData.LocationTypeIdColumn
    Friend Const InteractionModeColumn = "InteractionMode"

    Public Function ReadName(featureTypeId As Long) As String Implements IFeatureTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            FeatureTypes,
            FeatureTypeNameColumn,
            (FeatureTypeIdColumn, featureTypeId))
    End Function

    Public Function ReadLocationType(featureTypeId As Long) As Long? Implements IFeatureTypeData.ReadLocationType
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            FeatureTypes,
            LocationTypeIdColumn,
            (FeatureTypeIdColumn, featureTypeId))
    End Function

    Public Function ReadInteractionMode(featureTypeId As Long) As Long? Implements IFeatureTypeData.ReadInteractionMode
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            FeatureTypes,
            InteractionModeColumn,
            (FeatureTypeIdColumn, featureTypeId))
    End Function
    Public Function ReadAll() As IEnumerable(Of Long) Implements IFeatureTypeData.ReadAll
        Return Store.Record.All(Of Long)(
            AddressOf NoInitializer,
            FeatureTypes,
            FeatureTypeIdColumn)
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
End Class
