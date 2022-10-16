Public Class CharacterTypeSpawnLocationData
    Inherits BaseData
    Implements ICharacterTypeSpawnLocationData
    Friend Const TableName = "CharacterTypeSpawnLocations"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const DungeonLevelIdColumn = DungeonLevelData.DungeonLevelIdColumn
    Friend Const LocationTypeIdColumn = LocationTypeData.LocationTypeIdColumn
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(
                        characterTypeId As Long,
                        dungeonLevel As Long,
                        locationType As Long) As Boolean Implements ICharacterTypeSpawnLocationData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long, Long)(
            AddressOf NoInitializer,
            TableName,
            CharacterTypeIdColumn,
            (CharacterTypeIdColumn, characterTypeId),
            (DungeonLevelIdColumn, dungeonLevel),
            (LocationTypeIdColumn, locationType)).HasValue
    End Function
End Class
