Friend Class RouteTypeData
    Inherits BaseData
    Implements IRouteTypeData
    Friend Const TableName = "RouteTypes"
    Friend Const RouteTypeIdColumn = "RouteTypeId"
    Friend Const AbbreviationColumn = "Abbreviation"
    Friend Const IsSingleUseColumn = "IsSingleUse"

    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{RouteTypeIdColumn}],
                    [{AbbreviationColumn}],
                    [{IsSingleUseColumn}]) AS
                (VALUES
                    (1,'  ',0),
                    (2,'  ',0),
                    (3,'  ',0),
                    (4,'FE',0),
                    (5,'CU',0),
                    (6,'AG',0),
                    (7,'AU',0),
                    (8,'PT',0),
                    (9,'EO',0),
                    (10,'  ',1),
                    (11,'  ',0))
                SELECT 
                    [{RouteTypeIdColumn}],
                    [{AbbreviationColumn}],
                    [{IsSingleUseColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadAbbreviation(routeTypeId As Long) As String Implements IRouteTypeData.ReadAbbreviation
        Return Store.Column.ReadString(
            AddressOf Initialize,
            TableName,
            AbbreviationColumn,
            (RouteTypeIdColumn, routeTypeId))
    End Function

    Public Function ReadIsSingleUse(routeTypeId As Long) As Boolean Implements IRouteTypeData.ReadIsSingleUse
        Return If(Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName, IsSingleUseColumn,
            (RouteTypeIdColumn, routeTypeId)), 0L) > 0L
    End Function
End Class
