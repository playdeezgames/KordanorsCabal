Public Interface IItem
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property ItemType As IItemType

    ReadOnly Property Weapon As IWeapon
    ReadOnly Property IsWeapon() As Boolean
    ReadOnly Property AttackDice As Long
    ReadOnly Property MaximumDamage As Long?

    ReadOnly Property Durability As Long?
    ReadOnly Property MaximumDurability As Long?
    Sub ReduceDurability(amount As Long)
    ReadOnly Property IsBroken As Boolean

    ReadOnly Property NeedsRepair As Boolean
    Sub Repair()

    ReadOnly Property IsArmor() As Boolean
    ReadOnly Property DefendDice As Long

    ReadOnly Property CanEquip As Boolean
    ReadOnly Property EquipSlots() As IEnumerable(Of IEquipSlot)
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long?

    ReadOnly Property Encumbrance As Long
    Sub Purify()

    ReadOnly Property IsConsumed As Boolean
    ReadOnly Property CanUse(character As ICharacter) As Boolean
    Sub Use(character As ICharacter)

    Sub Destroy()

    'TODO: RepairCost needs unit test, but ShoppeType needs refactor first
    Function RepairCost(shoppeType As ShoppeType) As Long
End Interface
