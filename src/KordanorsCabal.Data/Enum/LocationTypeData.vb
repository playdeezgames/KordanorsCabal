Public Class LocationTypeData
    Inherits NameCacheData
    Friend Const TableName = "LocationTypes"
    Friend Const LocationTypeIdColumn = "LocationTypeId"
    Friend Const LocationTypeNameColumn = "LocationTypeName"
    Friend Const IsDungeonColumn = "IsDungeon"
    Friend Const CanMapColumn = "CanMap"
    Friend Const RequiresMPColumn = "RequiresMP"
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{LocationTypeIdColumn}],
                    [{LocationTypeNameColumn}],
                    [{IsDungeonColumn}],
                    [{CanMapColumn}],
                    [{RequiresMPColumn}]) AS
                (VALUES
                    (1,'Town Square',0,0,0),
                    (2,'Town',0,0,0),
                    (3,'Church Entrance',0,0,0),
                    (4,'Dungeon',1,1,1),
                    (5,'Dungeon Dead End',1,1,1),
                    (6,'Dungeon Boss',1,1,1),
                    (7,'Cellar',1,0,1),
                    (8,'Moon',1,1,1))
                SELECT 
                    [{LocationTypeIdColumn}],
                    [{LocationTypeNameColumn}],
                    [{IsDungeonColumn}],
                    [{CanMapColumn}],
                    [{RequiresMPColumn}]
                FROM [cte];")
    End Sub

    Public Function ReadRequiresMP(locationTypeId As Long) As Boolean
        Return If(Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            RequiresMPColumn,
            (LocationTypeIdColumn, locationTypeId)), 0) > 0
    End Function

    Public Function ReadCanMap(locationTypeId As Long) As Boolean
        Return If(Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            CanMapColumn,
            (LocationTypeIdColumn, locationTypeId)), 0) > 0
    End Function

    Public Function ReadIsDungeon(locationTypeId As Long) As Boolean
        Return If(Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            IsDungeonColumn,
            (LocationTypeIdColumn, locationTypeId)), 0) > 0
    End Function

    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
        lookUpByName = Function(name) store.ReadColumnValue(Of String, Long)(
            AddressOf Initialize,
            TableName,
            LocationTypeIdColumn,
            (LocationTypeNameColumn, name))
    End Sub

    Public Function ReadName(locationTypeId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            LocationTypeNameColumn,
            (LocationTypeIdColumn, locationTypeId))
    End Function
End Class
