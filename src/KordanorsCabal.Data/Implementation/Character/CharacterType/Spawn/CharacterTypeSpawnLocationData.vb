Public Class CharacterTypeSpawnLocationData
    Inherits BaseData
    Implements ICharacterTypeSpawnLocationData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(
                        characterTypeId As Long,
                        dungeonLevel As Long,
                        locationType As Long) As Boolean Implements ICharacterTypeSpawnLocationData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long, Long)(
            AddressOf NoInitializer,
            CharacterTypeSpawnLocations,
            CharacterTypeIdColumn,
            (CharacterTypeIdColumn, characterTypeId),
            (DungeonLevelIdColumn, dungeonLevel),
            (LocationTypeIdColumn, locationType)).HasValue
    End Function
End Class
