Public Interface IItemType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property IsConsumed As Boolean
    ReadOnly Property SpawnLocationTypes(dungeonLevel As IDungeonLevel) As HashSet(Of ILocationType)
    ReadOnly Property EquipSlots As IEnumerable(Of IEquipSlot)
    Function RollSpawnCount(dungeonLevel As IDungeonLevel) As Long
    ReadOnly Property AttackDice As Long
    ReadOnly Property Encumbrance As Long
    ReadOnly Property MaximumDamage As Long?
    ReadOnly Property DefendDice As Long
    ReadOnly Property MaximumDurability As Long?
    ReadOnly Property Offer As Long
    ReadOnly Property Price As Long
    ReadOnly Property RepairPrice As Long
    ReadOnly Property Purify As Action(Of Item) 'TODO: IItem
    ReadOnly Property Use As Action(Of ICharacter)
    ReadOnly Property CanUse As Func(Of ICharacter, Boolean)
End Interface
