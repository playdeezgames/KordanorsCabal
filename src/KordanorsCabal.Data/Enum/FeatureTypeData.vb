Public Class FeatureTypeData
    Inherits BaseData
    Friend Const TableName = "FeatureTypes"
    Friend Const FeatureTypeIdColumn = "FeatureTypeId"
    Friend Const FeatureTypeNameColumn = "FeatureTypeName"
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

    Public Sub New(store As Store)
        MyBase.New(store)
    End Sub
End Class
