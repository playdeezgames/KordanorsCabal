Public Interface IEquipment
    Inherits IBaseThingie
    ReadOnly Property CanEquip As Boolean
    ReadOnly Property EquipSlots() As IEnumerable(Of IEquipSlot)
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long?
End Interface
