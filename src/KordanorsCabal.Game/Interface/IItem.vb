Public Interface IItem
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property ItemType As IItemType

    ReadOnly Property IsWeapon() As Boolean
    ReadOnly Property AttackDice As Long
    ReadOnly Property MaximumDamage As Long?

    ReadOnly Property Durability As Long?
    ReadOnly Property MaximumDurability As Long?
    Sub ReduceDurability(amount As Long)
    ReadOnly Property IsBroken As Boolean

    ReadOnly Property NeedsRepair As Boolean
    Sub Repair()
    'TODO: RepairCost needs unit test, but ShoppeType needs refactor first
    Function RepairCost(shoppeType As ShoppeType) As Long

    ReadOnly Property IsArmor() As Boolean
    ReadOnly Property DefendDice As Long

    ReadOnly Property CanEquip As Boolean
    ReadOnly Property EquipSlots() As IEnumerable(Of IEquipSlot)
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long?

    Function Encumbrance() As Long
    Sub Purify()

    Sub Use(character As ICharacter)
    ReadOnly Property IsConsumed As Boolean
    ReadOnly Property CanUse(character As ICharacter) As Boolean

    Sub Destroy()
End Interface
