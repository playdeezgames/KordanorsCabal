﻿Public Interface IItemType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property IsConsumed As Boolean
    ReadOnly Property SpawnLocationTypes(dungeonLevel As IDungeonLevel) As HashSet(Of ILocationType)
    ReadOnly Property EquipSlots As IEnumerable(Of IEquipSlot)
    ReadOnly Property SpawnCounts(dungeonLevel As IDungeonLevel) As String
End Interface
