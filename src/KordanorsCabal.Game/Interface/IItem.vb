Public Interface IItem
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property ItemType As IItemType
    ReadOnly Property Weapon As IWeapon
    ReadOnly Property Durability As IDurability
    ReadOnly Property Repair As IRepair
    ReadOnly Property Armor As IArmor
    ReadOnly Property Equipment As IEquipment

    ReadOnly Property CanEquip As Boolean
    ReadOnly Property EquipSlots() As IEnumerable(Of IEquipSlot)
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long?


    ReadOnly Property IsConsumed As Boolean
    ReadOnly Property CanUse(character As ICharacter) As Boolean
    Sub Use(character As ICharacter)

    ReadOnly Property Encumbrance As Long
    Sub Purify()
    Sub Destroy()

    'TODO: RepairCost needs unit test, but ShoppeType needs refactor first
    Function RepairCost(shoppeType As ShoppeType) As Long
End Interface
