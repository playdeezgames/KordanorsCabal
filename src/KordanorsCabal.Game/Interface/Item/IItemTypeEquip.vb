Public Interface IItemTypeEquip
    Inherits IBaseThingie
    ReadOnly Property EquipSlots As IEnumerable(Of IEquipSlot)
    Function EquippedBuff(statisticType As IStatisticType) As Long?
End Interface
