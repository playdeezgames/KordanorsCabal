Public Interface IItemTypeSpawn
    Inherits IBaseThingie
    ReadOnly Property SpawnLocationTypes(dungeonLevel As IDungeonLevel) As HashSet(Of ILocationType)
    Function RollSpawnCount(dungeonLevel As IDungeonLevel) As Long
End Interface
