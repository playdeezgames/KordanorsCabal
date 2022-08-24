Public Class CharacterData
    Inherits BaseData
    Public Const TableName = "Characters"
    Friend Const CharacterIdColumn = "CharacterId"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const CharacterTypeColumn = "CharacterType"
    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        World.Location.Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}]
        (
            [{CharacterIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
            [{LocationIdColumn}] INT NOT NULL,
            [{CharacterTypeColumn}] INT NOT NULL,
            FOREIGN KEY ([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
        );")
    End Sub
    Public Function ReadCharacterType(characterId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterTypeColumn,
            (CharacterIdColumn, characterId))
    End Function

    Public Function Create(characterType As Long, locationId As Long) As Long
        Return Store.CreateRecord(
            AddressOf Initialize,
            TableName,
            (CharacterTypeColumn, characterType),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadLocation(characterId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationIdColumn,
            (CharacterIdColumn, characterId))
    End Function

    Public Sub WriteLocation(characterId As Long, locationId As Long)
        Store.WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (CharacterIdColumn, characterId))
    End Sub

    Public Function ReadForLocation(locationId As Long) As IEnumerable(Of Long)
        Return Store.ReadRecordsWithColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (LocationIdColumn, locationId))
    End Function

    Public Sub Clear(characterId As Long)
        World.CharacterQuest.ClearForCharacter(characterId)
        World.CharacterQuestCompletion.ClearForCharacter(characterId)
        World.CharacterEquipSlot.ClearForCharacter(characterId)
        World.Inventory.ClearForCharacter(characterId)
        World.CharacterLocation.ClearForCharacter(characterId)
        World.CharacterStatistic.ClearForCharacter(characterId)
        World.Player.ClearForCharacter(characterId)
        World.CharacterSpell.ClearForCharacter(characterId)
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
