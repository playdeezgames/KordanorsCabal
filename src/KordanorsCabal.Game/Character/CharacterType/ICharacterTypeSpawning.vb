﻿Public Interface ICharacterTypeSpawning
    Function CanSpawn(locationType As ILocationType, level As IDungeonLevel) As Boolean
    Function SpawnCount(level As IDungeonLevel) As Long
    ReadOnly Property InitialStatistics As IReadOnlyList(Of (ICharacterStatisticType, Long))
End Interface
