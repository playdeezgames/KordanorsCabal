Public Interface IItemType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property IsConsumed As Boolean
    ReadOnly Property SpawnLocationTypes(dungeonLevel As IDungeonLevel) As HashSet(Of ILocationType)
    ReadOnly Property EquipSlots As IEnumerable(Of IEquipSlot)
    Function RollSpawnCount(dungeonLevel As IDungeonLevel) As Long
    ReadOnly Property AttackDice As Long
    ReadOnly Property Encumbrance As Long
End Interface
