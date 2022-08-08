Public Module CharacterLocationData
    Friend Const TableName = "CharacterLocations"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Private ReadOnly Store As SPLORR.Data.Store = StaticStore.Store
    Friend Sub Initialize()
        CharacterData.Initialize()
        LocationData.Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{LocationIdColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{LocationIdColumn}]),
                FOREIGN KEY([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}]),
                FOREIGN KEY([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
            );")
    End Sub
    Public Sub Write(characterId As Long, locationId As Long)
        ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (LocationIdColumn, locationId))
    End Sub

    Public Function Read(characterId As Long, locationId As Long) As Boolean
        Return ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (CharacterIdColumn, characterId),
            (LocationIdColumn, locationId)).HasValue
    End Function

    Friend Sub ClearForCharacter(characterId As Long)
        ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Module
