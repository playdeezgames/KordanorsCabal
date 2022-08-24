Public Class FeatureTypeData
    Inherits BaseData
    Friend Const TableName = "FeatureTypes"
    Friend Const FeatureTypeIdColumn = "FeatureTypeId"
    Friend Const FeatureTypeNameColumn = "FeatureTypeName"


    Public Function ReadName(featureTypeId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            FeatureTypeNameColumn,
            (FeatureTypeIdColumn, featureTypeId))
    End Function

    Public Function ReadLocationType(featureTypeId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationTypeColumn,
            (FeatureTypeIdColumn, featureTypeId))
    End Function

    Public Function ReadInteractionMode(featureTypeId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            PlayerModeColumn,
            (FeatureTypeIdColumn, featureTypeId))
    End Function

    Friend Const LocationTypeColumn = "LocationType"
    Friend Const PlayerModeColumn = "PlayerMode"
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{FeatureTypeIdColumn}],
                    [{FeatureTypeNameColumn}],
                    [{LocationTypeColumn}],
                    [{PlayerModeColumn}]) AS
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
                    [{LocationTypeColumn}],
                    [{PlayerModeColumn}]
                FROM [cte];")
    End Sub

    Public Function ReadAll() As IEnumerable(Of Long)
        Return Store.ReadRecords(Of Long)(
            AddressOf Initialize,
            TableName,
            FeatureTypeIdColumn)
    End Function

    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
    End Sub
End Class
