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

    Friend Sub Initialize()
        CType(World.Character, CharacterData).Initialize()
        CType(World.Location, LocationData).Initialize()
        Store.Primitive.Execute(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{InventoryIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{CharacterIdColumn}] INT NULL UNIQUE,
                [{LocationIdColumn}] INT NULL UNIQUE,
                FOREIGN KEY ([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}]),
                FOREIGN KEY ([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}]),
                CHECK(
                    ([{CharacterIdColumn}] IS NULL AND [{LocationIdColumn}] IS NOT NULL) OR 
                    ([{CharacterIdColumn}] IS NOT NULL AND [{LocationIdColumn}] IS NULL)
                )
            );")
    End Sub
    Public Function CreateForCharacter(characterId As Long) As Long Implements IInventoryData.CreateForCharacter
        Return Store.Create.Entry(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Function
    Public Function CreateForLocation(locationId As Long) As Long Implements IInventoryData.CreateForLocation
        Return Store.Create.Entry(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId))
    End Function
    Sub ClearForCharacter(characterId As Long) Implements IInventoryData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
    Public Function ReadForCharacter(characterId As Long) As Long? Implements IInventoryData.ReadForCharacter
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            InventoryIdColumn,
            (CharacterIdColumn, characterId))
    End Function
    Public Function ReadForLocation(locationId As Long) As Long? Implements IInventoryData.ReadForLocation
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            InventoryIdColumn,
            (LocationIdColumn, locationId))
    End Function
End Class
