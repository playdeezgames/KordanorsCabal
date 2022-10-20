Public Interface IItemTypeEquip
    Inherits IBaseThingie
    ReadOnly Property EquipSlots As IEnumerable(Of IEquipSlot)
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long?
End Interface
