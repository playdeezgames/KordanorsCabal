﻿Public Class ItemTypeData
    Inherits BaseData
    Implements IItemTypeData
    Friend Const TableName = "ItemTypes"
    Friend Const ItemTypeIdColumn = "ItemTypeId"
    Friend Const ItemTypeNameColumn = "ItemTypeName"
    Friend Const IsConsumedColumn = "IsConsumed"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadIsConsumed(itemTypeId As Long) As Long? Implements IItemTypeData.ReadIsConsumed
        Return Store.Column.ReadValue(Of Long, Long)(AddressOf NoInitializer, TableName, IsConsumedColumn, (ItemTypeIdColumn, itemTypeId))
    End Function
    Public Function ReadName(itemTypeId As Long) As String Implements IItemTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            ItemTypeNameColumn,
            (ItemTypeIdColumn, itemTypeId))
    End Function

    Public Function ReadAll() As IEnumerable(Of Long) Implements IItemTypeData.ReadAll
        Return Store.Record.All(Of Long)(
            AddressOf NoInitializer,
            TableName,
            ItemTypeIdColumn)
    End Function
End Class
