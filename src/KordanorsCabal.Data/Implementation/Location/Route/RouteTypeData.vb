Friend Class RouteTypeData
    Inherits BaseData
    Implements IRouteTypeData
    Friend Const TableName = "RouteTypes"
    Friend Const RouteTypeIdColumn = "RouteTypeId"
    Friend Const AbbreviationColumn = "Abbreviation"
    Friend Const IsSingleUseColumn = "IsSingleUse"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadAbbreviation(routeTypeId As Long) As String Implements IRouteTypeData.ReadAbbreviation
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            AbbreviationColumn,
            (RouteTypeIdColumn, routeTypeId))
    End Function

    Public Function ReadIsSingleUse(routeTypeId As Long) As Boolean Implements IRouteTypeData.ReadIsSingleUse
        Return If(Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName, IsSingleUseColumn,
            (RouteTypeIdColumn, routeTypeId)), 0L) > 0L
    End Function
End Class
