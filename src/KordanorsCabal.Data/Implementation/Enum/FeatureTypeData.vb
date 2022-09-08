Public Class FeatureTypeData
    Inherits BaseData
    Implements IFeatureTypeData
    Friend Const TableName = "FeatureTypes"
    Friend Const FeatureTypeIdColumn = "FeatureTypeId"
    Friend Const FeatureTypeNameColumn = "FeatureTypeName"
    Friend Const LocationTypeIdColumn = LocationTypeData.LocationTypeIdColumn
    Friend Const InteractionModeColumn = "InteractionMode"

    Public Function ReadName(featureTypeId As Long) As String Implements IFeatureTypeData.ReadName
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            FeatureTypeNameColumn,
            (FeatureTypeIdColumn, featureTypeId))
    End Function

    Public Function ReadLocationType(featureTypeId As Long) As Long? Implements IFeatureTypeData.ReadLocationType
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationTypeIdColumn,
            (FeatureTypeIdColumn, featureTypeId))
    End Function

    Public Function ReadInteractionMode(featureTypeId As Long) As Long? Implements IFeatureTypeData.ReadInteractionMode
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            InteractionModeColumn,
            (FeatureTypeIdColumn, featureTypeId))
    End Function
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{FeatureTypeIdColumn}],
                    [{FeatureTypeNameColumn}],
                    [{LocationTypeIdColumn}],
                    [{InteractionModeColumn}]) AS
                (VALUES
                    (6,'Marcus the Black Mage',2,9),
                    (5,'""Honest"" Dan',2,8),
                    (7,'Samuli the Blacksmith',2,10),
                    (4,'Sander the Chicken',2,7),
                    (9,'David the Constable',2,11),
                    (1,'Zooperdan the Elder',1,4),
                    (8,'Nihilist Healer Marten',2,12),
                    (2,'Graham the Innkeeper',2,5),
                    (3,'Yermom the Drunk',2,6)
                )
                SELECT 
                    [{FeatureTypeIdColumn}],
                    [{FeatureTypeNameColumn}],
                    [{LocationTypeIdColumn}],
                    [{InteractionModeColumn}]
                FROM [cte];")
    End Sub

    Public Function ReadAll() As IEnumerable(Of Long) Implements IFeatureTypeData.ReadAll
        Return Store.ReadRecords(Of Long)(
            AddressOf Initialize,
            TableName,
            FeatureTypeIdColumn)
    End Function

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub
End Class
