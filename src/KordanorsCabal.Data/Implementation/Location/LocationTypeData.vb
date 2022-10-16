Public Class LocationTypeData
    Inherits BaseData
    Implements ILocationTypeData
    Friend Const TableName = "LocationTypes"
    Friend Const LocationTypeIdColumn = "LocationTypeId"
    Friend Const LocationTypeNameColumn = "LocationTypeName"
    Friend Const IsDungeonColumn = "IsDungeon"
    Friend Const CanMapColumn = "CanMap"
    Friend Const RequiresMPColumn = "RequiresMP"
    Public Function ReadRequiresMP(locationTypeId As Long) As Boolean Implements ILocationTypeData.ReadRequiresMP
        Return If(Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            RequiresMPColumn,
            (LocationTypeIdColumn, locationTypeId)), 0) > 0
    End Function

    Public Function ReadCanMap(locationTypeId As Long) As Boolean Implements ILocationTypeData.ReadCanMap
        Return If(Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            CanMapColumn,
            (LocationTypeIdColumn, locationTypeId)), 0) > 0
    End Function

    Public Function ReadIsDungeon(locationTypeId As Long) As Boolean Implements ILocationTypeData.ReadIsDungeon
        Return If(Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            IsDungeonColumn,
            (LocationTypeIdColumn, locationTypeId)), 0) > 0
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadName(locationTypeId As Long) As String Implements ILocationTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            LocationTypeNameColumn,
            (LocationTypeIdColumn, locationTypeId))
    End Function
End Class
