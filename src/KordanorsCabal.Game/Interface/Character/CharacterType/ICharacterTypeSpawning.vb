Public Interface ICharacterTypeSpawning
    Inherits IBaseThingie
    Function CanSpawn(locationType As ILocationType, level As IDungeonLevel) As Boolean
    Function SpawnCount(level As IDungeonLevel) As Long
    ReadOnly Property InitialStatistics As IReadOnlyList(Of (IStatisticType, Long))
End Interface
