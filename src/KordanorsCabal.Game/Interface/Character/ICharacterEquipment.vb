Public Interface ICharacterEquipment
    Inherits IBaseThingie
    Function DoArmorWear(wear As Long) As IEnumerable(Of IItemType)
    Function DoWeaponWear(wear As Long) As IEnumerable(Of IItemType)
    ReadOnly Property HasEquipment As Boolean
    ReadOnly Property EquippedSlots As IEnumerable(Of IEquipSlot)
    Sub Unequip(equipSlot As IEquipSlot)
    Function CurrentEquipment(equipSlot As IEquipSlot) As IItem
    Sub Equip(item As IItem)
End Interface
