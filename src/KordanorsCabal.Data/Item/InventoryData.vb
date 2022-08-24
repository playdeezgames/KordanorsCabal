Public Class InventoryData
    Inherits BaseData
    Friend Const TableName = "Inventories"
    Friend Const InventoryIdColumn = "InventoryId"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const LocationIdColumn = LocationData.LocationIdColumn

    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        World.Character.Initialize()
        World.Location.Initialize()
        Store.ExecuteNonQuery(
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
    Public Function CreateForCharacter(characterId As Long) As Long
        Return Store.CreateRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Function
    Public Function CreateForLocation(locationId As Long) As Long
        Return Store.CreateRecord(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId))
    End Function
    Friend Sub ClearForCharacter(characterId As Long)
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
    Public Function ReadForCharacter(characterId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            InventoryIdColumn,
            (CharacterIdColumn, characterId))
    End Function
    Public Function ReadForLocation(locationId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            InventoryIdColumn,
            (LocationIdColumn, locationId))
    End Function
End Class
