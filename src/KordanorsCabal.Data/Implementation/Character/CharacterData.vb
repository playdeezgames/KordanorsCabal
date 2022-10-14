Public Class CharacterData
    Inherits BaseData
    Implements ICharacterData
    Friend Const TableName = "Characters"
    Friend Const CharacterIdColumn = "CharacterId"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Location, LocationData).Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}]
        (
            [{CharacterIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
            [{LocationIdColumn}] INT NOT NULL,
            [{CharacterTypeIdColumn}] INT NOT NULL,
            FOREIGN KEY ([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}])
        );")
    End Sub
    Public Function ReadCharacterType(characterId As Long) As Long? Implements ICharacterData.ReadCharacterType
        Return Store.Column.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterTypeIdColumn,
            (CharacterIdColumn, characterId))
    End Function

    Public Function Create(characterType As Long, locationId As Long) As Long Implements ICharacterData.Create
        Return Store.Create.CreateRecord(
            AddressOf Initialize,
            TableName,
            (CharacterTypeIdColumn, characterType),
            (LocationIdColumn, locationId))
    End Function

    Public Function ReadLocation(characterId As Long) As Long? Implements ICharacterData.ReadLocation
        Return Store.Column.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            LocationIdColumn,
            (CharacterIdColumn, characterId))
    End Function

    Public Sub WriteLocation(characterId As Long, locationId As Long) Implements ICharacterData.WriteLocation
        Store.Column.WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (CharacterIdColumn, characterId))
    End Sub

    Public Function ReadForLocation(locationId As Long) As IEnumerable(Of Long) Implements ICharacterData.ReadForLocation
        Return Store.Record.ReadRecordsWithColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (LocationIdColumn, locationId))
    End Function

    Public Sub Clear(characterId As Long) Implements ICharacterData.Clear
        World.CharacterQuest.ClearForCharacter(characterId)
        World.CharacterQuestCompletion.ClearForCharacter(characterId)
        World.CharacterEquipSlot.ClearForCharacter(characterId)
        World.Inventory.ClearForCharacter(characterId)
        World.CharacterLocation.ClearForCharacter(characterId)
        World.CharacterStatistic.ClearForCharacter(characterId)
        World.Player.ClearForCharacter(characterId)
        World.CharacterSpell.ClearForCharacter(characterId)
        Store.Clear.ForValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
