Public Class CharacterLocationData
    Inherits BaseData
    Friend Const TableName = "CharacterLocations"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const LocationIdColumn = LocationData.LocationIdColumn

    Public Sub New(store As Store)
        MyBase.New(store)
    End Sub

    Friend Sub Initialize()
        StaticWorldData.Character.Initialize()
        StaticWorldData.Location.Initialize()
        Store.ExecuteNonQuery(
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
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (LocationIdColumn, locationId))
    End Sub

    Public Function Read(characterId As Long, locationId As Long) As Boolean
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (CharacterIdColumn, characterId),
            (LocationIdColumn, locationId)).HasValue
    End Function

    Friend Sub ClearForCharacter(characterId As Long)
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
