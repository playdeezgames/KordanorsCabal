Public Module CharacterData
    Friend Const TableName = "Characters"
    Friend Const CharacterIdColumn = "CharacterId"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const CharacterTypeColumn = "CharacterType"
    Friend Sub Initialize()
        LocationData.Initialize()
        ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}]
        (
            [{CharacterIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
            [{LocationIdColumn}] INT NOT NULL,
            [{CharacterTypeColumn}] INT NOT NULL,
            FOREIGN KEY ([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
        );")
    End Sub
    Public Function ReadCharacterType(characterId As Long) As Long
        Throw New NotImplementedException()
    End Function

    Public Function Create(characterType As Long, locationId As Long) As Long
        Return CreateRecord(
            AddressOf Initialize,
            TableName,
            (CharacterTypeColumn, characterType),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadLocation(characterId As Long) As Long?
        Throw New NotImplementedException()
    End Function
End Module
