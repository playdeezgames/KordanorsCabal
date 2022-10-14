Public Class CharacterLocationData
    Inherits BaseData
    Implements ICharacterLocationData
    Friend Const TableName = "CharacterLocations"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const LocationIdColumn = LocationData.LocationIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Character, CharacterData).Initialize()
        CType(World.Location, LocationData).Initialize()
        Store.Primitive.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{LocationIdColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{LocationIdColumn}]),
                FOREIGN KEY([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}]),
                FOREIGN KEY([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
            );")
    End Sub
    Public Sub Write(characterId As Long, locationId As Long) Implements ICharacterLocationData.Write
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (LocationIdColumn, locationId))
    End Sub

    Public Function Read(characterId As Long, locationId As Long) As Boolean Implements ICharacterLocationData.Read
        Return Store.Column.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (CharacterIdColumn, characterId),
            (LocationIdColumn, locationId)).HasValue
    End Function

    Public Sub ClearForCharacter(characterId As Long) Implements ICharacterLocationData.ClearForCharacter
        Store.Clear.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
