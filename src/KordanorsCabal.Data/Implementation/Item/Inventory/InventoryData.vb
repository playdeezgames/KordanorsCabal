Public Class InventoryData
    Inherits BaseData
    Implements IInventoryData
    Friend Const TableName = "Inventories"
    Friend Const InventoryIdColumn = "InventoryId"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const LocationIdColumn = LocationData.LocationIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function CreateForCharacter(characterId As Long) As Long Implements IInventoryData.CreateForCharacter
        Return Store.Create.Entry(
            AddressOf NoInitializer,
            TableName,
            (CharacterIdColumn, characterId))
    End Function
    Public Function CreateForLocation(locationId As Long) As Long Implements IInventoryData.CreateForLocation
        Return Store.Create.Entry(
            AddressOf NoInitializer,
            TableName,
            (LocationIdColumn, locationId))
    End Function
    Sub ClearForCharacter(characterId As Long) Implements IInventoryData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
    Public Function ReadForCharacter(characterId As Long) As Long? Implements IInventoryData.ReadForCharacter
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            InventoryIdColumn,
            (CharacterIdColumn, characterId))
    End Function
    Public Function ReadForLocation(locationId As Long) As Long? Implements IInventoryData.ReadForLocation
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            InventoryIdColumn,
            (LocationIdColumn, locationId))
    End Function
End Class
