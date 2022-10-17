Public Class CharacterLocationData
    Inherits BaseData
    Implements ICharacterLocationData
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const LocationIdColumn = LocationData.LocationIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Sub Write(characterId As Long, locationId As Long) Implements ICharacterLocationData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            Tables.CharacterLocations,
            (CharacterIdColumn, characterId),
            (LocationIdColumn, locationId))
    End Sub

    Public Function Read(characterId As Long, locationId As Long) As Boolean Implements ICharacterLocationData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            Tables.CharacterLocations,
            CharacterIdColumn,
            (CharacterIdColumn, characterId),
            (LocationIdColumn, locationId)).HasValue
    End Function

    Public Sub ClearForCharacter(characterId As Long) Implements ICharacterLocationData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            Tables.CharacterLocations,
            (CharacterIdColumn, characterId))
    End Sub
End Class
