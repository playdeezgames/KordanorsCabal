Public Class InventoryData
    Inherits BaseData
    Implements IInventoryData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function CreateForCharacter(characterId As Long) As Long Implements IInventoryData.CreateForCharacter
        Return Store.Create.Entry(
            AddressOf NoInitializer,
            Inventories,
            (CharacterIdColumn, characterId))
    End Function
    Public Function CreateForLocation(locationId As Long) As Long Implements IInventoryData.CreateForLocation
        Return Store.Create.Entry(
            AddressOf NoInitializer,
            Inventories,
            (LocationIdColumn, locationId))
    End Function
    Sub ClearForCharacter(characterId As Long) Implements IInventoryData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            Inventories,
            (CharacterIdColumn, characterId))
    End Sub
    Public Function ReadForCharacter(characterId As Long) As Long? Implements IInventoryData.ReadForCharacter
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Inventories,
            InventoryIdColumn,
            (CharacterIdColumn, characterId))
    End Function
    Public Function ReadForLocation(locationId As Long) As Long? Implements IInventoryData.ReadForLocation
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Inventories,
            InventoryIdColumn,
            (LocationIdColumn, locationId))
    End Function
End Class
