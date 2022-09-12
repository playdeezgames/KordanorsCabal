Public Interface IItem
    Inherits IBaseThingie
    ReadOnly Property MaximumDurability As Long?
    ReadOnly Property IsWeapon() As Boolean
    Sub Purify()
    ReadOnly Property NeedsRepair As Boolean
    ReadOnly Property Name As String
    ReadOnly Property ItemType As OldItemType
    Sub ReduceDurability(amount As Long)
    ReadOnly Property MaximumDamage As Long?
    Sub Use(character As ICharacter)
    ReadOnly Property IsArmor() As Boolean
    ReadOnly Property IsConsumed As Boolean
    ReadOnly Property IsBroken As Boolean
    ReadOnly Property EquipSlots() As IEnumerable(Of EquipSlot)
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long?
    Sub Destroy()
    Function Encumbrance() As Long
    ReadOnly Property Durability As Long?
    ReadOnly Property CanUse(character As ICharacter) As Boolean
    ReadOnly Property DefendDice As Long
    ReadOnly Property CanEquip As Boolean
    ReadOnly Property AttackDice As Long
    Sub Repair()
    Function RepairCost(shoppeType As ShoppeType) As Long
End Interface
