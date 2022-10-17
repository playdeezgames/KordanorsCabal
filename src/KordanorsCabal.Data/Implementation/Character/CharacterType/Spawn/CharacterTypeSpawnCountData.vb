Public Class CharacterTypeSpawnCountData
    Inherits BaseData
    Implements ICharacterTypeSpawnCountData
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const DungeonLevelIdColumn = DungeonLevelData.DungeonLevelIdColumn
    Friend Const SpawnCountColumn = "SpawnCount"
    Public Function ReadSpawnCount(characterTypeId As Long, dungeonLevelId As Long) As Long? Implements ICharacterTypeSpawnCountData.ReadSpawnCount
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            CharacterTypeSpawnCounts,
            SpawnCountColumn,
            (CharacterTypeIdColumn, characterTypeId),
            (DungeonLevelIdColumn, dungeonLevelId))
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
End Class
