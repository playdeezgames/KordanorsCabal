Public Module CharacterData
    Public Const TableName = "Characters"
    Friend Const CharacterIdColumn = "CharacterId"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const CharacterTypeColumn = "CharacterType"
    Private ReadOnly Store As SPLORR.Data.Store = StaticStore.Store
    Public Sub Initialize()
        LocationData.Initialize()
        ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}]
        (
            [{CharacterIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
            [{LocationIdColumn}] INT NOT NULL,
            [{CharacterTypeColumn}] INT NOT NULL,
            FOREIGN KEY ([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
        );")
    End Sub
    Public Function ReadCharacterType(characterId As Long) As Long?
        Return ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterTypeColumn,
            (CharacterIdColumn, characterId))
    End Function

    Public Function Create(characterType As Long, locationId As Long) As Long
        Return CreateRecord(
            AddressOf Initialize,
            TableName,
            (CharacterTypeColumn, characterType),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadLocation(characterId As Long) As Long?
        Return ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationIdColumn,
            (CharacterIdColumn, characterId))
    End Function

    Public Sub WriteLocation(characterId As Long, locationId As Long)
        WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (CharacterIdColumn, characterId))
    End Sub

    Public Function ReadForLocation(locationId As Long) As IEnumerable(Of Long)
        Return ReadRecordsWithColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (LocationIdColumn, locationId))
    End Function

    Public Sub Clear(characterId As Long)
        CharacterQuestData.ClearForCharacter(characterId)
        CharacterQuestCompletionData.ClearForCharacter(characterId)
        CharacterEquipSlotData.ClearForCharacter(characterId)
        InventoryData.ClearForCharacter(characterId)
        CharacterLocationData.ClearForCharacter(characterId)
        CharacterStatisticData.ClearForCharacter(characterId)
        PlayerData.ClearForCharacter(characterId)
        CharacterSpellData.ClearForCharacter(characterId)
        ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Module
